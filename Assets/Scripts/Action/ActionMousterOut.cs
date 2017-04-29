
using UnityEngine;
using System.Collections;
using Assets.Scripts.View.Scene;
using Assets.Scripts.Logic.Scene.SceneObject;
using Assets.Scripts.Logic.Scene.SceneObject.Compont;
using Assets.Scripts.Manager;
using Assets.Scripts.Data;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.Utils;

public class ActionMousterOut :  Action {
	
	bool notInit = true;
	float beginTime = 0f;
	float totalTime = 1f;
	Color outColor = new Color32(25,0,50,255);
	public FxAsset assert = new FxAsset();
	float MonsterOutHeight;
	
	public ActionMousterOut(SceneEntity hero):base("ActionMousterOut",hero)
	{
		isDead = true;
	}
	
	/// <summary>
	/// 激活行为.
	/// </summary>
	public override void Active()
	{
		//FxAsset assert = new FxAsset();
        //assert.init(URLUtil.GetResourceLibPath() + buffInfor.BuffPath);
		
		base.Active();
		KParams kParams = KConfigFileManager.GetInstance().GetParams();
		outColor = KingSoftCommonFunction.StringToColor(kParams.MonsterOutColor);
        totalTime = kParams.MonsterOutTime;
		MonsterOutHeight = kParams.MonsterOutHeight;
		if (kParams.MonsterOutFx.Length > 0)
		{
			assert.init(URLUtil.GetResourceLibPath() + kParams.MonsterOutFx);
			GameObject fx = assert.CloneObj();
			if (null != fx)
			{
				fx.transform.position = hero.Position;
				fx.transform.localScale = hero.transform.localScale*kParams.MonsterOutFxScale;
				DestoryObject d = fx.AddComponent<DestoryObject>();
				d.delta = 1f;
			}
		}
		hero.DispatchEvent(ControllerCommand.CrossFadeAnimation, "idle1", AMIN_MODEL.ONCE,false);
		isFinish = false;
        hero.DispatchEvent(ControllerCommand.CLEAR_BUFF);
		beginTime = Time.realtimeSinceStartup;
	}
	public override void Update()
	{
		if (isFinish)
			return;
		float t = Time.realtimeSinceStartup - beginTime;
		if (t < totalTime)
		{
			if (null != hero.BodyGo)
			{
				if(notInit)
				{
					notInit = false;
					SkinnedMeshRenderer [] rds = hero.BodyGo.GetComponentsInChildren<SkinnedMeshRenderer>();
					foreach (SkinnedMeshRenderer rd in rds)
					{
						rd.material.SetColor("_Emission",outColor) ;
					}
				}
				hero.BodyGo.transform.localPosition = new Vector3(0f,MonsterOutHeight*(1f-t/totalTime),0f);
			}
		}
		else
		{
			if (null != hero.BodyGo)
				hero.BodyGo.transform.localPosition = Vector3.zero;
			isFinish = true;
		}
        
	}
	public override void Release()
	{
		if( null == hero.BodyGo )
			return;
		hero.BodyGo.transform.localPosition = Vector3.zero;
		SkinnedMeshRenderer [] rds = hero.BodyGo.GetComponentsInChildren<SkinnedMeshRenderer>();
		foreach (SkinnedMeshRenderer rd in rds)
		{
			rd.material.SetColor("_Emission",Color.black) ;
		}
		
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
		return isFinish;
	}
	public override bool IsCanFinish()
	{
		return isFinish;
	}
	/// <summary>
	/// 离开终止当前行为.
	/// </summary>
	public virtual bool IsFinish()
	{
		return isFinish;
	}
	
}