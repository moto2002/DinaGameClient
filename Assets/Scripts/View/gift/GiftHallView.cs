using Assets.Scripts.Lib.Loader;
using Assets.Scripts.View.UIDetail;
using Assets.Scripts.Lib.Resource;
using Assets.Scripts.Data;
using System.Collections.Generic;
using System;
using UnityEngine;
using Assets.Scripts.Logic;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Define;

namespace Assets.Scripts.View.Gift
{
    /************************************************************************/
    /* 礼包大厅界面 (包含战力和等级两个Tab)
     * author@wuheyang*/
    /************************************************************************/
    public class GiftHallView : GiftHallUIDetail
    {
        public List<GiftItem> CombatGiftItemList = new List<GiftItem>();
        public List<GiftItem> LevelGiftItemList = new List<GiftItem>();
        private readonly static int LIST_ITEM_MAX_SIZE = 7;
        private int curLevelListIndex = 1;
        private int curCombatListIndex = 1;
        private bool isLevelTabVisual = true;
        private bool isTabChanged = false;
        private bool isListChanged = false;

        public GiftHallView()
            : base(706, 504)
        {

        }

        protected override void Init()
        {
            base.Init();
            base.Hide();
          
            Dictionary<string, KGiftData> giftAllData = KConfigFileManager.GetInstance().giftTabFile.getAllData();
            List<KGiftData> levelAndCombatGiftDataList = new List<KGiftData>();

            foreach (KGiftData giftData in giftAllData.Values)
            {
                if (giftData.eType == KGiftType.gtLevel || giftData.eType == KGiftType.gtCombat)
                {
                    levelAndCombatGiftDataList.Add(giftData);
                }
            }

            levelAndCombatGiftDataList.Sort(
                delegate(KGiftData k1, KGiftData k2)
                {
                    int compare = k1.nCombatLimit - k2.nCombatLimit;
                    if (compare == 0)
                    {
                        return k1.nLevelLimit - k2.nLevelLimit;
                    }
                    else
                    {
                        return compare;
                    }
                }
            );

            foreach (KGiftData giftData in levelAndCombatGiftDataList)
            {
                UIDragPanelContents uiDragPanelContents = (UIDragPanelContents)UnityEngine.Object.Instantiate(base.GiftItem, base.GiftItem.transform.position, base.GiftItem.transform.rotation);
                uiDragPanelContents.transform.localScale = base.GiftItem.transform.lossyScale;
                GiftItem giftItem = uiDragPanelContents.gameObject.AddComponent<GiftItem>();
                giftItem.Init(giftData);

                if (giftData.eType == KGiftType.gtLevel)
                {
                    uiDragPanelContents.transform.parent = base.LevelList.gameObject.transform;
                    LevelGiftItemList.Add(giftItem);
                }
                else if (giftData.eType == KGiftType.gtCombat)
                {
                    uiDragPanelContents.transform.parent = base.CombatList.gameObject.transform;
                    CombatGiftItemList.Add(giftItem);
                }
            }

            base.GiftItem.gameObject.SetActive(false);
            base.LevelList.sorted = false;
            base.LevelList.repositionNow = true;
            base.CombatList.sorted = false;
            base.CombatList.repositionNow = true;

            this.isListChanged = true;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (isTabChanged || isListChanged)
            {
                isTabChanged = false;
                isListChanged = false;
                if (isLevelTabVisual)
                {
                    UpdateGiftList(base.LevelList, LevelGiftItemList, curLevelListIndex);
                }
                else
                {
                    UpdateGiftList(base.CombatList, CombatGiftItemList, curCombatListIndex);
                }
            }

        }
        private bool isBackBtnAbled = false;
        private bool isForwardBtnAbled = false;
        private void UpdateGiftList(UITable uiTable, List<GiftItem> giftItemList, int curListIndex)
        {
            int maxListIndex = Mathf.CeilToInt((float)giftItemList.Count / LIST_ITEM_MAX_SIZE);
            isBackBtnAbled = true;
            isForwardBtnAbled = true;

            if (curListIndex == 1)
            {
                isBackBtnAbled = false;
            }
            if (curListIndex == maxListIndex)
            {
                isForwardBtnAbled = false;     
            }

            string toolerLabelText = curListIndex + "/" + maxListIndex;
            base.ToolerLabel.text = toolerLabelText;

            foreach (GiftItem giftItem in giftItemList)
            {
                Disable(giftItem);
            }

            int beginIndex = (curListIndex - 1) * LIST_ITEM_MAX_SIZE;
            int endIndex = beginIndex + LIST_ITEM_MAX_SIZE;
            endIndex = endIndex < giftItemList.Count ? endIndex : giftItemList.Count;

            for (; beginIndex < endIndex; ++beginIndex)
            {
                GiftItem giftItem = giftItemList[beginIndex];
                Active(giftItem);
            }

            uiTable.repositionNow = true;
        }

        public override void UpdateUIOnDataChanged()
        {
            foreach (GiftItem levelGiftItem in LevelGiftItemList)
            {
                levelGiftItem.UpdateUIOnDataChanged();
            }

            foreach (GiftItem combatGiftItem in CombatGiftItemList)
            {
                combatGiftItem.UpdateUIOnDataChanged();
            }
        }

        protected override void InitEvent()
        {
            base.InitEvent();
            UIEventListener.Get(base.CloseButton.gameObject).onClick += base.OnClickClose;
            UIEventListener.Get(base.LevelBtn.gameObject).onClick += this.TabClick;
            UIEventListener.Get(base.CombatBtn.gameObject).onClick += this.TabClick;
            UIEventListener.Get(base.ForwardBtn.gameObject).onClick += this.ForwardBtnClick;
            UIEventListener.Get(base.BackBtn.gameObject).onClick += this.BackBtnClick;
        }

        private void TabClick(GameObject go)
        {
            if (go != null)
            {
                if (base.LevelBtn.gameObject == go)
                {
                    if (!isLevelTabVisual)
                    {
                        isTabChanged = true;
                    }
                    isLevelTabVisual = true;
                }
                else if (base.CombatBtn.gameObject == go)
                {
                    if (isLevelTabVisual)
                    {
                        isTabChanged = true;
                    }
                    isLevelTabVisual = false;
                }
            }
        }

        private void ForwardBtnClick(GameObject go)
        {
            if (isForwardBtnAbled)
            {
                if (isLevelTabVisual)
                {
                    ++curLevelListIndex;
                }
                else
                {
                    ++curCombatListIndex;
                }
                isListChanged = true;
            }
        }

        private void BackBtnClick(GameObject go)
        {
            if (isBackBtnAbled)
            {
                if (isLevelTabVisual)
                {
                    --curLevelListIndex;
                }
                else
                {
                    --curCombatListIndex;
                }
                isListChanged = true;
            }
        }

        private void Active(MonoBehaviour mo)
        {
            if (mo != null)
            {
                mo.gameObject.SetActive(true);
            }
        }

        private void Disable(MonoBehaviour mo)
        {
            if (mo != null)
            {
                mo.gameObject.SetActive(false);
            }
        }

        private static GiftHallView instance = null;
        public static GiftHallView GetInstance()
        {
            if (instance == null)
            {
                instance = new GiftHallView();
            }
            return instance;
        }
    }

}
