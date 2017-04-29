using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.View.Scene;
using Assets.Scripts.Logic.Scene.SceneObject;
using Assets.Scripts.Manager;

/// <summary>
/// Active move.
/// </summary>
public class ActionJump : Action {
	
	public Vector3 beginPosition;
	public Vector3 endPosition;
	public float speed = 2;
	public bool isLock = false;
	public float height = 5f;
	public float distance = 6f;
	
	ActionThrowUp action ;
	Vector3 forward ;
	public ActionJump(SceneEntity hero):base("ActionJump",hero)
	{
		action = new ActionThrowUp(hero);
	}
	
	public override bool MoveToDistance(Vector3 position,float speed)
	{
		return true;
	}
	/// <summary>
	/// 激活行为.
	/// </summary>
	public override void Active()
	{
		base.Active();
		//hero.CrossFadeAnimation(hero.CharacterStateName(CharacterState.MOVE1));
		beginPosition = hero.Position;
		//endPosition = KingSoftCommonFunction.NearPosition(endPosition);
		hero.DispatchEvent(ControllerCommand.LookAtPos, endPosition);
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
		hero.Forward = forward;
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
