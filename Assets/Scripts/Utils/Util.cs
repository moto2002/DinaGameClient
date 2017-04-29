using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Define;

namespace Assets.Scripts.Utils
{
    class Util
    {
		public static Dictionary<KAttributeType, string> atttibuteToText = new Dictionary<KAttributeType, string>{
			{KAttributeType.atMaxHP,"生命"},
			{KAttributeType.atMaxMP,"内力"},
			{KAttributeType.atAttack,"攻击"},
			{KAttributeType.atDefence,"防御"},
			{KAttributeType.atReflex,"身法"},
			{KAttributeType.atCrit,"暴击"},
			{KAttributeType.atCritHurt,"暴击伤害"},
			{KAttributeType.atReduceCrit,"无视暴击"},
			{KAttributeType.atReduceCritHurt,"暴击抵御"},
			{KAttributeType.atHpRecover,"生命回复"},
			{KAttributeType.atMpRecover,"内力回复"},
			{KAttributeType.atAttackSpeed,"攻击速度"},
			{KAttributeType.atReduceDamage,"受伤减免"},
			{KAttributeType.atReduceDefence,"穿透"},
			{KAttributeType.atMiss,"闪避"},
			{KAttributeType.atDamageMore,"必杀"},
			{KAttributeType.atDamageLess,"必杀抵御"},
			{KAttributeType.atDamageBack,"受伤反弹"},
			{KAttributeType.atAttackRecover,"吸血"},
			{KAttributeType.atExtDamage,"额外伤害"},
			{KAttributeType.atMoveSpeed,"移动速度"},
			{KAttributeType.atUpAttack,"攻击上限"},
		};

        public const string ADD_EXP = "获得经验：";
        public const string ADD_MONEY = "获得金币：";
        public const string DESCEND_MONEY = "消耗金币：";
        public const string ADD_COIN = "获得元宝：";
        public const string DESCEND_COIN = "消耗元宝：";
        public const string ADD_MENTER_POINT = "获得礼金：";
        public const string DESCEND_MENTER_POINT = "消耗礼金：";
        public const string ADD_EQUIP = "获得装备：";
        public const string USE_EQUIP = "使用装备：";
        public const string ADD_OTHER = "获得物品：";
        public const string USE_OTHER = "使用物品：";

        public static string formatSignedInteger(int integer)
		{
			return integer > 0 ? "+" + integer : integer.ToString();
		}

        public static int GetBit(int nBitIndex, char[] Byffer)
        {
            int     nResult    = 0;
            int     nByteIndex = nBitIndex / 8;
            int     nByteBit   = 8 - 1 - nBitIndex % 8;

            if (nByteIndex >= 0 && nByteBit >= 0)
            {
                char byValue = Byffer[nByteIndex];
                nResult = ((byValue >> nByteBit) & 0x1);
            }
            return nResult;
        }
		
		public static string GetAttributeText(KAttributeType attributeType)
		{
			string retString = atttibuteToText[attributeType];
			if (retString == null)
			{
				retString = "未知";
			}
			return retString;
		}
		
		public static int GetFightCalculate(Dictionary<KAttributeType, int> attributeDict)
		{
			int ret = 0;
			float fAttack = 0;
			float fDefence = 0;
			float fMaxHP = 0;
			float fMaxMP = 0;
			float fReflex = 0;
			float fCrit = 0;
			float fCritHurt = 0;
			float fReduceCritHurt = 0;
			float fHpRecover = 0;
			float fMpRecover = 0;
			float fAttackSpeed = 0;
			float fReduceDamage = 0;
			float fDamageMore = 0;
			float fDamageLess = 0;
			float fExtDamage = 0;
			float fReduceDefence = 0;
			float fReduceCrit = 0;
			float fMiss = 0;
			float fUpAttack = 0;
			float fAttackRecover = 0;
			float fDamageBack = 0;
			
			foreach(KeyValuePair<KAttributeType , int> tempData in attributeDict)
			{
				KAttributeType tempKey = tempData.Key;
				float tempValue = (float)tempData.Value;
				switch(tempKey)
				{
				case KAttributeType.atMaxHP:
					fMaxHP = tempValue;
					break;
				case KAttributeType.atMaxMP:
					fMaxMP = tempValue;
					break;
				case KAttributeType.atAttack:
					fAttack = tempValue;
					break;
				case KAttributeType.atDefence:
					fDefence = tempValue;
					break;
				case KAttributeType.atReflex:
					fReflex = tempValue;
					break;
				case KAttributeType.atCrit:
					fCrit = tempValue;
					break;
				case KAttributeType.atCritHurt:
					fCritHurt = tempValue;
					break;
				case KAttributeType.atReduceCrit:
					fReduceCrit = tempValue;
					break;
				case KAttributeType.atReduceCritHurt:
					fReduceCritHurt = tempValue;
					break;
				case KAttributeType.atHpRecover:
					fHpRecover = tempValue;
					break;
				case KAttributeType.atMpRecover:
					fMpRecover = tempValue;
					break;
				case KAttributeType.atAttackSpeed:
					fAttackSpeed = tempValue;
					break;
				case KAttributeType.atReduceDamage:
					fReduceDamage = tempValue;
					break;
				case KAttributeType.atReduceDefence:
					fReduceDefence = tempValue;
					break;
				case KAttributeType.atMiss:
					fMiss = tempValue;
					break;
				case KAttributeType.atDamageMore:
					fDamageMore = tempValue;
					break;
				case KAttributeType.atDamageLess:
					fDamageLess = tempValue;
					break;
				case KAttributeType.atDamageBack:
					fDamageBack = tempValue;
					break;
				case KAttributeType.atAttackRecover:
					fAttackRecover = tempValue;
					break;
				case KAttributeType.atExtDamage:
					fExtDamage = tempValue;
					break;
				case KAttributeType.atUpAttack:
					fUpAttack = tempValue;
					break;
				}
			}
			
			ret = (int)(fAttack
				+ fDefence 
				+ (fMaxHP*0.13)
				+ (fMaxMP*0.1) 
				+ (fReflex*0.2) 
				+ (fCrit*0.4) 
				+ (fCritHurt*0.45) 
				+ (fReduceCritHurt*0.45) 
				+ (fHpRecover*1.5) 
				+ (fMpRecover*2) 
				+ (fAttackSpeed * 120)
				+ (fReduceDamage * 100)
				+ (fDamageMore*1.2)
				+ (fDamageLess*1.2)
				+ (fExtDamage*1.1)
				+ (fReduceDefence*50)
				+ (fReduceCrit*0.9)
				+ (fMiss*0.4)
				+ (fUpAttack*0.6)
				+ (fAttackRecover*75)
				+ (fDamageBack*75));
			
			return ret;
		}
    }
}
