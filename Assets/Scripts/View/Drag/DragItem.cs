using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Logic.Item;
using Assets.Scripts.Manager;

namespace Assets.Scripts.View.Drag
{
    public class DragItem : MonoBehaviour, IDragable
    {
        public delegate void DragHandler(Vector2 delta);
        public delegate void DropHandler(GameObject go);
        public delegate void ToolTipHandler(bool show);

        public event DragHandler DragEvent = null;
        public event DropHandler DropEvent = null;
        public event ToolTipHandler ToolTipEvent = null;

        protected UISprite dragIcon = null;
        protected IDragInfo dragVO = null;
        public static IDragInfo mDraggedItem;
        public static bool isDragging = false;
        protected string dragType = "DRAG";


        public virtual string DragType
        {
            get { return dragType; }
            set { dragType = value; }
        }

        public UISprite DragIcon
        {
            get { return dragIcon; }
            set
            {
                if (value != dragIcon)
                {
                    dragIcon = value;
                }
            }
        }

        public IDragInfo DragItemVO
        {
            get { return dragVO; }
            set
            {
                if (dragVO != value)
                {
                    dragVO = value;
                }
            }
        }
        
        public void OnTooltip(bool show)
        {
            if (ToolTipEvent != null)
            {
                ToolTipEvent(show);
            }
        }

        public void OnClick()
        {
            if (mDraggedItem != null)
            {
                OnDrop(null);
            }
        }

        public void OnDrag(Vector2 delta)
        {
            if (mDraggedItem == null && DragItemVO != null)
            {
                UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
                mDraggedItem = DragItemVO;
                UpdateCursor();
            }
            isDragging = true;
            if (DragEvent != null)
            {
                DragEvent(delta);
            }
            
        }

        public void OnDrop(GameObject go)
        {
            if (DropEvent != null)
            {
                DropEvent(go);
            }

            mDraggedItem = null;
            isDragging = false;
            UpdateCursor();
        }

        public void UpdateCursor()
        {
            if (mDraggedItem != null)
            {
                CursorManager.GetInstance().SetDragingCur(dragIcon.atlas, mDraggedItem.DragIcon,DrawDataType.ITEM);
            }
            else
            {
                //UICursor.Clear();
                CursorManager.GetInstance().ClearDragCursor();
            }
        }
    }
}
