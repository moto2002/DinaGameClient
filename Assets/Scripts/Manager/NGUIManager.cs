using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Manager
{
    class NGUIManager
    {
        public NGUIManager()
        {
            Init();
        }

        public UILabel AddUILabel(GameObject parentGo)
        {
            GameObject go = new GameObject("UILabel");
            go.transform.parent = parentGo.transform;
            UILabel label = go.AddComponent<UILabel>();
            label.font = FontManager.GetInstance().Font;
            return label;
        }

        public void Init()
        {
            GameObject cursorGo = new GameObject("cursorGo");
            cursorGo.AddComponent<UICursor>();
            cursorGo.hideFlags = HideFlags.HideInHierarchy;
            Object.DontDestroyOnLoad(cursorGo);

        }

        public static NGUIManager instance;
        public static NGUIManager GetInstance()
        {
            if (instance == null)
            {
                instance = new NGUIManager();
            }
            return instance;
        }
    }
}
