using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Lib.Resource;
using Assets.Scripts.Define;

namespace Assets.Scripts.Data
{
    public class KAminEvent : AKTabFileObject
    {
        public int HeroId;
		public string Anim;
		public float time;

        public override string getKey()
        {
            return HeroId + "_" + Anim;
        }
    }
}
