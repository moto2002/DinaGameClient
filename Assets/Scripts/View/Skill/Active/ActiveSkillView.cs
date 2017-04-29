using UnityEngine;
using Assets.Scripts.Manager;
using Assets.Scripts.Data;
using Assets.Scripts.Model.Skill;
using Assets.Scripts.Controller;
using Assets.Scripts.Logic.Skill;
using Assets.Scripts.Lib.Loader;

namespace Assets.Scripts.View.Skill
{
    public class ActiveSkillView : SkillTabView
    {
        public const sbyte SKILL_ITEM_COUNT = 6;
        private ActiveSkillItem[] items = null;

        public GameObject bigEffect = null;

        private UILabel skillNameTxt = null;
        private UILabel curDescTxt = null;
        private UILabel nextDescTxt = null;
        private UILabel reqLevelTxt = null;
        private UILabel reqMoneyTxt = null;
        private UISprite proBarSp = null;
        private ActiveSkillItem currentItem = null;

        void Awake()
        {
            skillNameTxt = NGUIUtils.FindUIObject<UILabel>(gameObject, "skillNameTxt");
            curDescTxt = NGUIUtils.FindUIObject<UILabel>(gameObject, "curDescTxt");
            nextDescTxt = NGUIUtils.FindUIObject<UILabel>(gameObject, "nextDescTxt");
            reqLevelTxt = NGUIUtils.FindUIObject<UILabel>(gameObject, "reqLevelTxt");
            reqMoneyTxt = NGUIUtils.FindUIObject<UILabel>(gameObject, "reqMoneyTxt");
            proBarSp = NGUIUtils.FindUIObject<UISprite>(gameObject, "proBarSp");

            bigEffect = NGUIUtils.FindGameObject(gameObject, "BigEffect");

            UIEventListener.Get(NGUIUtils.FindGameObject(gameObject, "upgradeBtn")).onClick += OnUpgradeSkill;

            InitItem();
        }

        private void InitItem()
        {
            ActiveSkillData[] activeSkillDataList = new ActiveSkillData[SkillLogic.GetInstance().ActiveSkillDict.Count];
            SkillLogic.GetInstance().ActiveSkillDict.Values.CopyTo(activeSkillDataList, 0);

            items = new ActiveSkillItem[SKILL_ITEM_COUNT];                      //建立数组
            for (int i = 0; i < SKILL_ITEM_COUNT; i++)
            {
                items[i] = NGUITools.AddChild<ActiveSkillItem>(gameObject);
                ActiveSkillItem item = items[i];

                if (i < SkillLogic.GetInstance().ActiveSkillDict.Count)
                {
                    item.SkillData = activeSkillDataList[i];

                    if (item.gameObject.collider == null)
                    {
                        NGUITools.AddWidgetCollider(item.gameObject);
                    }
                    UIEventListener.Get(item.gameObject).onClick += OnClickItem;
                }
                else
                {
                    item.SkillData = null;
                }
                item.gameObject.transform.localPosition = new Vector3(-223 + i * 88, 104, 0);
            }
            //显示第一个
            if (SkillLogic.GetInstance().ActiveSkillDict.Count > 1)
            {
                OnClickItem(items[0].gameObject);
            }
        }


        public override void UpdateView(params object[] objs)
        {
            uint skillId = (uint)objs[1];
            ActiveSkillData newSkillData;
            if (SkillLogic.GetInstance().ActiveSkillDict.TryGetValue(skillId, out newSkillData) == false)
                return;
            ActiveSkillData skill = null;
            int firstNull = -1;

            for (int i = 0; i < SKILL_ITEM_COUNT; i++)
            {
                skill = items[i].SkillData;
                if (skill != null && skill.SkillID == newSkillData.SkillID)
                {
                    items[i].SkillData = newSkillData;
                    if (currentItem != null && currentItem == items[i])
                    {
                        UpdateSkillDesc(newSkillData);
                        firstNull = -1;
                        break;
                    }
                }
                if (skill == null && firstNull < 0)
                {
                    firstNull = i;
                }
            }
            if (firstNull >= 0)//学习技能
            {
                items[firstNull].SkillData = newSkillData;
                if (items[firstNull].gameObject.collider == null)
                {
                    NGUITools.AddWidgetCollider(items[firstNull].gameObject);
                }
                UIEventListener.Get(items[firstNull].gameObject).onClick += OnClickItem;
                OnClickItem(items[firstNull].gameObject);
            }
        }


        private void OnClickItem(GameObject go)
        {
            ActiveSkillItem item = go.GetComponent<ActiveSkillItem>();
            if (item == null)
            {
                return;
            }
            UpdateSkillDesc(item.SkillData);
            currentItem = item;
            ShowSelectedSkillEffect();
        }

        public void ShowSelectedSkillEffect()
        {
            if (currentItem != null)
            {
                bigEffect.SetActive(true);
                bigEffect.transform.position = currentItem.gameObject.transform.position;
            }
        }

        private void UpdateSkillDesc(ActiveSkillData skillData)
        {
            KActiveSkill currSkillSetting = KConfigFileManager.GetInstance().GetActiveSkill(skillData.SkillID, skillData.Level);
            KActiveSkill nextSkillSetting = KConfigFileManager.GetInstance().GetActiveSkill(skillData.SkillID, skillData.Level + 1);
			
			if (currSkillSetting != null)
			{
				skillNameTxt.text = "<fff1cc>" + currSkillSetting.Text + "<->";
            	curDescTxt.text = "<949393>" + currSkillSetting.Desc + "<->";
			}
            if (nextSkillSetting != null)
            {
                nextDescTxt.text = "<ebba10>" + nextSkillSetting.Desc + "<->";
                reqLevelTxt.text = "<ebba10>" + nextSkillSetting.LearnLevel + "<->";
                reqMoneyTxt.text = "<ebba10>" + nextSkillSetting.CostMoney + "<->";
                proBarSp.fillAmount = ((float)skillData.SkillExp) / nextSkillSetting.MaxSkillExp;
            }
            else
            {
                nextDescTxt.text = "<ff0000>已达最高等级<->";
                reqLevelTxt.text = "<ff0000>--<->";
                reqMoneyTxt.text = "<ff0000>--<->";
                proBarSp.fillAmount = 1.0f;
            }
        }

        private void OnUpgradeSkill(GameObject go)
        {
            if (currentItem != null)
            {
                SkillLogic.GetInstance().SendUpgradeSkill((byte)1, (ushort)currentItem.SkillData.SkillID);
            }
        }
    }
}
