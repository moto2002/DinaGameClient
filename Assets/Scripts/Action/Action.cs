using UnityEngine;
using System.Collections;
using Assets.Scripts.View.Scene;
using Assets.Scripts.Logic.Scene.SceneObject;
using Assets.Scripts.Logic.Scene.SceneObject.Compont;
using Assets.Scripts.Proto;

/// <summary>
/// 人物最小的行为.
/// </summary>
public class Action  {
	public enum ACTION_TYPE
	{
		IDLE,
		ANIM,
		FLY,
		OPERA,
	}
	public SceneEntity target = null;
	public bool IsPushStack = false;
	protected SceneEntity hero = null;
	protected bool isFinish = true;
	protected string active_name;
	//protected float beginTime = 1f;
	public bool isDead = false;
	public bool isAidAction = false;
	public MessageCase  msg_case = new MessageCase();
	public ACTION_TYPE actionType = ACTION_TYPE.ANIM;
	public WeaponComponent.BIND_POINT WeaponPosition = WeaponComponent.BIND_POINT.DEFAULT;
	public bool isFlying = false;
	public string NAME
	{
		get{ return active_name;}
	}
	
	
	public virtual void AddDeadMessage(S2C_HERO_DEATH respond)
	{
		msg_case.AddDeadMessage(respond);
	}
	
	public virtual void AddSkillEffectMessage(S2C_SKILL_EFFECT respond,float _time)
	{
		msg_case.AddSkillEffectMessage(respond,_time);
	}
	
	public Action(string active_name,SceneEntity hero)
	{
		this.active_name = active_name;
		this.hero = hero;
	}
	
	

	/// <summary>
	/// 激活行为.
	/// </summary>
	public virtual void Active()
	{
		isFinish = false;
	}
	public virtual void PopMsg()
	{
		if (null !=msg_case )
			msg_case.PopMessage(hero);
	}
	/// <summary>
	/// 每个行为结束时在这里做释放.
	/// </summary>
	public virtual void Release()
	{
		
	}
	/// <summary>
	/// Determines whether this instance is can active.
	/// </summary>
	/// <returns>
	/// <c>true</c> if this instance is can active; otherwise, <c>false</c>.
	/// </returns>
	public virtual bool IsCanActive()
	{
		return true;
	}
	/// <summary>
	/// Update this instance.
	/// </summary>
	public virtual void Update()
	{
		
	}
	/// <summary>
	/// 尝试终止当前行为.
	/// </summary>
	public virtual bool TryFinish()
	{
		return isFinish;
	}
	public virtual bool IsCanFinish()
	{
		return isFinish;
	}
	/// <summary>
	/// 离开终止当前行为.
	/// </summary>
	public  virtual bool FinishImmediate()
	{
		isFinish = true;
		return true;
	}
	public virtual bool IsFinish()
	{
		return isFinish;
	}
	public virtual bool MoveToDistance(Vector3 position,float speed)
	{
		return false;
	}
}
