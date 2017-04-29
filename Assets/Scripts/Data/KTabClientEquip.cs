using UnityEngine;
using System.Collections;
using Assets.Scripts.Lib.Resource;


namespace Assets.Scripts.Data
{
	public class KTabClientEquip : AKTabFileObject
	{
		public int nID;
		public int nIcon;
		public string FBX;
		public string Fx;
		
		public override string getKey ()
		{
			return nID.ToString();
		}
	}
}


