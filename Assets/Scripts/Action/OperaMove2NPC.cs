using UnityEngine;
using System.Collections;
using Assets.Scripts.View.Scene;
using Assets.Scripts.Controller;
using Assets.Scripts.Logic.Scene.SceneObject;
using Assets.Scripts.Logic.Scene.SceneObject.Compont;
using Assets.Scripts.Manager;
using Assets.Scripts.Logic.Scene;

public delegate void OnCatchNPCDel(SceneEntity skillTarget);
public class OperaMove2NPC  : Action {
	
	public Vector3 beginPosition;
	Vector3 endPosition;
	public SceneEntity targetHero;
	public float deltaSpace = 1.5f;
	public float speed = 5f;
	public bool isLock = false;
	Ticker ticker = new Ticker(1000);
	public OnCatchNPCDel del = null;
	ActiveMove action ;
	
	
	public OperaMove2NPC(SceneEntity hero):base("OperaMove2NPC",hero)
	{
		action = new ActiveMove(hero);
	}
	/// <summary>
	/// 激活行为.
	/// </summary>
	public override void Active()
	{	isFinish = false;
		beginPosition = hero.Position;
		if(null == targetHero)
		{
			isFinish = true;
			return;
		}
		endPosition = targetHero.Position;
		action.beginPosition = beginPosition;
		action.endPosition = endPosition;
		action.speed = speed;
		action.isLock = isLock;
		action.deltaSpace = deltaSpace;
		action.Active();
        hero.DispatchEvent(ControllerCommand.CrossFadeAnimation, hero.CharacterStateName(CharacterState.MOVE1));
		if (hero.property.isMainHero)
			hero.Net.SendHeroMove(endPosition);
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
		if(ticker.IsEnable())
		{
			if(Vector3.Distance(endPosition ,targetHero.Position)>0.3f)
				Active();
		}
		if(  action.IsFinish())
		{
			if( !isFinish)
			{
				
				if(null != targetHero )
				{
					hero.DispatchEvent(ControllerCommand.REACH_NPC);
					if (null != del)
					{
						hero.DispatchEvent(ControllerCommand.CrossFadeAnimation, hero.CharacterStateName(CharacterState.IDLE1));
						del(targetHero);
					}
                    
				}	
				isFinish = true;
			}
			
			return;
		}
		action.speed = speed;
		action.isLock = isLock;
		action.deltaSpace = deltaSpace;
		action.Update();
        EventRet ret = hero.DispatchEvent(ControllerCommand.IsPlayingActionFinish, hero.CharacterStateName(CharacterState.MOVE1) );
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
