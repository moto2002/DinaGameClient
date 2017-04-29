using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.Manager;
using Assets.Scripts.Utils;
using Assets.Scripts.Define;
using Assets.Scripts.Data;
using Assets.Scripts.Model.Skill;
using Assets.Scripts.Model.Player;
using Assets.Scripts;
using Assets.Scripts.View.Chat;

namespace Assets.Scripts.Logic.Scene.SceneObject.Compont
{
    public class ActionComponent : BaseComponent
    {
		Dictionary<string,GameObject> hitFxs = new Dictionary<string, GameObject>();
        public override string GetName()
        {
            return GetType().Name;
        }
        public override void OnAttachToEntity(SceneEntity ety)
        {
            BaseInit(ety);
			Regist(ControllerCommand.MOVE_TO_NPC, OnMoveToNpc);
        }
        public override void OnDetachFromEntity(SceneEntity ety)
        {
			UnRegist(ControllerCommand.MOVE_TO_NPC, OnMoveToNpc);
            base.OnDetachFromEntity(ety);
        }
        public void ToDead(SceneEntity killer)
        {
			if (null != killer)
				KingSoftCommonFunction.LootAt(Owner.gameObject,killer.gameObject);
			Owner.property.fightHp = 0;

			if (Owner.HeroType == KHeroObjectType.hotMonster)
			{
				if (null != killer && killer.property.isMainHero)
				{
					HitPanelView.GetInstance().hit();
				}
				KSkillDisplay skillDisplay = KConfigFileManager.GetInstance().GetSkillDisplay(Owner.property.lastHitSkillId,Owner.property.tabID);
				if (Owner.property.lastAttackEvent == (byte)KAttackEvent.aeCrit || skillDisplay.DeadType == KSkillDisplay.DEAD_TYPE.BOMB)
				{	
					ActionBomb action = new ActionBomb(Owner);
		            action.IsPushStack = false;
		            Owner.DispatchEvent(ControllerCommand.SetActiveAction, action);
				}
				else if(skillDisplay.DeadType == KSkillDisplay.DEAD_TYPE.PHYSICS)
				{
					ActionPlysiceDie action = new ActionPlysiceDie(Owner);
					action.attacker = killer;
		            action.IsPushStack = false;
		            Owner.DispatchEvent(ControllerCommand.SetActiveAction, action);
				}
				else
				{
					ActionMonsterDie action = new ActionMonsterDie(Owner);
					action.attacker = killer;
		            action.IsPushStack = false;
		            Owner.DispatchEvent(ControllerCommand.SetActiveAction, action);
				}
				//ActionBomb
			}
			else
			{
				ActionDie action = new ActionDie(Owner);
	            action.IsPushStack = false;
	            Owner.DispatchEvent(ControllerCommand.SetActiveAction, action);
			}
        }
        public void IsBeDead()
        {
            ActionDead action = new ActionDead(Owner);
            action.IsPushStack = false;
            Owner.DispatchEvent(ControllerCommand.SetActiveAction, action);
        }
        public void MoveAndSkill(ushort skillid, SceneEntity target)
        {
            OperaSkill action = new OperaSkill(Owner);
			Owner.property.target = target;
            EventDispatcher.GameWorld.DispatchEvent(ControllerCommand.CHANGE_TARGET);

            action.target = target;
            action.IsPushStack = true;
            action.skillId = skillid;
            KActiveSkill skill = KConfigFileManager.GetInstance().GetActiveSkill((uint)skillid, 1);
            float CastRange = ((float)skill.CastRange) / 100f;
			CastRange = Mathf.Max(1.5f,CastRange);
            action.deltaSpace = CastRange;
            Owner.DispatchEvent(ControllerCommand.SetActiveAction, action);
        }
        public void SendSkill(ushort skillId, SceneEntity target)
        {
            ActionWaitSkill action = new ActionWaitSkill(Owner);
            action.skillId = skillId;
            action.IsPushStack = true;
            action.target = target;
            Owner.DispatchEvent(ControllerCommand.SetActiveAction, action);
        }
        public void SendSkill(ushort skillId, Vector3 position)
        {
            ActionWaitSkill action = new ActionWaitSkill(Owner);
            action.skillId = skillId;
            action.IsPushStack = true;
            action.position = position;
            Owner.DispatchEvent(ControllerCommand.SetActiveAction, action);
        }
        public void SendSkill(ushort skillId)
        {
            ActionWaitSkill action = new ActionWaitSkill(Owner);
            action.skillId = skillId;
            action.IsPushStack = true;
            Owner.DispatchEvent(ControllerCommand.SetActiveAction, action);
        }
        public void SkillMovingAOE(AnimActionParam param,KSkillDisplay skillDisplay, bool isPushStack = false)
        {
            ActionMovingAOE action = new ActionMovingAOE(Owner);
            action.InitParam(param,skillDisplay);
            action.IsPushStack = isPushStack;
            if (!action.IsCanActive())
                return;
            Owner.DispatchEvent(ControllerCommand.SetActiveAction, action);
        }
        public void SkillSelf(AnimActionParam param,KSkillDisplay skillDisplay, bool isPushStack)
        {
            ActionByAnim action = new ActionByAnim(Owner);
            action.InitParam(param,skillDisplay);
            action.IsPushStack = isPushStack;
            if (!action.IsCanActive())
                return;
            Owner.DispatchEvent(ControllerCommand.SetActiveAction, action);
        }
		public void SkillRush(AnimActionParam param,KSkillDisplay skillDisplay, bool isPushStack)
        {
            ActionRush action = new ActionRush(Owner);
            action.InitParam(param,skillDisplay);
            action.speed = Owner.Speed;
            action.IsPushStack = isPushStack;
            if (!action.IsCanActive())
                return;
            Owner.DispatchEvent(ControllerCommand.SetActiveAction, action);
        }
        public void CommonAttack(SceneEntity hero)
        {
            OperaAttack action = new OperaAttack(Owner);
            action.IsPushStack = true;
            KActiveSkill skill = KConfigFileManager.GetInstance().GetActiveSkill((uint)Owner.Job, 1);
            if (null == skill)
                return;
            action.deltaSpace = ((float)skill.CastRange) / 100f;
            action.target = hero;
            Owner.DispatchEvent(ControllerCommand.SetActiveAction, action);

        }
		public void MoveToNPC(SceneEntity hero,float deltaSpace, OnCatchNPCDel del = null)
        {
            OperaMove2NPC action = new OperaMove2NPC(Owner);
			action.deltaSpace = deltaSpace;
            action.del = del;
            if (!action.IsCanActive())
                return;
            action.targetHero = hero;
            Owner.DispatchEvent(ControllerCommand.SetActiveAction, action);
        }
        public void MoveToNPC(SceneEntity hero, OnCatchNPCDel del = null)
        {
            MoveToNPC(hero,1.5f,del);
        }
		object OnMoveToNpc(params object[] objs)
		{
			if (objs.Length > 1)
			{
				SceneEntity entity = objs[0] as SceneEntity;
				float dis = (float) objs[1] ;
				MoveToNPC(entity,dis);
			}
			return null;
		}
		
        protected void UpdateAction()
        {
            if (Owner.Anim == null)
            {
                return;
            }
            string anim_name = Owner.CharacterStateName();
            Owner.DispatchEvent(ControllerCommand.CrossFadeAnimation, anim_name);
        }
        //播放攻击动画
        public void PlayFightAnimation(SceneEntity target, uint skillId, Vector3 position)
        {
			Owner.AnimCmp.ResetFightingState();
			uint jobId = Owner.TabID;
            KSkillDisplay skillDisplay = KConfigFileManager.GetInstance().GetSkillDisplay(skillId, jobId);
            KActiveSkill skillSetting = KConfigFileManager.GetInstance().GetActiveSkill(skillId, 1);

            AnimActionParam param = new AnimActionParam();
            param.skillId = (ushort)skillId;
			if (null != target)
			{
				KingSoftCommonFunction.LootAt(Owner.gameObject,target.gameObject);
            	param.targetId = target.property.Id;
			}
            param.position = position;
            param.target = target;
			if (null != Owner.property.activeAction)
				Owner.property.activeAction.FinishImmediate();
            if (skillDisplay.SkillType.CompareTo("MOVINGAOE") == 0)
            {
                SkillMovingAOE(param,skillDisplay, false);
            }
            else if (skillDisplay.SkillType.CompareTo("ANIM") == 0)
            {
                SkillSelf(param,skillDisplay,false);
            }
            else if (skillDisplay.SkillType.CompareTo("RUSH") == 0)
            {
                SkillRush(param,skillDisplay, false);
            }
			else if (skillDisplay.SkillType.CompareTo("SHOT") == 0)
            {
            }
			
        }

        //播放战斗效果
        public void PlayFightEffect(ushort wSkillID, int damage, byte byAttackEvent,SceneEntity killerHero)
        {
            
            if (byAttackEvent == (byte)KAttackEvent.aeMiss)
            {
				if (Owner.property.isMainHero)
            	{	
					Owner.TipsCmp.CreateTip( new Vector3(0,Owner.heroSetting.TipPos0,0), "2", "FightFont","effect_ui_shuzitanchu_putong.res");
				}
				else
				{
					Owner.TipsCmp.CreateTip( new Vector3(0,Owner.heroSetting.TipPos0,0), "1", "FightFont","effect_ui_shuzitanchu_putong.res");
				}
				return;
            }
            else if (byAttackEvent == (byte)KAttackEvent.aeCrit)
            {
                Owner.TipsCmp.CreateTip( new Vector3(0,Owner.heroSetting.TipPos0,0), "+" + damage, "CritFont","effect_ui_shuzitanchu_baoji2.res");
            }
            else if (Owner.property.isMainHero)
            {
                Owner.TipsCmp.CreateTip( new Vector3(0,Owner.heroSetting.TipPos0,0), "+" + damage, "HurtFont","effect_ui_shuzitanchu_putong.res", NumTip.OFFSET_TYPE.LEFT);
            }
            else
            {
                Owner.TipsCmp.CreateTip( new Vector3(0,Owner.heroSetting.TipPos0,0), "+" + damage, "AttackFont","effect_ui_shuzitanchu_putong.res");
            }
			KSkillDisplay skillDisplay = KConfigFileManager.GetInstance().GetSkillDisplay(wSkillID, Owner.TabID);
			if (skillDisplay.OnHitAction.CompareTo("JUMP") == 0)
			{
				ActionBeAttactedAndThrowUp action = new ActionBeAttactedAndThrowUp(Owner);
				action.hitAnim = skillDisplay.OnHitAnim;
				action.time = skillDisplay.OnHitEffecTime;
				action.height = skillDisplay.OnHitHeight;
				Owner.ActiveAction = action;
				//action
				
			}
			else if (Owner.property.heroObjType == KHeroObjectType.hotMonster && Owner.heroSetting.MonsterGrade == KMonsterGrade.mgQuestBoss)
			{
				if (Owner.property.activeAction.TryFinish())
				{
					ActiionBeAttack beAttack = new ActiionBeAttack(Owner);
					Owner.DispatchEvent(ControllerCommand.SetActiveAction, beAttack);
				}
			}
			else
			{
				Owner.DispatchEvent(ControllerCommand.BE_HIT);
			}
			
			Vector3 forward = Vector3.forward;
			if (null != killerHero)
			{
				forward = killerHero.transform.position - Owner.transform.position;
				forward = new Vector3(forward.x,0,forward.z);
				forward.Normalize();
			}
			if (skillDisplay.HitShakeTime>0 && skillDisplay.HitShakeDelta > 0)
			{
				Owner.DispatchEvent(ControllerCommand.HIT_SLOW,skillDisplay.HitShakeTime,skillDisplay.HitShakeDelta);	
			}
			
            if (null != skillDisplay && skillDisplay.HitEffect.Length > 0   /*&& skillDisplay.BulletEffect.Length == 0*/ )
            {
				
				AssetInfo inf = AssetLoader.GetInstance().Load(URLUtil.GetResourceLibPath() + skillDisplay.HitEffect);
                if (inf.isDone(false))
                {
					if (skillDisplay.SingleHitFx)
					{
						GameObject _hit = null;
						if (hitFxs.TryGetValue(skillDisplay.HitEffect,out _hit))
						{
							GameObject.Destroy(_hit);
						}
					}
					GameObject hitObject = inf.CloneGameObject();
					ObjectUtil.SetTagWithAllChildren(hitObject, CameraLayerManager.GetInstance().GetMissionSignName());
					hitObject.transform.parent =  Owner.transform.parent;
					hitObject.transform.position = Owner.transform.position;
					hitObject.transform.forward = forward;
					if (skillDisplay.SingleHitFx)
					{
						hitFxs[skillDisplay.HitEffect] = hitObject;
					}
                    KingSoftCommonFunction.SetLayer(hitObject, 11);
                    DestoryObject dos = hitObject.AddComponent<DestoryObject>();
                    dos.delta = 5;
					if (skillDisplay.HitBindPoint.Length>0)
					{
						if (skillDisplay.HitBindPoint.CompareTo("Ground")!=0)
						{
							Transform t = Owner.GetChildTransform(skillDisplay.HitBindPoint);
							if (null != t)
							{
								hitObject.transform.parent = t;
								hitObject.transform.localPosition = Vector3.zero;
								hitObject.transform.localScale = Vector3.one;
							}
						}
					}
					hitObject.SetActive(true);
                }
            }
        }
    }
}
