using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Manager;
using Assets.Scripts.Data;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.Model.Player;
using Assets.Scripts.Model.Skill;
using Assets.Scripts.Controller;
using Assets.Scripts.Logic.Skill;

namespace Assets.Scripts.View.Skill
{
    public class TalentSkillView : SkillTabView
    {
        private static readonly int[] postions = { 2,3,6,7,8,10,11,14,15,16,17,19,21,22};
        private static readonly uint[][] skillIDList = {
                                        new uint[]{1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14},
                                        new uint[]{15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28},
                                        new uint[]{29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42}
                                                       };

        private static readonly Vector3[] posList = { new Vector3(-48.97f, 113.615f,0), new Vector3(46.72f,114.59f,0), new Vector3(-237.42f,38.40f,0),
                                                        new Vector3(-142.286f,39.305f,0), new Vector3(-48.92f,39.983f,0), new Vector3(140.94f,40.21f,0),
                                                        new Vector3(235.737f,40.21f,0), new Vector3(-48.714f,-36.026f,0), new Vector3(47.028f,-35.486f,0),
                                                        new Vector3(141.344f,-34.806f,0), new Vector3(236.34f,-34.134f,0), new Vector3(-142.256f,-110.065f,0),
                                                        new Vector3(46.875f,-108.65f,0), new Vector3(141.674f,-107.992f,0)};


        private Dictionary<string, GameObject> arrowList = new Dictionary<string, GameObject>();
        private Dictionary<uint, TalentSkillItem> skillItems = new Dictionary<uint, TalentSkillItem>();

        private UIImageButton addBtn = null;
        private UILabel skillPointTxt = null;


        protected void Init()
        {
            arrowList.Add("arrow_0_1", NGUIUtils.FindGameObject(gameObject, "arrow_0_1"));
            arrowList.Add("arrow_1_6", NGUIUtils.FindGameObject(gameObject, "arrow_1_6"));
            arrowList.Add("arrow_2_3", NGUIUtils.FindGameObject(gameObject, "arrow_2_3"));
            arrowList.Add("arrow_3_4", NGUIUtils.FindGameObject(gameObject, "arrow_3_4"));
            arrowList.Add("arrow_4_5", NGUIUtils.FindGameObject(gameObject, "arrow_4_5"));
            arrowList.Add("arrow_5_6", NGUIUtils.FindGameObject(gameObject, "arrow_5_6"));
            arrowList.Add("arrow_7_8", NGUIUtils.FindGameObject(gameObject, "arrow_7_8"));
            arrowList.Add("arrow_8_9", NGUIUtils.FindGameObject(gameObject, "arrow_8_9"));
            arrowList.Add("arrow_9_10", NGUIUtils.FindGameObject(gameObject, "arrow_9_10"));
            arrowList.Add("arrow_11_12", NGUIUtils.FindGameObject(gameObject, "arrow_11_12"));
            arrowList.Add("arrow_12_13", NGUIUtils.FindGameObject(gameObject, "arrow_12_13"));
            arrowList.Add("arrow_13_10", NGUIUtils.FindGameObject(gameObject, "arrow_12_10"));

            addBtn = NGUIUtils.FindUIObject<UIImageButton>(gameObject, "addBtn");
            skillPointTxt = NGUIUtils.FindUIObject<UILabel>(gameObject, "skillPointTxt");

            InitItem();
            UpateItem();
        }

        protected void InitEvent()
        {
            EventDispatcher.GameWorld.Regist(ControllerCommand.UPGRADE_TALENT_SKILL, UpgradeSkill);
            EventDispatcher.GameWorld.Regist(ControllerCommand.UPDATE_SKILL_POINT, UpgradeSkillPoint);
        }

        private void InitItem()
        {
            int Career = PlayerManager.GetInstance().MajorPlayer.Job - 1;//职业从１开始
            int n = 0;
            uint skillID = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    if (i * 6 + j == postions[n])
                    {
                        skillID = skillIDList[Career][n];
                        TalentSkillItem item = NGUITools.AddChild<TalentSkillItem>(gameObject);
                        GameObject go = GameObject.Instantiate(addBtn.gameObject) as GameObject;
                        UIEventListener.Get(go).onClick += OnUpgradeSkill;
                        item.AddPointBtn(go);
                        item.gameObject.transform.localPosition = posList[n++];
                        skillItems.Add(skillID, item);
                        KPassiveSkill skill = KConfigFileManager.GetInstance().GetPassiveSkill(skillID, 1);
                        PassiveSkillData skillData = new PassiveSkillData();
                        skillData.Learned = true;
                        item.SkillData = skillData;
                        if (n == 14)
                        {
                            i = 4;
                            break;
                        }
                    }
                }
            }
            GameObject.Destroy(addBtn.gameObject);
        }

        private void UpateItem()
        {
            foreach (PassiveSkillData skillData in SkillLogic.GetInstance().TalentSkillDict.Values)
            {
                skillItems[skillData.SkillID].SkillData = skillData;
            }

            foreach (TalentSkillItem item in skillItems.Values)
            {
                KPassiveSkill passiveSkillSetting = KConfigFileManager.GetInstance().GetPassiveSkill(item.SkillData.SkillID, item.SkillData.Level);
                List<KPassiveSkill> reqSkills = passiveSkillSetting.GetReqSkillList();
                int count = reqSkills.Count;
                for (int i = 0; i < count; i++)
                {
                    KPassiveSkill reqSkill = reqSkills[i];
                    PassiveSkillData reqSkillVO = skillItems[reqSkill.SkillID].SkillData;
                    GameObject arrow;
                    string key = "arrow_" + (reqSkill.Index - 1) + "_" + (passiveSkillSetting.Index - 1);
                    //if(!arrowList.TryGetValue(key, out arrow))
                    //{
                    //    Debug.Log(key + "-----------");
                    //}
                    //if (reqSkillVO.Learned && reqSkillVO.currentSkill.Level >= reqSkill.Level)
                    //{
                    //    arrow.SetActive(true);
                    //}
                    //else
                    //{
                    //    arrow.SetActive(false);
                    //}
                }
            }
        }

        private object UpgradeSkill(params object[] objs)
        {
            PassiveSkillData skillVO = objs[0] as PassiveSkillData;
            skillItems[skillVO.SkillID].SkillData = skillVO;
            return null;
        }

        private object UpgradeSkillPoint(params object[] objs)
        {
            skillPointTxt.text = SkillLogic.GetInstance().SkillPoint.ToString();
            return null;
        }

        private void OnUpgradeSkill(GameObject go)
        {
            TalentSkillItem item = go.transform.parent.gameObject.GetComponent<TalentSkillItem>();
            if (item != null)
            {
                SkillLogic.GetInstance().UpgradeSkill(0, (ushort)item.SkillData.SkillID);
            }
        }
    }
}
