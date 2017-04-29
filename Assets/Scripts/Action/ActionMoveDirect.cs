using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.View.Scene;
using Assets.Scripts.Logic.Scene.SceneObject;

public class ActionMoveDirect  : Action {
	
	public Vector3 beginPosition;
	public Vector3 endPosition;
	public float deltaSpace = 0f;
	public float speed;
	public bool isLock = false;
	public float height = 5f;
	Vector3 curPosition ;
	Vector3 forward = Vector3.forward;
	public Vector3 FORWARD
	{
		get
		{
			return forward;
		}
	}
	
	public ActionMoveDirect(SceneEntity hero):base("ActionMoveDirect",hero)
	{
		
	}
	
	/// <summary>
	/// 激活行为.
	/// </summary>
	public override void Active()
	{
		base.Active();
		curPosition = beginPosition = hero.Position;
		forward = endPosition - beginPosition;
		forward = new Vector3(forward.x,0,forward.y);
	}
	public bool isForwardEnable()
	{
		return forward.x != 0 && forward.z != 0;
	}
	
	/// <summary>
	/// 每个行为结束时在这里做释放.
	/// </summary>
	public override void Release()
	{
		base.Release();
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
		bool b =  KingSoftMath.MoveTowards(ref curPosition,endPosition,speed*Time.deltaTime);
		if(b)
		{
			isFinish = true;
			hero.Position = KingSoftCommonFunction.GetGoundHeight(curPosition);
		}
		else
		{
			hero.Position = curPosition;
		}
	}

	
}