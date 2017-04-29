using UnityEngine;
using System.Collections;
using Assets.Scripts.Logic.Scene.SceneObject;

public class DestoryMonster : MonoBehaviour {
	Ticker tick = new Ticker();
	public float delta = 2.0f;
	static Material baseMat = null;
	Material mat = null;
	// Use this for initialization
	void Start () {
		if (baseMat == null)
			baseMat = new Material(TipShaderStr.shaderText);
		SceneEntity sn = GetComponent<SceneEntity>();
		if(null==sn || null == sn.BodyGo || !sn.BodyGo.activeSelf)
		{
			gameObject.AddComponent<DestoryObject>();
			GameObject.Destroy(this);
			return;
		}
		mat = new Material(baseMat.shader);
		foreach (Renderer render in sn.property.bodyGo.GetComponentsInChildren<Renderer>())
        {
			mat.mainTexture = render.material.mainTexture;
			render.material = mat;
			break;
		}
		//mat.SetColor("_Emission",new Color(0.5f,0.5f,0.5f,1f));
		foreach(FixedJoint  j in sn.property.bodyGo.GetComponentsInChildren<FixedJoint>())
		{
			GameObject.Destroy(j);
		}
		
		foreach(CharacterJoint  j in sn.property.bodyGo.GetComponentsInChildren<CharacterJoint>())
		{
			GameObject.Destroy(j);
		}
		foreach(Collider  cn in sn.property.bodyGo.GetComponentsInChildren<Collider>())
		{
			GameObject.Destroy(cn);
		}
		foreach(Rigidbody  rb in sn.property.bodyGo.GetComponentsInChildren<Rigidbody>())
		{
			GameObject.Destroy(rb);
		}
		
		
		tick.Restart();
	}
	
	// Update is called once per frame
	void Update () {
		if(tick.GetEnableTime() > delta)
		{
			Destroy(mat);
			Destroy(gameObject);
		}
		else
		{
			transform.position = transform.position + Vector3.down*Time.deltaTime*0.15f; 
			mat.SetColor("_Color",new Color(1f,1f,1f,1f-(tick.GetEnableTime()/delta)));
			
		}
	}
}
