using UnityEngine;
using System.Collections;
using Assets.Scripts.View.Scene;
using Assets.Scripts.Logic.Scene.SceneObject;
using Assets.Scripts.Logic.Scene.SceneObject.Compont;
using Assets.Scripts.Manager;

public class ActiionBeAttack :  Action {
	
	string hitAnim = "hit";
	ActionMoveDirect action = null;
	public ActiionBeAttack(SceneEntity hero):base("ActiionBeAttack",hero)
	{
	}

	/// <summary>
	/// 激活行为.
	/// </summary>
	public override void Active()
	{
		base.Active();
		hero.AnimCmp.StopAnim();
        hero.DispatchEvent(ControllerCommand.CrossFadeAnimation, hitAnim, AMIN_MODEL.ONCE,false);
		isFinish = false;
		if (!hero.property.isMainHero )
		{
			if (hero.Position != hero.property.finalDestination)
			{
				action = new ActionMoveDirect(hero);
				action.beginPosition = hero.Position;
				action.endPosition = hero.property.finalDestination;
				action.height = 0f;
				action.speed = 0.5f;
				action.Active();
			}
		}
	}
	public virtual bool TryFinish()
	{
		return true;
	}
	public override void Update()
	{
        EventRet ret = hero.DispatchEvent(ControllerCommand.IsPlayingActionFinish, hitAnim);
        bool b = (bool)ret.GetReturn<AnimationComponent>();
		if (b)
			isFinish = true;
		if (null != action)
			action.Update();
	}
	
	
	
}
