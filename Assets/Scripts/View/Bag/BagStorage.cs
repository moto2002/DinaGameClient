using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Logic.Item;
using Assets.Scripts.View.Drag;
using Assets.Scripts.Logic.Bag;
using Assets.Scripts.Define;
using Assets.Scripts.Manager;

namespace Assets.Scripts.View.Bag
{
    public class BagStorage : MonoBehaviour
    {
        public int maxItemCount = 28;
        public int maxRows = 4;
        public int maxColumns = 7;
        public GameObject template;

        public UIWidget background;
        public int spacing = 48;
        public int padding = 10;

        protected BagItem[] bagItems = new BagItem[28];

        //List<ItemVO> mItems = new List<ItemVO>();
        protected ItemInfo[] mItems = new ItemInfo[28];

        public ItemInfo GetItem(int slot)
        {
            return (slot < mItems.Length) ? mItems[slot] : null;
        }

        public void UpdateGoods(int slot, ItemInfo item)
        {
            mItems[slot] = item;
			if (slot < bagItems.Length)
			{
            	bagItems[slot].UpdateContent(item);
			}
        }

        public void SetGoods(ItemInfo[] items)
        {
            mItems = items;
            for (int i = 0; i < mItems.Length; ++i)
            {
                UpdateGoods(i, mItems[i]);
            }
        }

        public void CreateItems()
        {
            int count = 0;
            Bounds b = new Bounds();

            for (int y = 0; y < maxRows; ++y)
            {
                for (int x = 0; x < maxColumns; ++x)
                {
                    BagItem slot = NGUITools.AddChild<BagItem>(gameObject);
                    GameObject go = slot.gameObject;
                    NGUITools.AddWidgetCollider(go);
                    Transform t = go.transform;
                    t.localPosition = new Vector3(padding + (x + 0.5f) * spacing, -padding - (y + 0.5f) * spacing, 0f);

                    UIEventListener.Get(slot.gameObject).onDoubleClick += OnDoubleClickItem;
                    //增加一个单击消息处理
                    UIEventListener.Get(slot.gameObject).onClick += OnClickItem;
                    slot.slot = count;
                    bagItems[count] = slot;

                    b.Encapsulate(new Vector3(padding * 2f + (x + 1) * spacing, -padding * 2f - (y + 1) * spacing, 0f));

                    if (++count >= maxItemCount)
                    {
                        if (background != null)
                        {
                            background.transform.localScale = b.size;
                        }
                        return;
                    }
                }
            }
            if (background != null) background.transform.localScale = b.size;
        }

        private void OnDoubleClickItem(GameObject go)
        {
            IDragable item = go.GetComponent<DragItem>();
            ItemInfo itemVO = item.DragItemVO as ItemInfo;
            if (itemVO != null)
            {
                if (itemVO.Genre == (int)KItemGenre.igEquip)
                {
                    EquipInfo equipVo = itemVO as EquipInfo;
                    BagLogic.GetInstance().LoadEquip(equipVo.Position, equipVo.PutWhere);
                }
                else if (itemVO.Genre == (int)KItemGenre.igCommon)
                {
                    //BagLogic.GetInstance().SendUseItem(KPackageType.ePlayerPackage, KPlayerPackageIndex.eppiPlayerItemBox, itemVO.Position);
                }
                item.DragIcon.enabled = false;
            }
        }
        private void OnClickItem(GameObject go)
        {
            IDragable item = go.GetComponent<DragItem>();
            ItemInfo itemVO = item.DragItemVO as ItemInfo;
            if (itemVO != null)
            {
                EventDispatcher.GameWorld.Dispath(ControllerCommand.OPEN_BAGITEMCLICK_PANEL, new object[]{itemVO.Position, go.transform});
            }
        }
    }
}
