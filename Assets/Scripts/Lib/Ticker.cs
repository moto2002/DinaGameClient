using UnityEngine;
using System.Collections;

public class Ticker  {
	
	public int cd = 1000;
	bool stop = false;
	int lastTime = 0;
	public Ticker(int cd = 1000)
	{
		this.cd = cd;
		lastTime = System.Environment.TickCount;
	}
	public float GetTimeNormal()
	{
		float d = System.Environment.TickCount-lastTime ;
		float _cd = cd;
		return d / _cd;
	}
	public void SetCD(float s)
	{
		this.cd = (int)(s*1000);
	}
	public bool isStop()
	{
		return stop;
	}
	public bool isPlaying()
	{
		return !stop;
	}
	public void Stop()
	{
		stop  = true;
	}
	public void SetActive()
	{
		stop = false;
		lastTime = System.Environment.TickCount-cd;
	}
	public void Restart()
	{
		stop = false;
		lastTime = System.Environment.TickCount;
	}
	public bool IsActiveOneTime()
	{
		if(IsEnable())
		{
			Stop();
			return true;
		}
		return false;
	}
	public int GetEnableTickCount()
	{
		return System.Environment.TickCount-lastTime ;
	}
	public float GetEnableTime()
	{
		float d = System.Environment.TickCount-lastTime ;
		return d/1000f;
	}
	public bool IsEnable()
	{
		if(stop)
		{
			return false;
		}
		int d = System.Environment.TickCount-lastTime ;
		if(d>=cd)
		{
			lastTime+=cd;
			return true;
		}
		return false;
	}
	public bool IsInCD()
	{
		if(stop)
		{
			return false;
		}
		int d = System.Environment.TickCount-lastTime ;
		if(d>=cd)
		{
			return false;
		}
		return true;
	}
}
