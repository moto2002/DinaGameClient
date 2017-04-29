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
using Assets.Scripts.View.UIDetail;

namespace Assets.Scripts.View.Bag
{
    class BagItemUseView : BagItemUseUIDetail
    {
        private int pos = -1;//背包中位置

        public BagItemUseView()
            : base(200, 100)
        {

        }

        private static BagItemUseView instance;
        public static BagItemUseView GetInstance()
        {
            if (instance == null)
                instance = new BagItemUseView();
            return instance;
        }

        protected override void PreInit()
        {
            base.PreInit();
            Input.selected = true;
        }

        protected override void Init()
        {
            base.Init();
            UIEventListener.Get(reduceBtn.gameObject).onClick += OnReduceCount;
            UIEventListener.Get(closeBtn.gameObject).onClick += OnCancel;
            UIEventListener.Get(addBtn.gameObject).onClick += OnAddCount;
            UIEventListener.Get(cancelBtn.gameObject).onClick += OnCancel;
            UIEventListener.Get(sureBtn.gameObject).onClick += OnSure;
        }
        public void OpenView(int itemPos)
        {
            pos = itemPos;
            Show();
        }
       
        //使用按钮
        private void OnAddCount(GameObject go)
        {
            ItemInfo itemInfo = BagLogic.GetInstance().bagItems[pos];
            int inputNum = Int32.Parse(Input.label.text);
            if (inputNum >= itemInfo.CurNum)
                return;
            Input.label.text = (inputNum + 1).ToString();
        }
        private void OnReduceCount(GameObject go)
        {
            int inputNum = Int32.Parse(Input.label.text);
            if (inputNum <= 1)
                return;
            Input.label.text = (inputNum - 1).ToString();
        }

        private void OnSure(GameObject go)
        {
            int stackNum = Int32.Parse(Input.label.text);
            BagLogic.GetInstance().SendUseItemByNum(KPackageType.ePlayerPackage, KPlayerPackageIndex.eppiPlayerItemBox, pos, stackNum);
            Hide();
        }

        private void OnCancel(GameObject go)
        {
            Hide();
        }
    }
}
