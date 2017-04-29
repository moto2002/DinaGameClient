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
    class BagItemPartView : BagItemPartUIDetail
    {
        private int pos = -1;//背包中位置

        public BagItemPartView()
            : base(200, 100)
        {

        }

        private static BagItemPartView instance;
        public static BagItemPartView GetInstance()
        {
            if (instance == null)
                instance = new BagItemPartView();
            return instance;
        }

        protected override void PreInit()
        {
            base.PreInit();
            Input.selected = true;//被选中状态
        }

        protected override void Init()
        {
            base.Init();
            UIEventListener.Get(reduceBtn.gameObject).onClick += OnReduceCount;
            UIEventListener.Get(addBtn.gameObject).onClick += OnAddCount;
            UIEventListener.Get(cancelBtn.gameObject).onClick += OnCancel;
            UIEventListener.Get(sureBtn.gameObject).onClick += OnSure;
            UIEventListener.Get(closeBtn.gameObject).onClick += OnCancel;
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
            if (inputNum >= itemInfo.CurNum - 1)
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
            BagLogic.GetInstance().SendPartItemRequest(KPackageType.ePlayerPackage, KPlayerPackageIndex.eppiPlayerItemBox, pos, stackNum);
            Hide();
        }

        private void OnCancel(GameObject go)
        {
            Hide();
        }
    }
}
