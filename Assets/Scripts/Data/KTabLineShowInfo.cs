using System;
using System.Collections.Generic;
using Assets.Scripts.Lib.Resource;

namespace Assets.Scripts.Data
{
    public class KTabLineShowInfo : AKTabFileObject
	{
        public int ID;
        public string Character;
        public string Category;
        public string OrderNumber;

        public override string getKey()
        {
            return ID.ToString();
        }
	}
}
