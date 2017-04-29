using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Lib.Loader
{
	
    public class AssetInfo
    {
		public enum LOAD_TYPE
		{
			NO_LOADED = 1,
			LOADING = 2,
			LOADED = 3,
		}
        public int nLoadLevel = AssetLoaderLevel.IMMEDIATELY;
		public bool donotRelease = false;
		public LOAD_TYPE load_type = LOAD_TYPE.LOADING;
        public string url;
        public bool useCache;
        public float useTime;
        public AssetType type;
        public AssetBundle bundle;
        public Texture texture;
        public string text;
        public byte[] binary;
        public event LoadComplete OnLoadComplete;
		
		public AssetBundleRequest subRequest = null; 
		
		/// <summary>
		/// 用于检测资源是否加载完成.
		/// </summary>
		/// <returns>
		/// 是否加载完成.
		/// </returns>
		/// <param name='requestSubData'>
		/// 是否有附加数据,一般无附加数据.
		/// </param>
		public bool isDone(bool requestSubData = false)
		{
			if (!requestSubData)
				return load_type == LOAD_TYPE.LOADED;
			else
			{
				if (null == bundle)
					return false;
				if (null == subRequest)
					subRequest = bundle.LoadAsync("binddata", typeof(EditorObjectData));
				return subRequest.isDone;
			}
		}
		
		
        public AssetInfo(string _url, bool _useCache, int _loadLevel = AssetLoaderLevel.IMMEDIATELY)
        {
            this.url = _url;
            this.useCache = _useCache;
            this.nLoadLevel = _loadLevel;
        }
		public GameObject CloneGameObject()
		{
			if (type == AssetType.BUNDLER && null != bundle.mainAsset)
            {
				GameObject obj = (GameObject)Object.Instantiate(bundle.mainAsset);
                return obj;
            }
			return null;
		}
		/// <summary>
		/// 不要在外面调用这个方法
		/// Releases the by loader.
		/// 
		/// </summary>
		/// <param name='l'>
		/// L.
		/// </param>
		public void Release()
		{
			if (type == AssetType.BINARY)
            {
                bundle.Unload(true);
                bundle = null;
            }
		}

		public void Fill(WWW www)
        {
            if (type == AssetType.TEXT)
                text = www.text;
            else if (type == AssetType.BINARY)
                binary = www.bytes;
            else if (type == AssetType.ICON)
                texture = www.texture;
            else if (type == AssetType.ONLY_DOWNLOAD)
            {
            }
            else
                bundle = www.assetBundle;

            RaiseLoadComplete();
            ClearEvent();
		}

        public void RaiseLoadComplete()
        {
			try
			{
				if (OnLoadComplete != null)
                	OnLoadComplete(this);
			}
			catch(System.Exception e)
			{
				Debug.LogError(e.StackTrace);
			}
            
        }

        public void ClearEvent()
        {
            OnLoadComplete = null;
        }
    }
}
