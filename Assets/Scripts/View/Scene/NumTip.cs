using UnityEngine;
using System.Collections;

public class NumTip : MonoBehaviour {
	public Vector3 position = Vector3.zero;
	public Vector3 delta = Vector3.zero;
	public GameObject root;
	public GameObject aim;
	public Animation animCmp = null;
	public UIDrawCallForShaderMat dc = null;
	public enum OFFSET_TYPE
	{
		NONE,
		LEFT,
		RIGHT
	}

	public float life = 1.5f;
	float curLife = 0f;
	float fadeRade = 0.6f;
	float fadeTime = 1f;
	public Vector3 localScale = new Vector3(10,10,10);
	public GameObject owner = null;
	
	
	

		
		
	//public OFFSET_TYPE type = OFFSET_TYPE.NONE;
	void Start () {
		animCmp = root.GetComponent<Animation>();
		if (null == animCmp)
			animCmp = root.GetComponentInChildren<Animation>();
		life = animCmp.clip.length;
		fadeTime = life*fadeRade;
		animCmp.Stop();
		animCmp.Play();
		
		
	}
	
	// Update is called once per frame
	//void FixedUpdate() {
	void Update () {
		curLife+=Time.deltaTime;
		if(curLife > life)
		{
			Destroy(gameObject);
			Destroy(root);
			return;
		}
		if(curLife > fadeTime)
		{
			float t = 1.0f - ( curLife - fadeTime )/(life-fadeTime);
			Color _color = new Color(1f,1f,1f,t);
			if (null !=dc)
			{
				int _len = dc.mFilter.mesh.colors.Length;
				Color [] ary = new Color[_len];
				for ( int i = 0 ; i < _len ; i++ )
				{
					ary[i]  = _color;
				}
				dc.mFilter.mesh.colors = ary;
			}
			
		}
		if (null != aim)
		{
			position = aim.transform.position + delta;
			transform.localScale = aim.transform.localScale*4f;
		}
		Vector3 forward = Camera.main.transform.forward;
		forward = new Vector3(forward.x,0,forward.z);
		Vector3 f = Camera.main.transform.forward;
		if (null!=root && null!= owner)
		{
			root.transform.position = owner.transform.position;
			root.transform.forward = forward;
			root.transform.forward = new Vector3(f.x,0,f.z);
		}
		transform.forward = f;
		transform.position = position;
	}
}
