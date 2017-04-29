using System;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.View.UIDetail;
using Assets.Scripts.Lib.View;
using Assets.Scripts.Manager;
using Assets.Scripts.Logic.Mission;
using Assets.Scripts.Logic.Npc;
using Assets.Scripts.Data;
using Assets.Scripts.Utils;
using Assets.Scripts.View.Scene;
using Assets.Scripts.Define;

namespace Assets.Scripts.View.Mission
{
    public class MissionFollowView : TaskFollow2UIDetail
    {
        private MissionInfo mainInfo;

        private static MissionFollowView instance;
        public static MissionFollowView GetInstance()
        {
            if (instance == null)
                instance = new MissionFollowView();
            return instance;
        }

        public MissionFollowView()
            : base(0, 0)
        {

        }

        protected override void PreInit()
        {
            base.PreInit();
            NGUITools.AddWidgetCollider(FeixieButton.gameObject);
            ReplacementLayer();
        }

        protected override void Init()
        {
            base.Init();

            EventDispatcher.GameWorld.Regist(ControllerCommand.CHANGE_MAP, OnChangeMap);
            SetViewPosition(ViewPosition.Right);
            UIEventListener.Get(MissionCon.gameObject).onURL += OnLinkConditionHandler;
            UIEventListener.Get(FeixieButton.gameObject).onClick += OnClickFeixieHandler;
            UIEventListener.Get(FeixieButton.gameObject).onHover += OnFeixieHover;
            UIEventListener.Get(ContinueButton.gameObject).onClick += OnContinueBtnHandler;
            EventDispatcher.GameWorld.Regist(ControllerCommand.UPDATE_MISSION, UpdateMission);
            EventDispatcher.GameWorld.Regist(ControllerCommand.CONTINUE_MISSION, ContinueMission);
            UpdateMission();
        }

        public object OnChangeMap(params object[] objs)
        {
            if (viewGo == null)
                return null;

            if (SceneView.GetInstance().setting.Type == (uint)KMapType.mapPVEMap)
            {
                Hide();
            }
            else
            {
               base.viewGo.SetActive(true);
            }

            return null;
        }

        public override void DestroyObject()
        {
            base.DestroyObject();
            EventDispatcher.GameWorld.Remove(ControllerCommand.CHANGE_MAP, OnChangeMap);
            EventDispatcher.GameWorld.Remove(ControllerCommand.UPDATE_MISSION, UpdateMission);
            EventDispatcher.GameWorld.Remove(ControllerCommand.CONTINUE_MISSION, ContinueMission);
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

        private object ContinueMission(params object[] objs)
        {
            OnContinueBtnHandler(null);
            return null;
        }

        private object UpdateMission(params object[] objs)
        {
            Dictionary<int, MissionInfo> curMission = MissionLogic.GetInstance().GetCurrentMissionList();
            Dictionary<int, MissionInfo> canGetMission = MissionLogic.GetInstance().GetCanAcceptList();

            CollectObjLogic.GetInstance().ClearNeedCollectObjList();

            foreach (MissionInfo info in curMission.Values)
            {
                if (info.type == (int)MissionInfo.MissionType.MainMission)
                {
                    MissionName.text = info.questName;
                    MissionDes.text = info.desc;
                    MissionCon.transform.localPosition = MissionDes.transform.localPosition + new Vector3(0, -MissionDes.printedSize.y - 10, 0);
                    FeixieButton.transform.localPosition = new Vector3(FeixieButton.transform.localPosition.x, MissionCon.transform.localPosition.y-6, 0);
                    if (info.curStatus == MissionInfo.MisssionStatus.BeenAccepted)
                    {
                        MissionCon.text = info.condition + " <FF0000>(未完成)<->";
                    }
                    else if (info.curStatus == MissionInfo.MisssionStatus.Finish)
                    {
                        KHeroSetting npcInfo = NpcLogic.GetInstance().GetNpcLocalInfo(info.submitNpcID);
                        // 要加个" " 不然颜色变化会错乱
                        MissionCon.text = " " + "<a:" + info.submitNpcID + ">" + "去找" + npcInfo.Name + "</a>" + " <ffa200>(已完成)<->";
                    }
                    mainInfo = info;
                }
            }

            foreach (MissionInfo info in canGetMission.Values)
            {
                if (info.type == (int)MissionInfo.MissionType.MainMission)
                {
                    MissionName.text = info.questName;
                    MissionDes.text = info.desc;
                    MissionCon.transform.localPosition = MissionDes.transform.localPosition + new Vector3(0, -MissionDes.printedSize.y - 10, 0);
                    FeixieButton.transform.localPosition = new Vector3(FeixieButton.transform.localPosition.x, MissionCon.transform.localPosition.y-6, 0);
                    KHeroSetting npcInfo = NpcLogic.GetInstance().GetNpcLocalInfo(info.npcID);
                    MissionCon.text = "<a:" + info.npcID + ">" + "去找" + npcInfo.Name + "</a>";
                    mainInfo = info;
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

            return null;
        }

        private void OnLinkConditionHandler(GameObject go, string targetIDStr)
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

        private void OnClickFeixieHandler(GameObject go)
        {
            int targetID;
            if (!TryGetIntIDFormString(MissionCon.text, out targetID))
            {
                return;
            }

            CarryToNPC(targetID, mainInfo);
        }

        private void OnFeixieHover(GameObject go, bool state)
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

        private void OnContinueBtnHandler(GameObject go)
        {
            int targetID;
            if (!TryGetIntIDFormString(MissionCon.text, out targetID))
            {
                return;
            }

            FindNPC(targetID, mainInfo);
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
    }
}
