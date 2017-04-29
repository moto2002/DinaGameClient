using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;
using Assets.Scripts.View.Scene;
using Assets.Scripts.View.MainUI;
using Assets.Scripts.Model.Scene;
using Assets.Scripts.Utils;
using Assets.Scripts.Lib.Net;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.Proto;
using Assets.Scripts.Data;
using Assets.Scripts.Model.Player;
using Assets.Scripts.View.Chat;
using Assets.Scripts.Manager;
using Assets.Scripts.Model.Skill;
using Assets.Scripts.Define;
using Assets.Scripts.Logic.Scene.SceneObject;
using Assets.Scripts.Controller;
using SObject = System.Object;
using UObject = UnityEngine.Object;
using Assets.Scripts.Logic.Skill;
using Assets.Scripts.Logic.Scene;

class SkillEffectItem
{
	public SkillEffectItem(S2C_SKILL_EFFECT effect,int hp,float _time)
	{
		this.effect = effect;
		this.hp = hp;
		this.hitTime = _time;
	}
	public S2C_SKILL_EFFECT effect;
	public int hp;
	public float hitTime = 0;
}
public class MessageCase  {
	
	
	List<S2C_HERO_DEATH> death_msgs = new List<S2C_HERO_DEATH>();
	List<SkillEffectItem> effect_msgs = new List<SkillEffectItem>();
	
	public  void AddDeadMessage(S2C_HERO_DEATH respond)
	{
		death_msgs.Add(respond);
	}
	
	public void AddSkillEffectMessage(S2C_SKILL_EFFECT respond,float _time)
	{
		SceneEntity targetHero = SceneLogic.GetInstance().GetSceneObject(respond.wTargetHeroID) as SceneEntity;
		effect_msgs.Add(new SkillEffectItem(respond, (int)(targetHero.property.hp-respond.wDamage),_time));
	}
	
	public void PopMessage( SceneEntity hero, float deltaTimeScale = 0f)
	{
		List<S2C_HERO_DEATH> _death_msgs = death_msgs;;
		List<SkillEffectItem> _effect_msgs = effect_msgs;
		effect_msgs = new List<SkillEffectItem>();
		death_msgs = new List<S2C_HERO_DEATH>();
		
		
		/*float distance = Vector3.Distance(hero.Position,target.Position);
			float _delta_time = distance*displayInfor.HitDelayTimeScale;
			Debug.LogWarning("_delta_time = "+_delta_time);
		*/
		foreach ( S2C_HERO_DEATH death_msg in _death_msgs )
		{
			float _time = 0f;
			SceneEntity targetHero = null ;
			if( deltaTimeScale > 0 &&  SceneLogic.GetInstance().TryGetSceneObject(death_msg.heroID ,out targetHero) && null != hero )
			{
				float distance = Vector3.Distance(hero.Position,targetHero.Position);
				_time = distance*deltaTimeScale;
			}
			if (_time <= 0.00001f)
			{
				SceneLogic.GetInstance().OnHeroDeathParam(death_msg,false);
			}
			else
			{
				Debug.LogWarning("deltaTime = "+_time);
				DelayCallManager.instance.CallFunction(SceneLogic.GetInstance().OnHeroDeathParam,_time,death_msg,false);
			}
		}
		try
		{
			foreach ( SkillEffectItem skill_effect_msg in _effect_msgs )
			{
				float _time = 0f;
				SceneEntity targetHero = null ;
				if( SceneLogic.GetInstance().TryGetSceneObject(skill_effect_msg.effect.wTargetHeroID ,out targetHero) )
				{
					if (deltaTimeScale > 0 && null != hero)
					{
						float distance = Vector3.Distance(hero.Position,targetHero.Position);
						_time = distance*deltaTimeScale;
					}
					if (_time <= 0.00001f)
					{
						SceneLogic.GetInstance().OnSkillEffectParam(skill_effect_msg.effect,false,targetHero,skill_effect_msg.hp,skill_effect_msg.hitTime);
					}
					else
					{
						Debug.LogWarning("deltaTime = "+_time);
						DelayCallManager.instance.CallFunction(SceneLogic.GetInstance().OnSkillEffectParam,_time,skill_effect_msg.effect,false,targetHero,skill_effect_msg.hp,skill_effect_msg.hitTime);
					}
					
					
				}	
			}
		}
		catch (Exception e)
		{
			Debug.LogError(e.ToString());
		}
		
		_effect_msgs.Clear();
		_death_msgs.Clear();
	}
}
