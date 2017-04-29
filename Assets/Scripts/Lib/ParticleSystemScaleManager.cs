using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticleSystemInfor
{
	public float startSize;
	public float startSpeed;
}
public class ParticleSystemScaleManager {
	public static readonly ParticleSystemScaleManager instance = new ParticleSystemScaleManager();
	
	Dictionary<GameObject,Dictionary<ParticleSystem,ParticleSystemInfor>> dict = new Dictionary<GameObject, Dictionary<ParticleSystem, ParticleSystemInfor>>();
	public void Scale(float f , GameObject g)
	{
		Dictionary<ParticleSystem,ParticleSystemInfor> o = null;
		if ( !dict.TryGetValue(g,out o) )
		{
			o = new Dictionary<ParticleSystem, ParticleSystemInfor>();
			ParticleSystem [] pss = g.GetComponentsInChildren<ParticleSystem>(true);
			foreach (ParticleSystem s in pss)
			{
				ParticleSystemInfor infor = new ParticleSystemInfor();
				infor.startSize = s.startSize;
				infor.startSpeed = s.startSpeed;
				o[s] = infor;	
			}
			dict[g]=o;
			
		}
		foreach (KeyValuePair<ParticleSystem,ParticleSystemInfor> kvp in o )
		{
			kvp.Key.startSize = kvp.Value.startSize*f;
			kvp.Key.startSpeed = kvp.Value.startSpeed*f;
		}
	}
	public void ClearBad()
	{
		dict.Remove(null);
	}
	
	
	
	public static  void ScaleParticle( GameObject g)
	{
		ParticleSystem [] pss = g.GetComponentsInChildren<ParticleSystem>(true);
		foreach (ParticleSystem s in pss)
		{
			s.startSize = s.startSize*g.transform.localScale.y;
			s.startSpeed = s.startSpeed*g.transform.localScale.y;
		}
	}
}
