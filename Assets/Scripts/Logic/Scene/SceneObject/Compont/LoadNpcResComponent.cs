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
    class LoadNpcResComponent : BaseComponent
    {
        public override string GetName()
        {
            return GetType().Name;
        }

        public override void OnAttachToEntity(SceneEntity ety)
        {
            BaseInit(ety);

            // 注册事件响应函数
            Regist(ControllerCommand.LOAD_RES, LoadResource);

        }

        public override void OnDetachFromEntity(SceneEntity ety)
        {
            UnRegist(ControllerCommand.LOAD_RES, LoadResource);

            base.OnDetachFromEntity(ety);
        }

        private object LoadResource(params object[] objs)
        {
            KHeroSetting heroSetting = KConfigFileManager.GetInstance().heroSetting.getData(Owner.TabID.ToString());

            AssetLoader.GetInstance().Load(URLUtil.GetResourceLibPath() + "Hero/h_" + heroSetting.RepresentID.ToString() + ".hero", LoadResource_OnLoadComplete, AssetType.BUNDLER);
            return null;
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
				//对象月已经释放.
				return;
			}
            
            Owner.BodyGo.transform.parent = Owner.transform;
            Owner.BodyGo.transform.localPosition = Vector3.zero;
            Owner.BodyGo.transform.rotation = Quaternion.identity;
            Owner.BodyGo.transform.localScale = new Vector3(1, 1, 1);
            Owner.BodyGo.layer = CameraLayerManager.GetInstance().GetSceneObjectTag();
            Owner.Anim = Owner.BodyGo.animation;
			BoxCollider _boxCollider = Owner.BodyGo.GetComponent<BoxCollider>();
			if (null == _boxCollider)
			{
				Vector3 _size = KingSoftCommonFunction.GetGameObjectSize(Owner.BodyGo);
				float _h =  _size.y / Owner.transform.localScale.y;
				float _s =  Mathf.Max(_size.x,_size.z)*0.5f / Owner.transform.localScale.y;
				Owner.property.characterController.size = new Vector3(_s,_h,_s);
				Owner.property.characterController.center = new Vector3(0,_h/2 ,0);
			}
			else
			{
				Owner.property.characterController.size = _boxCollider.size*Owner.BodyGo.transform.localScale.y;
				Owner.property.characterController.center = _boxCollider.center*Owner.BodyGo.transform.localScale.y;
				GameObject.Destroy(_boxCollider);
			}
			if (Owner.L_Anim_Name.Length > 0)
            {
                Owner.DispatchEvent(ControllerCommand.PlayAnimation, Owner.L_Anim_Name, Owner.AnimModel);
            }
			if (Owner.ActiveAction.NAME.CompareTo("ActionMousterOut")==0)
				Owner.BodyGo.transform.localPosition = Vector3.down*3f;
            Owner.DispatchEvent(ControllerCommand.UPDATE_MISSION_SIGN);
        }
    }
}
