using UnityEngine;
using System.Collections;
using Assets.Scripts.Lib.Loader;

namespace Assets.Scripts.Lib.Loader
{
    public class FxAsset
    {
        private bool isInit = false;
        public bool noShadow = true;
        public AssetInfo handle = null;

        private Object _ma = null;
        public Object MainAssert
        {
            get
            {
                return _ma;
            }
        }

        public int GetLoadType()
        {
            if (handle == null)
            {
                return -1;
            }
            return (int)handle.load_type;
        }

        public bool isLoaded()
        {
            return null != _ma;
        }

        public void init(string url)
        {
            if (isInit)
                return;
            isInit = true;
            handle = AssetLoader.GetInstance().Load(url, GlobalLoadComplete, AssetType.BUNDLER);
        }

        private void GlobalLoadComplete(AssetInfo info)
        {
            _ma = info.bundle.mainAsset;
            handle = info;
        }

        public GameObject CloneObj()
        {
            if (null == _ma)
                return null;
            return (GameObject)GameObject.Instantiate(_ma);
        }

        public GameObject CloneAndAddToParent(Transform transform)
        {
            return CloneAndAddToParent(transform, Vector3.zero);
        }

        public GameObject CloneAndAddToParent(Transform transform, Vector3 loacalPosition)
        {
            if (null == _ma)
                return null;
            GameObject ob = (GameObject)GameObject.Instantiate(_ma);
            ob.transform.parent = transform;
            ob.transform.localPosition = loacalPosition;
            ob.transform.localRotation = Quaternion.identity;
            if (noShadow)
            {
                Renderer[] rs = ob.GetComponents<Renderer>();
                foreach (Renderer r in rs)
                {
                    r.castShadows = false;
                    r.receiveShadows = false;
                }
            }
            return ob;
        }

        public void Release()
        {
            AssetLoader.GetInstance().DestroyImmediate(handle);
        }
    }
}
