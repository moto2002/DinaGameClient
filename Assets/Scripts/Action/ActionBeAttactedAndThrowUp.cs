using UnityEngine;
using System.Collections;
using Assets.Scripts.View.Scene;
using Assets.Scripts.Logic.Scene.SceneObject;
using Assets.Scripts.Logic.Scene.SceneObject.Compont;
using Assets.Scripts.Manager;
using Assets.Scripts.Data;
using Assets.Scripts.Lib.Loader;

public class ActionBeAttactedAndThrowUp :  Action {
	
	public float time = 2f;
	public float height = 1.5f;
	public string hitAnim = "hitfly";
	ActionThrowUp action = null;
	public float delta = 1f;
	public float deltaTime = 0f;
	public ActionBeAttactedAndThrowUp(SceneEntity hero):base("ActionBeAttactedAndFly",hero)
	{
		actionType = ACTION_TYPE.FLY;
	}

	/// <summary>
	/// 激活行为.
	/// </summary>
	public override void Active()
	{
		base.Active();
		if (hitAnim.Length > 0)
        	hero.DispatchEvent(ControllerCommand.CrossFadeAnimation, hitAnim);
		isFinish = false;
		action = new ActionThrowUp(hero);
		action.beginPosition = hero.Position;
		action.endPosition = KingSoftCommonFunction.NearPosition(hero.Position); 
		KParams kParams = KConfigFileManager.GetInstance().GetParams();
		action.height = kParams.HitHeight;
		action.changeForward = false;
		
		action.type = ActionThrowUp.ThrowUpType.TIME;		
		action.totalTime = time;
		action.Active();
		
		if (hero.property.isMainHero)
			hero.DispatchEvent(ControllerCommand.HERO_MOVE);

	}
	public override void Release()
	{
	}
	
	public override void Update()
	{
		isFlying = true;
		if (action.IsFinish())
		{
			isFinish = true;
			return;
		}
		if (null != action)
			action.Update();
	}	
}