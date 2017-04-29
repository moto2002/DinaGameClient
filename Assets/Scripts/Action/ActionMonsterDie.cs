using UnityEngine;
using System.Collections;
using Assets.Scripts.Logic.Scene.SceneObject;
using Assets.Scripts.Manager;
using Assets.Scripts.Logic.Scene.SceneObject.Compont;
using Assets.Scripts.Data;
using Assets.Scripts.Lib.Loader;

public class ActionMonsterDie : Action {
	
	ActionThrowUp jump;
	public SceneEntity attacker;
	public float distance = 2f;
	public float height =  2f;
	public float speed = 2f;
	public float playSpeed = 1f;
	public Vector3 lookForward;
	Vector3 attackerPosition;
	
	public ActionMonsterDie(SceneEntity hero):base("ActionDie",hero)
	{
		jump = new ActionThrowUp(hero);
		jump.changeForward = false;
		isDead = true;
	} 

	
	public override void Active()
	{
		
		base.Active();
		KSkillDisplay skillDisplay = KConfigFileManager.GetInstance().GetSkillDisplay(hero.property.lastHitSkillId,hero.property.tabID);
		Vector3 forward = hero.Position - attacker.Position;
		forward.Normalize();
		jump.beginPosition = hero.Position;
		jump.dampen = true;
		if (null==skillDisplay)
		{
			height = 0;
			distance = 0;
			speed = 100f;
			jump.endPosition = KingSoftCommonFunction.NearPosition(hero.Position);
		}
		else
		{
			height = skillDisplay.DieJump;
			distance = skillDisplay.DieDistance;
			speed = skillDisplay.DieSpeed;
			jump.endSpeed = skillDisplay.DieSpeed2;
			jump.endPosition = KingSoftCommonFunction.NearPosition(hero.Position + forward*distance);
		}
		lookForward = new Vector3(-forward.x,0f,-lookForward.z);
		jump.speed = speed;
		attackerPosition = attacker.transform.position;
		jump.height = height;
        jump.Active();
		hero.DispatchEvent(ControllerCommand.CrossFadeAnimation, "dead", AMIN_MODEL.ONCE,false);
	}
	public override void Update()
	{
		if ( jump.IsFinish() )
		{
			if (null != hero && null != attacker)
			{
				Vector3 forward = attackerPosition - hero.Position ;
				forward = new Vector3(forward.x,0f,forward.z);
				hero.Forward = forward;
			}
		}
		else
		{
			jump.Update();
		}
	}
	
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