using UnityEngine;
using Assets.Scripts.Manager;
using Assets.Scripts.Data;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.Utils;
using System.Collections;
using Assets.Scripts.Logic.Mission;

namespace Assets.Scripts.Logic.Scene.SceneObject.Compont
{
    public class CollectObjComponent : BaseComponent
    {
        public override string GetName()
        {
            return GetType().Name;
        }

        public override void OnAttachToEntity(SceneEntity ety)
        {
            BaseInit(ety);

            LoadResource();
        }

        public override void OnDetachFromEntity(SceneEntity ety)
        {
            base.OnDetachFromEntity(ety);
        }

        private void LoadResource()
        {
            AssetLoader.GetInstance().Load(URLUtil.GetResourceLibPath() + "Scene/Collect/c_" + Owner.collectInfo.nTargetID.ToString() + ".collect", LoadResource_OnLoadComplete, AssetType.BUNDLER);
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
            Owner.name += Owner.collectInfo.strName;
        }

        public override void DoUpdate()
        {
        }

        public bool CheckNearBy(int allowDis)
        {
            float dis = Vector3.Distance(Owner.Position, SceneLogic.GetInstance().MainHero.Position);
            if (dis <= allowDis)
                return true;

            return false;
        }
    }
}
