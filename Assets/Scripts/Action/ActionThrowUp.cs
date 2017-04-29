using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.View.Scene;
using Assets.Scripts.Logic.Scene.SceneObject;

/// <summary>
/// Active move.
/// </summary>
public class ActionThrowUp : Action {
	public enum ThrowUpType
	{
		DISTANCE,
		TIME,
	}
	public Vector3 beginPosition;
	public Vector3 endPosition;
	public float speed;
	public bool isLock = false;
	public float height = 1f;
	Vector3 curPosition ;
	Vector3 forward;
	int curStepIndex ;
	int stepsLen;
	public bool changeForward = true;
	Vector3 [] steps ;
	public bool dampen = false;
	public float endSpeed = 0.1f;
	public float a = 0.0f;
	public float totalTime = 0f;
	float curTime = 0f;
	float distance = 0f;
	float deltaHeight = 0f;
	
	public ThrowUpType type = ThrowUpType.DISTANCE;
	
	public ActionThrowUp(SceneEntity hero):base("ActiveThrowUp",hero)
	{
		
	}
	Vector3 NearPosition(Vector3 p)
	{
		NavMeshHit hit;
		if(NavMesh.SamplePosition(p,out hit,3f,-1))
		{
			return hit.position;
		}
		return p;
	}
	/// <summary>
	/// 激活行为.
	/// </summary>
	public override void Active()
	{
		base.Active();
		curPosition = beginPosition = hero.Position;
		endPosition = KingSoftCommonFunction.GetGoundHeight(endPosition);
		forward = (endPosition-beginPosition).normalized;
		distance = Vector3.Distance(beginPosition,endPosition);
		float deltaY = Mathf.Abs(curPosition.y - beginPosition.y);
		
		
		if (type == ThrowUpType.DISTANCE)
		{
			deltaHeight = beginPosition.y - endPosition.y  ;//因为服务器的速度是水平的速度.
			curPosition = beginPosition = new Vector3(beginPosition.x,endPosition.y,beginPosition.z);//起点和终点放在一个水平面上.
			float v = (speed + endSpeed)/2;
			totalTime = distance / v;
			a = (endSpeed - speed) / totalTime;
		}
		
	}
	
	
	
	/// <summary>
	/// Determines whether this instance is can active.
	/// </summary>
	/// <returns>
	/// <c>true</c> if this instance is can active; otherwise, <c>false</c>.
	/// </returns>
	public override bool IsCanActive()
	{
		return true;
	}
	float last_t = 0;
	/// <summary>
	/// Update this instance.
	/// </summary>
	public override void Update()
	{
		curTime += Time.deltaTime;
		
		if ( type == ThrowUpType.DISTANCE )
		{
			bool b =  false;
			if (dampen && distance > 0)
			{
				float d = speed*curTime + a*curTime*curTime*0.5f;
				
				if (d<distance)
				{
					float t = d / distance;
					last_t = d;
					curPosition = new Vector3(Mathf.Lerp(beginPosition.x,endPosition.x,t),Mathf.Lerp(beginPosition.y,endPosition.y,t),Mathf.Lerp(beginPosition.z,endPosition.z,t));//按加速度计算位移.
					b = false;
				}
				else
				{
					b = true;
				}
			}
			else
			{
				b = KingSoftMath.MoveTowards(ref curPosition,endPosition,hero.Speed*Time.deltaTime);//按水平速度计算位移.
			}
			if(changeForward)
			{
				hero.Forward = forward;
			}
			if(b)
			{
				hero.Position =  endPosition ;
				isFinish = true;
			}
			else
			{
				/*float _d1 = Vector3.Distance(beginPosition,endPosition);
				float _d2 = Vector3.Distance(beginPosition,curPosition);
				float _t1 = _d2/_d1;*/
				float t = KingSoftMath.GetValueBetweenToPoint(beginPosition,endPosition,curPosition);
				
				if(height<=0)
				{
					hero.Position = curPosition + Vector3.up*deltaHeight*(1f-t) ;//加回高度偏移值.
					return;
				}
				float _a = Mathf.PI*t;
				hero.Position = curPosition +  Vector3.up*(height*Mathf.Sin(_a) + deltaHeight*(1f-t));
			}
		}
		else
		{
				if( curTime < totalTime )
				{
					float  t =  curTime / totalTime;
					hero.Position = KingSoftMath.lerp(beginPosition,endPosition,t);
					if( height > 0 )
					{
						float _a = Mathf.PI*t;
						hero.Position += Vector3.up*height*Mathf.Sin(_a);
					}
				}
				else
				{
					hero.Position = endPosition;
					isFinish = true;
				}
		}
		
	}
}
