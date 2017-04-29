using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.Manager;
using Assets.Scripts.Utils;
using Assets.Scripts.Define;
using Assets.Scripts.Data;


namespace Assets.Scripts.Logic.Scene.SceneObject.Compont
{
	class LoadStruct
	{
		public AssetInfo infor;
		public LoadComplete funOnLoadComplete;
	}
	public class LoadAssetComponent  : BaseComponent {
		List<LoadStruct>  loadingAsset = new List<LoadStruct>();
		
		public int Count = 0;
		public override string GetName()
        {
            return GetType().Name;
        }
		
		public void Load(
            string url, 
            LoadComplete funOnLoadComplete, 
            AssetType type
        )
		{
			AssetInfo infor = AssetLoader.GetInstance().Load(url,type);
			if (infor.isDone())
			{
				funOnLoadComplete(infor);
				return;
			}
			LoadStruct ls = new LoadStruct();
			ls.infor = infor;
			ls.funOnLoadComplete = funOnLoadComplete;
			loadingAsset.Insert(0,ls);
			Count = loadingAsset.Count;
		}
		public override void DoUpdate()
        {
			int i = Count-1;
			while( i >= 0 )
			{
				LoadStruct ls = loadingAsset[i];
				if (ls.infor.isDone())
				{
					loadingAsset.RemoveAt(i);
					Count--;
					ls.funOnLoadComplete(ls.infor);
				}
				i--;
			}
		}
		public void Release()
		{
			loadingAsset.Clear();
			Count = 0;
		}
		
        public override void OnAttachToEntity(SceneEntity ety)
        {
            BaseInit(ety);	
			Owner.Loader = this;
        }
        public override void OnDetachFromEntity(SceneEntity ety)
        {
			Owner.Loader = null;
            base.OnDetachFromEntity(ety);
        }
		
	}
}