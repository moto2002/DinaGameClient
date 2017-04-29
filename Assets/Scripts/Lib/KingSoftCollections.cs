using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KingSoftCollections<T>  {
	public static List<T> ArrayToList(T [] arys)
	{
		List<T> list  = new List<T>();
		foreach(T t in arys)
		{
			list.Add(t);
		}
		return list;
	}
	
}
