using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Lib.Resource;
using Assets.Scripts.Define;


namespace Assets.Scripts.Data
{
	public class KTabEquipSuit : AKTabFileObject
	{
		public int nStep;
		public string Suit3;
		public string Suit6;
		public string Suit12;
		
		
		public Dictionary<KAttributeType , int> AttributeDict3;
		public Dictionary<KAttributeType , int> AttributeDict6;
		public Dictionary<KAttributeType , int> AttributeDict12;
		
		public override string getKey ()
		{
			return nStep.ToString();
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
		
		public override void onAllComplete ()
		{
			base.onAllComplete ();
			AttributeDict3 = SplitValue(Suit3);
			AttributeDict6 = SplitValue(Suit6);
			AttributeDict12 = SplitValue(Suit12);
		}
	}
}


