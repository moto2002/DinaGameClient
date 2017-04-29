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
	public class ItemGlowComponent : BaseComponent {
	
		Color hitColor = new Color(25,25,25,25);
		public override string GetName()
        {
            return GetType().Name;
        }

        public override void OnAttachToEntity(SceneEntity ety)
        {
            BaseInit(ety);
            // 注册事件响应函数
            Regist(ControllerCommand.SET_GLOW, OnSetGlow);
        }
		
		public object OnSetGlow(params object[] objs)
        {
         
			bool isGlow = Convert.ToBoolean(objs[0]);
            if (isGlow )
            {
				SetReadColor();
            }
            else
            {
               Clear();
            }
			return null;
        }

        public override void OnDetachFromEntity(SceneEntity ety)
        {
            UnRegist(ControllerCommand.SET_GLOW, OnSetGlow);
            base.OnDetachFromEntity(ety);
        }
		
		
		protected void Clear()
		{
			 if (!Owner.property.isInteractive || null == Owner.BodyGo)
                return;
			foreach (Renderer render in Owner.BodyGo.GetComponentsInChildren<Renderer>())
            {
				foreach (Material mat in render.materials)
	            {
					mat.SetColor("_Emission",new Color32(0,0,0,0));
	            }
			}
		}
		
		protected void SetReadColor()
        {
            if (!Owner.property.isInteractive || null == Owner.BodyGo)
                return;
			foreach (Renderer render in Owner.BodyGo.GetComponentsInChildren<Renderer>())
            {
				foreach (Material mat in render.materials)
	            {
					mat.SetColor("_Emission",hitColor) ;
	            }
			}
            
        }
	}
}