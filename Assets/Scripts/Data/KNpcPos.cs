using System;
using System.Collections.Generic;
using Assets.Scripts.Lib.Resource;

namespace Assets.Scripts.Data
{
    public class KNpcPos : AKTabFileObject
    {
        public int ID;
        public int NpcID;
        public int MapID;
        public int nX;
        public int nY;
        public int nZ;
        public int nPathX;
        public int nPathY;
        public int nPathZ;

        public override string getKey()
        {
            return ID.ToString();
        }
    }
}
