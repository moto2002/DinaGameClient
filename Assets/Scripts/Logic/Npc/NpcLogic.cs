using System;
using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.Data;
using Assets.Scripts.Logic.Mission;
using Assets.Scripts.Manager;
using Assets.Scripts.Utils;
using Assets.Scripts.Logic.Scene;
using Assets.Scripts.Logic.Scene.SceneObject;

namespace Assets.Scripts.Logic.Npc
{
    public class NpcLogic : BaseLogic
    {
        private Dictionary<int, KNpcPos> npcPosList = new Dictionary<int, KNpcPos>();

        private static NpcLogic instance;
        public static NpcLogic GetInstance()
        {
            if (instance == null)
                instance = new NpcLogic();
            return instance;
        }

        protected override void Init()
        {

        }

        protected override void InitListeners()
        {

        }

        public KHeroSetting GetNpcLocalInfo(int npcID)
        {
            KHeroSetting npcInfo = KConfigFileManager.GetInstance().heroSetting.getData(npcID.ToString());
            if (npcInfo == null)
            {
                log.Error("npcID为: " + npcID + "  的相关配置有错。");
                return null;
            }
            return npcInfo;
        }

        public NpcPanelInfo GetNPCPanelInfo(int npcID)
        {
			string typeName = "";
			
            KHeroSetting npcObj = KConfigFileManager.GetInstance().heroSetting.getData(npcID.ToString());
            if (npcObj == null)
            {
                log.Error("npcID为: " + npcID + "  的相关配置有错。");
                return null;
            }
            NpcPanelInfo npcPanelVO = new NpcPanelInfo();
            npcPanelVO.npcID = npcID;
            npcPanelVO.npcName = npcObj.Name;
            npcPanelVO.content = npcObj.Caption;
            npcPanelVO.actionLinks = new List<NpcLinkInfo>();
            npcPanelVO.missionLinks = new List<NpcLinkInfo>();

            Dictionary<int, MissionInfo> npcMissions = MissionLogic.GetInstance().GetNpcMissionList(npcID);
            if (npcMissions != null)
            {
                int eventID = 0;
                foreach (MissionInfo vo in npcMissions.Values)
                {
                    NpcLinkInfo linkVO = new NpcLinkInfo();
                    linkVO.npcID = npcID;
					
					if (vo.type == (int)MissionInfo.MissionType.MainMission)
					{
						typeName = "<FF0000>[主线]<->";
					}
					else if (vo.type == (int)MissionInfo.MissionType.SubLineMission)
					{
						typeName = "<FF0000>[支线]<->";
					}
					else if (vo.type == (int)MissionInfo.MissionType.DaliyMission)
					{
						typeName = "<FF0000>[日常]<->";	
					}
					
                    linkVO.linkName = typeName + HtmlUtil.Link(vo.tips, ControllerCommand.NPC_CLICK_MISSION_LINK + eventID);
                    linkVO.dispatchMessage = ControllerCommand.NPC_CLICK_MISSION_LINK;
                    linkVO.data = vo;
                    npcPanelVO.missionLinks.Add(linkVO);
                    eventID++;
                }
            }


            return npcPanelVO;
        }

        public void NpcPosLoadComplete()
        {
            Dictionary<string, KNpcPos> npcPosDic = KConfigFileManager.GetInstance().npcPoss.getAllData();
            foreach (KNpcPos pos in npcPosDic.Values)
            {
                //一群同NPCID的怪物只记录第一只的位置
                if (!npcPosList.ContainsKey(pos.NpcID))
                    npcPosList.Add(pos.NpcID, pos);
            }
        }

        public KNpcPos GetNpcPosByID(int npcID)
        {
            if (npcPosList.ContainsKey(npcID))
                return npcPosList[npcID];
            return null;
        }

        public bool CheckNpcNearby(int npcID)
        {
            KNpcPos pos = GetNpcPosByID(npcID);
            if (pos != null)
            {
                if (pos.MapID != SceneLogic.GetInstance().mapId)
                    return false;

                float dis = Vector3.Distance(new Vector3(pos.nX / 100, pos.nZ / 100, pos.nY / 100), SceneLogic.GetInstance().MainHero.Position);
                if (dis <= 2)
                    return true;
            }
            return false;
        }

        public bool CheckNpcNearby(int npcID, int allowDis)
        {
            KNpcPos pos = GetNpcPosByID(npcID);
            if (pos != null)
            {
                if (pos.MapID != SceneLogic.GetInstance().mapId)
                    return false;

                float dis = Vector3.Distance(new Vector3(pos.nX / 100, pos.nZ / 100, pos.nY / 100), SceneLogic.GetInstance().MainHero.Position);
                if (dis <= allowDis)
                    return true;
            }
            return false;
        }
    }
}
