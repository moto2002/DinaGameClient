using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.View.Scene;
using Assets.Scripts.Controller;
using Assets.Scripts.Utils;

namespace Assets.Scripts.Lib.Loader
{
    public class FxAssetManager : MonoBehaviour
    {
        static Dictionary<string, FxAsset> globalAsserts = new Dictionary<string, FxAsset>();

        private static FxAssetManager mInstance = null;
        public static FxAssetManager GetInstance()
        {
            if (mInstance == null)
            {
                GameObject go = new GameObject("_AssertLoader");
                DontDestroyOnLoad(go);
                mInstance = go.AddComponent<FxAssetManager>();
            }
            return mInstance;
        }

        public FxAsset GetFxAsset(string assertPath)
        {
            FxAsset ga;
            if (!globalAsserts.TryGetValue(assertPath, out ga))
            {
                ga = new FxAsset();
                ga.init(assertPath);
                globalAsserts.Add(assertPath, ga);
            }

            return ga;
        }
    }
}
