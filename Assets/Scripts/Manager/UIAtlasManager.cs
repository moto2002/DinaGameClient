using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Manager
{
    class UIAtlasManager
    {
        public delegate void LoadComplete();
        public event LoadComplete OnLoadComplete;

        public string defaultResourceName = "defaultresource";

        Dictionary<string, UIAtlas> mDictionary = new Dictionary<string, UIAtlas>();

        private Queue<string> resources;

        private int loadCount;

        public UIAtlasManager()
        {
            resources = new Queue<string>();
            resources.Enqueue(defaultResourceName);
        }

        public void AddResource(string resourceName)
        {
            resources.Enqueue(resourceName);
        }

        public void Load()
        {
            loadCount = resources.Count;
            while (resources.Count > 0)
            {
                string resource = resources.Dequeue();
                AssetLoader.GetInstance().Load(URLUtil.GetResourceLibPath() + "Atlas/" + resource + ".res", Atlas_OnLoadComplete, AssetType.BUNDLER);
            }
        }

        public void Atlas_OnLoadComplete(AssetInfo info)
        {
            GameObject go = info.bundle.mainAsset as GameObject;
            UIAtlas atlas = go.GetComponent<UIAtlas>();
            mDictionary.Add(info.url, atlas);
            loadCount--;
            if (loadCount == 0 && OnLoadComplete != null)
            {
                OnLoadComplete();
            }
        }

        public UIAtlas.Sprite GetSprite(string imageName)
        {
            return GetSprite(defaultResourceName, imageName); 
        }

        public UIAtlas.Sprite GetSprite(string resourceName, string imageName)
        {
            UIAtlas atlas = GetUIAtlas(resourceName);
            UIAtlas.Sprite sprite = null;
            if (atlas != null)
            {
                sprite = atlas.GetSprite(imageName);
            }
            return sprite;
        }

        public bool HasSprite(string imageName)
        {
            return HasSprite(defaultResourceName, imageName);
        }

        public bool HasSprite(string resourceName, string imageName)
        {
            UIAtlas.Sprite sp = GetSprite(resourceName, imageName);
            if (sp != null)
            {
                return true;
            }
            return false;
        }

        public UIAtlas GetUIAtlas()
        {
            return GetUIAtlas(defaultResourceName);
        }

        public UIAtlas GetUIAtlas(string resourceName)
        {
            UIAtlas atlas = null;
            mDictionary.TryGetValue(URLUtil.GetResourceLibPath() + "Atlas/" + resourceName + ".res", out atlas);
            return atlas;
        }

        public static UIAtlasManager instance;
        public static UIAtlasManager GetInstance()
        {
            if (instance == null)
            {
                instance = new UIAtlasManager();
            }
            return instance;
        }
    }
}
