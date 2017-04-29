using UnityEngine;
using System.Collections;
using Assets.Scripts.Lib.Resource;

namespace Assets.Scripts.Data
{
	public class KTEquipModel : AKTabFileObject 
	{
		public int nEquipID;
		public string EquipFBXName;
		
		public override string getKey ()
		{
			return nEquipID.ToString();
		}
	}
}

