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
using Assets.Scripts.Logic.Item;
namespace Assets.Scripts.Logic.Scene.SceneObject.Compont
{
public class GhostShadowComponent : BaseComponent {

 		GhostShadow gs = null;
		
		public override string GetName()
        {
            return GetType().Name;
        }
		public override void OnAttachToEntity(SceneEntity ety)
        {
			BaseInit(ety);
            
			Regist(ControllerCommand.GHOST_SHADOW, OnShowShadow);
           
        }
		public override void OnDetachFromEntity(SceneEntity ety)
        {
			
			UnRegist(ControllerCommand.GHOST_SHADOW, OnShowShadow);
            
            base.OnDetachFromEntity(ety);
        }
		private object OnShowShadow(params object[] objs)
        {
			bool b = (bool)objs[0];
			if (null==gs )
			{
				if (null==Owner.BodyGo)
					return null;
				gs = Owner.BodyGo.AddComponent<GhostShadow>();
			}
			gs.subMeshs = Owner.property.weapon;
			gs.enabled = b;
			return null;
		}
		
		/*public override void DoUpdate()
        {
			
		}*/
		
	}
}
