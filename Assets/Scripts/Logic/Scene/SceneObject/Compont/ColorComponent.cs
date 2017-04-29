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
	public class ColorComponent : BaseComponent {
	
		Ticker redTicker = new Ticker(200);
		Color hitColor = new Color(155,0,0,255);
		public override string GetName()
        {
            return GetType().Name;
        }

        public override void OnAttachToEntity(SceneEntity ety)
        {
            BaseInit(ety);
			redTicker.Stop();
			KParams kParams = KConfigFileManager.GetInstance().GetParams();
			redTicker.cd = kParams.HitColorTime;
			
			hitColor = KingSoftCommonFunction.StringToColor(kParams.HitColor);
			
            // 注册事件响应函数
            Regist(ControllerCommand.BE_HIT, OnBeHit);

        }
		
		public object OnBeHit(params object[] objs)
        {
            redTicker.Restart();
			return null;
        }

        public override void OnDetachFromEntity(SceneEntity ety)
        {
            UnRegist(ControllerCommand.BE_HIT, OnBeHit);
            base.OnDetachFromEntity(ety);
        }
		public override void DoUpdate()
        {
			if (redTicker.IsActiveOneTime())
			{
				Clear();
			}
			else if (redTicker.isPlaying())
			{
				SetReadColor();
			}
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
