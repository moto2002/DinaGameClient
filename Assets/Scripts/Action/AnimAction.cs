using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.View.Scene;
using Assets.Scripts;
using Assets.Scripts.Controller;
using Assets.Scripts.Utils;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.Data;
using Assets.Scripts.Logic.Scene.SceneObject;
using Assets.Scripts.Logic.Scene.SceneObject.Compont;
using Assets.Scripts.Manager;
using Assets.Scripts.Logic.Scene;
public class AnimActionParam
{
	public ushort skillId = 1;
	public uint targetId = 0;
	public uint level = 1;
	public Vector3 position;
	public SceneEntity target = null;
}
/// <summary>
/// 人物最小的行为.
/// </summary>
public class AnimAction  : Action {
	
	protected KSkillDisplay displayInfor;
	protected ushort skillId = 0;
	protected uint targetId = 0;
	protected uint level = 1;
	protected string [] animArys;
	protected string [] beginFxs;
	protected Vector3 endPosition;
	protected KActiveSkill activeSkill;
	protected float FirstAttackTime;
	GameObject BeginFXObj;
	GameObject EndFxObj;
	protected Ticker hitTicker = new Ticker();
	protected Ticker finishTicker = new Ticker();
	protected Ticker shakeTicker = new Ticker(100000);
	protected List<float> shakeTimes = new List<float>();
	public bool isRandomAnim = true;
	
	
	
	public override void Active()
	{
		base.Active();
		if (displayInfor.WeaponTrail)
		{
			hero.DispatchEvent(ControllerCommand.WEAPON_TRAIL, true);
		}
		shakeTicker.Restart();
		if ( displayInfor.Opera.CompareTo("DIR") == 0 )
		{
			Vector3 forward = endPosition - hero.Position   ;
			forward = new Vector3(forward.x,0,forward.z);
			hero.Forward = forward;
		}
		hitTicker.Restart();
		finishTicker.Restart();
		if (displayInfor.CameraEffect.CompareTo("SHAKE_BEGIN") == 0)
		{
			Shake();
		}
		if (displayInfor.GhostShadow)
		{
			hero.DispatchEvent(ControllerCommand.GHOST_SHADOW,true);
		}
		if (displayInfor.SoundType == KSkillDisplay.ACTION_AUIDO_TYPE.Begin)
		{
			if( displayInfor.Sound.Length>0 )
			{	
				AudioManager.instance.PlaySound3d(displayInfor.Sound,hero.Position);
			}
		}
	}
	public override void Release()
	{
		if (displayInfor.GhostShadow)
		{
			hero.DispatchEvent(ControllerCommand.GHOST_SHADOW,false);
		}
		base.Release();
		hero.DispatchEvent(ControllerCommand.WEAPON_TRAIL, false);
	}
	public virtual void InitParam(AnimActionParam param,KSkillDisplay skillDisplay)
	{
		displayInfor = skillDisplay;
		WeaponPosition =  displayInfor.WeaponPosition;
		skillId = param.skillId;
		targetId = param.targetId;
		level = param.level;
		target = param.target;
		endPosition = KingSoftCommonFunction.GetGoundHeight(KingSoftCommonFunction.NearPosition(param.position));
		activeSkill = KConfigFileManager.GetInstance().GetActiveSkill(skillId,level);
		FirstAttackTime = 0;
		animArys = displayInfor.Anim.Split('|');
		int [] ids = KingSoftCommonFunction.RandomAry(animArys.Length);
		beginFxs = displayInfor.BeginEffect.Split('|');
		beginFxs = KingSoftCommonFunction.GrowArrays(beginFxs,animArys.Length);
		if (isRandomAnim)
		{
			animArys = KingSoftCommonFunction.RandomAryByList(animArys,ids);
			beginFxs = KingSoftCommonFunction.RandomAryByList(beginFxs,ids);
		}
		if (beginFxs[0].Length>0)
            AssetLoader.GetInstance().PreLoad(URLUtil.GetResourceLibPath() + beginFxs[0]);
		if (displayInfor.EndEffect.Length>0)
            AssetLoader.GetInstance().PreLoad(URLUtil.GetResourceLibPath() + displayInfor.EndEffect);
		if (displayInfor.BulletEffect.Length>0)
            AssetLoader.GetInstance().PreLoad(URLUtil.GetResourceLibPath() + displayInfor.BulletEffect);
		
		try
		{
			string [] AttackTimes = activeSkill.AttackTimeList.Split(';');
			if (displayInfor.CameraEffect.CompareTo("SHAKE_ALL") == 0 )
			{
				foreach (string _t in AttackTimes)
				{
					try
					{
						shakeTimes.Add((float)System.Convert.ToDouble(_t));
					}
					catch (System.Exception e)
					{
					}
				}
				if (shakeTimes.Count>0)
				{
					FirstAttackTime = (float)shakeTimes[0];	
				}
			}
			else
			{
				if (AttackTimes.Length>0)
				{
					try
					{
						FirstAttackTime = (float)System.Convert.ToDouble(AttackTimes[0]);	
					}
					catch(System.Exception e)
					{
					}
				}
				
			}
			if (activeSkill.ClientCache)
			{
				KAminEvent _event = KConfigFileManager.GetInstance().GetAnimEvent(hero.property.tabID,animArys[0]);
				if (null == _event)
				{
					FirstAttackTime = hero.AnimCmp.GetAnimLong(animArys[0])-0.3f;
				}
				else
				{
					FirstAttackTime = _event.time;
				}
			}		
		}
		catch (System.Exception e){
		}
		hitTicker.SetCD(FirstAttackTime);
		finishTicker.SetCD(FirstAttackTime+displayInfor.hitDelay);
	}
	
	void Shake()
	{
		if (hero.property.isMainHero)
		{
			if (displayInfor.CameraShakeScale > 0 && displayInfor.CameraShakeTime > 0)
			{
				if (displayInfor.CameraShakeFile.Length > 0)
					SceneCamera.Shake(displayInfor.CameraShakeFile,displayInfor.CameraShakeTime,displayInfor.CameraShakeSpeed,displayInfor.CameraShakeScale);
				else
					SceneCamera.Shake(displayInfor.CameraShakeTime,displayInfor.CameraShakeScale,SHAKE_TYPE.SUDDENLY);
			}
		}
	}
	void OnHit()
	{
		if (displayInfor.CameraEffect.CompareTo("SHAKE_HIT") == 0 )
		{
			Shake();
		}
		if (displayInfor.BulletEffect.Length > 0 && target != null)
		{
			hero.property.AutoAttack = false;
			
			GameObject bulletObj = null;
			AssetInfo inf = AssetLoader.GetInstance().Load(URLUtil.GetResourceLibPath() + displayInfor.BulletEffect);
            if (inf.isDone(false))
            {
				bulletObj = inf.CloneGameObject();
			}
			else
			{
				bulletObj = new GameObject();
			}
			bulletObj.transform.position =  hero.transform.position;
			bulletObj.SetActive(true);
			KingSoftCommonFunction.SetLayer(bulletObj,11);
			Bullet bullet = bulletObj.AddComponent<Bullet>();
			bullet.msg_case = msg_case;
			msg_case = null;
			bullet.displayInfor = displayInfor;
			bullet.hero = hero;
			bullet.target = target;
			bullet.speed = (activeSkill.SkillFlySpeed / 100);
			bullet.hitFx = displayInfor.HitEffect;
		}
		else
		{
			if (displayInfor.SoundType == KSkillDisplay.ACTION_AUIDO_TYPE.Hit)
			{
				if( displayInfor.Sound.Length>0 )
				{
					AudioManager.instance.PlaySound3d(displayInfor.Sound,hero.Position);
				}
			}
			PopMsg();
		}
		
	}
	
	public override void PopMsg()
	{
		if (null == msg_case)
			return;
		if (displayInfor.HitDelayTimeScale > 0.00001f  )
		{
			
			msg_case.PopMessage(hero,displayInfor.HitDelayTimeScale);
		}
		else
		{
			msg_case.PopMessage(hero);
		}
	}
	public override void Update()
	{
		if (shakeTimes.Count>0)
		{
			float t = shakeTimes[0];
			if (shakeTicker.GetEnableTime() > t)
			{
				Shake();
				shakeTimes.RemoveAt(0);
			}
		}
		finishTicker.IsActiveOneTime();
		if (hitTicker.IsActiveOneTime())
		{
			OnHit();
		}	
	}
	public void ChangeDir()
	{
		if (null != target)
			hero.DispatchEvent(ControllerCommand.LookAtPos, target.Position);
	}
	public void CheckSendSkill()
	{
		if (IsPushStack)
		{
            hero.Net.SendCastSkill(skillId, targetId, hero.Position);
		}
	}
	public void CheckShendPos(Vector3 position)
	{
        if (hero.property.isMainHero)
			hero.Net.SendHeroMove(position);
	}
	
	public AnimAction(string actionName,SceneEntity hero):base(actionName,hero)
	{
	}
	public void PlayBeginFx(int index = 0)
	{
		float second = 6f;
		if(beginFxs.Length > index && beginFxs[index].Length > 0)
		{
			
			AssetInfo inf = AssetLoader.GetInstance().Load(URLUtil.GetResourceLibPath() + beginFxs[index]);
            if (inf.isDone(false))
            {
				BeginFXObj = inf.CloneGameObject();
				BeginFXObj.transform.parent =  hero.transform;
				BeginFXObj.transform.localPosition = Vector3.zero;
				ObjectUtil.SetTagWithAllChildren(BeginFXObj, CameraLayerManager.GetInstance().GetMissionSignName());
				
				if (displayInfor.BeginEffectBindPoint.Length == 0 || displayInfor.BeginEffectBindPoint.CompareTo("Ground")==0)
				{
					BeginFXObj.transform.localScale = Vector3.one;
					BeginFXObj.transform.localRotation = Quaternion.identity;
				}
				else
				{
					Transform t = hero.GetChildTransform(displayInfor.BeginEffectBindPoint);
					if (null != t)
					{
						BeginFXObj.transform.parent = t;
						BeginFXObj.transform.localPosition = Vector3.zero;
						BeginFXObj.transform.localScale = Vector3.one;
						BeginFXObj.transform.localRotation = Quaternion.identity;
					}
				}
				if (!displayInfor.BeginEffectBind)
				{
					BeginFXObj.transform.parent = hero.transform.parent;
				}
				ParticleSystemScaleManager.ScaleParticle(BeginFXObj);
				
				KingSoftCommonFunction.SetLayer(BeginFXObj,11);
				BeginFXObj.SetActive(true);
				DestoryObject dos = BeginFXObj.AddComponent<DestoryObject>();
				dos.delta = second;
			}
		}
	}
	public void PlayEndFx(float second)
	{
		if (displayInfor.EndEffect.Length>0)
		{
			AssetInfo inf = AssetLoader.GetInstance().Load(URLUtil.GetResourceLibPath() + displayInfor.EndEffect);
            if (inf.isDone(false))
            {
				EndFxObj = inf.CloneGameObject();
				EndFxObj.transform.parent =  hero.transform;
				EndFxObj.transform.localPosition = Vector3.zero;
				ObjectUtil.SetTagWithAllChildren(EndFxObj, CameraLayerManager.GetInstance().GetMissionSignName());
				if (displayInfor.EndEffectBindPoint.Length == 0 || displayInfor.BeginEffectBindPoint.CompareTo("Ground")==0)
				{
					EndFxObj.transform.localRotation = Quaternion.identity;
					EndFxObj.transform.localScale = Vector3.one;
				}
				else
				{
					Transform t = hero.GetChildTransform(displayInfor.BeginEffectBindPoint);
					if (null != t)
					{
						EndFxObj.transform.parent = t;
						EndFxObj.transform.localPosition = Vector3.zero;
						EndFxObj.transform.localRotation = Quaternion.identity;
						EndFxObj.transform.localScale = Vector3.one;
						EndFxObj.transform.parent = hero.transform;
					}
				}
				if(!displayInfor.EndEffectBind)
				{
					EndFxObj.transform.parent =  hero.transform.parent;
				}
				ParticleSystemScaleManager.ScaleParticle(EndFxObj);
				KingSoftCommonFunction.SetLayer(EndFxObj,11);
				EndFxObj.SetActive(true);
				DestoryObject dos = EndFxObj.AddComponent<DestoryObject>();
				dos.delta = second;
			}
		}
		if (hitTicker.isPlaying())
			OnHit();
		
	}
	public bool IsActionFinish(int index)
	{
		EventRet ret = hero.DispatchEvent(ControllerCommand.IsPlayingActionFinish, animArys[index]);
        bool b = (bool)ret.GetReturn<AnimationComponent>();
        return b;
	}
	public void CrossFadeAnim(int index)
	{
		if (index < animArys.Length)
		{
			if (hero.AnimCmp.IsPlaying(animArys[index]))
				hero.AnimCmp.StopAnim();
			if (displayInfor.IsCrossFade)
				hero.DispatchEvent(ControllerCommand.CrossFadeAnimation, animArys[index]);
			else
				hero.DispatchEvent(ControllerCommand.CrossFadeAnimation, animArys[index],0f);
		}
	}
	public void CrossFadeAnim()
	{
        CrossFadeAnim(0);
	}
	public void KeepActionPlay()
	{
        EventRet ret = hero.DispatchEvent(ControllerCommand.IsPlayingActionFinish, animArys[0]);
        bool b = (bool)ret.GetReturn<AnimationComponent>();
        if (b)
		{
            hero.DispatchEvent(ControllerCommand.CrossFadeAnimation, animArys[0]);
		}
	}
	public bool IsActionFinish()
	{
        EventRet ret = hero.DispatchEvent(ControllerCommand.IsPlayingActionFinish, animArys[0]);
        bool b = (bool)ret.GetReturn<AnimationComponent>();

        return b;
	}
	public void DestroyFx()
	{
		if (null != BeginFXObj)
		{
			BeginFXObj.AddComponent<DestoryObject>();
			GameObject.Destroy(BeginFXObj);
		}
	}
	public void DestroyEndFx()
	{
		if (null != EndFxObj)
		{
			EndFxObj.AddComponent<DestoryObject>();
			GameObject.Destroy(EndFxObj);
		}
	}
	
}
