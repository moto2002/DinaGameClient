using UnityEngine;
using System.Collections;
using Assets.Scripts.View.Scene;
using Assets.Scripts.Logic.Scene.SceneObject;
using Assets.Scripts.Logic.Scene.SceneObject.Compont;
using Assets.Scripts.Manager;
using Assets.Scripts.Data;
using Assets.Scripts.Lib.Loader;

public class ActionBeHitAndFly :  Action {
	
	public float time = 2f;
	public float height = 1.5f;
	public Vector3 endPosition ;
	string hitAnim = "hitfly";
	ActionThrowUp action = null;
	public float delta = 1f;
	public float deltaTime = 0f;
	public ActionBeHitAndFly(SceneEntity hero):base("ActionBeAttactedAndFly",hero)
	{
		actionType = ACTION_TYPE.FLY;
	}

	/// <summary>
	/// 激活行为.
	/// </summary>
	public override void Active()
	{
		base.Active();
        hero.DispatchEvent(ControllerCommand.CrossFadeAnimation, hitAnim);
		isFinish = false;
		action = new ActionThrowUp(hero);
		action.beginPosition = hero.Position;
		action.endPosition = KingSoftCommonFunction.NearPosition(endPosition); 
		KingSoftCommonFunction.LootAt(hero.gameObject,endPosition);
		hero.transform.forward = -hero.transform.forward;
		KParams kParams = KConfigFileManager.GetInstance().GetParams();
		action.height = kParams.HitHeight;
		action.changeForward = false;
		
		action.type = ActionThrowUp.ThrowUpType.DISTANCE;
		
		action.totalTime = time;
		action.Active();
		endPosition = action.endPosition;
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
			deltaTime += Time.deltaTime;
			if (deltaTime > delta)
			{
				isFinish = true;
				hero.transform.position = endPosition;
				hero.BodyGo.transform.localPosition = Vector3.zero;
				hero.DispatchEvent(ControllerCommand.CrossFadeAnimation, hitAnim);
			}
			else
			{
				hero.AnimCmp.StopAnim();
				hero.transform.position = endPosition;
				//hero.BodyGo.transform.localPosition = Vector3.up* Mathf.Sin(Mathf.PI*2*deltaTime/0.1f)*0.02f;
			}
			return;
		}
		if (null != action)
			action.Update();
	}	
}