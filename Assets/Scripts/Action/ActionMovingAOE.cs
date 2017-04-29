using UnityEngine;
using System.Collections;
using Assets.Scripts.View.Scene;
using Assets.Scripts.Controller;
using Assets.Scripts.Utils;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.Data;
using Assets.Scripts.Logic.Scene.SceneObject;
using Assets.Scripts.Manager;

/// <summary>
/// 人物最小的行为.
/// </summary>
public class ActionMovingAOE  : AnimAction {
	//effect_skill_daobin_02_dafengche.res ..	
	Ticker ticker = new Ticker(3000);
	ActiveMove actionMove ;
	public ActionMovingAOE(SceneEntity hero):base("ActionMovingAOE",hero){
		actionMove = new ActiveMove(hero);
	}
	public override bool MoveToDistance(Vector3 position,float speed){
		actionMove.endPosition = position;
		actionMove.speed = speed;
		actionMove.Active();
		CheckShendPos(position);
		return true;
	}
	
	public override void Active()
	{
		if(null != activeSkill){
			ticker.SetCD(activeSkill.SpellTime);
		}
		ticker.Restart();
		base.Active();
		CrossFadeAnim();
		CheckSendSkill();
		PlayBeginFx(0);
	}
	public override bool IsCanActive()
	{	
		return isFinish;
	}
	public override void Update()
	{
		base.Update();
		if(isFinish)
			return;
		if(!actionMove.IsFinish())
		{
			actionMove.Update();
		}
		KeepActionPlay();
		if(ticker.IsActiveOneTime())
		{
			try
			{
				CheckShendPos(hero.Position);
				DestroyFx();
				hero.DispatchEvent(ControllerCommand.CrossFadeAnimation,hero.CharacterStateName(CharacterState.IDLE1));
			}
			catch(System.Exception e)
			{
				Debug.Log(e.ToString());
			}
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
		DestroyFx();
		Resources.UnloadUnusedAssets();
	}
	
}
