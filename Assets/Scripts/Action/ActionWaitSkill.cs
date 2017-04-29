using UnityEngine;
using System.Collections;
using Assets.Scripts.View.Scene;
using Assets.Scripts.Controller;
using Assets.Scripts.Utils;
using Assets.Scripts.Logic.Scene.SceneObject;
using Assets.Scripts.Manager;
using Assets.Scripts.Logic.Scene;
using Assets.Scripts.Logic.Scene.SceneObject.Compont;

public class ActionWaitSkill  :  Action {
	
	public ushort skillId = 1;
	public Vector3 position ;
	public SceneEntity target = null;
	Ticker ticker = new Ticker(5000);
	public ActionWaitSkill(SceneEntity hero):base("ActionWaitSkill",hero)
	{
		
	}
	
	
	
	
	/// <summary>
	/// 激活行为.
	/// </summary>
	public override void Active()
	{
		base.Active();
		hero.DispatchEvent(ControllerCommand.CrossFadeAnimation, hero.CharacterStateName(CharacterState.IDLE1));
		
		isFinish = false;
		if(hero.property.isMainHero)
		{
            hero.Net.SendHeroMove(hero.Position);
			hero.DispatchEvent(ControllerCommand.HERO_MOVE);
		}
		
		if(null == target)
		{
			hero.Net.SendCastSkill(skillId,0,position);
		}
		else
		{
            hero.Net.SendCastSkill(skillId, target.property.Id, target.Position);
		}
		ticker.Restart();
		
	}
	public override void Update()
	{
		if(ticker.IsEnable())
		{
			isFinish = true;
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
		return isFinish;
	}
	public override bool TryFinish()
	{
		return false;
	}
	public override bool IsCanFinish()
	{
		return false;
	}
	/// <summary>
	/// 离开终止当前行为.
	/// </summary>
	public  override bool FinishImmediate()
	{
		isFinish = true;
		return isFinish;
	}
	public virtual bool IsFinish()
	{
		return false;
	}
	
}