using UnityEngine;
using System.Collections;
using Assets.Scripts.View.Scene;
using Assets.Scripts.Controller;
using Assets.Scripts.Utils;
using Assets.Scripts.Logic.Scene.SceneObject;
using Assets.Scripts.Manager;

/// <summary>
/// 人物最小的行为.
/// </summary>
public class ActionByAnim  : AnimAction {
	
	public int SkillTime = 3000;
	public float curTime = 0;
	public bool notBegin = true;
	Ticker ticker = new Ticker(5000);
	
	ActionMoveDirect action = null;
	public ActionByAnim(SceneEntity hero):base("ActionSelfSkill",hero)
	{
	}
	/// <summary>
	/// 激活行为.
	/// </summary>
	public override void Active()
	{
		base.Active();
		ticker.Restart();
		ChangeDir();
		CrossFadeAnim();
		CheckSendSkill();
		PlayBeginFx(0);
	}
	public override bool TryFinish()
	{
		if(finishTicker.isStop())
		{
			isFinish = true;
			return true;
		}
		return base.TryFinish();
	}
	public override bool IsCanFinish()
	{
		if(finishTicker.isStop())
		{
			return true;
		}
		return base.IsCanFinish();
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
	/// <summary>
	/// Update this instance.
	/// </summary>
	public override void Update()
	{
		base.Update();
		
		if (null != action && !action.IsFinish())
			action.Update();
		
		if(isFinish)
			return;
		if(displayInfor.Param0!=0)
			ChangeDir();
		if(IsActionFinish()||ticker.IsActiveOneTime()){
			PlayEndFx(5f);
			isFinish = true;
		}
	}
	/// <summary>
	/// 每个行为结束时在这里做释放.
	/// </summary>
	public override void Release()
	{
		base.Release();
		Resources.UnloadUnusedAssets();
	}
	
}
