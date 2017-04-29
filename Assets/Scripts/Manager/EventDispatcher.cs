using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Logic.Scene.SceneObject.Compont;
using UnityEngine;

namespace Assets.Scripts.Manager
{
    public delegate object MyEventHandler(params object[] objs);

    public class EventRet
    {
        private Dictionary<Type, object> allRet = new Dictionary<Type, object>();

        public void AddReturn(Type type, object obj)
        {
            allRet[type] = obj;
        }
        public object GetReturn<T>() where T : BaseComponent
        {
            object retObj = null;
            allRet.TryGetValue(typeof(T), out retObj);
            if (retObj != null)
            {
                return retObj;
            }

            foreach (KeyValuePair<Type, object> ret in allRet)
            {
                Type type = ret.Key;
                while (type != null)
                {
                    if (type == typeof(T))
                    {
                        return ret.Value;
                    }
                    type = type.BaseType;
                }
            }
            Debug.LogError(typeof(T).Name + " Return Error ");
            return null;
        }
    }

    public class EventDispatcher
    {
        private Dictionary<string, List<MyEventHandler>> listeners = new Dictionary<string, List<MyEventHandler>>();

        public EventDispatcher() { }

        public void AddEventListener(string type, MyEventHandler handler)
        {
            if (handler == null)
                return;
            List<MyEventHandler> handlers = null;
            if (!listeners.ContainsKey(type))
            {
                handlers = new List<MyEventHandler>();
                listeners.Add(type, handlers);
            }
            else
            {
                handlers = listeners[type];
            }
            handlers.Add(handler);
        }

        public void RemoveEventListener(string type, MyEventHandler handler)
        {
            List<MyEventHandler> handlers = listeners[type];
            if (handlers != null && handler != null && handlers.Contains(handler))
            {
                handlers.Remove(handler);
            }
        }

        public EventRet DispatchEvent(string evt, params object[] objs)
        {
            EventRet ret = new EventRet();
            List<MyEventHandler> handlers;
            if (listeners.TryGetValue(evt, out handlers))
            {
                MyEventHandler handler = null;
                int count = handlers.Count;
                for (int i = 0; i < count; i++)
                {
                    object retObj = null;
                    handler = handlers[i];
                    if (handler != null)
                    {
                        Type type = handler.Target.GetType();
                        retObj = handler(objs);
                        ret.AddReturn(type, retObj);
                    }
                }
            }
            return ret;
        }

        public void Regist(string type, MyEventHandler handler)
        {
            AddEventListener(type, handler);
        }

        public void Remove(string type, MyEventHandler handler)
        {
            RemoveEventListener(type, handler);
        }

        public EventRet Dispath(string type, params object[] objs)
        {
            return DispatchEvent(type, objs);
        }

        static EventDispatcher GameWorldEventDispatcher = new EventDispatcher();

        public static EventDispatcher GameWorld
        {
            get { return GameWorldEventDispatcher; }
        }
    }

}
