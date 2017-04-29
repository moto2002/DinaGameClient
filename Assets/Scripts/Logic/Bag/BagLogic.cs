using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Proto;
using Assets.Scripts.Lib.Net;
using Assets.Scripts.Logic.Item;
using Assets.Scripts.Model.Scene;
using Assets.Scripts.View;
using Assets.Scripts.Manager;
using Assets.Scripts.Model.Player;
using Assets.Scripts.Logic.Scene.SceneObject;
using Assets.Scripts.View.Scene;
using Assets.Scripts.Logic.Scene;
using Assets.Scripts.Define;
using NetMessage;
using ProtoBuf;

namespace Assets.Scripts.Logic.Bag
{
    public class BagLogic : BaseLogic
    {
        public static int MAIN_BAG_ROWS = 4;
        public static int MAIN_BAG_COLUMNS = 7;
        public static int MIAN_BAG_COUNT = MAIN_BAG_ROWS * MAIN_BAG_COLUMNS;

        public bool inited = false;
        public int currentBagSize = 0;
        public ItemInfo[] bagItems = new ItemInfo[28];
        public ItemInfo[] equipItems = new ItemInfo[16];

        private static BagLogic instance;
        public static BagLogic GetInstance()
        {
            if (instance == null)
                instance = new BagLogic();
            return instance;
        }

        protected override void Init()
        {

        }

        protected override void InitListeners()
        {
            RegistSocketListener(KS2C_Protocol.s2c_sync_item, OnSyncAddItem, typeof(S2C_SYNC_ITEM));
            RegistSocketListener(KS2C_Protocol.s2c_update_item_amount, OnUpdateItem, typeof(S2C_UPDATE_ITEM_AMOUNT));
            RegistSocketListener(KS2C_Protocol.s2c_destroy_item, OnDestroyItem, typeof(S2C_DESTROY_ITEM));
            RegistSocketListener(KS2C_Protocol.s2c_use_item, OnUseItem, typeof(S2C_USE_ITEM));
            RegistSocketListener(KS2C_Protocol.s2c_exchange_item, OnExchangeItem, typeof(S2C_EXCHANGE_ITEM));
            RegistSocketListener(KS2C_Protocol.s2c_sync_equips, OnSyncEquips, typeof(S2C_SYNC_EQUIPS));
            RegistSocketListener(KS2C_Protocol.s2c_package_size, OnPackageSize, typeof(S2C_PACKAGE_SIZE));
            RegistSocketListener(KS2C_Protocol.s2c_sale_item_to_sys, OnSaleItemToSysRespond, typeof(S2C_SALE_ITEM_TO_SYS));
        }

        public void SendUseItem(KPackageType ePackageType, KPlayerPackageIndex ePackageIndex, int pos)
        {
            C2S_USE_ITEM msg = new C2S_USE_ITEM();
            msg.protocolID = (byte)KC2S_Protocol.c2s_use_item;
            msg.byPackageType = (ushort)ePackageType;
            msg.byPackageIndex = (ushort)ePackageIndex;
            msg.byPos = (ushort)pos;
            SendMessage(msg);

        }

        public void SendPartItemRequest(KPackageType ePackageType, KPlayerPackageIndex ePackageIndex, int pos, int stackNum)
        {
            C2S_PART_ITEM msg = new C2S_PART_ITEM();
            msg.protocolID = (byte)KC2S_Protocol.c2s_part_item;
            msg.byPackageType = (ushort)ePackageType;
            msg.byPackageIndex = (ushort)ePackageIndex;
            msg.byPos = (ushort)pos;
            msg.stackNum = (ushort)stackNum;
            SendMessage(msg);
        }

        public void SendUseItemByNum(KPackageType ePackageType, KPlayerPackageIndex ePackageIndex, int pos, int stackNum)
        {
            C2S_USE_ITEM_BY_NUM msg = new C2S_USE_ITEM_BY_NUM();
            msg.protocolID = (byte)KC2S_Protocol.c2s_use_item_by_num;
            msg.byPackageType = (ushort)ePackageType;
            msg.byPackageIndex = (ushort)ePackageIndex;
            msg.byPos = (ushort)pos;
            msg.stackNum = (ushort)stackNum;
            SendMessage(msg);
        }

        public void SendSaleItem(KPackageType ePackageType, KPlayerPackageIndex ePackageIndex, int pos)
        {
            C2S_SALE_ITEM_TO_SYS msg = new C2S_SALE_ITEM_TO_SYS();
            msg.protocolID = (byte)KC2S_Protocol.c2s_sale_item_to_sys;
            msg.byPackageType = (ushort)ePackageType;
            msg.byPackageIndex = (ushort)ePackageIndex;
            msg.byPos = (ushort)pos;
            SendMessage(msg);
        }

        public void SendSortItem(KPlayerPackageIndex ePackageIndex, ArrayList itemList)
        {
            ItemInfo info = null;
            C2S_SORT_OUT_ITEM msg = new C2S_SORT_OUT_ITEM();
            msg.protocolID = (byte)KC2S_Protocol.c2s_sort_out_item;
            msg.byPackageIndex = (ushort)ePackageIndex;

            KNMBagItemPositionDataList positionList = new KNMBagItemPositionDataList();
            for (int i = 0; i < itemList.Count; i++)
            {
                info = (ItemInfo)itemList[i];
                if(info == null)
                    break;
                positionList.ItemPositionList.Add(info.Position);
            }
            Serializer.Serialize<KNMBagItemPositionDataList>(msg.itemPositionDataList, positionList);
            SendMessage(msg);
        }


        public void LoadEquip(int pos, int targetPos)
        {
            SendExChangeItem((ushort)pos, (ushort)targetPos, 1, 2);
        }

        public void UnLoadEquip(int pos, int targetPos)
        {
            SendExChangeItem((ushort)pos, (ushort)targetPos, 2, 1);
        }

        public void ExChangeItem(int pos, int targetPos)
        {
            SendExChangeItem((ushort)pos, (ushort)targetPos, 1, 1);
        }

        public void SendExChangeItem(ushort pos, ushort targetPos, ushort srcType, ushort targetType)
        {
            C2S_EXCHANGE_ITEM msg = new C2S_EXCHANGE_ITEM();
            msg.protocolID = (byte)KC2S_Protocol.c2s_exchange_item;
            msg.bySrcPackageType = srcType;
            msg.bySrcPackageIndex = 0;
            msg.bySrcPos = pos;
            msg.byTargetPackageType = targetType;
            msg.byTargetPackageIndex = 0;
            msg.byTargetPos = targetPos;
            SendMessage(msg);
        }

        private void OnSyncAddItem(KProtoBuf buf)
        {
            S2C_SYNC_ITEM respond = buf as S2C_SYNC_ITEM;
            ItemInfo item = ItemConstant.WrapperItemVO(respond);
            if (respond.byPackageType == 1 && respond.byPackageIndex == 0)
            {
                UpdateGoods(item.Position, item);
				if (item.Genre == (int)KItemTableType.ittEquip)
				{
					if (IsBetterEquip((EquipInfo)item))
					{
						EventDispatcher.GameWorld.Dispath(ControllerCommand.OPEN_CHANGEEQUIP_PANEL, item);
					}
				}
            }
            else if (respond.byPackageType == 2 && respond.byPackageIndex == 0)
            {
                UpdateEquip(item.Position, item);
            }
            EventDispatcher.GameWorld.Dispath(ControllerCommand.ADD_ITEM, item);
        }

        private void OnUpdateItem(KProtoBuf buf)
        {
            S2C_UPDATE_ITEM_AMOUNT respond = buf as S2C_UPDATE_ITEM_AMOUNT;
            if (respond.byPackageType == 1 && respond.byPackageIndex == 0)
            {
                ItemInfo item = GetItemByPos(respond.byPos);
                if (item != null)
                {
                    item.CurNum = (int)respond.dwStack;
                    UpdateGoods(item.Position, item);
                }
            }
        }

        private void OnDestroyItem(KProtoBuf buf)
        {
            S2C_DESTROY_ITEM respond = buf as S2C_DESTROY_ITEM;
            if (respond.byPackageType == 1 && respond.byPackageIndex == 0)
            {
                RemoveGoods(respond.byPos);
            }
            else if (respond.byPackageType == 2 && respond.byPackageIndex == 0)
            {
                RemoveEquip(respond.byPos);
            }
        }

        private void OnUseItem(KProtoBuf buf)
        {
            S2C_USE_ITEM respond = buf as S2C_USE_ITEM;
            if (respond.bResult != 0)
            {

            }
        }

        private void OnExchangeItem(KProtoBuf buf)
        {
            S2C_EXCHANGE_ITEM respond = buf as S2C_EXCHANGE_ITEM;
            if (respond.bResultCode == 1)
            {
                //如果成功

            }
            else if (respond.bResultCode == 0)
            {
                //进行错误提示
            }
        }

        private void OnSyncEquips(KProtoBuf buf)
        {
            S2C_SYNC_EQUIPS respond = buf as S2C_SYNC_EQUIPS;
            SceneEntity hero = SceneLogic.GetInstance().GetSceneObject(respond.uHeroID);
            hero.DispatchEvent(ControllerCommand.EQUIP_CHANGE, respond.equips);
        }

        private void OnPackageSize(KProtoBuf buf)
        {
            S2C_PACKAGE_SIZE respond = buf as S2C_PACKAGE_SIZE;
            currentBagSize = respond.byPackageSize;
            bagItems = new ItemInfo[currentBagSize];
        }
        private void OnSaleItemToSysRespond(KProtoBuf buf)
        {
            S2C_SALE_ITEM_TO_SYS respond = buf as S2C_SALE_ITEM_TO_SYS;
        }

        private void SetGoods(ItemInfo[] items)
        {
            bagItems = items;
            if (!inited)
            {
                inited = true;
            }
        }

        private void UpdateGoods(int pos, ItemInfo item)
        {
            int deleteId = -1;
            if (item != null && item.CurNum == 0)
            {
                deleteId = item.ServerID;
                item = null;
            }
            if (item == null)
            {
                ItemInfo distoryItem = bagItems[pos];
                if (distoryItem != null && distoryItem.ServerID != deleteId && deleteId != -1)
                    return;
            }
            if(item != null)
                item.Position = pos;
            bagItems[pos] = item;
            Dispatch(ControllerCommand.UPDATE_BAG_GOODS, pos);
        }

        private void RemoveGoods(int pos)
        {
            ItemInfo item = GetItemByPos(pos);
            if (item != null)
                UpdateGoods(item.Position, null);
        }

        public ItemInfo GetItemByPos(int pos)
        {
            foreach (ItemInfo item in bagItems)
            {
                if (item != null && item.Position == pos)
                    return item;
            }
            return null;
        }
		
		public ItemInfo GetItemByType(int subType)
		{
			foreach (ItemInfo item in bagItems)
			{
				log.Debug("Name" + item.Name);
				log.Debug("typeId" + item.typeId);
				if(item != null && item.typeId == subType)
					return item;
			}
			return null;
		}

        public bool IsBagFull()
        {
            int len = bagItems.Length;
            for (int i = 0; i < len; i++)
            {
                if (bagItems[i] == null)
                    return false;
            }
            return true;
        }

        public int GetNullPosition()
        {
            int len = bagItems.Length;
            for (int i = 0; i < len; i++)
            {
                if (bagItems[i] == null)
                    return i;
            }
            return -1;
        }

        public void SetEquips(ItemInfo[] items)
        {
            equipItems = items;
        }

        public void UpdateEquip(int pos, ItemInfo equip)
        {
            equipItems[pos] = equip;
            Dispatch(ControllerCommand.UPDATE_ROLE_EQUIP, pos);
        }

        public void RemoveEquip(int pos)
        {
            ItemInfo equip = GetEquipByPos(pos);
            if (equip != null)
                UpdateEquip(equip.Position, null);
        }

        public ItemInfo GetEquipByPos(int pos)
        {
            foreach (ItemInfo equip in equipItems)
            {
                if (equip != null && equip.Position == pos)
                {
                    return equip;
                }
            }
            return null;
        }
		
		
		
         //对背包进行整理
        public void BagItemSort()
        {
            ArrayList list = new ArrayList();
            for (int i = 0; i < bagItems.Length; i++)
            {
                list.Insert(i, bagItems[i]);
            }   
            BagItemSortComparer iComparers = new BagItemSortComparer();
            list.Sort(iComparers);
            list.Reverse();
            ItemInfo info = null;

            //将该位置同步到服务端(因为info的旧位置会在UpdateGoods(i, info)方法中改动)
            SendSortItem(KPlayerPackageIndex.eppiPlayerItemBox, list);
            //对背包中的道具进行移位置
            for (int i = 0; i < list.Count; i++)
            {
                info = (ItemInfo)list[i];
                UpdateGoods(i, info);
            }
            
        }
		
		protected bool IsBetterEquip(EquipInfo equipInfo)
		{
			return true;
		}
    }

    //道具整理比较器
    public class BagItemSortComparer : System.Collections.IComparer
    {
        public int Compare(Object object1, Object object2)
        {
            int compareResult = 0;
            if (object1 == null)
                return -1;
            else if (object2 == null)
                return 1;
            ItemInfo item1 = (ItemInfo)object1;
            
            ItemInfo item2 = (ItemInfo)object2;
            compareResult = item2.Genre.CompareTo(item1.Genre);

            return (compareResult == 0 ? item2.ID.CompareTo(item1.ID) : compareResult);
        }
    }
}
