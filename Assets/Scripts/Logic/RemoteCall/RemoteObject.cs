using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Proto;
using Assets.Scripts.Lib.Net;
using System.IO;
using UnityEngine;
using System.Collections;
using System.Reflection;

namespace Assets.Scripts.Logic.RemoteCall
{
    public class RemoteObject
    {
        public virtual int GetInt(){throw new NotImplementedException();}
        public virtual string GetString(){throw new NotImplementedException();}
        public virtual bool GetBool() { throw new NotImplementedException(); }
        public virtual ulong GetUlong() { throw new NotImplementedException(); }

        public virtual RemoteObject this[int index]
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
        public virtual RemoteObject this[string name]
        {
            get{throw new NotImplementedException();}
            set{throw new NotImplementedException();}
        }

        public static implicit operator int(RemoteObject f)
        {
            if (f == null) return 0;
            return f.GetInt();
        }
        public static implicit operator string(RemoteObject f)
        {
            if (f == null) return "";
            return f.GetString();
        }
        public static implicit operator bool(RemoteObject f)
        {
            if (f == null) return false;
            return f.GetBool();
        }
        public static implicit operator ulong(RemoteObject f) 
        { 
            if (f == null) return 0;
            return f.GetUlong(); 
        }
    }

    public class RemoteBool : RemoteObject
    {
        public bool Value;
        public RemoteBool(bool val) { Value = val; }

        public override bool GetBool(){return Value;}
        public static implicit operator RemoteBool(bool d){return new RemoteBool(d);}
    }

    public class RemoteInt : RemoteObject
    {
        public int Value;
        public RemoteInt(int val) { Value = val; }

        public override int GetInt(){return Value;}
        public override string GetString(){return Value.ToString();}
        public static implicit operator RemoteInt(int d){return new RemoteInt(d);}
    }

    public class RemoteString : RemoteObject
    {
        public string Value;
        public RemoteString(string val) { Value = val; }

        public override string GetString(){return Value;}
        public override ulong GetUlong(){return ulong.Parse(Value);}

        public static implicit operator RemoteString(string d){return new RemoteString(d);}
    }

    public class RemoteTable : RemoteObject
    {
        public Dictionary<object, object> dictKV = new Dictionary<object, object>();
		public void Print()
		{
			foreach(object k in dictKV.Keys)
			{
				Debug.LogWarning("key = " + k + " " + k.GetType().ToString());
				Debug.LogWarning("Value = " + dictKV[k]);
			}
		}
		public bool ContainsKey(object _key)
		{
			return dictKV.ContainsKey(_key);
		}
		public int Count
		{
			get
			{
				return dictKV.Count;
			}
		}
        public override RemoteObject this[int index]
        {
            get 
            { 
                if (!dictKV.ContainsKey(index))
                {
                    return null;
                }
                return dictKV[index] as RemoteObject; 
            }
            set 
            { 
                dictKV[index] = value; 
            }
        }

        public override RemoteObject this[string name]
        {
            get 
            {
                if (!dictKV.ContainsKey(name))
                {
                    return null;
                }
                return dictKV[name] as RemoteObject; 
            }
            set 
            {
                dictKV[name] = value; 
            }
        }
    }
}
