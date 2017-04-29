using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Lib.View;
using Assets.Scripts.Define;
using Assets.Scripts.View.UIDetail;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.Lib.Resource;
using Assets.Scripts.Data;
using Assets.Scripts.Logic;
using UnityEngine;
using Assets.Scripts.Manager;
using Assets.Scripts.Logic.RemoteCall;
using Assets.Scripts.Logic.Item;
using Assets.Scripts.View.Scene;
using Assets.Scripts.Logic.Scene;
using Assets.Scripts.Model.Player;

namespace Assets.Scripts.View.Pve
{
    class PveView : BalanceUIDetail
    {
        
		private UISprite[] Items = new UISprite[5];
        private UILabel[] ItemNum = new UILabel[5];
		private UISprite[] Stars = new UISprite[3];
        private ParticleSystem[] ps = null;
        int nShowStar = 0;

        public PveView()
            : base(706, 504)
        {

        }

        protected override void Init()
        {
            base.Init();
            base.Hide();
            Items[0] = ItemsBoxSprite1;
            Items[1] = ItemsBoxSprite2;
            Items[2] = ItemsBoxSprite3;
            Items[3] = ItemsBoxSprite4;
            Items[4] = ItemsBoxSprite5;

            ItemNum[0] = QuantityLabel1;
            ItemNum[1] = QuantityLabel2;
            ItemNum[2] = QuantityLabel3;
            ItemNum[3] = QuantityLabel4;
            ItemNum[4] = QuantityLabel5;

            Stars[0] = Star1;
            Stars[1] = Star2;
            Stars[2] = Star3;

            ps = effect_ui_BalanceUI_star_xing.gameObject.GetComponentsInChildren<ParticleSystem>(true);
            effect_ui_BalanceUI_star_xing.gameObject.SetActive(false);

            EventDispatcher.GameWorld.Regist(ControllerCommand.CHANGE_MAP, OnChangeMap);
        }


        protected override void InitEvent()
        {
            base.InitEvent();
            UIEventListener.Get(base.ExitButton.gameObject).onClick += this.ExitPveMap;
        }

        private static PveView instance = null;
        public static PveView GetInstance()
        {
            if (instance == null)
            {
                instance = new PveView();
            }
            return instance;
        }

        public object OnChangeMap(params object[] objs)
        {
            if (SceneView.GetInstance().setting.Type != (uint)KMapType.mapPVEMap)
            {
                Hide();
            }

            return null;
        }

        private int showTime = 0;
        public override void FixedUpdate()
        {
            if (SceneView.GetInstance().setting.Type != (uint)KMapType.mapPVEMap)
            {
                return;
            }

            if (!viewGo.activeSelf)
            {
                return;
            }

            int reliveTime = 5 - (int)PlayerManager.GetInstance().MajorPlayer.onlineTime + showTime;

            if (reliveTime == 0)
            {
                ExitPveMap(null);
            }

            if (reliveTime == 5)
            {
                effect_ui_BalanceUI_star_xing.gameObject.SetActive(true);
                Star1.gameObject.SetActive(true);
                foreach (ParticleSystem x in ps)
                {
                    x.Play(true);
                }
                effect_ui_BalanceUI_star_xing.gameObject.transform.localPosition = Star1.gameObject.transform.localPosition;
            }
            else if (reliveTime == 4)
            {
                if (nShowStar < 2)
                {
                    effect_ui_BalanceUI_star_xing.gameObject.SetActive(false);
                    return;
                }
                effect_ui_BalanceUI_star_xing.gameObject.SetActive(true);
                Star2.gameObject.SetActive(true);
                foreach (ParticleSystem x in ps)
                {
                    x.Play(true);
                }
         
                effect_ui_BalanceUI_star_xing.gameObject.transform.localPosition = Star1.gameObject.transform.localPosition;
            }
            else if (reliveTime == 3)
            {
                if (nShowStar < 3)
                {
                    effect_ui_BalanceUI_star_xing.gameObject.SetActive(false);
                    return;
                }
                effect_ui_BalanceUI_star_xing.gameObject.SetActive(true);
                Star3.gameObject.SetActive(true);
                foreach (ParticleSystem x in ps)
                {
                    x.Play(true);
                }
                effect_ui_BalanceUI_star_xing.gameObject.transform.localPosition = Star1.gameObject.transform.localPosition;
            }
            else
            {
                effect_ui_BalanceUI_star_xing.gameObject.SetActive(false);
            }
        }
        public object ShowPveView(int nPveID, float fCompleteTime, RemoteTable award)
        {
            KPve pveInfo = KConfigFileManager.GetInstance().pveList.getData(nPveID.ToString());

            Open(null);
            SuccSprite1.gameObject.SetActive(false);
            SuccSprite2.gameObject.SetActive(false);
            SuccSprite3.gameObject.SetActive(false);

            if (fCompleteTime > pveInfo.tNormalTime)
            {
                SuccSprite1.gameObject.SetActive(true);
                nShowStar = 1;
            }
            else if (fCompleteTime > pveInfo.tGoodTime)
            {
                SuccSprite2.gameObject.SetActive(true);
                Star3.gameObject.SetActive(false);
                nShowStar = 2;
            }
            else
            {
                SuccSprite3.gameObject.SetActive(true);
                nShowStar = 3;
            }

            CompleteTimeLabel.text = GiftItem.FormatTime((int)fCompleteTime);

            ShowAward(award);
            showTime = (int)PlayerManager.GetInstance().MajorPlayer.onlineTime;
            return null;
        }

        private void ShowAward(RemoteTable award)
        {
            int nMoney = award["nMoney"];
            int nExp = award["nExp"];
            MoneyLabel.text = nMoney.ToString();
            ExpLabel.text = nExp.ToString();

            RemoteTable ItemTypes = (RemoteTable)award["ItemTypes"];
            RemoteTable ItemIndexs = (RemoteTable)award["ItemIndexs"];
            RemoteTable ItemAmouts = (RemoteTable)award["ItemAmouts"];

            for (int i = 0; i < 5; i++)
            {
                Items[i].gameObject.SetActive(false);
                ItemNum[i].gameObject.SetActive(false);
            }

            for (int i = 0; i < 3; i++ )
            {
                Stars[i].gameObject.SetActive(false);
            }
            
            int nCount = ItemTypes.dictKV.Count;
            for (int i = 1; i <= nCount; i++)
            {
                int nItemType = ItemTypes[i];
                int nItemIndex = ItemIndexs[i];
                int nItemAmouts = ItemAmouts[i];

                UIAtlas atlas = UIAtlasManager.GetInstance().GetUIAtlas("IconAtlas");
                Items[i - 1].atlas = atlas;
                ItemInfo itemVO = ItemLocator.GetInstance().GetItemVO(nItemIndex, (byte)nItemType);
                Items[i - 1].gameObject.SetActive(true);
                Items[i - 1].spriteName = itemVO.Icon;
                ItemNum[i - 1].gameObject.SetActive(true);
                ItemNum[i - 1].text = "X" + nItemAmouts;
            }

            BlackBGSprite.gameObject.transform.localScale = new Vector3(3, 3, 3);
        }

        private void ExitPveMap(GameObject go)
        {
            RemoteCallLogic.GetInstance().CallGS("OnExitPveMap");
            PveQuitView.GetInstance().Hide();
            PveAutoFight.GetInstance().Hide();
            PveProcessView.GetInstance().HideProcess();
            Hide();
        }

    }
}
