using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class EditorDataMap  {
	
	public Dictionary<string,string> data = new Dictionary<string, string>();
	public Vector3 GetVector3(string key)
	{
		string [] arys = data[key].Split('|');
		return new Vector3(
			System.Convert.ToSingle(arys[0]),
			System.Convert.ToSingle(arys[1]),
			System.Convert.ToSingle(arys[2])
			);
	}
	public Vector4 GetVector4(string key)
	{
		string [] arys = data[key].Split('|');
		return new Vector4(
			System.Convert.ToSingle(arys[0]),
			System.Convert.ToSingle(arys[1]),
			System.Convert.ToSingle(arys[2]),
			System.Convert.ToSingle(arys[3])
			);
	}
	public void SetVector3(string key,Vector3 val)
	{
		data[key]=""+val.x+"|"+val.y+"|"+val.z;
	}
	public void SetVector4(string key,Vector4 val)
	{
		data[key]=""+val.x+"|"+val.y+"|"+val.z+"|"+val.w;
	}
	public void SetInt(string key,int val)
	{
		data[key]=""+val;
	}
	public void SetFloat(string key,float val)
	{
		data[key]=""+val;
	}
	public void SetString(string key,string val)
	{
		data[key]=val;
	}
	public Quaternion GetQuaternion(string key)
	{
		string [] arys = data[key].Split('|');
		return new Quaternion(
			System.Convert.ToSingle(arys[0]),
			System.Convert.ToSingle(arys[1]),
			System.Convert.ToSingle(arys[2]),
			System.Convert.ToSingle(arys[3])
			);
	}
	public void SetQuaternion(string key,Quaternion val)
	{
		data[key]=""+val.x+"|"+val.y+"|"+val.z+"|"+val.w;
	}
	public float GetFloat(string key)
	{
		return System.Convert.ToSingle(data[key]);
	}
	public int GetInt(string key)
	{
		return System.Convert.ToInt32(data[key]);
	}
	
	public string GetString(string key)
	{
		return data[key];
	}
	
	
	
	public List<string> Save()
	{
		List<string> ls = new List<string>();
		foreach ( KeyValuePair<string,string> kvp in data )
		{
			ls.Add(kvp.Key);
			ls.Add(kvp.Value);
		}
		return ls;
	}
	public void Load(List<string> ls)
	{
		int count = ls.Count;
		for (int i = 0 ; i < count ; i+=2 )
		{
			data[ls[i]] = ls[i+1];
		}
	}
	
}
