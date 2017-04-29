using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Lib.Resource;
using Assets.Scripts.Define;

namespace Assets.Scripts.Data
{
    public class KActiveSkill : AKTabFileObject
    {
        public uint SkillID;
        public uint Level;
        public string Text;
        public string Desc;
        public int LearnLevel;
        public bool CanLevelUp;                         //是否能升级
        public int NeedSkillID;                         //需要前置技能ID
        public int NeedSkillLevel;                      //需要前置技能级别
        public int MaxSkillExp;                         //最大升级技能经验
        public int CostMoney;                           //释放消耗钱
        public int CostMagic;                           //消耗魔法
        public int CostAnger;                           //消耗怒气
        public int CastRange;                           //使用距离
		public int CastMinRange;						//最小施法距离
        public KSkillTargetType SkillType;              //技能类型
        public bool SpellChannel;                       //是否持续是否 
        public bool SpellRun;                           //是否可以跑动施法  
        public float SpellTime;                           //施法时间  
        public float StiffTime;                           //硬直时间 不能放其他技能 不能移动
        public float SkillCD;                           //CD 时间
        public int SkillFlySpeed;                       //技能飞行速度
        public int DamageValue;                         //技能伤害系数
        public int ExDamage;                            //附件伤害
        public int IgnoreDef;                           //无视防御值
        public KEffectTarget EffectTarget;              //敌人 自己 友军 全体
        public KEffectArea EffectArea;                  //技能作用区域 圆形 方形
        public int EffectRange;                         //区域大小
        public int EffectAngle;                         //区域角度
        public int AttachBufSelf;                       //攻击buff敌人
        public int AttachBuffEnemy;
        public int AttachBuffTeammate;
        public bool ForceAttackSelf;                    //是否强目标迫攻击自己
        public int TriggerSkill;                        //触发其他技能
        public int TriggerSkillRate;                    //触发几率
        public int Job;
        public int Index;
        public int AttackAction;                        //攻击动作
        public bool AttackEffectDiff;                   //攻击效果不同
        public string AttackEffect;                     //攻击效果
        public bool HitterEffectDiff;                   //受击效果不同
        public string HitterEffect;                     //受击效果
		public string AttackTimeList;					//技能段数  浮点数递增序列，为技能触发时间序列，如0.6,0.8,0.9,1.5
		public bool ClientCache;						//客户端表格读取
        public float AllSkillCD;							//公共cd.
		public override string getKey()
        {
            return SkillID + "_" + Level;
        }
    }
}
