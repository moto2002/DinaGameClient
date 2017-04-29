using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Lib.Resource;
using Assets.Scripts.Define;

namespace Assets.Scripts.Data
{
	public class KParams : AKTabFileObject {
	
        public int ID;
        public float CommonSpeed;
		public string HitColor;
		public int HitColorTime;
		public int HitHeight;
		public string MonsterOutColor;
		public float MonsterOutTime;
		public string MonsterOutFx;
		public float MonsterOutHeight;
		public float MonsterOutFxScale;
		public float MaxEnemyDistance = 25f;

        public override string getKey()
        {
            return ID.ToString();
        }
    
		
	}
}
