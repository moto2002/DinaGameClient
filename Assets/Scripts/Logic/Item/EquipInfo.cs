using System;
using System.Collections.Generic;
using Assets.Scripts.Data;

namespace Assets.Scripts.Logic.Item
{
    public class EquipInfo : ItemInfo
    {
        public int PutWhere;        //装备穿在身上的位置
        public int StrengthenUpLv;  //强化等级上限
        public int CurStrengthenLv; //当前强化等级
        public int CanForge;        //是否可以洗练
        public int Endurance;       //耐久度
        public int CurEndurance;    //当前耐久度
        public int PunchNum;        //初始开孔数
        public int MaxPunchNum;     //最大开孔数
        public int CurPunchNum;     //当前开孔数
        public int Attack;          //攻击力
        public int Defence;         //防御力
        public int Hp;              //生命值
        public int Mp;              //魔法值
        public int MoveSpeed;       //移动速度
        public int Miss;            //闪避
        public int Hit;             //命中`
        public int Crit;            //暴击
        public int ReduceCrit;      //抗暴击
        public int CritHurt;        //暴击伤害
        public int Armor;           //护甲
        public int ReduceArmor;     //破甲
        public int AttackSpeed;     //攻击速度
        public int ShowID;          //外观ID
		public int ReqJob;          //职业限制
		public int ReqSex;          //性别限制
		public string FBX;          //fbx路径

        public override int typeId
        {
            get
            {
                return base.typeId;
            }
            set
            {
                if (_typeId != value)
                {
                    _typeId = value;
					KTabServerEquip itemProperty = ItemLocator.GetInstance().GetEquipProperty(typeId);
					KTabClientEquip itemView = ItemLocator.GetInstance().GetEquipView(typeId);
					ReqJob = itemProperty.ReqJob;
					ReqSex = itemProperty.ReqSex;
					Tips = itemProperty.Desc;
                    ID = itemProperty.ID;
                    Name = itemProperty.Name;
                    Genre = itemProperty.Genre;
                    SubType = itemProperty.SubType;
                    Quality = itemProperty.Quality;
					PutWhere = itemProperty.SubType;
                    StrengthenUpLv = itemProperty.MaxStrengthen;
					Icon = "icon" + itemView.nIcon;
					FBX = itemView.FBX;
//                    OverdueTime = item.OverdueTime;
//                    OverduePoint = item.OverduePoint;
//                    Icon = item.Icon;
//                    CanTrade = item.CanTrade;
//                    CanDestroy = item.CanDestroy;
//                    CanStack = item.CanStack;
//                    MaxStackNum = item.MaxStackNum;
//                    BuyPrice = item.BuyPrice;
//                    SellPrice = item.SellPrice;
//                    MallPrice = item.MallPrice;
//                    MallBindPrice = item.MallBindPrice;
//                    RequireLevel = item.RequireLevel;
//                    RequireGender = item.RequireGender;
//                    CoolDownID = item.CoolDownID;
//                    DestroyAfterUse = item.DestroyAfterUse;
//                    SellNotify = item.SellNotify;
//                    RequireJob = item.RequireJob;
//                    CanUse = item.CanUse;
//                    IsBroadcast = item.IsBroadcast;
//                    UIID = item.UIID;
//                    CanForge = item.CanForge;
//                    Endurance = item.Endurance;
//                    PunchNum = item.PunchNum;
//                    MaxPunchNum = item.MaxPunchNum;
//                    Attack = item.Attack;        
//                    Defence = item.Defence;
//                    Hp = item.Hp;
//                    Mp = item.Mp;
//                    MoveSpeed = item.MoveSpeed;
//                    Miss = item.Miss;
//                    Hit = item.Hit;
//                    Crit = item.Crit;
//                    ReduceCrit = item.ReduceCrit;
//                    CritHurt = item.CritHurt;
//                    Armor = item.Armor;
//                    ReduceArmor = item.ReduceArmor;
//                    AttackSpeed = item.AttackSpeed;
//                    ShowID = item.ShowID;
                }	
            }
        }

        public override void Copy(Proto.S2C_SYNC_ITEM vo)
        {
            base.Copy(vo);
            CurStrengthenLv = (int)vo.uStrengthenLevel;
            //CurEndurance = (int)vo.currentDurability;
            CurPunchNum = (int)vo.uHole;
        }
    }
}
