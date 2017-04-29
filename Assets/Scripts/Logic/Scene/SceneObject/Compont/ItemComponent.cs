using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.Manager;
using Assets.Scripts.Utils;
using Assets.Scripts.Define;
using Assets.Scripts.View.Scene;
using Assets.Scripts.Data;

namespace Assets.Scripts.Logic.Scene.SceneObject.Compont
{
    class ItemComponent : BaseComponent
    {
        public override string GetName()
        {
            return GetType().Name;
        }
		BoxCollider bc;
        public override void OnAttachToEntity(SceneEntity ety)
        {
            BaseInit(ety);
			bc = Owner.gameObject.AddComponent<BoxCollider>();
			bc.size = new Vector3(1,1,1);
            LoadResource();
        }

        public override void OnDetachFromEntity(SceneEntity ety)
        {
            base.OnDetachFromEntity(ety);
        }

        private void LoadResource()
        {
			if (Owner.property.dropType == KDropType.dtMoney)
			{
				AssetLoader.GetInstance().Load(URLUtil.GetResourceLibPath() + "Scene/Collect/baoxiang.obj", LoadResource_OnLoadComplete, AssetType.BUNDLER);
			}
			else
			{
				AssetLoader.GetInstance().Load(URLUtil.GetResourceLibPath() + "Scene/Collect/jingbi.obj", LoadResource_OnLoadComplete, AssetType.BUNDLER);
			}
            
        }

        private void LoadResource_OnLoadComplete(AssetInfo info)
        {
            if (this == null && Owner == null)
                return;
			try
			{
				Owner.BodyGo = GameObject.Instantiate(info.bundle.mainAsset) as GameObject;
			}
			catch(System.Exception e)
			{
				//游戏对象已经被释放.
				return;
			}
            Owner.BodyGo.transform.parent = Owner.transform;
            Owner.BodyGo.transform.localPosition = Vector3.zero;
            Owner.BodyGo.transform.rotation = Quaternion.identity;
            Owner.BodyGo.transform.localScale = new Vector3(1, 1, 1);
            Owner.BodyGo.layer = CameraLayerManager.GetInstance().GetSceneObjectTag();
            Owner.Anim = Owner.BodyGo.animation;
			BoxCollider bc0 = Owner.BodyGo.gameObject.GetComponent<BoxCollider>();
			if (null != bc0)
			{
				bc.size = bc0.size;
				bc.center = bc0.center;
				GameObject.DestroyObject(bc0);
			}
        }

        public override void DoUpdate()
        {
        }

        /*public bool CheckNearBy(int allowDis)*/
        
    }
}
