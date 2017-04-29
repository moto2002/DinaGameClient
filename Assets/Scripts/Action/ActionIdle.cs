using UnityEngine;
using System.Collections;
using Assets.Scripts.View.Scene;
using Assets.Scripts.Controller;
using Assets.Scripts.Utils;
using Assets.Scripts.Logic.Scene.SceneObject;
using Assets.Scripts.Logic.Scene.SceneObject.Compont;
using Assets.Scripts.Manager;
using Assets.Scripts.Logic.Scene;

/// <summary>
/// Active move.
/// </summary>
public class ActionIdle : Action {
	public ActionIdle(SceneEntity hero):base("ActionIdle",hero)
	{
		actionType = ACTION_TYPE.IDLE;
	}
	/// <summary>
	/// 激活行为.
	/// </summary>
	public override void Active()
	{		base.Active();
		if(IsPushStack)
			hero.Net.SendHeroMove(hero.Position);
		
	}
	
	/// <summary>
	/// 每个行为结束时在这里做释放.
	/// </summary>
	public override void Release()
	{
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
        string animName = "idle1";
		if (null != hero.AnimCmp && !hero.AnimCmp.IsFighting() )
		{
			hero.property.fightHp = hero.property.hp;
			if (hero.HeroType == Assets.Scripts.Define.KHeroObjectType.hotPlayer)
			{
				animName = "idle2";
			}
		}
        EventRet ret = hero.DispatchEvent(ControllerCommand.IsPlayingActionFinish, animName,true);
        bool b = (bool)ret.GetReturn<AnimationComponent>();
		if(b)
		{
            hero.DispatchEvent(ControllerCommand.CrossFadeAnimation, animName,0.3f);
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

	
}
