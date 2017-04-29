using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.Manager;
using Assets.Scripts.Utils;
using Assets.Scripts.Define;
using Assets.Scripts.Proto;
using Assets.Scripts.Logic.Npc;
using Assets.Scripts.Logic.Mission;
using Assets.Scripts.View;

namespace Assets.Scripts.Logic.Scene.SceneObject.Compont
{
    public class MoveComponent : BaseComponent
    {
        public override string GetName()
        {
            return GetType().Name;
        }

        public override void OnAttachToEntity(SceneEntity ety)
        {
            BaseInit(ety);

            // 注册事件响应函数
            Regist(ControllerCommand.SET_SPEED, OnSpeedChange);
            Regist(ControllerCommand.SET_DESTINATION, OnDestinationChange);
            Regist(ControllerCommand.MOVE_TO_DES, OnMoveToDestination);
            Owner.property.characterController = Owner.gameObject.AddComponent<BoxCollider>();
        }

        public override void OnDetachFromEntity(SceneEntity ety)
        {
            UnRegist(ControllerCommand.SET_SPEED, OnSpeedChange);

            base.OnDetachFromEntity(ety);
        }

        public object OnSpeedChange(params object[] objs)
        {
            float speed = Convert.ToSingle(objs[0]);
            Owner.Speed = speed;
            Owner.Speed /= 10.0f;
            return null;
        }

        public object OnDestinationChange(params object[] objs)
        {
            Vector3 destination = (Vector3)objs[0];
			destination = KingSoftCommonFunction.GetGoundHeight( KingSoftCommonFunction.NearPosition(destination));
            int moveType = (int)objs[1];
			
            Owner.property.destination = destination;
			if (!Owner.property.isMainHero)
            	Owner.property.finalDestination = KingSoftCommonFunction.GetGoundHeight( KingSoftCommonFunction.NearPosition(destination));
			if (Owner.Position == Owner.property.destination)
            {
                return null;
            }
            if (moveType == (int)KForceMoveType.fmtJump)
            {
                Owner.DispatchEvent(ControllerCommand.Jump, destination);
                return null;
            }
            else if (moveType == (int)KForceMoveType.fmtRush)
            {
                Owner.DispatchEvent(ControllerCommand.Drag, destination);
                return null;
            }
            else if (moveType == (byte)KForceMoveType.fmtPull)
            {
                Owner.DispatchEvent(ControllerCommand.Drag, destination);
                return null;
            }
            else if (moveType == (byte)KForceMoveType.fmtBack)
            {
                Owner.DispatchEvent(ControllerCommand.Back, destination);
                return null;
            }
			else if(moveType == (byte)KForceMoveType.fmtPlayerFuKong)
			{
				Owner.DispatchEvent(ControllerCommand.FuKong, destination);
				return null;
			}
            else //if (moveType == (byte)KForceMoveType.fmtInvalid)
            {
                bool bRet = Owner.ActiveAction.TryFinish();
				if (!bRet)
				{
					bool bRet2 = Owner.ActiveAction.MoveToDistance(destination, Owner.Speed);
					if (bRet2)
					{
						if (Owner.property.isMainHero)
							Owner.property.finalDestination = KingSoftCommonFunction.GetGoundHeight( KingSoftCommonFunction.NearPosition(destination));
						return null;
					}
				}
                if(Owner.Position.x == destination.x && Owner.Position.z == destination.z)
				{
					Owner.Position = destination;
					return null;
				}
                ActionWalk walk = new ActionWalk(Owner);
                walk.speed = Owner.Speed;
                walk.IsPushStack = false;
                walk.endPosition = destination;
                Owner.DispatchEvent(ControllerCommand.SetActiveAction, walk);
            }
            return null;
        }

        public object OnMoveToDestination(params object[] objs)
        {
            if (objs.Length < 1)
            {
                return null;
            }

            Vector3 _destination;
            _destination = (Vector3)objs[0];

            bool sendMessage = false;
            if (objs.Length >= 2)
            {
                sendMessage = Convert.ToBoolean(objs[1]);
            }

            if (Owner.property.isMainHero)
            {
                Owner.property.finalDestination = KingSoftCommonFunction.GetGoundHeight(_destination);
                AnimationComponent.OperaWalking = true;
            }

            EventRet ret = Owner.DispatchEvent(ControllerCommand.TryFinishAction);
            bool bRet = (bool)ret.GetReturn<AnimationComponent>();
            if (bRet)
            {
                ActionWalk walk = new ActionWalk(Owner);
				if (objs.Length >= 3)
				{
					walk.deltaSpace = (float)(objs[3]);
				}
                walk.speed = Owner.Speed;
                walk.IsPushStack = sendMessage;
                walk.endPosition = _destination;
                Owner.DispatchEvent(ControllerCommand.SetActiveAction, walk);
            }
            else
            {
                Owner.DispatchEvent(ControllerCommand.ActionMoveToDistance, _destination, Owner.Speed, sendMessage);
            }

            return null;
        }

        public override void DoUpdate()
        {
            if (PathUtil.NPC_ID != -1)
            {
                if (NpcLogic.GetInstance().CheckNpcNearby(PathUtil.NPC_ID))
                {
                    if (PathUtil.bAutoAttack)
                    {
                        SceneLogic.GetInstance().MainHero.property.CmdAutoAttack = true;
                    }
                    else
                    {
                        EventDispatcher.GameWorld.Dispath(ControllerCommand.OPEN_NPC_PANEL_BYID, PathUtil.NPC_ID);
                    }

                    PathUtil.bAutoAttack = false;
                    PathUtil.NPC_ID = -1;
                }
            }

            ViewManager.GetInstance().AutoCloseNpcPanel();

            CollectObjComponent[] collectObjs = SceneLogic.GetInstance().GetAllCollectObj();

            foreach (CollectObjComponent coc in collectObjs)
            {
                if (CollectObjLogic.GetInstance().CheckNeedCollectID(coc.Owner.collectInfo.nID) && coc.CheckNearBy(coc.Owner.collectInfo.nCanCollectDistance))
                {
                    EventDispatcher.GameWorld.Dispath(ControllerCommand.OPEN_COLLECT_PANEL, coc.Owner.collectInfo.nID, coc.Owner.property.Id);
                    break;
                }
            }
        }
    }
}
