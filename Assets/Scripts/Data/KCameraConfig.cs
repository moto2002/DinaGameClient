using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Lib.Resource;
using Assets.Scripts.Define;

namespace Assets.Scripts.Data
{
	public class KCameraConfig : AKTabFileObject {
	
        public int ID;
		public float CameraMaxDistance;
		public float CameraMinDistance;
		public float CameraMaxRotX;
		public float CameraMinRotX;
		public float RotY;

        public override string getKey()
        {
            return ID.ToString();
        }   
	}
}
