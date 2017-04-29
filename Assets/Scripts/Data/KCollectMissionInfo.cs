using System;
using System.Collections.Generic;
using Assets.Scripts.Lib.Resource;

namespace Assets.Scripts.Data
{
    public class KCollectMissionInfo : AKTabFileObject
    {
        public int nID;
        public int nTargetID;
        public string strName;
        public int nSceneID;
        public string strPosition;
        public string strIcon;
        public int nCanCollectTimes;
        public int nCanCollectDistance;
        public int nNeedTime;
        public bool bInterrupt;
        public bool bCommon;
        public int nRefreshTime;

        public override string getKey()
        {
            return nID.ToString();
        }
    }
}
