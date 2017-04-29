using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Manager;
/// <summary>
/// 常用函数
/// </summary>
public class KingSoftCommonFunction  {
	
	public static Vector3 GetMousePositionInUI()
	{
		float delta = Camera.main.pixelHeight/2;
		float delta0 = Camera.main.pixelWidth/2;
		Vector3 p1 = new Vector3((Input.mousePosition.x-delta0)/ delta ,
			(Input.mousePosition.y  - delta)/ delta ,
			-20f);
		return p1;
	}
	public static Color StringToColor(string _color)
	{
		string [] arys = _color.Split('|');
		if (arys.Length == 4)
		{
			return new Color32(
				System.Convert.ToByte(arys[0]),
				System.Convert.ToByte(arys[1]),
				System.Convert.ToByte(arys[2]),
				System.Convert.ToByte(arys[3])
				);
		}
		return new Color(0.5f,0.5f,0.5f,0.5f);
	}
	public static Vector3 NearPosition(Vector3 p,float d = 15f)
	{
		NavMeshHit hit;
		if(NavMesh.SamplePosition(p,out hit,d,-1))
		{
			return hit.position;
		}
		return p;
	}
	public static Vector3 GetGameObjectSize(GameObject obj)
	{
		Quaternion old_rot = obj.transform.rotation;
		obj.transform.rotation =  Quaternion.identity;
		float max_x = float.MinValue;
		float max_y = float.MinValue;
		float max_z = float.MinValue;
		float min_x = float.MaxValue;
		float min_y = float.MaxValue;
		float min_z = float.MaxValue;
		Vector3 point ;
		{
			SkinnedMeshRenderer [] filders = obj.GetComponentsInChildren<SkinnedMeshRenderer>();
			foreach (SkinnedMeshRenderer f in filders)
			{
				foreach (Vector3 v in f.sharedMesh.vertices)
				{
					//point = v;
					point = f.transform.TransformPoint(v);
					max_x = Mathf.Max(point.x,max_x);
					max_y = Mathf.Max(point.y,max_y);
					max_z = Mathf.Max(point.z,max_z);
					min_x = Mathf.Min(point.x,min_x);
					min_y = Mathf.Min(point.y,min_y);
					min_z = Mathf.Min(point.z,min_z);
				}
			}
		}
		obj.transform.rotation =  old_rot;
		
		if (max_x >= min_x)
		{
			return new Vector3(max_x - min_x,max_y - min_y,max_z - min_z);
		}
		else
		{
			return new Vector3(1f,2f,1f);
		}
	}
	static GameObject walkMark = null;
	public static void ResetGoundMesh()
	{
		Vector3 [] vs;
		int [] ts;
		NavMesh.Triangulate(out vs,out ts);
		
		Mesh mesh = new Mesh();
		mesh.vertices = vs;
		mesh.triangles = ts;
		if(null != walkMark)
			GameObject.DestroyImmediate(walkMark);
		walkMark = new GameObject();
		GameObject.DontDestroyOnLoad(walkMark);
		MeshCollider mc =  walkMark.AddComponent<MeshCollider>();
		mc.sharedMesh = mesh;
		walkMark.tag = TagManager.GetInstance().NavMeshTag;
		walkMark.name = "__NavMesh";
	}
	static GameObject plane ;
	public static Vector3 ScreenMouseToGround(Vector3 heroPosition)
	{
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit[] hitList = Physics.RaycastAll (ray, 200);
		System.Array.Sort(hitList, delegate(RaycastHit r1, RaycastHit r2) { return r1.distance.CompareTo(r2.distance); });
		System.Nullable<RaycastHit> heroHit = null;
		foreach (RaycastHit hit in hitList) 
        {
			
			if (hit.collider.gameObject.tag == TagManager.GetInstance ().NavMeshTag) 
            {
				//Debug.Log("get hit "+hit.collider.gameObject.name + " " + hit.distance);
				return KingSoftCommonFunction.NearPosition(hit.point);
			} 
            else 
            {
				//Debug.Log("hit "+hit.collider.gameObject.name + " " + hit.distance);
				heroHit = hit;
			}
		}
		if(null != heroHit)
		{
			return NearPosition(heroHit.GetValueOrDefault().point);
		}
		if(null == plane)
		{
			plane = new GameObject();
			GameObject.DontDestroyOnLoad(plane);
			BoxCollider box = plane.AddComponent<BoxCollider>();
			plane.tag = TagManager.GetInstance ().NavMeshTag;
			box.center = Vector3.zero;
			box.size = new Vector3(1000f,1,1000f);
		}
		
		{
			plane.transform.position = heroPosition-Vector3.down-Vector3.down;
			plane.SetActive(true);
			ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			hitList = Physics.RaycastAll (ray, 200);
			foreach (RaycastHit hit in hitList) 
	        {
				if (hit.collider.gameObject.tag == TagManager.GetInstance ().NavMeshTag) 
	            {
					plane.SetActive(false);
					return NearPosition(hit.point,2f);
				} 
			}
			plane.SetActive(false);
		}
		return NearPosition(Vector3.zero,100000f);
	}
	
	static readonly Vector3 UP_OFFSET = new Vector3(0,5f,0);
	static GameObject groundMesh = null;
	static MeshFilter groundMeshFilter = null;
	public static Vector3 GetGoundHeight(Vector3 p)
	{
		if (null == groundMesh)
			groundMesh = GameObject.Find("NavMeshPanel");
		
		groundMeshFilter = groundMesh.GetComponent<MeshFilter>();
		Ray ray = new Ray(p+UP_OFFSET,Vector3.down);
		RaycastHit hit;
		if (groundMeshFilter.collider.Raycast(ray,out hit,2000f) )
		{
			return hit.point;
		}
		return p;
	}
	public static bool IsPointCanWalk(Vector3 p)
	{
		if (null == groundMesh)
			groundMesh = GameObject.Find("NavMeshPanel");
		
		groundMeshFilter = groundMesh.GetComponent<MeshFilter>();
		Ray ray = new Ray(p+UP_OFFSET,Vector3.down);
		RaycastHit hit;
		if (groundMeshFilter.collider.Raycast(ray,out hit,2000f) )
		{
			return true;
		}
		return false;
	}
	
	public static GameObject ScreenMouseGetObject()
	{
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit[] hitList = Physics.RaycastAll (ray, 200);
		System.Array.Sort(hitList, delegate(RaycastHit r1, RaycastHit r2) { return r1.distance.CompareTo(r2.distance); });
		foreach (RaycastHit hit in hitList) 
        {
			if (hit.collider.gameObject.tag == TagManager.GetInstance ().SceneObjectTag) 
            {
				return hit.collider.gameObject;
			} 
            
		}
		return null;
	}
	
	public static void SetLayer(GameObject obj,int layerIndex)
	{
		obj.layer = layerIndex;
		foreach(Transform t in obj.transform.GetComponentsInChildren<Transform>())
		{
			t.gameObject.layer = layerIndex;
		}
	}
	
	
	public static int [] RandomAry(int count)
	{
		int index = 0;
		List<int> lists = new List<int>();
		List<int> lists2 = new List<int>();
		for (int i = 0 ; i < count ; i++)
		{
			lists.Add(i);
		}
		while(lists.Count > 0)
		{
			index = Random.Range(0,lists.Count);
			lists2.Add(lists[index]);
			lists.RemoveAt(index);
		}
		return lists2.ToArray();
	}
	public static string [] RandomAryByList(string [] arys,int [] indexs)
	{
		int len = arys.Length;
		string [] arys2 = new string[len];
		try
		{
			for (int i = 0 ; i < len ; i++)
			{
				arys2[i] = arys[indexs[i]];
			}
		}
		catch (System.IndexOutOfRangeException e)
		{
			Debug.LogError(e);
		}
		
		return arys2;
	}
	public static string [] GrowArrays(string [] arys ,int length)
	{
		List<string> lists = new List<string>();
		string last = "";
		foreach (string str in arys)
		{
			last = str;
			lists.Add(str);
		}
		while (lists.Count < length)
		{
			lists.Add(last);
		}
		return lists.ToArray();
	}
	public static void LootAt(GameObject obj,GameObject aim )
	{
		Vector3 dir = aim.transform.position - obj.transform.position ;
		dir = new Vector3(dir.x,0f,dir.z);
		obj.transform.forward = dir;
	}
	
	public static void LootAt(GameObject obj,Vector3 aim )
	{
		Vector3 dir = aim - obj.transform.position ;
		dir = new Vector3(dir.x,0f,dir.z);
		obj.transform.forward = dir;
	}
}
