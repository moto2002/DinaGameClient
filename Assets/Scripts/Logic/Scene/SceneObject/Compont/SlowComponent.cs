using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.Manager;
using Assets.Scripts.Utils;
using Assets.Scripts.Define;

namespace Assets.Scripts.Logic.Scene.SceneObject.Compont
{
	public class SlowComponent : BaseComponent {
	
		public string animName;
		public float speed = 0;
		public float time = 0;
		Animation anim = null;
		Ticker ticker = new Ticker();
		public override string GetName()
        {
            return GetType().Name;
        }

        public override void OnAttachToEntity(SceneEntity ety)
        {
            BaseInit(ety);
			ticker.Stop();
            // 注册事件响应函数
            Regist(ControllerCommand.SLOW, OnSlow);

        }
		
		public object OnSlow(params object[] objs)
        {
			if (ticker.isPlaying())
			{
				try
				{
					anim[animName].speed = 1f;
				}
				catch(System.Exception e)
				{
				}
			}
			animName = Convert.ToString(objs[0]);
			time = Convert.ToSingle(objs[1]);
			speed = Convert.ToSingle(objs[2]);
			anim = Owner.Anim;
			try
			{
				if(anim.IsPlaying(animName))
					anim[animName].speed = speed;
				ticker.Restart();
			}
			catch(System.Exception e)
			{
				ticker.Stop();
			}
			return null;
        }
		public override void DoUpdate()
        {
			if ( ticker.GetEnableTime() > time)
			{
				try
				{
					if (null!=anim)
					{
						anim[animName].speed = 1f;
					}
					
				}
				catch(System.Exception e)
				{
					ticker.Stop();
				}
			}
		}
	}
}
