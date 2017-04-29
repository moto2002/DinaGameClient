using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Lib.Resource;
using Assets.Scripts.Define;

namespace Assets.Scripts.Data
{
	/// <summary>
	/// 装备强化表  2014.03.12 by linfeng
	/// </summary>
	public class KEquipStrengthen : AKTabFileObject
	{
		public int nEquipID; //ID
		public string EquipName; //名字
		public int nLevel; // 强化等级
		public int nMoney; // 强化需要的金钱
		public string Attribute; // 属性加成
		public Dictionary<KAttributeType , int> AttributeDict;

		public override string getKey ()
		{
			return nEquipID + "_" + nLevel;
		}
		
		public Dictionary<KAttributeType , int> SplitValue (string attributeStr)
		{
			Dictionary<KAttributeType , int> dict = new Dictionary<KAttributeType , int>();
			string[] propertyDoc = attributeStr.Split('|');
			foreach (string str in propertyDoc)
			{
				string[] propertyNode = str.Split(':');
				if(propertyNode.Length > 1)
				{
					string propertyName = propertyNode[0].ToString();
					int propertyValue = int.Parse(propertyNode[1]);
					KAttributeType tempType = (KAttributeType)Enum.Parse(typeof(KAttributeType),propertyName);
					dict[tempType] = propertyValue;
				}
				else
					throw new Exception("strengthen.tab属性内容编辑错误");
			}
			return dict;
		}
		
		public override void onAllComplete()
        {
			AttributeDict = SplitValue(Attribute);
        }
	}
}

