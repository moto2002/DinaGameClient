using System.Collections;
using Assets.Scripts.Lib.Resource;
using System.Collections.Generic;
using Assets.Scripts.Logic.Intensify;

namespace Assets.Scripts.Data
{
	/// <summary>
	/// 装备强化表  2014.03.12 by linfeng
	/// </summary>
	public class KEquipStep : AKTabFileObject
	{
		public int nStep;
        public int nMinStrengthenLev;
        public int nMaxStrengthenLev;
		public int nNeedLevel;
		public int nItemType;
		public int nItemIndex;
		public int nItemCount;
		
		
		public override string getKey ()
		{
			return nStep.ToString();
		}
		/// <summary>
		/// 获得消耗的材料
		/// </summary>
		public string GetItemGoods ()
		{
			return nItemType + "_" + nItemIndex;
		}
		
		public override void onAllComplete ()
		{
			base.onAllComplete ();
		}
		
		
		
		
	}
}
