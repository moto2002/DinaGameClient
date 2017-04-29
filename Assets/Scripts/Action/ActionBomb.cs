using UnityEngine;
using System.Collections;
using Assets.Scripts.View.Scene;
using Assets.Scripts.Logic.Scene.SceneObject;
using Assets.Scripts.Logic.Scene.SceneObject.Compont;
using Assets.Scripts.Manager;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.Utils;
using Assets.Scripts.Logic.Scene;

public class ActionBomb:  Action {
	
	public ActionBomb(SceneEntity hero):base("ActionBomb",hero)
	{
		isDead = true;
	}

	/// <summary>
	/// 激活行为.
	/// </summary>
	public override void Active()
	{
		base.Active();
       	AssetInfo infor =  AssetLoader.GetInstance().PreLoad(URLUtil.GetEffectPath("effect_guaiwusiwang2"));
		if (infor.isDone() && null != infor.bundle)
		{
			GameObject fx = GameObject.Instantiate(infor.bundle.mainAsset) as GameObject;
			fx.transform.position = hero.transform.position;
			fx.transform.rotation = hero.transform.rotation;
			
			float _scale = hero.heroSetting.Scale;
			/*float _scale = hero.property.characterController.radius * hero.transform.localScale.y/ 0.6f;*/
			fx.transform.localScale = new Vector3(_scale,_scale,_scale);
			ParticleSystemScaleManager.instance.Scale(_scale,fx);
			if (null != hero.BodyGo)
				hero.BodyGo.SetActive(false);
		}
		else
		{
			ActionDie die = new ActionDie(hero);
			hero.ActiveAction = die;
			return;
		}
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