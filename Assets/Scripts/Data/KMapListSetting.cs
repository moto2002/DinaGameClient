using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Scripts.Lib.Log;
using Assets.Scripts.Lib.Resource;
using Assets.Scripts.Lib.Loader;

namespace Assets.Scripts.Data
{
    public class KMapListSetting : AKTabFileObject
    {
        public static Logger log = LoggerFactory.GetInstance().GetLogger(typeof(KMapListSetting));
        public uint ID;
        public string Name;
        public uint Type;
        public uint LimitTime;
        public uint TerrianType;
        public uint MaxLength;
        public uint MaxHeight;
        public uint MaxPlayerCount;
        public uint MaxCopyCount;
        public string ResourcePath;
        public string ServerResource;
        public uint BattleID;
        public uint Sound;
		public uint BackgroundType;
        public string BornPos;

        private Vector3 bornPosAttr;
        public Vector3 BornPosAttr
        {
            get
            {
                if (bornPosAttr == null)
                {
                    string[] values = BornPos.Split(';');
                    log.Assert(values.Length == 3);

                    bornPosAttr = new Vector3();
                    bornPosAttr.x = Convert.ToInt32(values[0]);
                    bornPosAttr.y = Convert.ToInt32(values[1]);
                    bornPosAttr.z = Convert.ToInt32(values[2]);
                }
                return bornPosAttr;
            }
        }


        public override string getKey()
        {
            return ID.ToString();
        }


        public override void onComplete()
        {
            Debug.Log("Map " + ID.ToString());
        }
    }
}
