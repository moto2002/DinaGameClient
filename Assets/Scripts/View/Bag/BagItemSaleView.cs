using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Manager;
using Assets.Scripts.Lib.View;
using Assets.Scripts.Logic.Bag;
using Assets.Scripts.Define;
using Assets.Scripts.Logic.Item;
using System.Collections;
using Assets.Scripts.View.UIDetail;

namespace Assets.Scripts.View.Bag
{
    class BagItemSaleView : BagItemSaleUIDetail
    {
        private int pos = -1;//背包中位置
        private int saleType = -1;//卖的方式
        private bool loadSign = false;//资源加载完成标识

        public BagItemSaleView()
            : base(200, 100)
        {

        }

        private static BagItemSaleView instance;
        public static BagItemSaleView GetInstance()
        {
            if (instance == null)
                instance = new BagItemSaleView();
            return instance;
        }

        protected override void PreInit()
        {
            base.PreInit();
        }

        protected override void Init()
        {
            base.Init();
            UIEventListener.Get(closeBtn.gameObject).onClick += OnCancel;
            UIEventListener.Get(cancelBtn.gameObject).onClick += OnCancel;
            UIEventListener.Get(sureBtn.gameObject).onClick += OnSure;

            loadSign = true;
            ShowView();
            Show();
        }
        public void OpenView(int itemPos, int type)
        {
            pos = itemPos;
            saleType = type;
            ShowView();
        
        }
        private void ShowView()
        {
            if (loadSign)
            {
                ItemInfo itemInfo = BagLogic.GetInstance().bagItems[pos];
                if (saleType == (int)ItemSaleType.eSaleToSys)
                {
                    showLabel.text = "您确定要将该物品<e642f9>[" + itemInfo.Name + "]<->出售给商店？";
                    titleLabel.text = "出售物品";
                }
                else
                {
                    showLabel.text = "您确定要将该物品<e642f9>[" + itemInfo.Name + "]<->丢弃给商店？";
                    titleLabel.text = "丢弃物品";
                }
                Show();
            }
        }

       

        private void OnSure(GameObject go)
        {
            BagLogic.GetInstance().SendSaleItem(KPackageType.ePlayerPackage, KPlayerPackageIndex.eppiPlayerItemBox, pos);
            Hide();
        }

        private void OnCancel(GameObject go)
        {
            Hide();
        }
    }
}
