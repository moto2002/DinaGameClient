using UnityEngine;
using System.Collections;

public class DestoryObject : MonoBehaviour {
	
	Ticker tick = new Ticker();
	public float delta = 0.0f;
	// Use this for initialization
	void Start () {
		tick.Restart();
	}
	
	// Update is called once per frame
	void Update () {
		if(tick.GetEnableTime() > delta)
		{
			Destroy(gameObject);
		}
	}
}
