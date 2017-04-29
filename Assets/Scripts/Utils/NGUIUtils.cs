using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Manager
{
    class NGUIUtils
    {
        public static UILabel AddUILabel(GameObject parentGo)
        {
            GameObject go = new GameObject("UILabel");
            go.transform.parent = parentGo.transform;
            go.layer = 
            go.layer = CameraLayerManager.GetInstance().Get2DLayTag();
            UILabel label = go.AddComponent<UILabel>();
            float s = 0.002857143f * 100;
            go.transform.localScale = new Vector3(s, s, s);
            go.transform.localPosition = Vector3.zero;
            label.font = FontManager.GetInstance().Font;
            return label;
        }


        public static UISprite AddUISprite(GameObject parentGo, string resourceName, string imageName)
        {
            GameObject go = new GameObject("UISprite");
            go.transform.parent = parentGo.transform;
            UISprite sprite = go.AddComponent<UISprite>();
            float s = 0.002857143f * 20f;
            go.transform.localScale = new Vector3(s, s, s);
            go.transform.localPosition = Vector3.zero;
            sprite.atlas = UIAtlasManager.GetInstance().GetUIAtlas(resourceName);
            sprite.spriteName = imageName;
            return sprite;
        }

        public static T FindUIObject<T>(GameObject parent, string name) where T : Component
        {
            T[] coms = parent.GetComponentsInChildren<T>();
            int count = coms.Length;
            for (int i = 0; i < count; i++)
            {
                if (coms[i].gameObject.name.Equals(name))
                {
                    return coms[i];
                }
            }
            return null;
        }

        public static GameObject FindGameObject(GameObject parent, string name)
        {
            Component[] cms = parent.GetComponentsInChildren<Transform>(true);
            int count = cms.Length;
            for (int i = 0; i < count; i++)
            {
                if (cms[i].gameObject.name.Equals(name))
                {
                    return cms[i].gameObject;
                }
            }
            return null;
        }

    }
}
