using UnityEngine;
using System.Collections;
using Assets.Scripts.Logic.Scene.SceneObject;
using Assets.Scripts.Data;
using Assets.Scripts.Lib.Loader;

public class AttackSlow : MonoBehaviour {
	public float speed = 0.5f;
	public float time = 1f;
	public string animName = "";
	Animation anim = null;
	Ticker ticker = new Ticker();
	
	// Use this for initialization
	void Start () {
		SceneEntity entity = GetComponent<SceneEntity>();
		anim = entity.GetAnimation();
		try
		{
			if(anim.IsPlaying(animName))
				anim[animName].speed = speed;
		}
		catch(System.Exception e)
		{
			Destroy(this);
		}
		ticker.Restart();
	}
	
	// Update is called once per frame
	void Update () {
		if (ticker.GetEnableTime() > time)
		{
			try
			{
				if (null!=anim)
				{
					anim["attack1"].speed = 1f;
				}
				
			}
			catch(System.Exception e)
			{
				Destroy(this);
				Debug.LogError(e.ToString());
			}
			Destroy(this);
		}
		
	}
}
