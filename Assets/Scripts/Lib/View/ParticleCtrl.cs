using UnityEngine;
using System.Collections;

public class ParticleCtrl : MonoBehaviour {
	ParticleSystem [] pss;  
	// Use this for initialization
	void Start () {
		pss = GetComponentsInChildren<ParticleSystem>();
		 
	}
	public void Play()
	{
		foreach (ParticleSystem ps in pss)
		{
			ps.Play();
		}
	}
	
}
