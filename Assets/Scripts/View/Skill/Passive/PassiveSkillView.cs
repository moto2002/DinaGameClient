using UnityEngine;
using System;
using System.Collections.Generic;
using Assets.Scripts.Manager;
using Assets.Scripts.Controller;
using Assets.Scripts.Model.Skill;
using Assets.Scripts.Logic.Item;
using Assets.Scripts.Model.Player;
using Assets.Scripts.Data;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.Logic.Skill;

namespace Assets.Scripts.View.Skill
{
    public class PassiveSkillView : SkillTabView
    {
        public const sbyte SKILL_ITEM_COUNT = 8;
        public const sbyte REQ_ITEM_COUNT = 3;
        public const sbyte SKILL_TYPE_COUNT = 3;
        public static readonly sbyte[] SKILL_TYPE = new sbyte[]{ 1, 2, 3};

        private int currentType = -1;
        private PassiveSkillItem currentItem = null;

        private PassiveSkillItem[] skillItems = null;
        private PassiveSkillTypeItem[] typeItems = null;
        private PassiveSkillReqItem[] reqItems = null;


        private UILabel reqLevelTxt = null;
        private UILabel reqMenoryTxt = null;

        private GameObject bigEffect;
        private GameObject smallEffect;

        void Awake()
        {
            reqLevelTxt = NGUIUtils.FindUIObject<UILabel>(gameObject, "reqLevelTxt");
            reqMenoryTxt = NGUIUtils.FindUIObject<UILabel>(gameObject, "reqMenoryTxt");
            bigEffect = NGUIUtils.FindGameObject(gameObject, "BigEffect");
            smallEffect = NGUIUtils.FindGameObject(gameObject.transform.parent.gameObject, "SmallEffect");

            InitTypeItem();
            InitSkillItem();
            InitReqItem();
            OnClickTypeItem(typeItems[0].gameObject);
            UIEventListener.Get(NGUIUtils.FindGameObject(gameObject, "upgradeBtn")).onClick = OnClickUpgrade;
        }

        protected void InitEvent()
        {
            EventDispatcher.GameWorld.Regist(ControllerCommand.UPGRADE_PASSIVE_SKILL, UpgradeSkill);
        }

        private void InitTypeItem()
        {
            typeItems = new PassiveSkillTypeItem[SKILL_TYPE_COUNT];
            for (int i = 0; i < REQ_ITEM_COUNT; i++)
            {
                typeItems[i] = NGUITools.AddChild<PassiveSkillTypeItem>(gameObject);
                typeItems[i].Data = SKILL_TYPE[i];
                if (typeItems[i].gameObject.collider == null)
                {
                    NGUITools.AddWidgetCollider(typeItems[i].gameObject);
                }
                UIEventListener.Get(typeItems[i].gameObject).onClick += OnClickTypeItem;
                if (i == 2)
                {
                    typeItems[i].gameObject.transform.localPosition = new Vector3(166.909f, 86.071f, 0f);
                }
                else
                {
                    typeItems[i].gameObject.transform.localPosition = new Vector3(-138.2f + i * 155.4f, 84.2f, 0f);
                }
            }
        }

        private void InitSkillItem()
        {
            //skillItems = new PassiveSkillItem[SKILL_ITEM_COUNT];

            //List<KPassiveSkill> skillDataList = KSkillManager.GetInstance().passiveSkillDataList[SKILL_TYPE[0]];
            //int count = skillDataList.Count;
            //for (int i = 0; i < SKILL_ITEM_COUNT; i++)
            //{
            //    skillItems[i] = NGUITools.AddChild<PassiveSkillItem>(gameObject);
            //    if (i < count)
            //    {
            //        PassiveSkillVO skillVO = new PassiveSkillVO();
            //        skillVO.SetSkill(skillDataList[i]);
            //        skillItems[i].SkillData = skillVO;
            //        if (skillItems[i].gameObject.collider == null)
            //        {
            //            NGUITools.AddWidgetCollider(skillItems[i].gameObject);
            //        }
            //        UIEventListener.Get(skillItems[i].gameObject).onClick += ClickItem;
            //    }
            //    else
            //    {
            //        skillItems[i].SkillData = null;
            //    }
            //    skillItems[i].gameObject.transform.localPosition = new Vector3(-214f + i * 64.5f, -15f, 0f);
            //}
        }

        private void InitReqItem()
        {
            if (reqItems == null)
            {
                reqItems = new PassiveSkillReqItem[REQ_ITEM_COUNT];
                for (int i = 0; i < REQ_ITEM_COUNT; i++)
                {
                    reqItems[i] = NGUITools.AddChild<PassiveSkillReqItem>(gameObject);
                    if (reqItems[i].gameObject.collider == null)
                    {
                        NGUITools.AddWidgetCollider(reqItems[i].gameObject);
                    }
                    reqItems[i].gameObject.transform.localPosition = new Vector3(-156.88f + i * 69.84f, -119.06f, 0f);
                    reqItems[i].gameObject.SetActive(false);
                }
            }
        }

        public void ShowSelectedSkill()
        {
            ShowSelectedType();
            ShowSelectedItem();
        }

        private void ShowSelectedType()
        {
            if (currentType > 0 )
            {
                bigEffect.SetActive(true);
                bigEffect.transform.position = typeItems[currentType - 1].gameObject.transform.position;
            }
        }

        private void ShowSelectedItem()
        {
            if (currentItem != null)
            {
                smallEffect.SetActive(true);
                smallEffect.transform.position = currentItem.gameObject.transform.position;
            }
        }


        private void OnClickTypeItem(GameObject go)
        {
            //PassiveSkillTypeItem type = go.GetComponent<PassiveSkillTypeItem>();
            //int typeInt = Convert.ToInt32(type.Data);
            //if(currentType == typeInt)
            //{
            //    return;
            //}
            //else
            //{
            //    currentType = typeInt;
            //}
            //int count = SkillLogic.Instance.PassiveSkillDict.Count;
            //for (int i = 0; i < SKILL_ITEM_COUNT; i++)
            //{
            //    if (i < count)
            //    {
            //        PassiveSkillData skillVO = null;
                    
            //        foreach (PassiveSkillData vo in SkillLogic.Instance.PassiveSkillDict.Values)
            //        {
            //            if (vo.SkillID == skillDataList.ToList() [i].SkillID)
            //            {
            //                skillVO = vo;
            //                break;
            //            }
            //        }
            //        if (skillVO == null)
            //        {
            //            skillVO = new PassiveSkillData();
            //     //      skillVO.SetSkill(skillDataList[i]);
            //        }
            //        skillItems[i].SkillData = skillVO;
            //    }
            //    else
            //    {
            //        skillItems[i].SkillData = null;
            //    }
            //}
            //ShowSelectedType();
            //ClickItem(skillItems[0].gameObject);
        }

        private void ClickItem(GameObject go)
        {
            PassiveSkillItem skillItem = go.GetComponent<PassiveSkillItem>();
            if (currentItem == skillItem)
            {
                return;
            }
            else
            {
                currentItem = skillItem;
            }
            ShowSelectedItem();
            UpdateReqItem();
        }

        private void UpdateReqItem()
        {
            KPassiveSkill currSkillSetting = KConfigFileManager.GetInstance().GetPassiveSkill(currentItem.SkillData.SkillID, currentItem.SkillData.Level);
            KPassiveSkill nextSkillSetting = KConfigFileManager.GetInstance().GetPassiveSkill(currentItem.SkillData.SkillID, currentItem.SkillData.Level + 1);
            List<ItemInfo> itemList = null;
            int count = 0;
            if (currSkillSetting != null)
            {
                itemList = nextSkillSetting.GetReqItemList();
                count = itemList.Count;
                reqLevelTxt.text = currSkillSetting.LearnLevel.ToString();
                reqMenoryTxt.text = currSkillSetting.CostMoney.ToString();
            }
            else
            {
                reqLevelTxt.text = "";
                reqMenoryTxt.text = "";
            }  
            for (int i = 0; i < REQ_ITEM_COUNT; i++)
            {
                if (i < count)
                {
                    reqItems[i].gameObject.SetActive(true);
                    reqItems[i].ItemData = itemList[i];
                }
                else
                {
                    reqItems[i].gameObject.SetActive(false);
                }
            }
            
            
        }

        private object UpgradeSkill(params object[] objs)
        {
            PassiveSkillData skillVO = objs[0] as PassiveSkillData;
            foreach (PassiveSkillItem item in skillItems)
            {
                if (item.SkillData.SkillID == skillVO.SkillID)
                {
                    item.SkillData = skillVO;
                    if (currentItem != null && currentItem == item)
                    {
                        UpdateReqItem();
                    }
                    break;
                }
            }
            return null;
        }

        private void OnClickUpgrade(GameObject go)
        {
            if (currentItem != null)
            {
                SkillLogic.GetInstance().UpgradeSkill((byte)0, (ushort)currentItem.SkillData.SkillID);
            }
        }
    }


}
