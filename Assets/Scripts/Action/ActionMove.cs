using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.View.Scene;
using Assets.Scripts.Controller;
using Assets.Scripts.Logic.Scene.SceneObject;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.Data;

public delegate void OnDistanceChangeFun(Vector3 p);

/// <summary>
/// Active move.
/// </summary>
public class ActiveMove : Action {
	
	public Vector3 beginPosition;
	public Vector3 endPosition;
	Vector3 curPosition;
	public float deltaSpace = 0f;
	public float speed;
	public bool isLock = false;
	int curStepIndex ;
	NavMeshPath path = new NavMeshPath();
	int stepsLen;
	Vector3 [] steps ;
	public OnDistanceChangeFun distanceChangeFun = null;
	
	protected  void OnDistanceChange(Vector3 v)
	{
		if(null != distanceChangeFun)
		{
			distanceChangeFun(v);
		}
	}
	public ActiveMove(SceneEntity hero):base("ActiveMove",hero)
	{
		
	}
	public void SendNextDistance()
	{
		if(stepsLen > 1)
		{
			if( curStepIndex == stepsLen-1 &&  deltaSpace > 0 ){
				Vector3 dir = steps[curStepIndex]-hero.Position;
				float dis = Vector3.Distance( dir,Vector3.zero);
				if(dis > deltaSpace)
				{
					OnDistanceChange(hero.Position + dir.normalized*(dis-deltaSpace));
				}
			}
			else
			{
				OnDistanceChange(steps[curStepIndex]);
			}
		}
	}
	/// <summary>
	/// 激活行为.
	/// </summary>
	public override void Active()
	{
		base.Active();
		
		curPosition = beginPosition = KingSoftCommonFunction.NearPosition(hero.Position);
		endPosition = KingSoftCommonFunction.NearPosition(endPosition);
		NavMesh.CalculatePath(beginPosition,endPosition,-1,path);
		
		steps = path.corners;
		stepsLen = steps.Length;
        if ( stepsLen <= 1)
		{
			endPosition = curPosition;
			isFinish = true;
			return;
		}
		curStepIndex = 1;
		if(stepsLen > 1)
		{
			SendNextDistance();
		}
		isFinish = false;
		
		
		if(Vector3.Distance(hero.Position,endPosition) < deltaSpace)
		{
			hero.Position = KingSoftCommonFunction.GetGoundHeight(hero.Position);
			Vector3 forward = endPosition-hero.Position;
			forward = new Vector3(forward.x,0,forward.z);
			hero.Forward = forward;
			OnDistanceChange(hero.Position);
			isFinish = true;
		}
	}
	
	/// <summary>
	/// 每个行为结束时在这里做释放.
	/// </summary>
	public override void Release()
	{
		
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
	/// <summary>
	/// Update this instance.
	/// </summary>
	public override void Update()
	{
		
		if(Vector3.Distance(hero.Position,endPosition) < deltaSpace)
		{
			Vector3 forward = endPosition-hero.Position;
			forward = new Vector3(forward.x,0,forward.z);
			hero.Forward = forward;
			isFinish = true;
			return;
		}
		
		if(curStepIndex < stepsLen)
		{
			curPosition = new Vector3(hero.Position.x,curPosition.y,hero.Position.z);
			bool b =  KingSoftMath.MoveTowards(ref curPosition,steps[curStepIndex],speed*Time.deltaTime);
			Vector3 forward = curPosition-hero.Position;
			forward = new Vector3(forward.x,0,forward.z);
			if(forward.x !=0 || forward.z !=0)
				hero.Rotation = Quaternion.RotateTowards(hero.Rotation,Quaternion.LookRotation(forward),600f*Time.deltaTime);
			hero.Position = KingSoftCommonFunction.GetGoundHeight(curPosition);
			if(b)
			{
				curStepIndex++;	
				if(curStepIndex == stepsLen)
				{
					hero.Position = endPosition;
					isFinish = true;
				}
				else
				{
					SendNextDistance();
				}
			}
			else if( curStepIndex == stepsLen-1 &&  deltaSpace > 0 )
			{
				if(Vector3.Distance(curPosition,steps[curStepIndex]) < deltaSpace)
				{
					hero.Position = KingSoftCommonFunction.GetGoundHeight(curPosition);
					hero.Forward = forward;
					OnDistanceChange(hero.Position);
					isFinish = true;
				}
			}
		}
		else
		{
			isFinish = true;
		}
		
	}

	
}
