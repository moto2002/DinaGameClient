using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.View.Scene;
using Assets.Scripts.Logic.Scene.SceneObject;
using Assets.Scripts.Manager;

public class ActionDrag : Action {
	
	public Vector3 beginPosition;
	public Vector3 endPosition;
	public float speed = 2;
	public bool isLock = false;
	public float height = 5f;
	public float distance = 6f;
	Vector3 forward;
	ActionMoveDirect action ;
	public ActionDrag(SceneEntity hero):base("ActionDrag",hero)
	{
		isAidAction = true;
		action = new ActionMoveDirect(hero);
	}
	
	/// <summary>
	/// 激活行为.
	/// </summary>
	public override void Active()
	{
		base.Active();
		beginPosition = hero.Position;
		//endPosition = KingSoftCommonFunction.NearPosition(endPosition);
		action.beginPosition = beginPosition;
		action.endPosition = endPosition;
		action.height = height;
		action.speed = speed;
		action.isLock = isLock;
		action.Active();
		isFinish = false;
		if (hero.property.isMainHero)
			hero.DispatchEvent(ControllerCommand.HERO_MOVE);
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
		
		if(!action.IsFinish())
		{
			action.Update();
			if(action.isForwardEnable())
				hero.Forward = action.FORWARD;
		}
		
	}
	public override bool TryFinish()
	{
		return action.IsFinish();
	}
	public override bool IsCanFinish()
	{
		return action.IsFinish();
	}
	public override bool IsFinish()
	{
		return action.IsFinish();
	}

}
