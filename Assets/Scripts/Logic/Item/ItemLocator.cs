using System;
using System.Collections.Generic;
using Assets.Scripts.Data;
using Assets.Scripts.Lib.Log;
using Assets.Scripts.Define;
using Assets.Scripts.Lib.Loader;

namespace Assets.Scripts.Logic.Item
{
    public class ItemLocator
    {
        private static Logger log = LoggerFactory.GetInstance().GetLogger(typeof(ItemLocator));
        private static ItemLocator instance;
        public static ItemLocator GetInstance()
        {
            if (instance == null)
                instance = new ItemLocator();
            return instance;
        }

        public ItemLocator()
        {
        }

        public KTabLineItem GetItem(KItemGenre type, int typeId)
        {
            switch (type)
            {
                case KItemGenre.igEquip:
                    return GetEquip(typeId);
                case KItemGenre.igCommon:
                    return GetOtherItem(typeId);
            }

            return null;
        }

        public KTabLineItem GetOtherItem(int typeId)
        {
            KTabLineItem item = KConfigFileManager.GetInstance().itemTabInfos.getData(typeId.ToString());
            if (item == null)
                log.Error("查询道具ID为：" + typeId + "物品报错");

            return item;
        }

        public KTabLineEquip GetEquip(int typeId)
        {
            KTabLineEquip item = KConfigFileManager.GetInstance().equipTabInfos.getData(typeId.ToString());
            if (item == null)
                log.Error("查询道具ID为：" + typeId + "装备报错");

            return item;
        }
		
		public KTabClientEquip GetEquipView (int typeId)
		{
			KTabClientEquip item = KConfigFileManager.GetInstance().equipClientTab.getData(typeId.ToString());
			if (item == null)
                log.Error("查询道具ID为：" + typeId + "装备报错");

            return item;
		}
		
		public KTabServerEquip GetEquipProperty (int typeId)
		{
			KTabServerEquip item = KConfigFileManager.GetInstance().equipServerTab.getData(typeId.ToString());
			if (item == null)
                log.Error("查询道具ID为：" + typeId + "装备报错");

            return item;
		}

        public KTabLineShowInfo GetEquipShowInfo(int showId)
        {
            KTabLineShowInfo item = KConfigFileManager.GetInstance().showTabInfos.getData(showId.ToString());
            if (item == null)
                log.Error("查询装备显示ID为：" + showId + "套装报错");

            return item;
        }

        public ItemInfo GetItemVO(int typeId, byte type)
        {
            ItemInfo itemVo = null;
            switch (type)
            {
                case ItemConstant.TYPE_OTHER:
                    itemVo = new ItemInfo();
                    break;
                case ItemConstant.TYPE_EQUIP:
                    itemVo = new EquipInfo();
                    break;
            }
            if (itemVo != null)
			{
                itemVo.typeId = typeId;
			}
            return itemVo;
        }
    }
}
