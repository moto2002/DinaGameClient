using Assets.Scripts.Lib.Loader;
using Assets.Scripts.View.UIDetail;
using Assets.Scripts.Lib.Resource;
using Assets.Scripts.Data;
using System.Collections.Generic;
using System;
using UnityEngine;
using Assets.Scripts.Logic;
using Assets.Scripts.Model.Player;
using Assets.Scripts.Define;

namespace Assets.Scripts.View.Gift
{
    /************************************************************************/
    /* 在线礼包界面 
     * author@wuheyang*/
    /************************************************************************/
    public class OnlineGiftView : OnlineGiftUIDetail
    {
        private readonly static string TODAY_ONLINE_TIME_TEXT = "今日在线时间：";
        private readonly static string MIN_TEXT = "分钟";

        public List<GiftItem> OnlineGiftItemList = new List<GiftItem>();

        public OnlineGiftView()
            : base(706, 504)
        {

        }
       
        protected override void Init()
        {
            base.Init();
            base.Hide();

            Dictionary<string, KGiftData> GiftAllData = KConfigFileManager.GetInstance().giftTabFile.getAllData();
            List<KGiftData> onlineGiftDataList = new List<KGiftData>();
            foreach (KGiftData giftData in GiftAllData.Values)
            {
                if (giftData.eType == KGiftType.gtOnlineTime)
                {
                    onlineGiftDataList.Add(giftData);
                }
            }

            onlineGiftDataList.Sort(
                delegate(KGiftData k1, KGiftData k2)
                {
                    return k1.nOnlineTime - k2.nOnlineTime;
                }
            );

            foreach (KGiftData onlineGiftData in onlineGiftDataList)
            {
                UIDragPanelContents uiDragPanelContents = (UIDragPanelContents)UnityEngine.Object.Instantiate(base.GiftItem, base.GiftItem.transform.position, base.GiftItem.transform.rotation);
                uiDragPanelContents.transform.localScale = base.GiftItem.transform.lossyScale;
                uiDragPanelContents.transform.parent = base.GiftList.gameObject.transform;

                GiftItem giftItem = uiDragPanelContents.gameObject.AddComponent<GiftItem>();
                giftItem.Init(onlineGiftData);
                OnlineGiftItemList.Add(giftItem);
            }

            base.GiftItem.gameObject.SetActive(false);
            base.GiftList.sorted = false;
            base.GiftList.repositionNow = true;
        }

        public override void UpdateUIOnDataChanged()
        {
            foreach(GiftItem giftItem in OnlineGiftItemList) 
            {
                giftItem.UpdateUIOnDataChanged();
            }
        }

        public override void FixedUpdate()
        {
            int min = (int)PlayerManager.GetInstance().MajorPlayer.onlineTime / 60;
            base.TodayOnlineTimeSprite.text = TODAY_ONLINE_TIME_TEXT + min + MIN_TEXT;
        }

        protected override void InitEvent()
        {
            base.InitEvent();
            UIEventListener.Get(base.CloseButton.gameObject).onClick += base.OnClickClose;
        }

        private static OnlineGiftView instance = null;
        public static OnlineGiftView GetInstance()
        {
            if (instance == null)
            {
                instance = new OnlineGiftView();
            }
            return instance;
        }
    }
}

