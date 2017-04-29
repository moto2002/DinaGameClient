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

namespace Assets.Scripts.View.Bag
{
    [RequireComponent(typeof(UIButtonScale))]
	public class BagItem : MonoBehaviour
	{
        public const string DRAG_TYPE = "BAG_TYPE";
        public UISprite background;
        public UISprite icon = null;
        public UILabel label;
        public int slot = 0;
        private DragItem drag = null;

        void Awake()
        {
            ConfigUI();
        }

        protected void ConfigUI()
        {
            background = NGUITools.AddChild<UISprite>(gameObject);
            background.transform.localScale = new Vector3(48, 48, 1);
            UIAtlas atlas = UIAtlasManager.GetInstance().GetUIAtlas("IconAtlas");
            background.atlas = atlas;
            background.type = UISprite.Type.Sliced;
            background.spriteName = "ItemBg";
            background.depth = 50;

            icon = NGUITools.AddChild<UISprite>(gameObject);
            icon.transform.localScale = new Vector3(40, 40, 1);
            icon.atlas = atlas;
            icon.spriteName = "icon11";
            icon.depth = 51;

            drag = gameObject.AddComponent<DragItem>();
            drag.DragIcon = icon;
            drag.DragType = DRAG_TYPE;
            drag.DropEvent += DoDrop;
            drag.ToolTipEvent += DoToolTip;
        }

        public void UpdateContent(ItemInfo itemVO)
        {
            if (itemVO == null)
            {
                drag.DragIcon.enabled = false;
            }
            else
            {
                drag.DragIcon.enabled = true;
                drag.DragIcon.spriteName = itemVO.Icon;
            }
            drag.DragItemVO = itemVO;

        }

        public void DoDrop(UnityEngine.GameObject go)
        {
            if (go == null)
                return;

            IDragable dragObj = go.GetComponent<DragItem>();
            string type = dragObj.DragType;
            if (type.Equals(DRAG_TYPE))
            {
                if (DragItem.mDraggedItem != null)
                {
                    BagLogic.GetInstance().ExChangeItem((DragItem.mDraggedItem as ItemInfo).Position, slot);
                }
            }
            else if (type.Equals(EquipItem.DRAG_TYPE))
            {
                if (DragItem.mDraggedItem != null)
                {
                    //int target = BagManager.GetInstance().GetNullPosition();
                    int target = BagLogic.GetInstance().GetNullPosition();
                    if(target != -1)
                    {
                        BagLogic.GetInstance().UnLoadEquip((DragItem.mDraggedItem as EquipInfo).PutWhere, slot);
                    }
                    else
                    {
                        Debug.Log("背包已满");
                    }
                }
            }
        }

        public void DoToolTip(bool show)
        {
            if (show && drag.DragItemVO != null)
            {
                TooltipsManager.GetInstance().Show(drag.DragItemVO as ItemInfo);
            }
            else
            {
                TooltipsManager.GetInstance().Hide();
            }
        }
	}
}
