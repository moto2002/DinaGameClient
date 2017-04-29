using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.View.Drag;
using Assets.Scripts.Logic.Item;
using Assets.Scripts.View.Bag;
using Assets.Scripts.Logic.Bag;
using Assets.Scripts.Manager;

namespace Assets.Scripts.View.Role
{
    [RequireComponent(typeof(UIButtonScale))]
    public class EquipItem : MonoBehaviour
    {
        public const string DRAG_TYPE = "EQUIP_TYPE";
        public UISprite background;
        public UISprite icon = null;
        public UILabel label;
        public int slot = 0;
        private DragItem drag = null;
        private int putWhere;

        public int PutWhere
        {
            get
            {
                return putWhere;
            }
            set
            {
                if (putWhere != value)
                {
                    putWhere = value;
                }
            }
        }

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

            icon = NGUITools.AddChild<UISprite>(gameObject);
            icon.transform.localScale = new Vector3(40, 40, 1);
            icon.atlas = atlas;
            icon.spriteName = "icon11";

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
            if(go == null)
                return;

            IDragable dragObj = go.GetComponent<DragItem>();
            if (dragObj == null)
                return;
            string type = dragObj.DragType;
            if(type.Equals(DRAG_TYPE))
            {

            }
            else if(type.Equals(BagItem.DRAG_TYPE))
            {
                if (DragItem.mDraggedItem != null)
                {
                    EquipInfo equipVo = DragItem.mDraggedItem as EquipInfo;
                    if (equipVo.PutWhere == PutWhere)
                    {
                        BagLogic.GetInstance().LoadEquip(equipVo.Position, equipVo.PutWhere);
                    }
                }
            }
            else
            {
                return;
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
