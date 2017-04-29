using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Lib.View;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.Utils;
using UnityEngine;
using Assets.Scripts.Logic.Item;
using Assets.Scripts.Logic.Bag;
using Assets.Scripts.Manager;
using Assets.Scripts.Define;
using Assets.Scripts.View.UIDetail;

namespace Assets.Scripts.View.Bag
{
    public class BagView : BagUIDetail
	{
        private BagStorage allStorage;

        public BagView()
            : base(350, 533)
        {

        }
        private static BagView instance;
        public static BagView GetInstance()
        {
            if (instance == null)
                instance = new BagView();
            return instance;
        }

        protected override void PreInit()
        {
            base.PreInit();
            CreateBagStorage();
        }

        protected override void Init()
        {
            base.Init();
            UIEventListener.Get(closeBtn.gameObject).onClick += OnClickCloseBtnHandler;
            UIEventListener.Get(sortBtn.gameObject).onClick += OnSortBagItemBox;
            EventDispatcher.GameWorld.Regist(ControllerCommand.UPDATE_BAG_GOODS, OnUpdateBagGoods);
        }

        private void CreateBagStorage()
        {
            allStorage = NGUITools.AddChild<BagStorage>(Panel.gameObject);
            allStorage.gameObject.transform.localPosition = new Vector3(-176, 220, -2);
            allStorage.CreateItems();
            ObjectUtil.SetLayerWithAllChildren(allStorage.gameObject, "2D");

            allStorage.SetGoods(BagLogic.GetInstance().bagItems);

            ReplacementLayer();
        }

        private object OnUpdateBagGoods(params object[] objs)
        {
            int pos = Convert.ToInt32(objs[0]);
            UpdateGoods(pos);
            return null;
        }

        private void UpdateGoods(int pos)
        {
            ItemInfo item = BagLogic.GetInstance().GetItemByPos(pos);
            allStorage.UpdateGoods(pos, item);
        }

        private void OnClickCloseBtnHandler(GameObject go)
        {
            Hide();
            EventDispatcher.GameWorld.DispatchEvent(ControllerCommand.CLOSE_BAGITEMCLICK_PANEL);
        }
        private void OnSortBagItemBox(GameObject go)
        {
            BagLogic.GetInstance().BagItemSort();
        }
	}
}
