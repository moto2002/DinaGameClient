using System;
using UnityEngine;

namespace Assets.Scripts.Utils
{
	public class GameObjectAsset
	{
		public string resource;
		public Transform transform;
		public Vector3 pos;
		public Quaternion rotation;
		public Vector3 scale;
        public Vector3 size;
		public bool loadFinish;

        public int lightmapIndex;
        public Vector4 lightmapTilingOffset;
		
		/// <summary>
		/// 资源释放句柄.
		/// </summary>
		public Assets.Scripts.Lib.Loader.AssetInfo infor = null;
	}
}

