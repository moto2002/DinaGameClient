using Assets.Scripts.Lib.Loader;
using Assets.Scripts.View.UIDetail;
using Assets.Scripts.Lib.Resource;
using Assets.Scripts.Data;
using System.Collections.Generic;
using System;
using UnityEngine;
using Assets.Scripts.Logic;
using Assets.Scripts.Model.Player;
using Assets.Scripts.View.Gift;
using Assets.Scripts.Lib.View;

namespace Assets.Scripts.View.MainMenu
{
    /************************************************************************/
    /* 右上角菜单界面 
     * author@wuheyang*/
    /************************************************************************/
    public class TopRightMenuView : TopRightMenuUIDetail
    {
        private GameObject onlineGiftBtnEffect = null;

        public TopRightMenuView()
            : base(0, 0)
        {

        }

        protected override void Init()
        {
            base.Init();
            base.SetViewPosition(ViewPosition.TopRight);
            uiPanel.transform.localPosition = new Vector3(-280, -40, 0);
            onlineGiftBtnEffect = GameObject.Find("effect_ui_tubiao");
            if (onlineGiftBtnEffect != null)
            {
                onlineGiftBtnEffect.SetActive(false);
                base.OnlineGiftBagBtnNumber.text = "";
            }
        }

        public override void UpdateUIOnDataChanged()
        {

        }

        public override void FixedUpdate()
        {
            UpdateGiftAvailableCount();
            if (OnlineGiftAvailableCount > 0)
            {
                if (onlineGiftBtnEffect != null && !onlineGiftBtnEffect.activeSelf)
                {
                    onlineGiftBtnEffect.SetActive(true);
                }
                base.OnlineGiftBagBtnNumber.text = "<ff0000>" + OnlineGiftAvailableCount.ToString();
            }
            else
            {
                if (onlineGiftBtnEffect != null && onlineGiftBtnEffect.activeSelf)
                {
                    onlineGiftBtnEffect.SetActive(false);
                    base.OnlineGiftBagBtnNumber.text = "";
                }
            }
        }

        public int OnlineGiftAvailableCount = 0;
        public int CombatGiftAvailableCount = 0;
        public int LevelGiftAvailableCount = 0;

        private void UpdateGiftAvailableCount()
        {
            OnlineGiftAvailableCount = 0;
            CombatGiftAvailableCount = 0;
            LevelGiftAvailableCount = 0;

            MajorPlayer player = PlayerManager.GetInstance().MajorPlayer;

            List<GiftItem> onlineGiftItemList = OnlineGiftView.GetInstance().OnlineGiftItemList;
            int playerOnlineTime = (int)player.onlineTime;
            foreach (GiftItem onlineGiftItem in onlineGiftItemList)
            {
                KGiftData onlineGiftData = onlineGiftItem.GiftData;
				if(player.rewardData != null && player.rewardData[onlineGiftData.nID] != null) 
				{
                	bool isReward = player.rewardData[onlineGiftData.nID];
                	if (!isReward && onlineGiftData.nOnlineTime * 60 - playerOnlineTime <= 0)
                	{
                    	++OnlineGiftAvailableCount;
                	}
				}
            }

            List<GiftItem> levelGiftItemList = GiftHallView.GetInstance().LevelGiftItemList;
            foreach (GiftItem levelGiftItem in levelGiftItemList)
            {
                KGiftData levelGiftData = levelGiftItem.GiftData;
				if(player.rewardData !=null && player.rewardData[levelGiftData.nID] != null) 
				{
                    bool isReward = player.rewardData[levelGiftData.nID];
                	if (!isReward && levelGiftData.nLevelLimit - player.levelCurrent <= 0)
                	{
                    	++LevelGiftAvailableCount;
                	}
				}
            }

            List<GiftItem> combatGiftItemList = GiftHallView.GetInstance().CombatGiftItemList;
            foreach (GiftItem combatGiftItem in combatGiftItemList)
            {
                KGiftData combatGiftData = combatGiftItem.GiftData;
				if(player.rewardData != null && player.rewardData[combatGiftData.nID] != null) 
				{
                    bool isReward = player.rewardData[combatGiftData.nID];
                	if (!isReward && combatGiftData.nCombatLimit - player.combat <= 0)
                	{
                   		 ++CombatGiftAvailableCount;
               		}
				}
            }
        }

        protected override void InitEvent()
        {
            base.InitEvent();
            UIEventListener.Get(base.OnlineGiftBagBtn.gameObject).onClick += OnlineGiftView.GetInstance().Open;
        }

        public bool IsGiftHallAvailable()
        {
            return CombatGiftAvailableCount > 0 || LevelGiftAvailableCount > 0;
        }

        private static TopRightMenuView instance = null;
        public static TopRightMenuView GetInstance()
        {
            if (instance == null)
            {
                instance = new TopRightMenuView();
            }
            return instance;
        }
    }
}

