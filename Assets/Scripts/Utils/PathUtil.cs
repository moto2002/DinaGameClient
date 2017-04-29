using System;
using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Logic.Npc;
using Assets.Scripts.Logic.Scene;
using Assets.Scripts.Logic.RemoteCall;
using Assets.Scripts.Manager;
using Assets.Scripts.Data;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.Lib.Log;
using Assets.Scripts.Logic.Mission;
using Assets.Scripts.Logic.Scene.SceneObject.Compont;

namespace Assets.Scripts.Utils
{
    public class PathUtil
    {
        public static int NPC_ID = -1;
        public static bool bAutoAttack = false;

        public static void FindNpc(int npcID)
        {
            KNpcPos npcPos = NpcLogic.GetInstance().GetNpcPosByID(npcID);
            if (npcPos != null)
            {
                Vector3 rolePosition = MapUtils.GetMetreFromInt(npcPos.nPathX, npcPos.nPathZ, npcPos.nPathY);
                Goto(npcPos.MapID, rolePosition);

                KHeroSetting npcInfo = NpcLogic.GetInstance().GetNpcLocalInfo(npcID);
                if (null != npcInfo && npcInfo.HeroType == Assets.Scripts.Define.KHeroObjectType.hotMonster)
                {
                    NPC_ID = npcID;
                    bAutoAttack = true;
                }
                    //SceneLogic.GetInstance().MainHero.property.CmdAutoAttack = (npcInfo.HeroType == Assets.Scripts.Define.KHeroObjectType.hotMonster);
            }
        }

        public static void FindNpcAndOpen(int npcID)
        {
            if (NpcLogic.GetInstance().CheckNpcNearby(npcID) == true)
            {
                EventDispatcher.GameWorld.Dispath(ControllerCommand.OPEN_NPC_PANEL_BYID, npcID);
                return;
            }

            KNpcPos npcPos = NpcLogic.GetInstance().GetNpcPosByID(npcID);
            if (npcPos != null)
            {
                NPC_ID = npcID;
                Vector3 rolePosition = MapUtils.GetMetreFromInt(npcPos.nPathX, npcPos.nPathZ, npcPos.nPathY);
                Goto(npcPos.MapID, rolePosition);

                KHeroSetting npcInfo = NpcLogic.GetInstance().GetNpcLocalInfo(npcID);
                if (null != npcInfo)
                    bAutoAttack = (npcInfo.HeroType == Assets.Scripts.Define.KHeroObjectType.hotMonster);
                    //SceneLogic.GetInstance().MainHero.property.CmdAutoAttack = (npcInfo.HeroType == Assets.Scripts.Define.KHeroObjectType.hotMonster);
            }
        }

        public static void CarryToNPCAndOpen(int npcID)
        {
            KNpcPos npcPos = NpcLogic.GetInstance().GetNpcPosByID(npcID);
            if (npcPos != null)
            {
                NPC_ID = npcID;
                Carry(npcPos.MapID, new Vector3(npcPos.nPathX, npcPos.nPathY, npcPos.nPathZ));
            }
        }

        public static void CarryToNPC(int npcID)
        {
            KNpcPos npcPos = NpcLogic.GetInstance().GetNpcPosByID(npcID);
            if (npcPos != null)
            {
                Carry(npcPos.MapID, new Vector3(npcPos.nPathX, npcPos.nPathY, npcPos.nPathZ));
            }
        }

        public static void GotoCollectObj(int collectObjID, bool bCarry = false)
        {
            KCollectMissionInfo info = KConfigFileManager.GetInstance().GetCollectInfo(collectObjID);
            if (info != null)
            {
                if (info.strPosition != null && info.strPosition != "0")
                {
                    string[] pos = info.strPosition.Split(';');

                    if (pos.Length != 0)
                    {
                        string[] posTemp = pos[0].Split(':');
                        if (posTemp.Length == 3)
                        {
                            if (bCarry)
                            {
                                Carry(info.nSceneID, new Vector3(int.Parse(posTemp[0]), int.Parse(posTemp[1]), int.Parse(posTemp[2])));
                            }
                            else
                            {
                                Vector3 vecPosition = MapUtils.GetMetreFromInt(int.Parse(posTemp[0]), int.Parse(posTemp[2]), int.Parse(posTemp[1]));
                                Goto(info.nSceneID, vecPosition);
                            }

                            CollectObjLogic.GetInstance().m_bAutoCollect = true;
                        }
                    }
                }
            }
        }

        public static void Goto(int mapID, Vector3 desPos)
        {
            if ((uint)mapID == SceneLogic.GetInstance().mapId)
            {
                SceneLogic.GetInstance().MainHero.DispatchEvent(ControllerCommand.MOVE_TO_DES, desPos, true);
            }
            else
            {
                SceneLogic.GetInstance().MoveAcrossMap((uint)mapID, desPos);
            }
        }

        public static void Carry(int mapID, Vector3 desPos)
        {
            float dis = KingSoftMath.CheckDistance(SceneLogic.GetInstance().MainHero.Position, desPos/100);
            if (dis<= 8 && SceneLogic.GetInstance().mapId == mapID)
            {
                Debug.Log("已经在目标点的范围内了，不需要传送");
                return;
            }
			AnimationComponent.OperaWalking = false;
            RemoteCallLogic.GetInstance().CallGS("OnChangeMap", mapID, (int)desPos.x, (int)desPos.z, (int)desPos.y);
        }
    }
}
