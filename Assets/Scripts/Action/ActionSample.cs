using UnityEngine;
using System.Collections;
using Assets.Scripts.View.Scene;
using Assets.Scripts.Logic.Scene.SceneObject;

/// <summary>
/// 人物最小的行为.
/// </summary>
public class ActionSample  : Action {
	
	string active_name;
	public string NAME
	{
		get{ return active_name;}
	}
	public ActionSample(SceneEntity hero):base("ActionSample",hero)
	{
		
	}
	
	/// <summary>
	/// 激活行为.
	/// </summary>
	public override void Active()
	{
		base.Active();
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
