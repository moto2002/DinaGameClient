using UnityEngine;
using System.Collections;
using Assets.Scripts.View.Scene;
using Assets.Scripts.Controller;
using Assets.Scripts.Logic.Scene.SceneObject;
using Assets.Scripts.Logic.Scene.SceneObject.Compont;
using Assets.Scripts.Manager;
using Assets.Scripts.Logic.Scene;


public class OperaAttack : Action {
	
	Vector3 beginPosition;
	public Vector3 endPosition;
	public float deltaSpace = 1.5f;
	public ushort skillId = 1;
	
	public bool isLock = false;
	Ticker ticker = new Ticker(1000);
	public OnCatchNPCDel del = null;
	
	ActiveMove action ;
	bool hasTarget = false;
	
	public OperaAttack(SceneEntity hero):base("OperaAttack",hero)
	{
		actionType = Action.ACTION_TYPE.OPERA;
		action = new ActiveMove(hero);
	}
	
	/// <summary>
	/// 激活行为.
	/// </summary>
	public override void Active()
	{	isFinish = false;
		skillId = (ushort)hero.heroSetting.AttackSkill;
		beginPosition = hero.Position;
		if(null == target)
		{
			isFinish = true;
			return;
		}
		if(null != target)
		{
			endPosition = target.Position;
			hasTarget = true;	
		}
		hero.property.AutoAttack = true;
		action.beginPosition = beginPosition;
		action.endPosition = endPosition;
        action.speed = hero.Speed;
		action.deltaSpace = deltaSpace;
		action.Active();
		if (!action.IsFinish())
		{
        	hero.DispatchEvent(ControllerCommand.CrossFadeAnimation, hero.CharacterStateName(CharacterState.MOVE1));
			hero.Net.SendHeroMove(endPosition);
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
		if(hasTarget && null == target)
		{
			isFinish = true;
			return;
		}
		if(hasTarget || ticker.IsEnable())
		{
			if(Vector3.Distance(endPosition ,target.Position)>0.3f)
				Active();
		}
		if( action.IsFinish())
		{
			if( !isFinish)
			{
				SceneEntity _hero = hero as SceneEntity;
				if(null == target)
				{
                    _hero.Action.SendSkill(skillId, endPosition);
				}
				else
				{
					SceneEntity _target = target as SceneEntity;
                    _hero.Action.SendSkill(skillId, _target);
				}	
				isFinish = true;
			}
			return;
		}
        action.speed = hero.Speed;
		action.deltaSpace = deltaSpace;
		action.Update();
        EventRet ret = hero.DispatchEvent(ControllerCommand.IsPlayingActionFinish, hero.CharacterStateName(CharacterState.MOVE1));
        bool bRet = (bool)ret.GetReturn<AnimationComponent>();
        if(bRet)
		{
            hero.DispatchEvent(ControllerCommand.CrossFadeAnimation, hero.CharacterStateName(CharacterState.MOVE1));
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
	public override bool IsFinish()
	{
		return isFinish;
	}
}
