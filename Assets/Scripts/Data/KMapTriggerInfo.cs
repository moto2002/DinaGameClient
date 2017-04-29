using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Lib.Resource;
using Assets.Scripts.Define;

namespace Assets.Scripts.Data
{
    public class KMapTriggerInfo : AKTabFileObject
    {
        public int ID;
        public string TriggerPoint;
        public uint ChangeMapID;
        public string ChangePos;

        public override string getKey()
        {
            return ID.ToString();
        }
    }
}
