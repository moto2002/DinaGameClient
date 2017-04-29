using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Manager;
using Assets.Scripts.Lib.Log;

namespace Assets.Scripts.Logic.Scene.SceneObject.Compont
{
    public class BaseComponent
    {
        public static Logger log = null;

        public SceneEntity Owner = null;
        // 事件映射表

        //public void Regist(string evt, Type type, object obj, string method)
        //{
        //    Delegate handler = Delegate.CreateDelegate(type, obj, method);
        //    Owner.eventDispatcher.AddEventListener(evt, handler);
        //}

        public void Regist(string type, MyEventHandler handler)
        {
            Owner.eventDispatcher.AddEventListener(type, handler);
        }

        public void UnRegist(string type, MyEventHandler handler)
        {
            Owner.eventDispatcher.RemoveEventListener(type, handler);
        }

        public virtual void OnAttachToEntity(SceneEntity ety)
        {
        }

        public void BaseInit(SceneEntity ety)
        {
            Owner = ety;
        }
        public virtual void OnDetachFromEntity(SceneEntity ety)
        {
            Owner = null;
        }

        public virtual void DoUpdate(){}

        public virtual string GetName()
        {
            throw new Exception("组件没起名");
        }
    }
}
