using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public enum SHAKE_TYPE
{
	SUDDENLY,
	COMMON,
}
class ShakeInfor
{
	float time = 2f;
	Ticker ticker = new Ticker();
	float oscillation = 0.5f;
	SHAKE_TYPE type = SHAKE_TYPE.SUDDENLY;
	bool up = false;
	public ShakeInfor(float _time,float _oscillation,SHAKE_TYPE type)
	{
		this.time = _time;
		this.oscillation = _oscillation;
		
	}
	public Vector3 Update()
	{
		//float a = Random.Range(0,6.28f);
		//Vector3 localDir = new Vector3( Mathf.Sin(a) , Mathf.Cos(a) , 0f );
		
		Vector3 localDir = Vector3.down;
		if(up)
		{
			localDir = Vector3.up;
		}
		localDir = localDir*oscillation;
		up = !up;
		
		
		if (type==SHAKE_TYPE.SUDDENLY)
		{
			return localDir * Mathf.Cos(ticker.GetEnableTime()*1.571f/time);
		}
		else
		{
			return localDir * Mathf.Sin(ticker.GetEnableTime()*3.14159f/time);
		}
	}
	public bool IsFinish()
	{
		return ticker.GetEnableTime() > time;
	}
}
public class ShakeObj  {
	
	List<ShakeInfor> lists = new List<ShakeInfor>();
	
	public void Add(float _time,float _oscillation,SHAKE_TYPE type = SHAKE_TYPE.SUDDENLY)
	{
		ShakeInfor infor = new ShakeInfor(_time,_oscillation,type);
		lists.Add(infor);
	}
	public Vector3 GetDelta()
	{
		Vector3 v = Vector3.zero;
		int len = lists.Count;
		for ( int i = 0 ; i < len ; i++ )
		{
			ShakeInfor infor = lists[i];
			if (infor.IsFinish())
			{
				lists.RemoveAt(i--);
				len--;
			}
			else
			{
				v += infor.Update();
			}
		}
		return v;
	}
	
	
}
