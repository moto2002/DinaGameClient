using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GhostShadowObject
{
	public GhostShadowObject(int index,Vector3 position,float height,Color color)
	{
		this.index = index;
		this.position = position;
		this.height = height;
		this.color = color;
		time = Time.realtimeSinceStartup;
		
	}
	public float time ;
	public int index;
	public Vector3 position;
	public float height;
	public Color color;
}
public class GhostShadowManager : MonoBehaviour {
	public GameObject globalMeshRender = null;
	public static readonly Color backgroundColor = new Color(0,0,0,0);
	public RenderTexture renderTarget = null;
	public RenderTexture aimTarget = null;
	public Material mat ;
	public static float fadeTime = 0.3f;
	
	public readonly int size = 1024;
	public readonly int delta = 128;
	
	
	MeshFilter meshFilter = null;
	MeshRenderer meshRenderer = null;
	Mesh mesh;
	float _scale = 1.3f;
	public int curIndex = 0;
	
	float tick = 0f;
	// Use this for initialization
	void Start () {
		
		
		string shaderText = 
		"Shader \"globalMeshRender\" {  \n"+
		"	Properties \n"+
		"	{ \n"+
		"		_MainTex (\"Base (RGB), Alpha (A)\", 2D) = \"white\" {} \n"+
		"	} \n"+
		"	 \n"+
		
		"	SubShader \n"+
		"	{ \n"+
		"		LOD 100 \n"+
		
		"		Tags \n"+
		"		{ \n"+
		"			\"Queue\" = \"Transparent\" \n"+
		"			\"IgnoreProjector\" = \"True\" \n"+
		"			\"RenderType\" = \"Transparent\" \n"+
		"		} \n"+
				
		"		Pass \n"+
		"		{ \n"+
		"			Cull Off \n"+
		"			Lighting Off \n"+
		"			ZWrite Off \n"+
		"			Fog { Mode Off } \n"+
		"			AlphaTest Greater 0.001 \n"+
		"			Blend SrcAlpha OneMinusSrcAlpha \n"+
		"			ColorMaterial AmbientAndDiffuse \n"+
		"			SetTexture [_MainTex] \n"+
		"			{ \n"+
		"				Combine Texture * Primary \n"+
		"			} \n"+
		"		} \n"+
		"	} \n"+
		"}";
		aimTarget = new RenderTexture(128,128,24);
		renderTarget = new RenderTexture(size,size,24);
		
		globalMeshRender = new GameObject();
		globalMeshRender.name = "globalMeshRender";
		GameObject.DontDestroyOnLoad(globalMeshRender);
		
		mat = new Material(shaderText);
		
		//mat = new Material(Shader.Find("Transparent/Diffuse"));
		
		meshFilter = globalMeshRender.AddComponent<MeshFilter>();
		meshRenderer = globalMeshRender.AddComponent<MeshRenderer>();
		mesh = meshFilter.mesh;
		meshRenderer.material = mat;
		meshRenderer.material.mainTexture = aimTarget;
	}
	public void RenderMesh(System.Collections.Generic.List<GhostShadowObject> list )
	{
		mesh.Clear();
		meshRenderer.material.mainTexture = renderTarget;
		int len = list.Count;
		float uvdelta = ((float)delta)/size; 
		int row_count = size / delta;
		Vector3[] vertices = new Vector3[4*len];
		Color [] colors = new Color[4*len];
		Vector2 [] uvs = new Vector2[4*len];
		Vector3 [] normals = new Vector3[4*len];
		for (int i = 0 ; i < len ; i++ )
		{
			GhostShadowObject o = list[i];
			
			float f = ( Time.realtimeSinceStartup - o.time )/ fadeTime;
			
			int _x = o.index / row_count;
			int _y = o.index % row_count;
			uvs[i*4+0] = new Vector3(_x*uvdelta,_y*uvdelta,0f);
			uvs[i*4+1] = new Vector3(_x*uvdelta,(_y+1)*uvdelta,0f);
			uvs[i*4+2] = new Vector3((_x+1)*uvdelta,_y*uvdelta,0f);
			uvs[i*4+3] = new Vector3((_x+1)*uvdelta,(_y+1)*uvdelta,0f);
			
			
			
			vertices[i*4+0] = new Vector3(-0.5f*o.height*_scale,0*o.height*_scale,0f)+o.position;
			vertices[i*4+1] = new Vector3(-0.5f*o.height*_scale,1f*o.height*_scale,0f)+o.position;
			vertices[i*4+2] = new Vector3(0.5f*o.height*_scale,0*o.height*_scale,0f)+o.position;
			vertices[i*4+3] = new Vector3(0.5f*o.height*_scale,1f*o.height*_scale,0f)+o.position;
			
			colors[i*4+0]=colors[i*4+1]=colors[i*4+2] = colors[i*4+3] = new Color(o.color.r,o.color.g,o.color.b,1f);//Mathf.Lerp(0.9f,0.3f,f));
			normals[i*4+0] = normals[i*4+1] = normals[i*4+2] = normals[i*4+3] = new Vector3(0f,0f,-1f);			
		}
		
		int [] triangles = new int [6*len];
		int _len = triangles.Length;
        for (int i = 0; i < _len / 6; i++) {
            triangles[i * 6 + 0] = i * 4 + 0;
            triangles[i * 6 + 1] = i * 4 + 1;
            triangles[i * 6 + 2] = i * 4 + 2;
            triangles[i * 6 + 3] = i * 4 + 2;
            triangles[i * 6 + 4] = i * 4 + 1;
            triangles[i * 6 + 5] = i * 4 + 3;
        }
		globalMeshRender.transform.position = Vector3.zero;
		mesh.vertices = vertices;
		mesh.colors = colors;
		mesh.normals = normals;
		mesh.uv = uvs;
		mesh.triangles = triangles;
	}
	public void RenderToTarget(Transform fellow,int index ,float height ,GameObject [] subMeshs )
	{
		height = height * _scale;
		int row_count = size / delta;
		int _x = index / row_count;
		int _y = index % row_count;
		float uvdelta = ((float)delta)/size; 
		int old_layer = fellow.gameObject.layer;
		camera.CopyFrom(Camera.main);
		camera.backgroundColor = backgroundColor;
		camera.isOrthoGraphic = true;
		camera.orthographicSize = height/2;
		camera.transform.position = fellow.position + Vector3.up* height/2;
		camera.transform.forward = new Vector3(0, 0, 1);
		float angle = Camera.main.transform.rotation.eulerAngles.y;
        camera.transform.Rotate(0, angle, 0, Space.Self);
        camera.transform.Translate(new Vector3(0, 0, -10), Space.Self);
		camera.targetTexture = aimTarget;
		camera.cullingMask = 1 << 30;
		camera.clearFlags = CameraClearFlags.SolidColor;
		fellow.gameObject.layer = 30;
		if (null != subMeshs)
		{
			if (null != subMeshs[0])
				subMeshs[0].layer = 30;
			if (null != subMeshs[1])
				subMeshs[1].layer = 30;
		}
		//camera.RenderWithShader(Shader.Find("Hidden/Show Render Types"),"RenderType");
		camera.RenderWithShader(ShaderManager.GetInstance().KingSoftGhost.shader,"RenderType");
		fellow.gameObject.layer = old_layer;
		if (null != subMeshs)
		{
			if (null != subMeshs[0])
				subMeshs[0].layer = old_layer;
			if (null != subMeshs[1])
				subMeshs[1].layer = old_layer;
		}
		ImageEffects.Blit(aimTarget, new Rect(0,0,1,1),renderTarget ,new Rect(_x*uvdelta,_y*uvdelta,uvdelta,uvdelta),BlendMode.Copy );
		RenderMesh(list);
	}
	
	public void AddShadow(Transform tf,GameObject [] subMeshs)
	{
		int count = size/delta;
		count = count*count;
		curIndex++;
		curIndex %= count;
		
		list.Add(new GhostShadowObject(curIndex,tf.position,2f,new Color(1.0f,1.0f,1.0f,0.5f)));
		RenderToTarget(tf,curIndex,2, subMeshs);
	}
	
	List<GhostShadowObject> list = new List<GhostShadowObject>();
	// Update is called once per frame
	void Update () {
		tick += Time.deltaTime;
		if (tick < 0.05f)
			return;
		tick = 0f;
		int c = list.Count;
		bool dirty = false;
		for(int i = c-1 ; i >= 0; i-- )
		{
			GhostShadowObject o = list[i];
			if ( Time.realtimeSinceStartup - o.time > fadeTime )
			{
				list.RemoveAt(i);
				dirty = true;
			}
		}
		if (dirty)
		{
			RenderMesh(list);
		}
	}
}
