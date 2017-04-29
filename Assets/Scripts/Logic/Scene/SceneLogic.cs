using System;
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
using Assets.Scripts.Logic.RemoteCall;
using Assets.Scripts.Logic.Scene.SceneObject.Compont;
using Assets.Scripts.View.Other;
using Assets.Scripts.Lib.Resource;

namespace Assets.Scripts.Logic.Scene
{
    class SceneLogic : BaseLogic
    {
        private Dictionary<uint, SceneEntity> allObjectsMap;
        private SceneEntity mainHero;

        private Dictionary<int, uint> npcidToSceneidDic = new Dictionary<int, uint>();
		
		
        public uint mapId;
        public uint backgroundType = 1;

        private static SceneLogic instance;
        public static SceneLogic GetInstance()
        {
            if (instance == null)
                instance = new SceneLogic();
            return instance;
        }
		
		public List<SceneEntity> GetAllSceneObject(KHeroObjectType heroType)
		{
			List<SceneEntity> lists = new List<SceneEntity>();
			foreach (KeyValuePair<uint, SceneEntity> kvp in allObjectsMap)
			{
				if (kvp.Value.HeroType == heroType)
				{
					lists.Add(kvp.Value);
				}
			}
			return lists;
		}

        protected override void Init()
        {
            allObjectsMap = new Dictionary<uint, SceneEntity>();
        }
		
		//RegistProctect(KS2C_Protocol.s2c_sync_max_hp, typeof(S2C_SYNC_MAX_HP));
        //  RegistProctect(KS2C_Protocol.s2c_sync_max_mp, typeof(S2C_SYNC_MAX_MP));

        protected override void InitListeners()
        {
            RegistSocketListener(KS2C_Protocol.s2c_hero_relive, SyncHeroRelive, typeof(S2C_HERO_RELIVE));
            RegistSocketListener(KS2C_Protocol.s2c_switch_map, SwitchMap, typeof(S2C_SWITCH_MAP));
            RegistSocketListener(KS2C_Protocol.s2c_sync_new_hero, SyncNewHero, typeof(S2C_SYNC_NEW_HERO));
			
			RegistSocketListener(KS2C_Protocol.s2c_sync_max_hp, SyncMaxHp, typeof(S2C_SYNC_MAX_HP));
			RegistSocketListener(KS2C_Protocol.s2c_sync_max_mp, SyncMaxMp, typeof(S2C_SYNC_MAX_MP));

            RegistSocketListener(KS2C_Protocol.s2c_sync_scene_hero_data_end, SyncSceneHeroData, typeof(S2C_SYNC_SCENE_HERO_DATA_END));
            RegistSocketListener(KS2C_Protocol.s2c_sync_scene_obj, SyncSceneObject, typeof(S2C_SYNC_SCENE_OBJ));
            RegistSocketListener(KS2C_Protocol.s2c_sync_scene_obj_end, OnSyncSceneObjEnd, typeof(S2C_SYNC_SCENE_OBJ_END));
            RegistSocketListener(KS2C_Protocol.s2c_remove_scene_obj, NotifyRemoveSceneObj, typeof(S2C_REMOVE_SCENE_OBJ));
            RegistSocketListener(KS2C_Protocol.s2c_sync_hero_hpmp, OnSyncHeroHpMp, typeof(S2C_SYNC_HERO_HPMP));
            RegistSocketListener(KS2C_Protocol.s2c_battle_startedframe, BattleStartedFrame, typeof(S2C_BATTLE_STARTEDFRAME));
            RegistSocketListener(KS2C_Protocol.s2c_hero_move, OnHeroMoveHandler, typeof(S2C_HERO_MOVE));
            RegistSocketListener(KS2C_Protocol.s2c_cast_skill, OnCastSkill, typeof(S2C_CAST_SKILL));
            RegistSocketListener(KS2C_Protocol.s2c_cast_skill_fail_notify, NotifyCastSkillFail, typeof(S2C_CAST_SKILL_FAIL_NOTIFY));
            RegistSocketListener(KS2C_Protocol.s2c_skill_effect, OnSkillEffect, typeof(S2C_SKILL_EFFECT));

            RegistSocketListener(KS2C_Protocol.s2c_add_buff_notify, OnAddBuffNotify, typeof(S2C_ADD_BUFF_NOTIFY));
            RegistSocketListener(KS2C_Protocol.s2c_del_buff_notify, OnDelBuffNotify, typeof(S2C_DEL_BUFF_NOTIFY));
            RegistSocketListener(KS2C_Protocol.s2c_hero_death, OnHeroDeath, typeof(S2C_HERO_DEATH));

            RegistSocketListener(KS2C_Protocol.s2c_pvp_game_over, OnPvpGameOver, typeof(S2C_PVP_GAME_OVER));
            RegistSocketListener(KS2C_Protocol.s2c_sync_add_hp, OnSyncAddHp, typeof(S2C_SYNC_ADD_HP));
            RegistSocketListener(KS2C_Protocol.s2c_sync_add_mp, OnSyncAddMp, typeof(S2C_SYNC_ADD_MP));

            RegistSocketListener(KS2C_Protocol.s2c_move_fail_notify, NotifyMoveFail, typeof(S2C_MOVE_FAIL_NOTIFY));
            RegistSocketListener(KS2C_Protocol.s2c_sync_move_speed, OnMoveSpeed, typeof(S2C_SYNC_MOVE_SPEED));
            RegistSocketListener(KS2C_Protocol.s2c_hero_pos, OnHeroPos, typeof(S2C_HERO_POS));
            RegistSocketListener(KS2C_Protocol.s2c_scene_obj_change_pos, OnSceneObjChangePos, typeof(S2C_SCENE_OBJ_CHANGE_POS));

            RegistSocketListener(KS2C_Protocol.s2c_sync_all_attribute, OnAllAttribute, typeof(S2C_SYNC_ALL_ATTRIBUTE));
            RegistSocketListener(KS2C_Protocol.s2c_sync_one_attribute, OnOneAttribute, typeof(S2C_SYNC_ONE_ATTRIBUTE));
            RegistSocketListener(KS2C_Protocol.s2c_sync_self_attribute, OnSelfAttribute, typeof(S2C_SYNC_SELF_ATTRIBUTE));

        }
		private void OnSkillEffect(KProtoBuf buf)
        {
			OnSkillEffectOp(buf,true);
        }
		private void OnHeroDeath(KProtoBuf buf)
        {
            OnHeroDeathOp(buf,true);
        }
        public void InitScene()
        {
            EventDispatcher.GameWorld.DispatchEvent(ControllerCommand.INIT_SCENE_VIEW);
            
        }
        
        public void PickUpDrop(SceneEntity obj)
        {
            if (obj.property.sceneObjType != KSceneObjectType.sotDoodad) 
                return;
            if (obj.property.doodadObjType != KDoodadType.dddDrop)
                return;

            RemoteCallLogic.GetInstance().CallGS("OnPickUpDrop", (int)obj.property.Id);
        }

        public void SendApplySceneObj()
        {
            C2S_APPLY_SCENE_OBJ msg = new C2S_APPLY_SCENE_OBJ();
            msg.protocolID = (byte)KC2S_Protocol.c2s_apply_scene_obj;
            SendMessage(msg);
        }

        private void SyncHeroRelive(KProtoBuf buf)
        {
            S2C_HERO_RELIVE respond = buf as S2C_HERO_RELIVE;
            SceneEntity hero = GetSceneObject(respond.heroID) as SceneEntity;
            hero.property.isDeaded = hero.property.isDeadTemp = false;
            hero.DispatchEvent(ControllerCommand.FinishImmediate);
            hero.DispatchEvent(ControllerCommand.Idle,false);
			if (hero.property.isMainHero)
				hero.DispatchEvent(ControllerCommand.HERO_MOVE);

        }
        private void SwitchMap(KProtoBuf buf)
        {
            S2C_SWITCH_MAP respond = buf as S2C_SWITCH_MAP;
            log.Debug("开始加载场景地图配置文件 mapID : " + respond.mapID);
            mapId = respond.mapID;
            SceneView.GetInstance().SwitchMap(mapId);
        }

        private void SyncSceneObject(KProtoBuf buf)
        {
            log.Debug("同步场景物件数据");
            S2C_SYNC_SCENE_OBJ respond = buf as S2C_SYNC_SCENE_OBJ;
            AddSceneObj(respond);
        }

        private void OnSyncSceneObjEnd(KProtoBuf buf)
        {
            S2C_SYNC_SCENE_OBJ_END respond = buf as S2C_SYNC_SCENE_OBJ_END;
            SendApplyScendHeroData();
        }

        private void SendApplyScendHeroData()
        {
            C2S_APPLY_SCENE_HERO_DATA msg = new C2S_APPLY_SCENE_HERO_DATA();
            msg.protocolID = (byte)KC2S_Protocol.c2s_apply_scene_hero_data;
            SendMessage(msg);
        }

        private void SyncSceneHeroData(KProtoBuf buf)
        {
            S2C_SYNC_SCENE_HERO_DATA_END respond = buf as S2C_SYNC_SCENE_HERO_DATA_END;
            SendLoadComplete();
        }

        private void SendLoadComplete()
        {
            C2S_LOADING_COMPLETE msg = new C2S_LOADING_COMPLETE();
            msg.protocolID = (byte)KC2S_Protocol.c2s_loading_complete;
            SendMessage(msg);
        } 

		private void SyncMaxHp(KProtoBuf buf)
        {
            S2C_SYNC_MAX_HP respond = buf as S2C_SYNC_MAX_HP;
            MaxHp(respond);
        }
		
		private void SyncMaxMp(KProtoBuf buf)
        {
			S2C_SYNC_MAX_MP respond = buf as S2C_SYNC_MAX_MP;
            MaxMp(respond);
        }
		//public HashSet<uint> debugHeroSet = new HashSet<uint>();
        private void SyncNewHero(KProtoBuf buf)
        {
			S2C_SYNC_NEW_HERO respond = buf as S2C_SYNC_NEW_HERO;
            //log.Error("同步新的英雄 " + respond.id);
			//debugHeroSet.Add(respond.id);
            AddHero(respond);
        }

        private void NotifyRemoveSceneObj(KProtoBuf buf)
        {
            S2C_REMOVE_SCENE_OBJ respond = buf as S2C_REMOVE_SCENE_OBJ;
            //Debug.LogError("删除 hero " + respond.objID);
            SceneEntity removeObj = GetSceneObject(respond.objID) as SceneEntity;
            GameObject obj = null;
            if (null == removeObj)
            {
                log.Error("can not delete hero " + respond.objID);
                return;
            }
                
            obj = removeObj.gameObject;
            try
            {
                RemoveSceneObject(respond);
            }
            catch (Exception e)
            {
                GameObject.Destroy(obj);
                throw e;
            }
            //GameObject.Destroy(obj);
        }

        private void NotifyMoveFail(KProtoBuf buf)
        {
            S2C_MOVE_FAIL_NOTIFY respond = buf as S2C_MOVE_FAIL_NOTIFY;

        }

        private void BattleStartedFrame(KProtoBuf buf)
        {
            S2C_BATTLE_STARTEDFRAME respond = buf as S2C_BATTLE_STARTEDFRAME;
        }

        private void OnHeroPos(KProtoBuf buf)
        {
            S2C_HERO_POS respond = buf as S2C_HERO_POS;
            SceneEntity hero = GetSceneObject(respond.heroID);
            if (hero == null){return;}
            Vector3 rolePosition = MapUtils.GetMetreFromInt(respond.posX, respond.posZ, respond.posY);
            hero.property.ServerPos = rolePosition;
        }

        private void OnSceneObjChangePos(KProtoBuf buf)
        {
            S2C_SCENE_OBJ_CHANGE_POS respond = buf as S2C_SCENE_OBJ_CHANGE_POS;
            SceneEntity hero = GetSceneObject(respond.objID);
            if (hero == null) { return; }
            Vector3 rolePosition = MapUtils.GetMetreFromInt(respond.posX, respond.posZ, respond.posY);
            hero.Position = rolePosition; 
        }

        private void OnAllAttribute(KProtoBuf buf)
        {
            S2C_SYNC_ALL_ATTRIBUTE respond = buf as S2C_SYNC_ALL_ATTRIBUTE;
            MajorPlayer majorPlayer = PlayerManager.GetInstance().MajorPlayer;

            majorPlayer.HeroData[KAttributeType.atMaxHP] = respond.MaxHP;
            majorPlayer.HeroData[KAttributeType.atMaxMP] = respond.MaxMP;
            majorPlayer.HeroData[KAttributeType.atAttack] = respond.Attack;
            majorPlayer.HeroData[KAttributeType.atDefence] = respond.Defence;
            majorPlayer.HeroData[KAttributeType.atMiss] = respond.Miss;
            majorPlayer.HeroData[KAttributeType.atCrit] = respond.Crit;
            majorPlayer.HeroData[KAttributeType.atReduceCrit] = respond.ReduceCrit;
            majorPlayer.HeroData[KAttributeType.atCritHurt] = respond.CritHurt;
            majorPlayer.HeroData[KAttributeType.atReduceCritHurt] = respond.ReduceCritHurt;
            majorPlayer.HeroData[KAttributeType.atAttackSpeed] = respond.AttackSpeed;
            majorPlayer.HeroData[KAttributeType.atMoveSpeed] = respond.MoveSpeed;
            majorPlayer.HeroData[KAttributeType.atHpRecover] = respond.HpRecover;
            majorPlayer.HeroData[KAttributeType.atMpRecover] = respond.MpRecover;
            majorPlayer.HeroData[KAttributeType.atReflex] = respond.Reflex;
            majorPlayer.HeroData[KAttributeType.atReduceDefence] = respond.ReduceDefence;
            majorPlayer.HeroData[KAttributeType.atDamageMore] = respond.DamageMore;
            majorPlayer.HeroData[KAttributeType.atDamageLess] = respond.DamageLess;
            majorPlayer.HeroData[KAttributeType.atReduceDamage] = respond.ReduceDamage;
            majorPlayer.HeroData[KAttributeType.atExtDamage] = respond.ExtDamage;
            majorPlayer.HeroData[KAttributeType.atDamageBack] = respond.DamageBack;
            majorPlayer.HeroData[KAttributeType.atAttackRecover] = respond.AttackRecover;
            majorPlayer.HeroData[KAttributeType.atUpAttack] = respond.UpAttack;
			
			if (MainHero != null)
			{
				MainHero.property.maxHp = respond.MaxHP;
            	MainHero.property.maxMp = respond.MaxMP;
            	MainHero.DispatchEvent(ControllerCommand.SET_SPEED, respond.MoveSpeed);
			}
        }

        private void OnOneAttribute(KProtoBuf buf)
        {
            S2C_SYNC_ONE_ATTRIBUTE respond = buf as S2C_SYNC_ONE_ATTRIBUTE;
            KAttributeType eType = (KAttributeType)respond.AttributeType;
            int nValue = respond.AttributeValue;
            SceneEntity hero = GetSceneObject(respond.HeroID);

            if (hero.property.Id == MainHero.property.Id)
            {
                MajorPlayer majorPlayer = PlayerManager.GetInstance().MajorPlayer;
                majorPlayer.HeroData[eType] = nValue;
            }

            switch (eType)
            {
                case KAttributeType.atMaxHP:
                    hero.property.maxHp = nValue;
                    break;
                case KAttributeType.atMaxMP:
                    hero.property.maxMp = nValue;
                    break;
                case KAttributeType.atMoveSpeed:
                    hero.DispatchEvent(ControllerCommand.SET_SPEED, nValue);
                    break;
            }
        }
        private void OnSelfAttribute(KProtoBuf buf)
        {
            S2C_SYNC_SELF_ATTRIBUTE respond = buf as S2C_SYNC_SELF_ATTRIBUTE;
            KAttributeType eType = (KAttributeType)respond.AttributeType;
            int nValue = respond.AttributeValue;

            MajorPlayer majorPlayer = PlayerManager.GetInstance().MajorPlayer;
            majorPlayer.HeroData[eType] = nValue;

            switch (eType)
            {
                case KAttributeType.atMaxHP:
                    MainHero.property.maxHp = nValue;
                    break;
                case KAttributeType.atMaxMP:
                    MainHero.property.maxMp = nValue;
                    break;
                case KAttributeType.atMoveSpeed:
                    MainHero.DispatchEvent(ControllerCommand.SET_SPEED, nValue);
                    break;
            }
        }
        private void OnHeroMoveHandler(KProtoBuf buf)
        {
            S2C_HERO_MOVE respond = buf as S2C_HERO_MOVE;
            SceneEntity hero = GetSceneObject(respond.heroID);
            log.Assert(hero != null);
            Vector3 rolePosition = MapUtils.GetMetreFromInt(respond.posX, respond.posZ, respond.posY);
            hero.DispatchEvent(ControllerCommand.SET_DESTINATION, rolePosition, (int)respond.moveType);
        }

        private void OnCastSkill(KProtoBuf buf)
        {
            S2C_CAST_SKILL respond = buf as S2C_CAST_SKILL;
            SceneEntity targetHero = GetSceneObject(respond.targetID) as SceneEntity;
            SceneEntity attacker = GetSceneObject(respond.casterID) as SceneEntity;
            attacker.Action.PlayFightAnimation(targetHero, respond.skillID, MapUtils.GetMetreFromInt(respond.x, respond.z, respond.y));
            if (attacker == MainHero)
            {
				AutoAttackComponent.curIndex++;
                SkillLogic.GetInstance().ResetSkillCD(respond.skillID);
            }
        }

        private void NotifyCastSkillFail(KProtoBuf buf)
        {
            S2C_CAST_SKILL_FAIL_NOTIFY respond = buf as S2C_CAST_SKILL_FAIL_NOTIFY;
			Debug.LogWarning("NotifyCastSkillFail " );
			if (SceneLogic.GetInstance().MainHero.ActiveAction is ActionWaitSkill)
            	MainHero.DispatchEvent(ControllerCommand.FinishImmediate);
        }

        private void OnAddBuffNotify(KProtoBuf buf)
        {
            S2C_ADD_BUFF_NOTIFY respond = buf as S2C_ADD_BUFF_NOTIFY;
            SceneEntity targetHero = GetSceneObject(respond.herID) as SceneEntity;
            if (null != targetHero)
            {
                targetHero.DispatchEvent(ControllerCommand.ADD_BUFF, respond.wBuffID);
            }
        }
        private void OnDelBuffNotify(KProtoBuf buf)
        {
            S2C_DEL_BUFF_NOTIFY respond = buf as S2C_DEL_BUFF_NOTIFY;
            SceneEntity targetHero = GetSceneObject(respond.herID) as SceneEntity;
            if (null != targetHero)
                targetHero.DispatchEvent(ControllerCommand.REMOVE_BUFF, respond.wBuffID);
        }

        private void OnPvpGameOver(KProtoBuf buf)
        {
            S2C_PVP_GAME_OVER respond = buf as S2C_PVP_GAME_OVER;
        }

        private void OnSyncAddHp(KProtoBuf buf)
        {
            S2C_SYNC_ADD_HP respond = buf as S2C_SYNC_ADD_HP;
			SceneEntity hero = GetSceneObject(respond.heroID) as SceneEntity;
			hero.TipsCmp.CreateTip( new Vector3(0,hero.heroSetting.TipPos0,0), "+" + respond.hp, "HpFont","effect_ui_shuzitanchu_baoji2.res");
        }

        private void OnSyncAddMp(KProtoBuf buf)
        {
            S2C_SYNC_ADD_MP respond = buf as S2C_SYNC_ADD_MP;
			SceneEntity hero = GetSceneObject(respond.heroID) as SceneEntity;
			hero.TipsCmp.CreateTip( new Vector3(0,hero.heroSetting.TipPos0,0), "+" + respond.mp, "MpFont","effect_ui_shuzitanchu_baoji2.res");
        }
		
		public object OnHeroDeathParam(params object[] objs)
		{
			OnHeroDeathOp((KProtoBuf)objs[0],(bool)objs[1] );
			return null;
		}

		public void OnHeroDeathOp(KProtoBuf buf,bool fristTime)
		{
			S2C_HERO_DEATH respond = buf as S2C_HERO_DEATH;
            SceneEntity targetHero = GetSceneObject(respond.heroID) as SceneEntity;
            SceneEntity killerHero = GetSceneObject(respond.KillerID) as SceneEntity;
			if(null == targetHero)
				return;
            if (targetHero == mainHero)
            {
                ReliveView.GetInstance().ShowView();
            }
            
			targetHero.property.isDeaded = true;
			uint lastSkillId = targetHero.property.lastHitSkillId;
			KSkillDisplay skillDisplay = KConfigFileManager.GetInstance().GetSkillDisplay(lastSkillId,targetHero.property.tabID);
			if (fristTime && null != killerHero)
			{
				KActiveSkill activeSkill = KConfigFileManager.GetInstance().GetActiveSkill(lastSkillId,1);
				if(null != activeSkill && activeSkill.ClientCache )
				{
					if (null != killerHero.ActiveAction)
					{
						killerHero.ActiveAction.AddDeadMessage(respond);
						return;
					}
				}
				else if (null != killerHero && skillDisplay.HitDelayTimeScale > 0.00001f)
				{
					float distance = Vector3.Distance(killerHero.Position,targetHero.Position);
					float _time = distance*skillDisplay.HitDelayTimeScale;
					DelayCallManager.instance.CallFunction(OnHeroDeathParam,_time,buf,false);
					return;
				}
			}
            if (null != targetHero)
            {
				if(targetHero.property.activeAction.isFlying)
				{
					if (skillDisplay.DeadType != KSkillDisplay.DEAD_TYPE.PHYSICS )
					{
						targetHero.property.isDeadTemp = true;
						return;
					}	
				}
                targetHero.DispatchEvent(ControllerCommand.FinishImmediate);
				targetHero.DispatchEvent(ControllerCommand.CLEAR_BUFF);
                targetHero.Action.ToDead(killerHero);
                targetHero.property.isDeadTemp = true;
				if (null != killerHero)
	            {
	                if (killerHero.gameObject.GetComponent<AttackSlow>() == null)
	                {
						if (null == skillDisplay || skillDisplay.DieAttackSlowTime <=0 ){
							return;
						}
						killerHero.DispatchEvent(
							ControllerCommand.SLOW,
							killerHero.AnimCmp.GetLastLogicAnimName(),
							skillDisplay.DieAttackSlowTime,
							skillDisplay.DieAttackSpeed);
	                }
            	}
            }
		}
		public object OnSkillEffectParam(params object[] objs)
		{
			SceneEntity targetHero = (SceneEntity)objs[2];
			try
			{
				int _hp0 = (int)objs[3];
			}
			catch(System.Exception e)
			{
				string err = objs[3].GetType().ToString();
				Debug.Log(err);
			}
			int _hp = (int)objs[3];
			float _time = (float)objs[4];
			if (targetHero.property.updateFightHpTime <= _time)
				targetHero.property.fightHp = _hp;
			OnSkillEffectOp((KProtoBuf)objs[0],(bool)objs[1] );
			return null;
		}
		public void OnSkillEffectOp(KProtoBuf buf , bool fristTime)
        {
            S2C_SKILL_EFFECT respond = buf as S2C_SKILL_EFFECT;
            KAttackEvent kaEvt = (KAttackEvent)respond.byAttackEvent;
			SceneEntity killerHero = GetSceneObject(respond.wCasterID) as SceneEntity;
            SceneEntity targetHero = GetSceneObject(respond.wTargetHeroID) as SceneEntity;
			if(null == targetHero)
				return;
			if (!fristTime  && null == targetHero)
				return;
			targetHero.property.lastHitSkillId = respond.wSkillID;
			targetHero.property.lastAttackEvent = respond.byAttackEvent;
			if(null != targetHero.AnimCmp)
			{
				targetHero.AnimCmp.ResetFightingState();
			}
			KSkillDisplay skillDisplay = KConfigFileManager.GetInstance().GetSkillDisplay(targetHero.property.lastHitSkillId,1);
			if (fristTime && null != killerHero)
			{
				KActiveSkill activeSkill = KConfigFileManager.GetInstance().GetActiveSkill(targetHero.property.lastHitSkillId,1);
				if(null != activeSkill  )
				{
					if (activeSkill.ClientCache)
					{
						if (null != killerHero.ActiveAction)
						{
							killerHero.ActiveAction.AddSkillEffectMessage(respond,Time.realtimeSinceStartup);
							return;
						}
					}
					else if( null != killerHero && skillDisplay.HitDelayTimeScale > 0.00001f)
					{
						float distance = Vector3.Distance(killerHero.Position,targetHero.Position);
						float _time = distance*skillDisplay.HitDelayTimeScale;
						DelayCallManager.instance.CallFunction(OnSkillEffectParam,_time,buf,false,targetHero,targetHero.property.hp-(int)respond.wDamage,Time.realtimeSinceStartup  );
						return;
					}	
				}
			}
			if (fristTime)
			{
				targetHero.property.fightHp = (int)(targetHero.property.hp-respond.wDamage);
				targetHero.property.updateFightHpTime = Time.realtimeSinceStartup;
			}
			if (targetHero.property.heroObjType == KHeroObjectType.hotPlayer)
			{
				if(null != targetHero.AnimCmp)
				{
					targetHero.AnimCmp.ResetFightingState();
				}
			}
            targetHero.Action.PlayFightEffect(respond.wSkillID, (int)respond.wDamage, respond.byAttackEvent,killerHero);
        }
        
        private void OnSyncHeroHpMp(KProtoBuf buf)
        {
            S2C_SYNC_HERO_HPMP respond = buf as S2C_SYNC_HERO_HPMP;
            SceneEntity hero = GetSceneObject(respond.heroID);
            if (hero)
            {
                hero.Hp = respond.hp;
                hero.Mp = respond.mp;

                if (SceneLogic.GetInstance().mainHero == hero)
                {
                    EventDispatcher.GameWorld.DispatchEvent(ControllerCommand.CHANGE_PLAYER_HP_MP);
                }

                if (SceneLogic.GetInstance().mainHero.property.target == hero && hero.heroSetting.MonsterGrade > KMonsterGrade.mgNormal)//主角正打boss
                {
                    EventDispatcher.GameWorld.DispatchEvent(ControllerCommand.ATTACK_BOSS);
                }

				if (hero.property.fightHp < hero.Hp )
				{
					hero.property.fightHp = hero.Hp;
				}
            }
        }

        private void OnMoveSpeed(KProtoBuf buf)
        {
            S2C_SYNC_MOVE_SPEED respond = buf as S2C_SYNC_MOVE_SPEED;
            SceneEntity hero = GetSceneObject(respond.heroID) as SceneEntity;
            hero.DispatchEvent(ControllerCommand.SET_SPEED, respond.moveSpeed);
        }

        public void RemoveSceneObject(S2C_REMOVE_SCENE_OBJ respond)
        {
            DisposeSceneObject(respond.objID);
        }
		public void MaxHp(S2C_SYNC_MAX_HP hero)
		{
			SceneEntity targetHero = GetSceneObject(hero.heroID) as SceneEntity;
			if (null!=targetHero)
				targetHero.property.maxHp = (int)hero.MaxHP;
			
		}
		
		public void MaxMp(S2C_SYNC_MAX_MP hero)
		{
			return;
			SceneEntity targetHero = GetSceneObject(hero.heroID) as SceneEntity;
			if (null!=targetHero)
				targetHero.property.maxMp = (int)hero.MaxMP;
		}
        //public static DebugCounter debugCounter =  new DebugCounter();
        public void AddHero(S2C_SYNC_NEW_HERO hero)
        {
            //debugCounter.Restart();
            //debugCounter.AddMark("heroSet");
            KSceneObjectType sot  = KSceneObjectType.sotHero;
            KHeroObjectType hot = KHeroObjectType.hotInvalid;
            KDoodadType dt = KDoodadType.dddInvalid;
            KHeroSetting heroSetting = KConfigFileManager.GetInstance().heroSetting.getData(hero.wNpcIDOrJob.ToString());
            log.Assert(heroSetting != null);
            hot = heroSetting.HeroType;
            string Name = "";
            string Title = "";

            switch (heroSetting.HeroType)
            {
                case KHeroObjectType.hotNpc:
                    {
                        if (!npcidToSceneidDic.ContainsKey((int)heroSetting.ID))
                            npcidToSceneidDic.Add((int)heroSetting.ID, hero.id);
                        else
                            npcidToSceneidDic[(int)heroSetting.ID] = hero.id;
                        Name = heroSetting.Name;
                        Title = heroSetting.Title;
                    }
                    break;
                case KHeroObjectType.hotMonster:
                    {
                        Name = heroSetting.Name;
                    }
                    break;
                case KHeroObjectType.hotPlayer:
                    {
                        Assets.Scripts.Model.Player.Player newPlayerInfo = PlayerManager.GetInstance().GetPlayer(hero.uOwnerID);
                        if (newPlayerInfo != null)
                        {
                            Name = newPlayerInfo.PlayerName;
                        }
                        else
                        {
                            Name = "not init";
                        }
                    }
                    break;
                default:
                    {
                        throw new ArgumentException("error heroSetting.HeroType = " + heroSetting.HeroType + " at hero which id = " + hero.id + " uOwnerID =  " + hero.uOwnerID);
                    }
            }

            if (hero.uOwnerID == PlayerManager.GetInstance().MajorPlayer.PlayerID)
            {
                PlayerManager.GetInstance().MajorPlayer.hero = hero.id;
                EventDispatcher.GameWorld.Dispath(ControllerCommand.CHANGE_NICKNAME, PlayerManager.GetInstance().MajorPlayer.PlayerName);
            }

            SceneEntity entity = CreateSceneObject(hero.id, sot, hot, dt, MapUtils.GetMetreFromInt(hero.posX, hero.posY), hero.uOwnerID);
            entity.heroSetting = heroSetting;

			if (heroSetting.HeroType == KHeroObjectType.hotMonster && hero.bNewHero != 0)
			{
				entity.property.bNewHero = true;
			}
			entity.Init();
			entity.DispatchEvent(ControllerCommand.SET_SPEED, hero.moveSpeed);
            entity.property.isDeaded = entity.property.isDeadTemp = (hero.moveState == (byte)KMoveState.mosDeath);
            entity.property.Face = hero.faceDir;
			entity.property.maxHp = (int)hero.MaxHP;
			entity.property.maxMp = (int)hero.MaxMP;
			entity.property.fightHp = entity.property.hp = (int)hero.HP;
			entity.property.mp = (int)hero.MP;
            entity.AttackSpeed = (int)hero.attackSpeed;
			entity.transform.localScale = new Vector3(heroSetting.Scale,heroSetting.Scale,heroSetting.Scale);
			entity.property.characterController.size  = new Vector3(0.7f, 2f ,0.7f)  ;
			entity.property.characterController.center = new Vector3(0,1,0) ;

            if (hot == KHeroObjectType.hotPlayer && hero.uOwnerID == PlayerManager.GetInstance().MajorPlayer.PlayerID)
            {
                SceneLogic.GetInstance().MainHero = entity;
            }
			
            Vector3 destination = MapUtils.GetMetreFromInt(hero.desX, hero.desZ, hero.desY);
			if (hero.desX == 0 && hero.desZ == 0 && hero.desY == 0)
			{
				destination = MapUtils.GetMetreFromInt(hero.posX, hero.posY);
			}
            entity.TabID = hero.wNpcIDOrJob;
            entity.Job = KJob.jobNone;
            if (entity.HeroType == KHeroObjectType.hotPlayer)
            {
                entity.Job = (KJob)hero.wNpcIDOrJob;
            }

            entity.DispatchEvent(ControllerCommand.SET_DESTINATION, destination, (int)KForceMoveType.fmtInvalid);
            entity.DispatchEvent(ControllerCommand.CHANGE_NAME, Name);
            entity.DispatchEvent(ControllerCommand.LOAD_RES);
            entity.DispatchEvent(ControllerCommand.LOAD_NAME_LABEL, Name);
            if (Title.Length > 0)
            {
                entity.DispatchEvent(ControllerCommand.CHANGE_TITLE, Title);
            }
            entity.EquipIDs = hero.equipIDs;
			
			if (entity.property.isDeadTemp)
            {
                entity.property.isDeadTemp = true;
                entity.Action.IsBeDead();
            }
			else if (heroSetting.HeroType == KHeroObjectType.hotMonster && hero.bNewHero != 0)
			{
				entity.ActiveAction = new ActionMousterOut(entity);
			}
        }

        public void AddSceneObj(S2C_SYNC_SCENE_OBJ sceneObj)
        {
            KSceneObjectType sot = (KSceneObjectType)sceneObj.type;
            KHeroObjectType hot = KHeroObjectType.hotInvalid;
            KDoodadType dt = KDoodadType.dddInvalid;

            SceneEntity entity = CreateSceneObject(sceneObj.id, sot, hot, dt, MapUtils.GetMetreFromInt(sceneObj.x, sceneObj.y), ConfigManager.INVALID_ID);
			entity.Init();
        }

        public void OnAddDoodad(uint objID, int objType, int posX, int posY, RemoteTable table)
        {
            KSceneObjectType sot = KSceneObjectType.sotDoodad;
            KHeroObjectType hot = KHeroObjectType.hotInvalid;
            KDoodadType dt = (KDoodadType)objType;
                
            SceneEntity entity = CreateSceneObject(objID, sot, hot, dt, MapUtils.GetMetreFromInt(posX, posY), ConfigManager.INVALID_ID);

            if (dt == KDoodadType.dddCollect)
            {
                int collectID = (int)table["CollectMissionID"];

                KCollectMissionInfo info = KConfigFileManager.GetInstance().GetCollectInfo(collectID);
                if (info == null)
                {
                    Debug.Log("AddSceneObj Error Collect ID" + collectID.ToString());
                }

                entity.collectInfo = info;
                entity.Init();
                entity.DispatchEvent(ControllerCommand.LOAD_NAME_LABEL);
                entity.DispatchEvent(ControllerCommand.CHANGE_NAME, info.strName);
            }
            else if (dt == KDoodadType.dddDrop)
            {
                KDropType dType = (KDropType)(int)table["eDropType"];
                int nValue = table["nDropValue"];
                int nIndex = table["nDropIndex"];
				entity.property.dropType = dType;
				entity.property.dropValue = nValue;
				entity.property.dropIndex = nIndex;
                entity.Init();
                entity.DispatchEvent(ControllerCommand.LOAD_NAME_LABEL);

                if (dType == KDropType.dtMoney)
                {
                    string strMoney = "money *" + nValue;
                    entity.DispatchEvent(ControllerCommand.CHANGE_NAME, strMoney);
                }
                else if(dType == KDropType.dtEquip)
                {
                    KTabLineEquip equipData = KConfigFileManager.GetInstance().equipTabInfos.getData(nIndex.ToString());
                    entity.DispatchEvent(ControllerCommand.CHANGE_NAME, equipData.Name);

                }
                else if (dType == KDropType.dtItem)
                {
                    KTabLineItem itemData  = KConfigFileManager.GetInstance().itemTabInfos.getData(nIndex.ToString());
                    entity.DispatchEvent(ControllerCommand.CHANGE_NAME, itemData.Name);
                }
            }
        }

		//这个接口只给登陆界面使用.
		public void RemoveSceneObjInfor(uint id)
		{
			allObjectsMap.Remove(id);
		}

        public SceneEntity CreateSceneObject(uint id, KSceneObjectType type, KHeroObjectType heroType, KDoodadType doodadType, Vector3 pos, ulong OwnerID)
        {
            log.Assert(type > (int)KSceneObjectType.sotInvalid);
            log.Assert(id != ConfigManager.INVALID_ID);
            SceneEntity obj = null;

            string soName = type.ToString() + "_" + id.ToString()+"("+heroType.ToString()+")" ;
            obj = SceneEntity.Create(soName);
            obj.Position = pos;
            obj.property.Id = id;
			obj.OwnerID = OwnerID;
            obj.property.sceneObjType = type;
            obj.property.heroObjType = heroType;
            obj.property.doodadObjType = doodadType;
            
            allObjectsMap[obj.property.Id] = obj;
            obj.enabled = true;
            obj.gameObject.layer = CameraLayerManager.GetInstance().GetSceneObjectTag();
            return obj;
        }

        public uint GetNpcSceneID(int npcID)
        {
            if (npcidToSceneidDic.ContainsKey(npcID))
                return npcidToSceneidDic[npcID];

            return 0;
        }
		
		public SceneEntity[] GetSceneObjs()
        {
            SceneEntity[] ids = new SceneEntity[allObjectsMap.Count];
            int i = 0;
            foreach (uint k in allObjectsMap.Keys)
            {
                ids[i++] = allObjectsMap[k];
            }
            return ids;
        }

        public uint[] GetSceneObjIds()
        {
            uint[] ids = new uint[allObjectsMap.Count];
            int i = 0;
            foreach (uint k in allObjectsMap.Keys)
            {
                ids[i++] = k;
            }
            return ids;
        }

        public CollectObjComponent[] GetAllCollectObj()
        {
            List<CollectObjComponent> collectObjs = new List<CollectObjComponent>();
            foreach (SceneEntity se in allObjectsMap.Values)
            {
                if (se.property.doodadObjType == KDoodadType.dddCollect)
                {
                    CollectObjComponent coc = se.GetEntityComponent<CollectObjComponent>();
                    collectObjs.Add(coc);
                }
            }

            return collectObjs.ToArray();
        }

		public bool TryGetSceneObject(uint id, out SceneEntity data)
        {
            return allObjectsMap.TryGetValue(id, out data);
        }

        public SceneEntity GetSceneObject(uint id)
        {
            SceneEntity data = null;
            allObjectsMap.TryGetValue(id, out data);
            return data;
        }

        public void DisposeSceneObject(uint id)
        {
            log.Assert(id != ConfigManager.INVALID_ID);

            SceneEntity sceneObject;
            if (allObjectsMap.TryGetValue(id, out sceneObject))
            {
                sceneObject.Dispose();
				if(sceneObject.HeroType == KHeroObjectType.hotMonster)
				{
					sceneObject.gameObject.AddComponent<DestoryMonster>();
				}
                else
				{
					GameObject.DestroyImmediate(sceneObject.gameObject);
				}
                allObjectsMap.Remove(id);
            }
        }
		public void Clear()
		{
			allObjectsMap.Clear();
			npcidToSceneidDic.Clear();
		}

        public SceneEntity MainHero
        {
            set
            {
                mainHero = value;
                mainHero.property.isMainHero = true;
                mainHero.property.isInteractive = false;
				mainHero.AddEntityComponent<AutoAttackComponent>();
                EventDispatcher.GameWorld.DispatchEvent(ControllerCommand.SET_MAIN_HERO);
                ContinueMoveAcrossMap();
            }
            get
            {
                return mainHero;
            }
        }

        bool m_bInAutoAcrossMap = false;
        uint m_currentMoveToMapID;
        List<uint> m_mapAcrossIDList;
        Vector3 m_vecTargetPos;

        public void ClearAutoMoveAcrossMap()
        {
            m_bInAutoAcrossMap = false;
            m_currentMoveToMapID = 0;
            m_mapAcrossIDList = null;
            m_vecTargetPos = Vector3.zero;
        }

        void ContinueMoveAcrossMap()
        {
            if (m_bInAutoAcrossMap)
            {
                if (this.mapId == m_currentMoveToMapID)
                {
                    MoveToNextMap();
                }
                else
                {
                    m_bInAutoAcrossMap = false;
                }
            }
        }

        public void MoveAcrossMap(uint mapID, Vector3 _destination)
        {
            uint fromMapID = this.mapId;
            m_mapAcrossIDList = SearchAcrossMapRoad(fromMapID, mapID);
            if (m_mapAcrossIDList == null)
            {
                Debug.LogError("MoveAcrossMap  error targetMapID; from " + fromMapID.ToString() + "to " + mapID.ToString());
            }

            m_bInAutoAcrossMap = true;
            m_vecTargetPos = new Vector3(_destination.x, _destination.y, _destination.z);

            MoveToNextMap();
        }

        private void MoveToNextMap()
        {
            if (m_mapAcrossIDList.Count > 0)
            {
                uint fromMapID = this.mapId;
                m_currentMoveToMapID = m_mapAcrossIDList[0];
                m_mapAcrossIDList.Remove(m_currentMoveToMapID);

                KTabFile<KMapTriggerInfo> focusMap = KConfigFileManager.GetInstance().mapMapsTrigger[fromMapID];
                foreach (KeyValuePair<string, KMapTriggerInfo> kmt in focusMap.getAllData())
                {
                    if (kmt.Value.ChangeMapID == m_currentMoveToMapID)
                    {
                        string[] pos = kmt.Value.TriggerPoint.Split(';');

                        if (pos.Length != 2)
                        {
                            Debug.LogError("MoveToNextMap  error TriggerPoint; from " + fromMapID.ToString() + "to " + m_currentMoveToMapID.ToString());
                        }

                        Vector3 target = MapUtils.GetMetreFromInt(int.Parse(pos[0]), int.Parse(pos[1]));
                        this.MainHero.DispatchEvent(ControllerCommand.MOVE_TO_DES, target, true);
                        break;
                    }
                }
            }
            else
            {
                m_bInAutoAcrossMap = false;
                this.MainHero.DispatchEvent(ControllerCommand.MOVE_TO_DES, m_vecTargetPos, true);
            }
        }

        private uint m_toMapId;
        private uint m_fromMapId;
        private List<List<uint>> m_resultList = new List<List<uint>>();

        static int Compare(List<uint> a, List<uint> b)
        {
            if (a.Count < b.Count) return 1;
            if (a.Count > b.Count) return -1;
            return 0;
        }

        public List<uint> SearchAcrossMapRoad(uint fromMapId, uint toMapId)
        {
            KMapListSetting fromMap = KConfigFileManager.GetInstance().mapListSetting.getData(fromMapId.ToString());
            KMapListSetting toMap = KConfigFileManager.GetInstance().mapListSetting.getData(toMapId.ToString());

            m_resultList.Clear();
            if(fromMap != null && toMap != null)
            {
                if (!KConfigFileManager.GetInstance().mapMapsTrigger.ContainsKey(fromMapId))
                {
                    return null;
                }

                List<uint> haveSearchedList = new List<uint>();
                haveSearchedList.Add(fromMapId);
                List<uint> resultList = new List<uint>();
                resultList.Add(fromMapId);
                m_toMapId = toMapId;
                m_fromMapId = fromMapId;

                this.SearchMap(KConfigFileManager.GetInstance().mapMapsTrigger[fromMapId], haveSearchedList, resultList);
                m_resultList.Sort(Compare);
            }

            if (m_resultList.Count > 0)
            {
                return m_resultList[0];
            }
            else
            {
                return null;
            }
        }

        private void SearchMap(KTabFile<KMapTriggerInfo> focusMap, List<uint> haveSearched, List<uint> resultList)
        {
            foreach(KeyValuePair<string, KMapTriggerInfo> kmt in focusMap.getAllData())
			{
                uint id = kmt.Value.ChangeMapID;
                if (haveSearched.IndexOf(id) == -1)
                {
                    List<uint> tempR = new List<uint>();
                    tempR.AddRange(resultList);
                    tempR.Add(id);

                    if (id == m_toMapId)
                    {
                        tempR.Remove(m_fromMapId);
                        m_resultList.Add(tempR);
                        break;
                    }
                    else
                    {
                        List<uint> tempH = new List<uint>();
                        tempH.AddRange(haveSearched);
                        foreach (KeyValuePair<string, KMapTriggerInfo> exitKmt in focusMap.getAllData())
                        {
                            tempH.Add(exitKmt.Value.ChangeMapID);
                        }

                        if (KConfigFileManager.GetInstance().mapMapsTrigger.ContainsKey(id))
                        {
                            this.SearchMap(KConfigFileManager.GetInstance().mapMapsTrigger[id], tempH, tempR);
                        }
                    }
                }
            }
            haveSearched = null;
            resultList = null;
        }
    }
}

