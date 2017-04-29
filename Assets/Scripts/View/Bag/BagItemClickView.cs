using System;
using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Logic.Item;
using Assets.Scripts.Logic.Bag;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.Utils;
using Assets.Scripts.Controller;
using Assets.Scripts.View.Drag;
using Assets.Scripts.View.Role;
using Assets.Scripts.Manager;
using Assets.Scripts.Lib.View;
using Assets.Scripts.Define;
using Assets.Scripts.View.UIDetail;

namespace Assets.Scripts.View.Bag
{
    public class BagItemClickView : BagItemClickUIDetail
    {
        public BagItemClickView()
            : base(20, 40)
        {

        }
        UIAtlas atlas = null;

        private float scaleX = 0.5f, scaleY = 0.5f, scaleZ = 0.5f;
        private int space = 15;//各个lable见的间隔
        private int itemPos = -1;
        private bool loadSign = false;
        private Transform transform = null;
        private static BagItemClickView instance;
        public static BagItemClickView GetInstance()
        {
            if (instance == null)
                instance = new BagItemClickView();
            return instance;
        }

        protected override void PreInit()
        {
            base.PreInit();
            atlas = UIAtlasManager.GetInstance().GetUIAtlas("ChatsystemAtlas");

            background.atlas = atlas;
            background.spriteName = "选项弹出底";
            background.pivot = UIWidget.Pivot.Top;
            
            useLabel.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
            useLabel.transform.localPosition = new Vector3(useLabel.transform.localPosition.x, useLabel.transform.localPosition.y, 0);
            NGUITools.AddWidgetCollider(useLabel.gameObject);
            BoxCollider collider = useLabel.gameObject.GetComponent<BoxCollider>();
            collider.center = new Vector3(collider.center.x, collider.center.y, 0);

            useallLabel.transform.localPosition = new Vector3(useLabel.transform.localPosition.x,
            useLabel.transform.localPosition.y - space, 0);
            useallLabel.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
            NGUITools.AddWidgetCollider(useallLabel.gameObject);
            collider = useallLabel.gameObject.GetComponent<BoxCollider>();
            collider.center = new Vector3(collider.center.x, collider.center.y, 0);

            partLabel.transform.localPosition = new Vector3(useLabel.transform.localPosition.x,
            useallLabel.transform.localPosition.y - space, 0);
            partLabel.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
            NGUITools.AddWidgetCollider(partLabel.gameObject);
            collider = partLabel.gameObject.GetComponent<BoxCollider>();
            collider.center = new Vector3(collider.center.x, collider.center.y, 0);

            saleLabel.transform.localPosition = new Vector3(useLabel.transform.localPosition.x,
            partLabel.transform.localPosition.y - space,0);
            saleLabel.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
            NGUITools.AddWidgetCollider(saleLabel.gameObject);
            collider = saleLabel.gameObject.GetComponent<BoxCollider>();
            collider.center = new Vector3(collider.center.x, collider.center.y, 0);

            showLabel.transform.localPosition = new Vector3(useLabel.transform.localPosition.x,
            saleLabel.transform.localPosition.y - space,0);
            showLabel.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
            NGUITools.AddWidgetCollider(showLabel.gameObject);
            collider = showLabel.gameObject.GetComponent<BoxCollider>();
            collider.center = new Vector3(collider.center.x, collider.center.y, 0);

            dropLabel.transform.localPosition = new Vector3(useLabel.transform.localPosition.x,
            showLabel.transform.localPosition.y - space,0);
            dropLabel.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
            NGUITools.AddWidgetCollider(dropLabel.gameObject);
            collider = dropLabel.gameObject.GetComponent<BoxCollider>();
            collider.center = new Vector3(collider.center.x, collider.center.y, 0);
        }

        protected override void Init()
        {
            base.Init();
            UIEventListener.Get(useLabel.gameObject).onClick += OnUseItem;
            UIEventListener.Get(useallLabel.gameObject).onClick += OnUseItemByNum;
            UIEventListener.Get(partLabel.gameObject).onClick += OnPartItem;
            UIEventListener.Get(saleLabel.gameObject).onClick += OnSaleItem;
            UIEventListener.Get(dropLabel.gameObject).onClick += OnDropItem;
           
            loadSign = true;
            ShowLableView();
            Show();
        }

        public void ChangeItemClickView(int pos, object object1)
        {
            itemPos = pos;
            transform = (Transform)object1;
            ShowLableView(); 
        }
        
        private void ShowLableView()
        {
            if (!loadSign)
                return;
            int showClickCount = 0;
            ItemInfo itemInfo = BagLogic.GetInstance().bagItems[itemPos];

            ////判断是否可以显示
            if (itemInfo.CanUse != 0)
            {
                ++showClickCount;
                useLabel.gameObject.SetActive(true);
            }
            else
            {
                useLabel.gameObject.SetActive(false);
            }

            //判断拆分是否显示
            if (itemInfo.CurNum > 1)
            {
                ++showClickCount;
                useallLabel.transform.localPosition
                    = new Vector3(useLabel.transform.localPosition.x,
                        useLabel.transform.localPosition.y - (showClickCount - 1) * space,
                        useallLabel.transform.localPosition.z);
                useallLabel.gameObject.SetActive(true);

                ++showClickCount;
                partLabel.transform.localPosition
                    = new Vector3(partLabel.transform.localPosition.x,
                useLabel.transform.localPosition.y - (showClickCount - 1) * (space),
                partLabel.transform.localPosition.z);
                partLabel.gameObject.SetActive(true);
            }
            else
            {
                useallLabel.gameObject.SetActive(false);
                partLabel.gameObject.SetActive(false);
            }


            //判断是否可以出售，丢弃
            //if (itemInfo.CanDrop != 0)
            //{
            ++showClickCount;
            saleLabel.transform.localPosition
                = new Vector3(saleLabel.transform.localPosition.x,
                    useLabel.transform.localPosition.y - (showClickCount - 1) * (space),
                    saleLabel.transform.localPosition.z);
            //}
            //else
            //{
            //    saleLabel.gameObject.SetActive(false);
            //}

            //展示
            ++showClickCount;
            showLabel.transform.localPosition = new Vector3(saleLabel.transform.localPosition.x,
                        useLabel.transform.localPosition.y - (showClickCount - 1) * (space),
                        saleLabel.transform.localPosition.z);

            //if (itemInfo.CanDrop != 0)
            //{
            ++showClickCount;
            dropLabel.transform.localPosition
                 = new Vector3(dropLabel.transform.localPosition.x,
                    useLabel.transform.localPosition.y - (showClickCount - 1) * (space),
                    dropLabel.transform.localPosition.z);
            //}
            //else
            //{
            //    dropLabel.gameObject.SetActive(false);
            //}

            //设置背景大小
            background.width = useLabel.width;
            background.height = (showClickCount - 1) * useLabel.height + 5;
            background.transform.localPosition = new Vector3(useLabel.transform.localPosition.x
                , useLabel.transform.localPosition.y + space, 0);

            uiPanel.transform.position = transform.position;
            uiPanel.transform.localPosition = new Vector3(uiPanel.transform.localPosition.x, uiPanel.transform.localPosition.y - 45, transform.transform.localPosition.z);

            Show();
        }
 

        //使用按钮
        private void OnUseItem(GameObject go)
        {
            ItemInfo itemInfo = BagLogic.GetInstance().bagItems[itemPos];
            if (itemInfo.Genre == (int)KItemGenre.igEquip)
            {
                EquipInfo equipInfo = itemInfo as EquipInfo;
                BagLogic.GetInstance().LoadEquip(itemPos, equipInfo.PutWhere);
            }
            else
            {
                BagLogic.GetInstance().SendUseItem(KPackageType.ePlayerPackage, KPlayerPackageIndex.eppiPlayerItemBox, itemPos);
            }
            Hide();
        }
        //批量使用
        private void OnUseItemByNum(GameObject go)
        {
            EventDispatcher.GameWorld.Dispath(ControllerCommand.OPEN_BAGITEMUSE_PANEL, new object[] { itemPos});
            Hide();
        }
        //拆分按钮
        private void OnPartItem(GameObject go)
        {
            EventDispatcher.GameWorld.Dispath(ControllerCommand.OPEN_BAGITEMPART_PANEL, new object[] { itemPos});
            Hide();
        }
        //卖出去按钮
        private void OnSaleItem(GameObject go)
        {
            EventDispatcher.GameWorld.Dispath(ControllerCommand.OPEN_BAGITEMSALE_PANEL, new object[] { itemPos, (int)ItemSaleType.eSaleToSys });
            Hide();

        }
        //丢弃按钮
        private void OnDropItem(GameObject go)
        {
            EventDispatcher.GameWorld.Dispath(ControllerCommand.OPEN_BAGITEMSALE_PANEL, new object[] { itemPos, (int)ItemSaleType.eDropToSys});
            Hide();
        }
    }

    public enum ItemSaleType
    {
        eDropToSys = 0,
        eSaleToSys = 1,
    }
}
