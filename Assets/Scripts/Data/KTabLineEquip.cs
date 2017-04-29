using System;
using System.Collections.Generic;
using Assets.Scripts.Lib.Resource;

namespace Assets.Scripts.Data
{
    public class KTabLineEquip : KTabLineItem
    {
        public int PutWhere;        //装备穿在身上的位置
		public string FBX;			//装备模型
		public string Fx;		    //装备攻击特效
        public int StrengthenUpLv;  //强化等级上限
        public int CanForge;        //是否可以洗练
        public int Endurance;       //耐久度
        public int PunchNum;        //初始开孔数
        public int MaxPunchNum;     //最大开孔数
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
		public int Step;            //当前的品阶
		  

        public override string getKey()
        {
            return base.getKey();
        }
    }
}
