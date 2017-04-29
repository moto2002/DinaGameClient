using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Lib.Resource;

namespace Assets.Scripts.Data
{
    class KHeroLevelData : AKTabFileObject
    {
        public int Level = 0;                           //等级
        public int Experience = 0;                      //经验
        public int Hp = 0;
        public int HpReplenish = 0;
        public int HpeReplenishPercent = 0;
        public int Mp = 0;
        public int MpReplenish = 0;
        public int MpeReplenishPercent = 0;
        public int Angry = 0;
        public int MaxAngry = 0;
        public int AdditionalAddAngry = 0;

        public int AttackPoint = 0;                     //攻击力
        public int AttackPointPercent = 0;              //攻击力百分比
        public int Defense = 0;                         //防御
        public int DefensePercent = 0;                  //防御百分比
        public int Hit = 0;                             //命中
        public int HitPercent = 0;                      //命中百分比
        public int Dodge = 0;                           //命中
        public int DodgePercent = 0;                    //命中百分比
        public int CritPoint = 0;                       //暴击数值
        public int CritRate = 0;                        //暴击率
        public int Armor = 0;                           //护甲
        public int SunderArmor = 0;                     //破甲

        public int Power = 0;                           //力量
        public int Stamina = 0;                         //耐力
    //    public int Stamina = 0;                         //体魄
        public int Agility = 0;                         //敏捷

        public int MoveSpeed = 0;                       //移动速度
        public int AddMoveSpeedPercent = 0;             //添加移动速度百分比

        public override string getKey()
        {
            return "";
        }
    }
}
