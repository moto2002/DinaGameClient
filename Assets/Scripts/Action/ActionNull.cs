using UnityEngine;
using System.Collections;
using Assets.Scripts.View.Scene;
using Assets.Scripts.Logic.Scene.SceneObject;

/// <summary>
/// 人物最小的行为.
/// </summary>
public class ActionNull  : Action {
	
	string active_name;
	public string NAME
	{
		get{ return active_name;}
	}
	public ActionNull(SceneEntity hero):base("ActionNull",hero)
	{
		isFinish = true;
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
	
	/// <summary>
	/// 激活行为.
	/// </summary>
	public override void Active()
	{
		base.Active();
		isFinish = true;
	}
	/// <summary>
	/// 每个行为结束时在这里做释放.
	/// </summary>
	public override void Release()
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
		return true;
	}
	/// <summary>
	/// Update this instance.
	/// </summary>
	public override void Update()
	{
		
	}

}
