using UnityEngine;
using System.Collections;
using Assets.Scripts.View.Scene;
using Assets.Scripts.Controller;
using Assets.Scripts.Logic.Scene.SceneObject;
using Assets.Scripts.Logic.Scene.SceneObject.Compont;
using Assets.Scripts.Manager;
using Assets.Scripts.Logic.Scene;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.Data;

public class ActionWalk  : Action {
	
	public Vector3 beginPosition;
	public Vector3 endPosition;
	public float deltaSpace = 0f;
	public float speed = 5f;
	public bool isLock = false;
	ActiveMove action ;
	float commonSpeed = 3f;
	
	public ActionWalk(SceneEntity hero):base("ActionWalk",hero)
	{
		action = new ActiveMove(hero);
	}
	
	
	public override bool MoveToDistance(Vector3 position,float speed)
	{
		beginPosition = hero.Position;
		action.beginPosition = beginPosition;
		action.endPosition = endPosition;
		action.Active();
		return true;
	}
	/// <summary>
	/// 激活行为.
	/// </summary>
	public override void Active()
	{
		
		KParams kParams = KConfigFileManager.GetInstance().GetParams();
		commonSpeed = kParams.CommonSpeed;
		beginPosition = hero.Position;
		
		if(hero.Position.x == endPosition.x && hero.Position.z == endPosition.z)
		{
			action.FinishImmediate();
			isFinish = true;
			return;
		}
		else
		{
			action.beginPosition = beginPosition;
			action.endPosition = endPosition;
			action.speed = speed;
			action.isLock = isLock;
			action.deltaSpace = deltaSpace;
			action.distanceChangeFun = OnDistanceChangeFun;
			action.Active();
			endPosition = action.endPosition;
		}
		if (hero.property.isMainHero)
			hero.DispatchEvent(ControllerCommand.HERO_MOVE);
        hero.DispatchEvent(ControllerCommand.CrossFadeAnimation, hero.CharacterStateName(CharacterState.MOVE1), AMIN_MODEL.LOOP, true);
		
	}
	protected  void OnDistanceChangeFun(Vector3 v)
	{
        if (hero.property.isMainHero)
			hero.Net.SendHeroMove(v);
	}
	
	/// <summary>
	/// 每个行为结束时在这里做释放.
	/// </summary>
	public override void Release()
	{
		if (hero.property.isMainHero)
			hero.DispatchEvent(ControllerCommand.STOP_MOVE);
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
		if (null != hero.AnimCmp && !hero.AnimCmp.IsFighting() )
		{
			hero.property.fightHp = hero.property.hp;
		}
		
		action.speed = hero.Speed;
		action.isLock = isLock;
		action.deltaSpace = deltaSpace;
		action.Update();
        EventRet ret = hero.DispatchEvent(ControllerCommand.IsPlayingActionFinish, hero.CharacterStateName(CharacterState.MOVE1));
		float _time = action.speed / commonSpeed;
		if( hero.AnimCmp.IsSpeedStackEmpty() )
			hero.AnimCmp.SetSpeed(_time);
        bool b = (bool)ret.GetReturn<AnimationComponent>();
		if(b)
		{
			hero.DispatchEvent(ControllerCommand.CrossFadeAnimation,hero.CharacterStateName(CharacterState.MOVE1));
		}
		if(action.IsFinish())
		{
            hero.DispatchEvent(ControllerCommand.CrossFadeAnimation, hero.CharacterStateName(CharacterState.IDLE1));
			return;
		}
		
	}
	public override bool TryFinish()
	{
		isFinish = true;
		return true;
	}
	public override bool IsCanFinish()
	{
		return true;
	}
	public  virtual bool FinishImmediate()
	{
		isFinish = true;
        hero.DispatchEvent(ControllerCommand.CrossFadeAnimation, hero.CharacterStateName(CharacterState.IDLE1));
		return true;
	}
	public override bool IsFinish()
	{
		if(action.IsFinish())
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}
