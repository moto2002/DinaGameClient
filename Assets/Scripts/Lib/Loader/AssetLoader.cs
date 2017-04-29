using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets;
using AssetInfoList = System.Collections.Generic.List<Assets.Scripts.Lib.Loader.AssetInfo>;

namespace Assets.Scripts.Lib.Loader
{
    public delegate void AssetLoadComplete();
    public delegate void LoadComplete(AssetInfo info);

    public class AssetLoaderLevel
    {
        public const int IMMEDIATELY   = 0; // 马上就要使用的资源，需要立刻加载
        public const int WAIT_A_MOMENT = 1; // 一会就要用的资源，加载了就驻留不释放
        public const int ONLY_DOWNLOAD = 2; // 不着急使用的资源，加载了就释放，适合提前下载下个场景用到的资源

        public const int MAX_LOAD_LEVEL = AssetLoaderLevel.ONLY_DOWNLOAD;
    }

    class AssetLoader : MonoBehaviour
    {
		private AssetInfo curLoadding = null;
		private Dictionary<string, AssetInfo> assertDict = new Dictionary<string, AssetInfo>();

        private AssetInfoList[] waitList = null;
		
		public void DestroyImmediate(AssetInfo assetInfo)
		{
			if (assertDict.ContainsKey(assetInfo.url))
			{
				if (assetInfo.donotRelease)
				{
					Debug.LogWarning("resource mark as no release " + assetInfo.url);
					return;
				}
				if (null != assetInfo.bundle)
				{
					assetInfo.bundle.Unload(true);
					assetInfo.bundle = null;
					assertDict.Remove(assetInfo.url);
				}
			}
		}
        
        public void Init()
        {
            waitList = new AssetInfoList[AssetLoaderLevel.MAX_LOAD_LEVEL + 1];
            for (int i = 0; i < waitList.Length; ++i)
            {
                waitList[i] = new AssetInfoList();
            }
        }

        long m_lastTicksOnlyDownload = System.DateTime.Now.Ticks;
        private AssetInfo GetAssetInfoFromWaitList()
        {
            for (int i = 0; i < waitList.Length; ++i)
            {
                AssetInfoList assetInfoList = waitList[i];
                if (assetInfoList.Count == 0)
                    continue;

                if (i >= AssetLoaderLevel.ONLY_DOWNLOAD)
                {
                    long currentTicksOnlyDownload = System.DateTime.Now.Ticks;
                    if (currentTicksOnlyDownload - m_lastTicksOnlyDownload < 200 * 1000 * 10) // 200ms
                        continue;
                    m_lastTicksOnlyDownload = currentTicksOnlyDownload;
                }

                AssetInfo info = assetInfoList[0];
                assetInfoList.RemoveAt(0);
                return info;
            }

            return null;
        }

        private void AddAssetInfoToWaitList(AssetInfo assetInfo, int nLoadLevel)
        {
            if (nLoadLevel > AssetLoaderLevel.MAX_LOAD_LEVEL)
                nLoadLevel = AssetLoaderLevel.MAX_LOAD_LEVEL;
            if (nLoadLevel < 0)
                nLoadLevel = 0;

            AssetInfoList assetInfoList = waitList[nLoadLevel];
            assetInfoList.Add(assetInfo);
            return;
        }

        private void UpdateAssetInfoInWaitList(AssetInfo assetInfo, int nLoadLevel)
        {
            if (nLoadLevel >= assetInfo.nLoadLevel)
                return;
            AssetInfoList assetInfoList = waitList[assetInfo.nLoadLevel];
            bool bRetCode = assetInfoList.Remove(assetInfo);
            if (!bRetCode)
            {
                Debug.LogError(
                    "AssetLoader UpdateAssetInfoInWaitList error, can not remove assetInfo:" + assetInfo.url + 
                    ", assetInfo.nLoadLevel=" + assetInfo.nLoadLevel + ", nLoadLevel=" + nLoadLevel
                );
                return;
            }
            assetInfoList = waitList[nLoadLevel];
            assetInfo.nLoadLevel = nLoadLevel;
            assetInfoList.Add(assetInfo);
        }
		
		public AssetInfo Load(string url)
        {
            return Load(
                url,
                null,
                AssetType.BUNDLER,
                false,
                AssetLoaderLevel.IMMEDIATELY
            );
        }
		public AssetInfo Load(string url,AssetType type)
        {
            return Load(
                url,
                null,
                type,
                false,
                AssetLoaderLevel.IMMEDIATELY
            );
        }

        public AssetInfo PreLoad(string url)
        {
            return Load(
                url,
                null,
                AssetType.BUNDLER,
                false,
                AssetLoaderLevel.WAIT_A_MOMENT
            );
        }

        public AssetInfo PreLoadOnlyDownload(string url)
        {
            return Load(
                url,
                null,
                AssetType.ONLY_DOWNLOAD,
                false,
                AssetLoaderLevel.ONLY_DOWNLOAD
            );
        }

        public AssetInfo Load(
            string url, 
            LoadComplete funOnLoadComplete,
            AssetType type
        )
        {
            return Load(
                url, 
                funOnLoadComplete, 
                type, 
                false,
                AssetLoaderLevel.IMMEDIATELY
            );
        }
		
		public void ReleaseFun(string url, 
            LoadComplete funOnLoadComplete)
		{
			AssetInfo assetInfo;
			if (assertDict.TryGetValue(url, out assetInfo))
            {
                assetInfo.OnLoadComplete -= funOnLoadComplete;
            }
		}
        public AssetInfo Load(
            string url, 
            LoadComplete funOnLoadComplete, 
            AssetType type, 
            bool useCache,
            int nLoadLevel
        )
        {
            AssetInfo assetInfo;
            if (assertDict.TryGetValue(url, out assetInfo))
            {
                if (type != AssetType.ONLY_DOWNLOAD)
                    assetInfo.type = type;

				if (assetInfo.load_type == AssetInfo.LOAD_TYPE.LOADED)
				{
					if (null != funOnLoadComplete)
					{
						assetInfo.OnLoadComplete += funOnLoadComplete;
					}
	                assetInfo.RaiseLoadComplete();
	                assetInfo.ClearEvent();
				}
				else
				{
					if (null != funOnLoadComplete)
					{
						assetInfo.OnLoadComplete += funOnLoadComplete;
					}
                    UpdateAssetInfoInWaitList(assetInfo, nLoadLevel);
				}
            }
			else
			{
				assetInfo = new AssetInfo(url, useCache);
                assetInfo.type = type;
                assetInfo.nLoadLevel = nLoadLevel;
                if (null != funOnLoadComplete)
				{
					assetInfo.OnLoadComplete += funOnLoadComplete;
				}
                AddAssetInfoToWaitList(assetInfo, nLoadLevel);
				assertDict.Add(assetInfo.url, assetInfo);
			}
			return assetInfo;
        }
		
        public void Update()
        {
			if (null == curLoadding)
			{
                AssetInfo info = GetAssetInfoFromWaitList();
                if (info == null)
                    return;

	            StartCoroutine(DoLoad(info));
			}
        }

        private IEnumerator DoLoad(AssetInfo info)
        {
            WWW www = null;
			info.load_type = AssetInfo.LOAD_TYPE.LOADING;
            if (info.useCache == true && info.type != AssetType.TEXT)
            {
                www = WWW.LoadFromCacheOrDownload(info.url, KConfigFileManager.GetInstance().resourceVersion.GetVersion(info.url));
            }
            else
            {
                www = new WWW(info.url);
            }
            yield return www;
			
            // Debug.Log("AssetLoader Load compelete " + www.url);
			info.load_type = AssetInfo.LOAD_TYPE.LOADED;
			curLoadding = null;
            if (string.IsNullOrEmpty(www.error) == true)
            {
				info.Fill(www);
                if (info.nLoadLevel == AssetLoaderLevel.ONLY_DOWNLOAD)
                {
                    assertDict.Remove(www.url);
                    info.Release();//effect_skill_daobin_04_chongfeng_xuanyun.res
                    info = null;
                    // Debug.Log("AssetLoader del when only download " + www.url);
                }
            }
            else
            {
                 Debug.LogError("Load Assert Error : " + www.error);
            }
        }

        private static AssetLoader mInstance = null;
        public static AssetLoader GetInstance()
        {
            if (mInstance == null)
            {
                mInstance = Object.FindObjectOfType(typeof(AssetLoader)) as AssetLoader;

                if (mInstance == null)
                {
                    GameObject go = new GameObject("_AssertLoader");
					go.hideFlags = HideFlags.HideAndDontSave;
                    DontDestroyOnLoad(go);
                    mInstance = go.AddComponent<AssetLoader>();
                    mInstance.Init();
                }
            }
            return mInstance;
        }

    }
}


