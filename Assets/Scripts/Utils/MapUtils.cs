using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Scripts.Lib.Log;

namespace Assets.Scripts.Utils
{
    class MapUtils
    {
        private static Logger log = LoggerFactory.GetInstance().GetLogger(typeof(MapUtils));

        public static void GetIntFromMetre(Vector3 value, out ushort x, out ushort y, out short z)
        {
            x = (ushort)(value.x * 100);
            y = (ushort)(value.z * 100);
            z = (short)(value.y * 100);
        }

        
		public static Vector3 GetMetreFromInt(float x, float y, float z)
        {
            Vector3 vec = new Vector3(x / 100f,y / 100f,z / 100f);
            return vec;
        }

        public static Vector3 GetMetreFromInt(float x, float y)
        {
            Vector2 vec2 = new Vector2(x / 100f,y / 100f);
            Vector3 vec3 = PhysicUtils.GetMapPoint(vec2);
            return vec3;
        }

        public static byte GetFaceFromRotation(Quaternion quate)
        {
            Vector3 axis = Vector3.zero;
            float angle = 0.0f;
            quate.ToAngleAxis(out angle, out axis);
            return (byte)(angle * 255 / 360);
        }

        public static Quaternion GetRotationFromFace(byte value)
        {
            float angle = value * 360 / 255;
            return Quaternion.AngleAxis(angle, Vector3.zero);
        }

        public static Vector3 GetEulerAngles(byte value)
        {
            float angle = value * 360.0f/256.0f;
            return new Vector3(0, angle+90, 0);
        }

        public static Vector2 GetRectCenter(Rect rect)
        {
            return new Vector2(rect.x + rect.width/2, rect.y + rect.height/2);
        }

        public static float GetS2CRatio(int val)
        {
            return val / 100;
        }

        public static int GetC2SRatio(float val)
        {
            return (int)(val * 100);
        }

        public static bool IsMoveable(Vector3 position)
        {
            NavMeshHit hit;
            bool isa = NavMesh.SamplePosition(position, out hit, 1f, -1);
            log.Debug("____________________________________" + hit.hit + "_________" + isa + "__" + position.ToString() + "___" + NavMesh.GetNavMeshLayerFromName("Default"));
            return hit.hit;
        }
    }
}
