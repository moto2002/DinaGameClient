using UnityEngine;
using System.Collections;
using Assets.Scripts.Lib.Log;

namespace Assets.Scripts
{
    public class JavascriptManager : MonoBehaviour
    {
        private static Logger log = LoggerFactory.GetInstance().GetLogger(typeof(JavascriptManager));
        public void SetNetConfig(string conf)
        {
            log.Debug("conf:" + conf);
        }

        void Start()
        {

        }

        void Update()
        {

        }

        private static JavascriptManager mInstance;
        public static JavascriptManager GetInstance()
        {
            if (mInstance == null)
            {
                mInstance = Object.FindObjectOfType(typeof(JavascriptManager)) as JavascriptManager;

                if (mInstance == null)
                {
                    GameObject go = new GameObject("_JavascriptManager");
                    DontDestroyOnLoad(go);
                    mInstance = go.AddComponent<JavascriptManager>();
                }
            }
            return mInstance;
        }
    }
}