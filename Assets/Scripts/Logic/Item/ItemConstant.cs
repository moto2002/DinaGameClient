using System;
using System.Collections.Generic;
using Assets.Scripts.Proto;

namespace Assets.Scripts.Logic.Item
{
    public class ItemConstant
    {
        private static int[] itemCDS;
        private static int[] pos;

        public const int TYPE_OTHER = 2;
        public const int TYPE_EQUIP = 1;

        public const int PUT_ARMET = 0;         //头盔
        public const int PUT_CHEST = 1;         //上衣
        public const int PUT_PANTS = 2;         //裤子
        public const int PUT_BANGLE = 3;        //护腕
        public const int PUT_BELT = 4;          //腰带
        public const int PUT_SHOES = 5;         //鞋子
        public const int PUT_PAD = 6;           //肩饰
        public const int PUT_NECKLACE = 7;      //项链
        public const int PUT_RING = 8;          //戒子
        public const int PUT_SACHET = 9;        //香囊
        public const int PUT_AMULET = 10;        //护身符
        public const int PUT_WEAPON = 11;       //武器
        public const int PUT_MASK = 12;         //面罩
        public const int PUT_WING = 13;         //翅膀
        public const int PUT_OUTDRESS1 = 14;    //外装1
        public const int PUT_OUTDRESS2 = 15;    //外装2
		//武器在第11位.
        public static string[] ROLE_CATEGORY_NAME = { "armet", "body", "pants", "bangle", "", "", "", "", "", "", "", "", "", "", "", "", "" };

        /**
		 * 装备颜色
		 *
		 */
        public const int COLOR_GRAY = 1; //白色
        public const int COLOR_GREEN = 2; //绿色
        public const int COLOR_BLUE = 3; //蓝色
        public const int COLOR_PURPLE = 4; //紫色
        public const int COLOR_ORANGE = 5; //橙色
        public const int COLOR_GOLD = 6; //金色
        public static string[] ITEM_QUALITY = { "普通的", "精良的", "稀有的", "完美的", "史诗的", "传说的" };
        public static string[] COLOR_VALUES = { "#e0e0e0", "#48eb00", "#005fff", "#ff3fa7", "#f76e09", "#FFD700" };
        public static int[] COLOR_VALUES2 = { 0xe0e0e0, 0x48eb00, 0x005fff, 0xff3fa7, 0xf76e09, 0xFFD700 };
        public static string[] COLOR_NAME = { "白色", "绿色", "蓝色", "紫色", "橙色", "金色" };

        private static void Init()
        {
            itemCDS[0] = 500;
            itemCDS[1] = 500;
            itemCDS[2] = 500;
            itemCDS[3] = 500;

            pos[0] = PUT_ARMET;
            pos[1] = PUT_CHEST;
            pos[2] = PUT_PANTS;
            pos[3] = PUT_BANGLE;
            pos[4] = PUT_BELT;
            pos[5] = PUT_SHOES;
            pos[6] = PUT_PAD;
            pos[7] = PUT_NECKLACE;
            pos[8] = PUT_RING;
            pos[9] = PUT_SACHET;
            pos[10] = PUT_AMULET;
            pos[11] = PUT_WEAPON;
            pos[12] = PUT_MASK;
            pos[13] = PUT_WING;
            pos[14] = PUT_OUTDRESS1;
            pos[15] = PUT_OUTDRESS2;
        }

        public static int GetCDS(int cdID)
        {
            if (cdID >= itemCDS.Length)
                return 0;
            return itemCDS[cdID];
        }

        public static int GetPositionByPutWhere(int putWhere)
        {
            int temp = -1;
            for (int i = 0; i < pos.Length; i++)
            {
                if (pos[i] == putWhere)
                    temp = putWhere;
            }

            return temp;
        }

        public static string GetNameByPutWhere(int putWhere)
        {
            switch (putWhere)
            {
                case PUT_ARMET:
                    return "头盔";
                case PUT_CHEST:
                    return "上衣";
                case PUT_PANTS:
                    return "裤子";
                case PUT_BANGLE:
                    return "护腕";
                case PUT_BELT:
                    return "腰带";
                case PUT_SHOES:
                    return "鞋子";
                case PUT_PAD:
                    return "肩饰";
                case PUT_NECKLACE:
                    return "项链";
                case PUT_RING:
                    return "戒子";
                case PUT_SACHET:
                    return "香囊";
                case PUT_AMULET:
                    return "护身符";
                case PUT_WEAPON:
                    return "武器";
                case PUT_MASK:
                    return "面罩";
                case PUT_WING:
                    return "翅膀";
                case PUT_OUTDRESS1:
                    return "外装1";
                case PUT_OUTDRESS2:
                    return "外装2";
            }

            return "";
        }

        public static ItemInfo WrapperItemVO(S2C_SYNC_ITEM vo)
        {
            ItemInfo itemVO;
            if (vo == null)
                return null;
            switch (vo.byTabType)
            {
                case TYPE_OTHER:
                    itemVO = new ItemInfo();
                    itemVO.Copy(vo);
                    return itemVO;
                case TYPE_EQUIP:
                    itemVO = new EquipInfo();
                    itemVO.Copy(vo);
                    return itemVO;
            }

            return null;
        }
    }
}
