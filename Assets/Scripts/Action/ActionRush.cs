using UnityEngine;
using System.Collections;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.Data;
using Assets.Scripts.Controller;
using Assets.Scripts.Utils;
using Assets.Scripts.Logic.Scene;
using Assets.Scripts.Manager;
using Assets.Scripts.Logic.Scene.SceneObject;

public class ActionRush : AnimAction {
	
	ActionThrowUp action ;
	private Vector3 beginPosition;
	public float speed ;
	public bool isLock = false;
	public float distance ;
	bool Jumpping ;
	Ticker ticker = new Ticker();
	Ticker animTicker = new Ticker();
	int animIndex = 0;
	static FxAsset fx = new FxAsset();
	public override bool MoveToDistance(Vector3 position,float speed)
	{
		return true;
	}
	
	//
	
	public ActionRush(SceneEntity hero):base("ActionRush",hero){
		action = new ActionThrowUp(hero);	
	}
	public override void InitParam(AnimActionParam param,KSkillDisplay skillDisplay)
	{
		isRandomAnim = false;
		base.InitParam(param,skillDisplay);
	}
	public override void Active()
	{
		base.Active();
		ticker.Stop();
		ticker.cd = (int)(displayInfor.hitDelay*1000);
		Jumpping = true;
		beginPosition = hero.Position;
		hero.DispatchEvent(ControllerCommand.LookAtPos,endPosition);
		float distance = Vector3.Distance(beginPosition,endPosition);
		Vector3 forward = endPosition - beginPosition;
		action.beginPosition = beginPosition;
  		action.endPosition = endPosition;
		action.height = displayInfor.Param0;
		action.speed = speed;
		action.isLock = isLock;
		action.Active();
		isFinish = false;
		
		
		CrossFadeAnim();
		CheckSendSkill();
 		PlayBeginFx(0);
		animTicker.Restart(); 
		
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
        action.speed = hero.Speed;
		if (animIndex == 0 && IsActionFinish() )
		{
			CrossFadeAnim(++animIndex);
			PlayBeginFx(animIndex);
		}
		if(action.IsFinish())
		{
			if(Jumpping)
			{
				Jumpping = false;
				ticker.Restart();
				PlayEndFx(5f);
			}
			if(ticker.IsActiveOneTime()){
				isFinish = true;
			}
		}
		else
		{
			action.Update();
		}
	}
	
	public override bool TryFinish()
	{
		/*if(!Jumpping)
		{
			isFinish = true;
			return true;
		}*/
		return isFinish;
	}
	public override bool IsCanFinish()
	{
		/*if(!Jumpping)
		{
			return true;
		}*/
		return isFinish;
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
