using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Define;
using UnityEngine;
using Assets.Scripts.Manager;
using Assets.Scripts.Data;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.Model.Skill;
namespace Assets.Scripts.Logic.Scene.SceneObject.Compont
{
	public class SpeedItem
	{
		public SpeedItem(string animName,float speed)
		{
			this.animName = animName;
			this.speed =  speed;
		}
		public string animName;
		public float speed ;
	}
    public class AnimationComponent : BaseComponent
    {
		public static bool OperaWalking = false;
		public Ticker fightTicker = new Ticker(10000); //10秒战斗待机.
		public bool pause = false;
        public override string GetName()
        {
            return GetType().Name;
        }
		public bool IsFighting()
		{
			return fightTicker.IsInCD();
		}
		public void ResetFightingState()
		{
			fightTicker.Restart();
		}
		
        public override void OnAttachToEntity(SceneEntity ety)
        {
            BaseInit(ety);
			fightTicker.Stop();
            // 注册事件响应函数
            Regist(ControllerCommand.CrossFadeAnimation, CrossFadeAnimation);
            Regist(ControllerCommand.SetActiveAction, SetActiveAction);
            Regist(ControllerCommand.FinishImmediate, FinishImmediate);
            Regist(ControllerCommand.TryFinishAction, TryFinishAction);
            Regist(ControllerCommand.ActionMoveToDistance, ActionMoveToDistance);
            Regist(ControllerCommand.PlayAnimation, PlayAnimation);
            Regist(ControllerCommand.LookAtPos, LookAtPos);
            Regist(ControllerCommand.IsPlayingActionFinish, IsPlayingActionFinish);
            Regist(ControllerCommand.Back, Back);
			Regist(ControllerCommand.FuKong, FuKong);
            Regist(ControllerCommand.Drag, Drag);
            Regist(ControllerCommand.Jump, Jump);
            Regist(ControllerCommand.Idle, Idle);

        }

        public override void OnDetachFromEntity(SceneEntity ety)
        {
            UnRegist(ControllerCommand.CrossFadeAnimation, CrossFadeAnimation);
            UnRegist(ControllerCommand.SetActiveAction, SetActiveAction);
            UnRegist(ControllerCommand.FinishImmediate, FinishImmediate);
            UnRegist(ControllerCommand.TryFinishAction, TryFinishAction);
            UnRegist(ControllerCommand.ActionMoveToDistance, ActionMoveToDistance);
            UnRegist(ControllerCommand.PlayAnimation, PlayAnimation);
            UnRegist(ControllerCommand.LookAtPos, LookAtPos);
            UnRegist(ControllerCommand.IsPlayingActionFinish, IsPlayingActionFinish);
            UnRegist(ControllerCommand.Back, Back);
			UnRegist(ControllerCommand.FuKong, FuKong);
            UnRegist(ControllerCommand.Drag, Drag);
            UnRegist(ControllerCommand.Jump, Jump);
            UnRegist(ControllerCommand.Idle, Idle);

            base.OnDetachFromEntity(ety);
        }
		public void StopAnim()
		{
			if (null != Owner.Anim)
				Owner.Anim.Stop();
		}
        public virtual object PlayAnimation(params object[] objs)
        {
            string animName = objs[0] as string;
            AMIN_MODEL model = AMIN_MODEL.DEFAULT;
            bool noReset = true;
            if (objs.Length >= 2)
            {
                model = (AMIN_MODEL)objs[1];
            }
            if (objs.Length >= 3)
            {
                noReset = Convert.ToBoolean(objs[2]);
            }

            Owner.L_Anim_Name = animName;
            Owner.AnimModel = model;
            if (null == Owner.Anim)
            {
                return false;
            }
            if (noReset && Owner.Anim.IsPlaying(animName))
            {
                return true;
            }
            StopAnim();
			try
			{
				if (model == AMIN_MODEL.ONCE)
	            {
	                Owner.Anim[animName].wrapMode = WrapMode.ClampForever;
	            }
	            else if (model == AMIN_MODEL.LOOP)
	            {
	                Owner.Anim[animName].wrapMode = WrapMode.Loop;
	            }
			}catch (NullReferenceException e)
			{
				
			}
            if (Owner.Anim.GetClip(animName))
            {
            	Owner.Anim.CrossFade(animName);
            }
            return true;
        }

        public virtual object CrossFadeAnimation(params object[] objs)
        {
			string animName = Convert.ToString(objs[0]);			
			AMIN_MODEL model = AMIN_MODEL.DEFAULT;
            bool noReset = true;
			bool isFade = false;
			float fadeTime = 0;
            if (objs.Length == 3)
            {
				model = (AMIN_MODEL)objs[1];
                noReset = Convert.ToBoolean(objs[2]);
            }
			else if(objs.Length == 2)
			{
				isFade = true;
				fadeTime = (float)objs[1];
			}
			else if(objs.Length == 4)
			{
				isFade = true;
				fadeTime = (float)objs[3];
			}
            Owner.L_Anim_Name = animName;
            Owner.AnimModel = model;
            if (null == Owner.Anim)
            {
                return false;
            }
            if (noReset && Owner.Anim.IsPlaying(animName))
            {
                return true;
            }
			try
			{
				if (model == AMIN_MODEL.ONCE)
	            {
	                Owner.Anim[animName].wrapMode = WrapMode.Once;
	            }
	            else if (model == AMIN_MODEL.LOOP)
	            {
	                Owner.Anim[animName].wrapMode = WrapMode.Loop;
	            }
			}
			catch(NullReferenceException e)
			{
				
			}
            
            if (Owner.Anim.GetClip(animName))
            {
				if (isFade)
				{
					if (fadeTime < 0.01f)
						Owner.Anim.Play(animName);
					else
						Owner.Anim.CrossFade(animName,fadeTime);
					//Debug.LogWarning("CrossFade "+animName+" "+fadeTime);
					
				}
				else
				{
					//Debug.LogWarning("CrossFade "+animName+" ");
					Owner.Anim.CrossFade(animName);
				}
            }
            return true;
        }

        public void SetLoop(string animName, bool b)
        {
            if (null == Owner.Anim)
            {
                return;
            }
            AnimationClip clip = Owner.Anim.GetClip(animName);
            if (clip == null)
            {
                return;
            }
            if (!Owner.Anim.IsPlaying(animName))
                CrossFadeAnimation(animName);
            if (b)
            {
                if (clip.wrapMode != WrapMode.Loop)
                    clip.wrapMode = WrapMode.Loop;
                return;
            }
            if (clip.wrapMode != WrapMode.Once)
                clip.wrapMode = WrapMode.Once;

        }
		public bool IsPlayingEx(string animName)
		{
			 if (null == Owner.Anim)
				return false;
			return Owner.Anim.IsPlaying(animName) && Owner.Anim[animName].normalizedTime < 1f;
		}
		public bool IsPlaying(string animName)
		{
			 if (null == Owner.Anim)
				return false;
			return Owner.Anim.IsPlaying(animName);
		}
		public string GetLastLogicAnimName()
		{
			return Owner.L_Anim_Name;
		}
        public string GetCurAnimName()
        {
            if (null != Owner.Anim && null != Owner.Anim.clip)
                return Owner.Anim.clip.name;
            return "";
        }
		public float GetAnimLong(string animName)
		{
			if (null == Owner.Anim)
                return 0.5f;
            if (Owner.Anim.GetClip(animName) == null)
            {
                return 0.5f;
            }
            return Owner.Anim[animName].clip.length;
		}
        public float GetCurAnimLong()
        {
            if (null == Owner.Anim)
                return 0.5f;
            if (Owner.Anim.GetClip(Owner.L_Anim_Name) == null)
            {
                return 0.5f;
            }
            return Owner.Anim[Owner.L_Anim_Name].clip.length;
        }
		
		
		public void PushBackSpead(float Speed)
		{
			if (null == Owner.Anim)
                return;
            if (Owner.Anim.GetClip(Owner.L_Anim_Name) == null)
            {
                return;
            }
            speedStake.Add(new SpeedItem(Owner.L_Anim_Name,Owner.Anim[Owner.L_Anim_Name].normalizedSpeed));
            Owner.Anim[Owner.L_Anim_Name].normalizedSpeed = Speed;
		}
		List<SpeedItem> speedStake = new List<SpeedItem>();
		
		public bool IsSpeedStackEmpty()
		{
			return speedStake.Count == 0;
		}
		public void PopSpeed()
		{
			if ( speedStake.Count > 0 )
			{
				SpeedItem item = speedStake[0];
				speedStake.RemoveAt(0);
				Owner.Anim[item.animName].normalizedSpeed = item.speed;
			}
		}
        public virtual void SetSpeed(float s)
        {
            if (null == Owner.Anim)
                return;
            if (Owner.Anim.GetClip(Owner.L_Anim_Name) == null)
            {
                return;
            }
            Owner.Anim[Owner.L_Anim_Name].normalizedSpeed = s;
        }

        public virtual void SetCurAnimTime(float t)
        {
            if (null == Owner.Anim)
                return;
            if (Owner.Anim.GetClip(Owner.L_Anim_Name) == null)
            {
                return;
            }
            //if( !anim.IsPlaying(l_anim_name))
            //	CrossFadeAnimation(l_anim_name);
            Owner.Anim[Owner.L_Anim_Name].time = t;
        }

        public virtual object IsPlayingActionFinish(params object[] objs)
        {
            string anim_name = Convert.ToString(objs[0]);
            if (null == Owner.property.bodyGo)
                return false;
            bool b = Owner.Anim.IsPlaying(anim_name);
			if (objs.Length>1)
			{
				if((bool)objs[1])
				{
					return !b;
				}
			}
            if (b)
            {
                return Owner.Anim[anim_name].normalizedTime >= 1.0;
            }
            return true;
        }

        public virtual object LookAtPos(params object[] objs)
        {
            Vector3 position = (Vector3)objs[0];
            Vector3 dir = position - Owner.transform.position;
            dir = new Vector3(dir.x, 0, dir.z);
            if (dir.x != 0 && dir.z != 0)
            {
                Owner.transform.forward = dir;
            }
            return null;
        }

        public virtual object FinishImmediate(params object[] objs)
        {
            Owner.ActiveAction = new ActionNull(Owner);
            return null;
        }

        public virtual object TryFinishAction(params object[] objs)
        {
            return Owner.ActiveAction.TryFinish();
        }

        public virtual object ActionMoveToDistance(params object[] objs)
        {
            Vector3 position = (Vector3)objs[0];
            float speed = Convert.ToSingle(objs[1]);
            bool sendMessage = Convert.ToBoolean(objs[2]);

            return Owner.ActiveAction.MoveToDistance(position, speed);
        }

        public virtual object SetActiveAction(params object[] objs)
        {
            Action active = objs[0] as Action;
            if (active.isAidAction)
                Owner.AidAction = active;
            //nextAction2
            if (active.IsPushStack)
            {
                if (Owner.ActiveAction.TryFinish())
                {
                    Owner.ActiveAction = active;
                }
                else
                {
                    Owner.property.nextAction = active;
                }
            }
            else
            {
                Owner.ActiveAction = active;
            }
            return null;
        }

        public override void DoUpdate()
        {
			if(pause)
				return;
			if (Owner.property.heroObjType == KHeroObjectType.hotPlayer && null != Owner.AnimCmp && null != Owner.Weapon)
			{
				if (Owner.ActiveAction != null && Owner.ActiveAction.WeaponPosition != WeaponComponent.BIND_POINT.DEFAULT )
				{
					Owner.Weapon.SetWeaponPosition(Owner.ActiveAction.WeaponPosition);
				}
				else if (Owner.AnimCmp.IsFighting() )
				{
					Owner.Weapon.SetWeaponPosition(WeaponComponent.BIND_POINT.RIGHT_HAND);
				}
				else
				{
					Owner.Weapon.SetWeaponPosition(WeaponComponent.BIND_POINT.BEI);
				}
			}
            if (Owner.property.isDeadTemp)
            {
                if (null == Owner.ActiveAction || (Owner.ActiveAction.isDead == false && Owner.ActiveAction.actionType != Action.ACTION_TYPE.FLY ))
                {
                    ActionDead dead = new ActionDead(Owner);
                    Owner.ActiveAction = dead;
                }
            }
            if (Owner.ActiveAction.IsFinish())
            {
                if (null == Owner.property.nextAction || !Owner.property.nextAction.IsCanActive())
                {
					if (Owner.property.isMainHero)
					{
						if (  null != Owner.property.target && Owner.property.target.property.isCanAttack && Owner.property.AutoAttack && !Owner.property.target.property.isDeadTemp && !Owner.property.CmdAutoAttack )
	                    {
	                        OperaAttack action = new OperaAttack(Owner);
	                        action.IsPushStack = true;
							KActiveSkill skill = KConfigFileManager.GetInstance().GetActiveSkill((uint)Owner.Job, 1);
				            if (null == skill)
				                return;
				            action.deltaSpace = ((float)skill.CastRange) / 100f;
	                        action.target = Owner.property.target;
	                        Owner.ActiveAction = action;
	                    }
	                    else
	                    {
							if (OperaWalking)
							{
								if (Owner.Position.x != Owner.property.finalDestination.x||Owner.Position.z != Owner.property.finalDestination.z)
								{
									ActionWalk action = new ActionWalk(Owner);
	                        		action.endPosition = Owner.property.finalDestination;
									Owner.ActiveAction = action;
									Owner.property.finalDestination = action.endPosition;
								}
								else
								{
									OperaWalking = false;
									ActionIdle action = new ActionIdle(Owner);
	                        		Owner.ActiveAction = action;
								}
								
							}
							else
							{
								ActionIdle action = new ActionIdle(Owner);
	                        	Owner.ActiveAction = action;
							}
	                        
	                    }
					}
					else
					{
						if (Owner.Position.x != Owner.property.finalDestination.x || Owner.Position.z != Owner.property.finalDestination.z)
						{
							ActionWalk action = new ActionWalk(Owner);
							action.beginPosition = Owner.Position;
							action.endPosition = Owner.property.finalDestination;
							action.speed = Owner.Speed;
							Owner.ActiveAction = action;
							Owner.property.finalDestination = action.endPosition;
						}
						else
						{
							ActionIdle action = new ActionIdle(Owner);
	                   		Owner.ActiveAction = action;
						}
					}
                    
                }
                else
                {
                    Owner.ActiveAction = Owner.property.nextAction;
                    Owner.property.nextAction = null;
                }
            }
            else
            {
				if (Owner.ActiveAction.actionType != Action.ACTION_TYPE.OPERA && !Owner.property.isMainHero )
				{
					if (
						Mathf.Abs(Owner.Position.x - Owner.property.finalDestination.x ) > 0.001f
						||
						Mathf.Abs(Owner.Position.z - Owner.property.finalDestination.z ) > 0.001f 
						)
					{
						Owner.ActiveAction.TryFinish();
					}
					
				}
                Owner.ActiveAction.Update();
            }
            if (null != Owner.AidAction && !Owner.AidAction.IsFinish())
            {
                Owner.AidAction.Update();
            }
        }

        public object Jump(params object[] objs)
        {
            Vector3 _destination = (Vector3)objs[0];
            bool sendMessage = false;

            if (objs.Length >= 2)
            {
                sendMessage = Convert.ToBoolean(objs[1]);
            }

            bool bRet = (bool)TryFinishAction();
            bool bRet2 = (bool)ActionMoveToDistance(_destination, Owner.Speed, sendMessage);

            if (bRet || !bRet2)
            {
                ActionJump walk = new ActionJump(Owner);
                walk.speed = Owner.Speed;
                walk.IsPushStack = sendMessage;
                walk.endPosition = _destination;
                Owner.DispatchEvent(ControllerCommand.SetActiveAction, walk);
            }

            return null;
        }
		
		public object FuKong(params object[] objs)
        {
			Vector3 _destination = (Vector3)objs[0];
			ActionBeHitAndFly action = new ActionBeHitAndFly(Owner);
			action.time = 1f;
			action.endPosition = _destination;
			Owner.DispatchEvent(ControllerCommand.SetActiveAction, action);
			return null;
		}
        public object Back(params object[] objs)
        {
            Vector3 _destination = (Vector3)objs[0];
            bool sendMessage = false;

            if (objs.Length >= 2)
            {
                sendMessage = Convert.ToBoolean(objs[1]);
            }

            bool bRet = (bool)TryFinishAction();
            bool bRet2 = (bool)ActionMoveToDistance(_destination, Owner.Speed, sendMessage);

            if (bRet || !bRet2)
            {
                ActionBack walk = new ActionBack(Owner);
                walk.speed = Owner.Speed;
                walk.IsPushStack = sendMessage;
                walk.endPosition = _destination;
                Owner.DispatchEvent(ControllerCommand.SetActiveAction, walk);
            }
            return null;
        }

        public object Drag(params object[] objs)
        {
            Vector3 _destination = (Vector3)objs[0];
            bool sendMessage = false;

            if (objs.Length >= 2)
            {
                sendMessage = Convert.ToBoolean(objs[1]);
            }

            bool bRet = (bool)TryFinishAction();
            bool bRet2 = (bool)ActionMoveToDistance(_destination, Owner.Speed, sendMessage);

            if (bRet || !bRet2)
            {
                ActionDrag walk = new ActionDrag(Owner);
                walk.speed = Owner.Speed;
                walk.IsPushStack = sendMessage;
                walk.endPosition = _destination;
                Owner.DispatchEvent(ControllerCommand.SetActiveAction, walk);
            }
            return null;
        }

        public object Idle(params object[] objs)
        {
            bool sendMessage = Convert.ToBoolean(objs[0]);

            bool bRet = (bool)TryFinishAction();
            if (bRet)
            {
                ActionIdle idle = new ActionIdle(Owner);
                idle.IsPushStack = sendMessage;
                Owner.DispatchEvent(ControllerCommand.SetActiveAction, idle);
            }
            return null;
        }

    }
}
