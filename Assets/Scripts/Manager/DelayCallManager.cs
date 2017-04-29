using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Logic.Scene.SceneObject.Compont;
using UnityEngine;

namespace Assets.Scripts.Manager
{
	public class DelayCallFun
	{
		public float callTime;
		public MyEventHandler fun;
		public object[] param;
	}
	//延迟调用管理类.
	public class DelayCallManager  {
	
		public static DelayCallManager instance = new DelayCallManager();
		public List<DelayCallFun> functionList = new List<DelayCallFun>();
		private DelayCallManager(){}
		
		public void CallFunction(MyEventHandler handler ,float _time, params object[] objs)
        {
            DelayCallFun dc = new DelayCallFun();
			dc.callTime = Time.realtimeSinceStartup + _time;
			dc.fun = handler;
			dc.param = objs;
			int _len = functionList.Count;
			for(int i = 0 ; i < _len ; i++)
			{
				DelayCallFun _dc = functionList[i];
				if(_dc.callTime > dc.callTime)
				{
					functionList.Insert(i,dc);
					return;
				}
			}
			functionList.Add(dc);
        }
		public void Update()
		{
			int _len = functionList.Count;
			for(int i = 0 ; i < _len ; )
			{
				DelayCallFun _dc = functionList[i];
				if (_dc.callTime <= Time.realtimeSinceStartup)
				{
					_dc.fun(_dc.param);
					functionList.RemoveAt(i);
					_len--;
				}
				else
				{
					i++;
				}
			}
		}
	}
}
