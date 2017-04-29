using UnityEngine;
using System.Collections;
using Assets.Scripts.View.Scene;
using Assets.Scripts.Logic.Scene.SceneObject;
using Assets.Scripts.Logic.Scene.SceneObject.Compont;
using Assets.Scripts.Manager;

public class ActionDie :  Action {
	
	public ActionDie(SceneEntity hero):base("ActionDie",hero)
	{
		isDead = true;
	}

	/// <summary>
	/// 激活行为.
	/// </summary>
	public override void Active()
	{
		base.Active();
        hero.DispatchEvent(ControllerCommand.CrossFadeAnimation, "dead", AMIN_MODEL.ONCE,false);
		isFinish = false;
        hero.DispatchEvent(ControllerCommand.CLEAR_BUFF);
	}
	public override void Update()
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
	public virtual bool IsFinish()
	{
		return false;
	}
	
}