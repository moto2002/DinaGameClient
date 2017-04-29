using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Lib.Resource;
using Assets.Scripts.Define;

namespace Assets.Scripts.Data
{
	public class KBuff : AKTabFileObject {
	
        public int BuffID;
        public string BuffPath;
		public string BindPoint;
		public string AttributeKey1;
        public override string getKey()
        {
            return BuffID.ToString();
        }
    
	}
}
