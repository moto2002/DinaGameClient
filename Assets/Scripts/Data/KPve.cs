using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Lib.Resource;
using Assets.Scripts.Define;

namespace Assets.Scripts.Data
{
    public class KPve : AKTabFileObject
    {
        public int nID;
        public string Name;
        public int nReqPlayerLevel;
        public int nDailyCount;
        public int tExistTime;
        public int nGoodAwardID;
        public int nNormalAwardID;
        public int nBadAwardID;
        public float tGoodTime;
        public float tNormalTime;

        public string Text;

        public override string getKey()
        {
            return nID.ToString();
        }
    }
}
