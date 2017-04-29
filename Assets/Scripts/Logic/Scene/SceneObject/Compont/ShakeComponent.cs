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
	public class ShakeComponent : BaseComponent {
	
		Ticker redTicker = new Ticker(200);
		public Vector3 localHitDir = Vector3.back;
		public float hitDelta = 0.1f;
		public bool isShakeing = false;
		public Vector3 shakeDirect = new Vector3(0.15f,0f,0f);
		float hitSpeed = 1f;
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
            // 注册事件响应函数
            Regist(ControllerCommand.HIT_SLOW, OnBeHit);

        }
		
		public object OnBeHit(params object[] objs)
        {
			//hitSpeed
			float _cd = (float)objs[0];
			float _delta = (float)objs[1];
			redTicker.SetCD(_cd);
			shakeDirect = new Vector3(0f,0f,-_delta);
			if (_cd > 0)
				hitSpeed = _delta/_cd;
			else
				hitSpeed = 3.0f;
			isShakeing = true; 
			Shake();
            redTicker.Restart();
			StartShake();
			return null;
        }

        public override void OnDetachFromEntity(SceneEntity ety)
        {
            UnRegist(ControllerCommand.HIT_SLOW, OnBeHit);
            base.OnDetachFromEntity(ety);
        }
		public void Shake()
		{
			if (null != Owner.BodyGo)
			{
				Owner.BodyGo.transform.localPosition = Vector3.MoveTowards(Owner.BodyGo.transform.localPosition, shakeDirect,Time.deltaTime*hitSpeed);
			}
		}
		public override void DoUpdate()
        {
			if (redTicker.IsActiveOneTime())
			{
				if (null != Owner.BodyGo)
					Owner.BodyGo.transform.localPosition = Vector3.zero;
				Reset();
				isShakeing = false;
			}
			if (isShakeing)
			{
				Shake();
			}
		}
		
		protected void Reset()
		{
			//Owner.AnimCmp.PopSpeed();
		}
		
		protected void StartShake()
        {
            Owner.AnimCmp.PopSpeed();
			//Owner.AnimCmp.PushBackSpead(0.0001f);  
        }
	}
}
