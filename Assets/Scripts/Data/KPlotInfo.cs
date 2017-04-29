using UnityEngine;
using System.Collections;
using Assets.Scripts.Lib.Resource;

namespace Assets.Scripts.Data
{
    public class KPlotInfo : AKTabFileObject
    {
        public enum PlotType
        {
            NormalCamera = 1,
            TransCamera,
        };

        public int nID;
        public PlotType plotType;
        public uint nNpcID;
        public string Content;
        public string strCameraPosition;
        public float fCameraDis;
        public int nextID;

        public override string getKey()
        {
            return nID.ToString();
        }
    }
}
