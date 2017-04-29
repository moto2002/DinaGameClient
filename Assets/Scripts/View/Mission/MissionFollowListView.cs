using System;
using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Lib.View;
using Assets.Scripts.Logic.Mission;
using Assets.Scripts.Logic.Npc;
using Assets.Scripts.Utils;
using Assets.Scripts.Data;
using Assets.Scripts.Manager;
using Assets.Scripts.View.UIDetail;
using Assets.Scripts.View.Scene;
using Assets.Scripts.Define;

namespace Assets.Scripts.View.Mission
{
    public class MissionFollowListView : TaskFollowUIDetail
    {
        private MissionInfo mainInfo;
        private MissionInfo daliyInfo;
        private MissionInfo attackInfo;

        private GameObject missionFollow = null;
        private GameObject attackItem = null;
        private GameObject mainItem = null;
        private GameObject daliyItem = null;

        private static MissionFollowListView instance;
        public static MissionFollowListView GetInstance()
        {
            if (instance == null)
                instance = new MissionFollowListView();
            return instance;
        }

        public MissionFollowListView()
            : base(0, 0)
        {

        }

        protected override void PreInit()
        {
            base.PreInit();
            missionFollow = FindGameObject("MissionFollow");
            attackItem = FindGameObject("AttackItem");
            daliyItem = FindGameObject("DaliyItem");
            mainItem = FindGameObject("MainItem");
            ReplacementLayer();
        }

        protected override void Init()
        {
            base.Init();

            EventDispatcher.GameWorld.Regist(ControllerCommand.CHANGE_MAP, OnChangeMap);
            SetViewPosition(ViewPosition.Right);
            UIEventListener.Get(MainFeiButton.gameObject).onClick += OnMainMisCarryHandler;
            UIEventListener.Get(MainFeiButton.gameObject).onHover = OnCarryHover;
            UIEventListener.Get(DaliyFeiButton.gameObject).onClick += OnDaliyMisCarryHandler;
            UIEventListener.Get(DaliyFeiButton.gameObject).onHover = OnCarryHover;
            UIEventListener.Get(AttackFeiButton.gameObject).onClick += OnAttackMisCarryHandler;
            UIEventListener.Get(AttackFeiButton.gameObject).onHover = OnCarryHover;

            UIEventListener.Get(OneKeyButton.gameObject).onClick += OnOneKeyHandler;
            UIEventListener.Get(OneKeyButton.gameObject).onHover += OnOneKeyHover;
            //UIEventListener.Get(ImmediateButton.gameObject).onClick += OnImmediateHandler;
            //UIEventListener.Get(ImmediateButton.gameObject).onHover += OnImmediateHover;

            UIEventListener.Get(MainCondition.gameObject).onURL += OnMainMisLinkHandler;
            UIEventListener.Get(DaliyCondition.gameObject).onURL += OnDaliyMisLinkHandler;
            UIEventListener.Get(AttackCondition.gameObject).onURL += OnAttackMisLinkHandler;

            UIEventListener.Get(MiniButton.gameObject).onClick += OnClickMiniBtnHandler;
            UIEventListener.Get(MaxButton.gameObject).onClick += OnClickMaxBtnHandler;
            MaxButton.gameObject.SetActive(false);
            EventDispatcher.GameWorld.Regist(ControllerCommand.CONTINUE_MISSION, ContinueMission);
            EventDispatcher.GameWorld.Regist(ControllerCommand.UPDATE_MISSION, UpdateMission);
            UpdateMission();
        }

        public object OnChangeMap(params object[] objs)
        {
            if (viewGo == null)
                return null;

            if (SceneView.GetInstance().setting.Type == (uint)KMapType.mapPVEMap)
            {
                Hide();
                MissionTransferTipsView.GetInstance().Hide();
                ImmediateTipsView.GetInstance().Hide();
            }
            else
            {
                base.viewGo.SetActive(true);
            }

            return null;
        }

        private object ContinueMission(params object[] objs)
        {
            int targetID;
            if (!TryGetIntIDFormString(MainCondition.text, out targetID))
            {
                return null;
            }

            FindNPC(targetID, mainInfo);
            return null;
        }

        private bool TryGetIntIDFormString(string strIn, out int nOut)
        {
            nOut = 0;
            int start = strIn.IndexOf("<a:");
            if (start == -1)
                return false;

            int end = strIn.IndexOf(">", start);
            if (end == -1)
                return false;

            strIn = strIn.Substring(start + 3, end - start - 3);
            nOut = int.Parse(strIn);
            return true;
        }

        public Transform GetEffectPos()
        {
            GameObject obj = FindGameObject("EffectPos");

            if (obj != null)
            {
                return obj.transform;
            }

            return null;
        }

        private void OnMainMisCarryHandler(GameObject go)
        {
            int targetID;
            if (!TryGetIntIDFormString(MainCondition.text, out targetID))
            {
                return;
            }

            CarryToNPC(targetID, mainInfo);
        }

        private void OnCarryHover(GameObject go, bool state)
        {
            if (state)
            {
                MissionTransferTipsView.GetInstance().ShowTips();
            }
            else
            {
                MissionTransferTipsView.GetInstance().Hide();
            }
        }

        private void OnDaliyMisCarryHandler(GameObject go)
        {
            int targetID;
            if (!TryGetIntIDFormString(DaliyCondition.text, out targetID))
            {
                return;
            }

            CarryToNPC(targetID, daliyInfo);
        }

        private void OnAttackMisCarryHandler(GameObject go)
        {
            int targetID;
            if (!TryGetIntIDFormString(AttackCondition.text, out targetID))
            {
                return;
            }

            CarryToNPC(targetID, attackInfo);
        }

        private void OnOneKeyHandler(GameObject go)
        {

        }

        private void OnOneKeyHover(GameObject go, bool state)
        {

        }

        private void OnImmediateHandler(GameObject go)
        {
            /*int missionID;
            if (!TryGetIntIDFormString(ImmediateButtonLabel.text, out missionID))
            {
                return;
            }

            MissionLogic.GetInstance().SendQuickFinishQuestMsg(missionID);

            ViewManager.GetInstance().CloseNpcPanel();
            ImmediateTipsView.GetInstance().Hide();*/
        }

        private void OnImmediateHover(GameObject go, bool state)
        {
            /*if (state)
            {
                ImmediateTipsView.GetInstance().ShowTips(mainInfo);
            }
            else
            {
                ImmediateTipsView.GetInstance().Hide();
            }*/
        }
		
		private void OnMainMisLinkHandler(GameObject go, string targetIDStr)
        {
            int targetID = int.Parse(targetIDStr);

            if (mainInfo.bScript && mainInfo.curStatus == MissionInfo.MisssionStatus.BeenAccepted)
			{
				PathUtil.CarryToNPC(targetID);
			}
			else
			{
            	FindNPC(targetID, mainInfo);
			}
        }

        private void OnDaliyMisLinkHandler(GameObject go, string targetIDStr)
        {
            int targetID = int.Parse(targetIDStr);

            FindNPC(targetID, daliyInfo);
        }

        private void OnAttackMisLinkHandler(GameObject go, string targetIDStr)
        {
            int targetID = int.Parse(targetIDStr);

            FindNPC(targetID, attackInfo);
        }

        private void OnClickMiniBtnHandler(GameObject go)
        {
            MaxButton.gameObject.SetActive(true);
            MiniButton.gameObject.SetActive(false);
            missionFollow.gameObject.SetActive(false);
        }

        private void OnClickMaxBtnHandler(GameObject go)
        {
            MaxButton.gameObject.SetActive(false);
            MiniButton.gameObject.SetActive(true);
            missionFollow.gameObject.SetActive(true);
        }

        private void FindNPC(int targetID, MissionInfo info)
        {
            if (info.curStatus == MissionInfo.MisssionStatus.Accept || info.curStatus == MissionInfo.MisssionStatus.Finish)
            {
                PathUtil.FindNpcAndOpen(targetID);
            }
            else if (mainInfo.curStatus == MissionInfo.MisssionStatus.BeenAccepted)
            {
                if (mainInfo.subType == (int)MissionInfo.MissionSubType.Collect)
                {
                    PathUtil.GotoCollectObj(targetID);
                }
                else
                {
                    PathUtil.FindNpc(targetID);
                }
            }
        }

        private void CarryToNPC(int targetID, MissionInfo info)
        {
            if (info.curStatus == MissionInfo.MisssionStatus.Accept || info.curStatus == MissionInfo.MisssionStatus.Finish)
            {
                PathUtil.CarryToNPCAndOpen(targetID);
            }
            else if (mainInfo.curStatus == MissionInfo.MisssionStatus.BeenAccepted)
            {
                if (mainInfo.subType == (int)MissionInfo.MissionSubType.Collect)
                {
                    PathUtil.GotoCollectObj(targetID, true);
                }
                else
                {
                    PathUtil.CarryToNPC(targetID);
                }
            }

            MissionTransferTipsView.GetInstance().Hide();
        }

        private object UpdateMission(params object[] objs)
        {
            Dictionary<int,MissionInfo> curMission =  MissionLogic.GetInstance().GetCurrentMissionList();
            Dictionary<int, MissionInfo> canGetMission = MissionLogic.GetInstance().GetCanAcceptList();

            attackItem.SetActive(false);
            daliyItem.SetActive(false);
            mainItem.SetActive(false);
            mainInfo = null;
            daliyInfo = null;
            attackInfo = null;
            CollectObjLogic.GetInstance().ClearNeedCollectObjList();

            foreach (MissionInfo info in curMission.Values)
            {
                if (info.type == (int)MissionInfo.MissionType.MainMission)
                {
                    MainMisName.text = info.questName;
                    if(info.curStatus == MissionInfo.MisssionStatus.BeenAccepted)
                    {
                        MainCondition.text = info.condition + " <FF0000>(未完成)<->";
                    }
                    else if (info.curStatus == MissionInfo.MisssionStatus.Finish)
                    {
                        KHeroSetting npcInfo = NpcLogic.GetInstance().GetNpcLocalInfo(info.submitNpcID);
                        MainCondition.text = " " + "<a:" + info.submitNpcID + ">" + "去找" + npcInfo.Name + "</a>" + " <ffa200>(已完成)<->";
                    }

                    mainInfo = info;
                    mainItem.SetActive(true);
                }
                else if (info.type == (int)MissionInfo.MissionType.DaliyMission)
                {
                    DaliyMisName.text = info.questName;
                    if (info.curStatus == MissionInfo.MisssionStatus.BeenAccepted)
                    {
                        DaliyCondition.text = info.condition + " <FF0000>(未完成)<->";
                    }
                    else if (info.curStatus == MissionInfo.MisssionStatus.Finish)
                    {
                        KHeroSetting npcInfo = NpcLogic.GetInstance().GetNpcLocalInfo(info.submitNpcID);
                        DaliyCondition.text = " " + "<a:" + info.submitNpcID + ">" + "去找" + npcInfo.Name + "</a>" + " <ffa200>(已完成)<->";
                    }

                    daliyInfo = info;
                    attackItem.SetActive(true);
                }
                else if (info.type == (int)MissionInfo.MissionType.SubLineMission)
                {
                    AttackMisName.text = info.questName;
                    if (info.curStatus == MissionInfo.MisssionStatus.BeenAccepted)
                    {
                        AttackCondition.text = info.condition + " <FF0000>(未完成)<->";
                    }
                    else if (info.curStatus == MissionInfo.MisssionStatus.Finish)
                    {
                        KHeroSetting npcInfo = NpcLogic.GetInstance().GetNpcLocalInfo(info.submitNpcID);
                        AttackCondition.text = " " + "<a:" + info.submitNpcID + ">" + "去找" + npcInfo.Name + "</a>" + " <ffa200>(已完成)<->";
                    }

                    attackInfo = info;
                    daliyItem.SetActive(true);
                }
            }

            foreach (MissionInfo info in canGetMission.Values)
            {
                if (info.type == (int)MissionInfo.MissionType.MainMission)
                {
                    MainMisName.text = info.questName;
                    KHeroSetting npcInfo = NpcLogic.GetInstance().GetNpcLocalInfo(info.npcID);
                    MainCondition.text = " " + "<a:" + info.npcID + ">" + "去找" + npcInfo.Name + "</a>";
                    mainInfo = info;
                    mainItem.SetActive(true);
                    //ImmediateButtonLabel.text = "<a:" + info.id + ">" + ImmediateButtonLabel.text;
                }
                else if (info.type == (int)MissionInfo.MissionType.DaliyMission)
                {
                    DaliyMisName.text = info.questName;
                    KHeroSetting npcInfo = NpcLogic.GetInstance().GetNpcLocalInfo(info.npcID);
                    DaliyCondition.text = " " + "<a:" + info.npcID + ">" + "去找" + npcInfo.Name + "</a>";
                    daliyInfo = info;
                    attackItem.SetActive(true);
                }
                else if (info.type == (int)MissionInfo.MissionType.SubLineMission)
                {
                    AttackMisName.text = info.questName;
                    KHeroSetting npcInfo = NpcLogic.GetInstance().GetNpcLocalInfo(info.npcID);
                    AttackCondition.text = " " + "<a:" + info.npcID + ">" + "去找" + npcInfo.Name + "</a>";
                    attackInfo = info;
                    daliyItem.SetActive(true);
                }
            }

            if (mainInfo != null)
            {
                if (mainInfo.curStatus == MissionInfo.MisssionStatus.BeenAccepted && mainInfo.subType == (int)MissionInfo.MissionSubType.Collect)
                {
                    int targetID;
                    if (TryGetIntIDFormString(mainInfo.condition, out targetID))
                    {
                        CollectObjLogic.GetInstance().AddNeedCollectObjID(targetID, mainInfo.id);
                    }
                }
            }

            if (daliyInfo != null)
            {
                if (daliyInfo.curStatus == MissionInfo.MisssionStatus.BeenAccepted && daliyInfo.subType == (int)MissionInfo.MissionSubType.Collect)
                {
                    int targetID;
                    if (TryGetIntIDFormString(daliyInfo.condition, out targetID))
                    {
                        CollectObjLogic.GetInstance().AddNeedCollectObjID(targetID, daliyInfo.id);
                    }
                }
            }

            if (attackInfo != null)
            {
                if (attackInfo.curStatus == MissionInfo.MisssionStatus.BeenAccepted && attackInfo.subType == (int)MissionInfo.MissionSubType.Collect)
                {
                    int targetID;
                    if (TryGetIntIDFormString(attackInfo.condition, out targetID))
                    {
                        CollectObjLogic.GetInstance().AddNeedCollectObjID(targetID, attackInfo.id);
                    }
                }
            }

            return null;
        }
    }
}

