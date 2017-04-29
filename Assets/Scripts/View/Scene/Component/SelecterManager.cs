using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Define;
using Assets.Scripts.Data;
using Assets.Scripts.Logic.Scene;
using Assets.Scripts.Logic.Scene.SceneObject;
using Assets.Scripts.Logic.Scene.SceneObject.Compont;
using Assets.Scripts.Lib.Loader;

public class SelecterManager : MonoBehaviour {
	
	
	static SelecterManager globalMgr = null;
	
	Vector3 delta  = new Vector3(0f,0.2f,0f);
	public static SelecterManager GetInstance()
	{
		if (null==globalMgr)
		{
			GameObject go = new GameObject("_SelecterManager");
			globalMgr = go.AddComponent<SelecterManager>();
			go.hideFlags = HideFlags.HideAndDontSave;
		}
		return globalMgr;
	}
	// Use this for initialization
	void Start () {
		GameObject.DontDestroyOnLoad(gameObject);
	}
	void ClearSelecter()
	{
		if( null != GlowComponent.globalPlayerSelectGameObject)
			GlowComponent.globalPlayerSelectGameObject.SetActive(false);
		if( null != GlowComponent.globalSelectGameObject)
			GlowComponent.globalSelectGameObject.SetActive(false);
	}
	// Update is called once per frame
	void LateUpdate () {
		
		if (null == SceneLogic.GetInstance().MainHero || null == SceneLogic.GetInstance().MainHero.property.target )
		{	
			ClearSelecter();
			return;
		}
		
		KParams kParams = KConfigFileManager.GetInstance().GetParams(); 
		if (!SceneLogic.GetInstance().MainHero.property.AutoAttack)
		{
			float distance = KingSoftMath.CheckDistanceXZ(SceneLogic.GetInstance().MainHero.property.target.Position , SceneLogic.GetInstance().MainHero.Position);
			if (distance > kParams.MaxEnemyDistance)
			{
				SceneLogic.GetInstance().MainHero.property.target = null;
				ClearSelecter();
				return;
			}
		}
		if (SceneLogic.GetInstance().MainHero.property.target.HeroType == KHeroObjectType.hotPlayer)
		{
			if( null != GlowComponent.globalPlayerSelectGameObject)
			{
				float _scale = SceneLogic.GetInstance().MainHero.property.target.heroSetting.Scale;
				GlowComponent.globalPlayerSelectGameObject.transform.localScale = new Vector3(_scale,_scale,_scale);
				ParticleSystemScaleManager.instance.Scale(_scale,GlowComponent.globalPlayerSelectGameObject);
				GlowComponent.globalPlayerSelectGameObject.SetActive(true);
				GlowComponent.globalPlayerSelectGameObject.transform.position = SceneLogic.GetInstance().MainHero.property.target.transform.position + delta ;
			}
			if( null != GlowComponent.globalSelectGameObject)
			{
				GlowComponent.globalSelectGameObject.SetActive(false);
			}
		}
		else
		{
			if( null != GlowComponent.globalPlayerSelectGameObject)
				GlowComponent.globalPlayerSelectGameObject.SetActive(false);
			if( null != GlowComponent.globalSelectGameObject && null != SceneLogic.GetInstance().MainHero.property.target.heroSetting)
			{
				float _scale = SceneLogic.GetInstance().MainHero.property.target.heroSetting.Scale;
				GlowComponent.globalSelectGameObject.transform.localScale = new Vector3(_scale,_scale,_scale);
				ParticleSystemScaleManager.instance.Scale(_scale,GlowComponent.globalSelectGameObject);
				GlowComponent.globalSelectGameObject.SetActive(true);
				GlowComponent.globalSelectGameObject.transform.position = SceneLogic.GetInstance().MainHero.property.target.transform.position + delta;
			}
		}
	}
}
