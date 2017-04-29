using UnityEngine;
using System.Collections;
using Assets.Scripts.View.Scene;
using Assets.Scripts.Controller;
using Assets.Scripts.Utils;
using Assets.Scripts.Logic.Scene.SceneObject;
using Assets.Scripts.Manager;
using Assets.Scripts.Logic.Scene.SceneObject.Compont;

public class ActionDead :  Action {
	public ActionDead(SceneEntity hero):base("ActionDie",hero)
	{
		isDead = true;
	}
	/// <summary>
	/// 激活行为.
	/// </summary>
	public override void Active()
	{
		base.Active();
        hero.DispatchEvent(ControllerCommand.CrossFadeAnimation, "hitfly", AMIN_MODEL.ONCE,false);
        hero.DispatchEvent(ControllerCommand.CLEAR_BUFF);
		hero.AnimCmp.StopAnim();
		isFinish = false;
	}
	public override void Update()
	{
		hero.AnimCmp.StopAnim();
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
	public virtual bool IsFinish()
	{
		return false;
	}
	
}