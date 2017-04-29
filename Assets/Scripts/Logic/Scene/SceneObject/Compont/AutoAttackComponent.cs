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
using Assets.Scripts.Logic.Skill;

namespace Assets.Scripts.Logic.Scene.SceneObject.Compont
{
	
	public class AutoAttackComponent : BaseComponent {
		
		public static float MaxEnemyDistance = 15;
		Ticker ticker = new Ticker(500);
		protected bool enable = false;//是否挂机状态.
		protected uint [] skillList = {}; //挂机的技能列表. 
		public override string GetName()
	    {
	        return GetType().Name;
	    }
		public static int curIndex = 0;
	
	    public override void OnAttachToEntity(SceneEntity ety)
	    {
			ticker.Restart();
	        BaseInit(ety);	
	        // 注册事件响应函数
	        //Regist(ControllerCommand.AUTO_ATTACK, OnAutoAttack);
	
	    }
		
		public object OnAutoAttack(params object[] objs)
	    {
			Owner.property.CmdAutoAttack =  System.Convert.ToBoolean( objs[0] );
			return null;
	    }
	
	    public override void OnDetachFromEntity(SceneEntity ety)
	    {
	        //UnRegist(ControllerCommand.BE_HIT, OnBeHit);
	        base.OnDetachFromEntity(ety);
	    }
		
		public void UpdateMoveAoe(ActionMovingAOE aoe)
		{
			if( !ticker.IsEnable() )
				return;
			ticker.Restart();
			List<SceneEntity> entitys =  SceneLogic.GetInstance().GetAllSceneObject(KHeroObjectType.hotMonster);
			float distance = float.MaxValue;
			SceneEntity aim = null;
			Vector3 selfPosition = Owner.Position;
			foreach (SceneEntity entity in entitys)
			{
				if (entity.property.isDeadTemp)
					continue;
				float dis = Vector3.Distance(entity.transform.position,selfPosition);
				if ( dis < distance )
				{
					aim = entity;
					distance = dis;
				}
			}
			if (null==aim)
			{
				return;
			}
			float aimDis = Vector3.Distance(aim.Position,Owner.Position) - 2f;
			if ( aimDis < 0 )
			{
				return;
			}
			Vector3 fw = aim.Position - Owner.Position;
			fw.Normalize();
			aim.Position = Owner.Position + (fw*aimDis);
			aoe.MoveToDistance(aim.Position,Owner.property.speed);
		}
		bool SelectAim()
		{
			List<SceneEntity> entitys =  SceneLogic.GetInstance().GetAllSceneObject(KHeroObjectType.hotMonster);
			float distance = MaxEnemyDistance;
			SceneEntity aim = Owner.property.target;
			if ( aim!=null &&(aim.property.isDeaded || aim.property.activeAction.isDead || aim.property.heroObjType == KHeroObjectType.hotMonster) )
				aim = null;
			if (null == aim)
			{
				Vector3 selfPosition = Owner.Position;
				foreach (SceneEntity entity in entitys)
				{
					if (entity.property.isDeaded || ( null != entity.property.activeAction && entity.property.activeAction.isDead))
						continue;
					float dis = Vector3.Distance(entity.transform.position,selfPosition);
					if ( dis < distance )
					{
						aim = entity;
						distance = dis;
					}
				}
				if (null!=aim)
				{
					Owner.property.target = aim;
                    EventDispatcher.GameWorld.DispatchEvent(ControllerCommand.CHANGE_TARGET);
				}
			}
			
			return null != aim;
		}
		public override void DoUpdate()
	    {
			int _listLen = SkillLogic.GetInstance().activeSkillList.Length;
			if (  _listLen >  skillList.Length)
			{
				skillList = new uint[_listLen-1];
				Array.Copy(SkillLogic.GetInstance().activeSkillList,1,skillList,0,_listLen-1);
			}
			
			if(!Owner.property.CmdAutoAttack)
			{
				return;
			}
			if(Owner.ActiveAction.actionType == Action.ACTION_TYPE.FLY)
			{
				return;
			}
			if(Owner.ActiveAction.actionType == Action.ACTION_TYPE.ANIM)
			{
				if (Owner.ActiveAction is ActionMovingAOE )
				{
					ActionMovingAOE aoe = (ActionMovingAOE) Owner.ActiveAction;
					UpdateMoveAoe(aoe);
					return;
				}
				else
				{
					if (!Owner.ActiveAction.IsCanFinish())
					{
						return;
					}
				}
				
			}
			else if (Owner.ActiveAction.actionType == Action.ACTION_TYPE.OPERA )
			{
				return;
			}
			
			if (!SelectAim())
				return;
			
			int _len = skillList.Count();
			if (_len > 0)
			{
				curIndex  = curIndex  % + _len;
				for ( int i = 0 ; i < _len ; i++ )
				{
					
					uint skillId = skillList[curIndex];
					try
					{
						if(!SkillLogic.GetInstance().RequestSkill((uint)skillId))
						{
							curIndex  = (curIndex + 1) % + _len;
							continue;
						}
						KSkillDisplay skillDisplay = KConfigFileManager.GetInstance().GetSkillDisplay(skillId,Owner.property.tabID);
						if(!Owner.ActiveAction.TryFinish())
						{
							continue;
						}
						if(skillDisplay.Opera.CompareTo("TARGET")==0)
						{
			                SceneLogic.GetInstance().MainHero.Action.MoveAndSkill((ushort)skillId, Owner.property.target);
							Owner.property.AutoAttack = false;
							return;
						}
						else if(skillDisplay.Opera.CompareTo("NONE")==0)
						{
							SceneLogic.GetInstance().MainHero.Action.MoveAndSkill((ushort)skillId, Owner.property.target);
							return;
						}
						else if(skillDisplay.Opera.CompareTo("TARGET_DIR")==0)
						{
							SceneLogic.GetInstance().MainHero.Action.MoveAndSkill((ushort)skillId, Owner.property.target);
							Owner.property.AutoAttack = false;
							return;
	
						}
					}
					catch( NullReferenceException e )
					{
						
					}
				}
			}
			OperaAttack action = new OperaAttack(Owner);
            action.IsPushStack = true;
			KActiveSkill skill = KConfigFileManager.GetInstance().GetActiveSkill((uint)Owner.Job, 1);
            if (null == skill)
                return;
            action.deltaSpace = ((float)skill.CastRange) / 100f;
            action.target = Owner.property.target;
            EventDispatcher.GameWorld.DispatchEvent(ControllerCommand.CHANGE_TARGET);
            Owner.ActiveAction = action;
			Owner.property.AutoAttack = false;
		}
	}
}
