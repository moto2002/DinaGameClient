using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Manager
{
    class ScreenManager :MonoBehaviour
    {
        public delegate void ChangeSize(uint width, uint height);
        public event ChangeSize OnChangeSize;

        private uint width;
        private uint height;


        public void SetSize(uint _width, uint _height)
        {
            if (width != _width || height != _height)
            {
                width = _width;
                height = _height;
                if (OnChangeSize != null)
                {
                    OnChangeSize(width, height);
                }
            }
        }

        void Update()
        {
            SetSize((uint)Screen.width, (uint)Screen.height);
        }

        private static ScreenManager mInstance;
        public static ScreenManager GetInstance()
        {
            if (mInstance == null)
            {
                mInstance = Object.FindObjectOfType(typeof(ScreenManager)) as ScreenManager;

                if (mInstance == null)
                {
                    GameObject go = new GameObject("_ScreenManager");
                    DontDestroyOnLoad(go);
                    mInstance = go.AddComponent<ScreenManager>();
                }
            }
            return mInstance;
        }
    }
}
