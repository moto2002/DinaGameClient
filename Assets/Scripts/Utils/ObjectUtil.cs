using UnityEngine;

namespace Assets.Scripts.Utils
{
    public class ObjectUtil
    {
        public static void SetLayerWithAllChildren(UnityEngine.GameObject go, int layer)
        {
            if (go != null)
            {
                go.layer = layer;
                Transform trans;
                for (int i = 0; i < go.transform.childCount; i++)
                {
                    trans = go.transform.GetChild(i);
                    trans.gameObject.layer = layer;
                    SetLayerWithAllChildren(trans.gameObject, layer);
                }
            }
        }


        public static void SetLayerWithAllChildren(UnityEngine.GameObject go, string layer)
        {
            if (go != null)
            {
                int iLayer = LayerMask.NameToLayer(layer);
                SetLayerWithAllChildren(go, iLayer);
            }
        }

        public static void SetTagWithAllChildren(UnityEngine.GameObject go, string tagStr)
        {
            if (go != null)
            {
                go.tag = tagStr;
                Transform trans;
                for (int i = 0; i < go.transform.childCount; i++)
                {
                    trans = go.transform.GetChild(i);
                    trans.gameObject.tag = tagStr;
                    SetTagWithAllChildren(trans.gameObject, tagStr);
                }
            }
        }
    }
}
