using UnityEngine;
using System.Collections;
using Assets.Scripts.Logic.RemoteCall;
using System.Collections.Generic;
using Assets.Scripts.Manager;
using Assets.Scripts.View;
using Assets.Scripts.Define;
using Assets.Scripts.Logic.Scene;

namespace Assets.Scripts.Logic.Mission
{
    public class CollectObjLogic : BaseLogic
    {
        private Dictionary<int, int> needCollectIDs = new Dictionary<int, int>();

        private int m_collectID;

        public bool m_bAutoCollect = false;

        private static CollectObjLogic instance;
        public static CollectObjLogic GetInstance()
        {
            if (instance == null)
                instance = new CollectObjLogic();
            return instance;
        }

        protected override void Init()
        {

        }

        protected override void InitListeners()
        {
            EventDispatcher.GameWorld.Regist(ControllerCommand.HERO_MOVE, OnInterruptCollect);
            EventDispatcher.GameWorld.Regist(ControllerCommand.HERO_PLAYSKILL, OnInterruptCollect);
            EventDispatcher.GameWorld.Regist(ControllerCommand.HERO_DEAD, OnInterruptCollect);
        }

        public void SendStartCollectObj(int collectID, int objID)
        {
            SceneLogic.GetInstance().MainHero.property.CmdAutoAttack = false;
            RemoteCallLogic.GetInstance().CallGS("OnStartCollectRequest", collectID, objID);
        }

        public void SendFinishCollectObj(int collectID, int missionID)
        {
            RemoteCallLogic.GetInstance().CallGS("OnFinishCollectRequest", collectID, missionID);
        }

        public void SendInterruptCollectObj(int collectID)
        {
            RemoteCallLogic.GetInstance().CallGS("OnInterruptCollectRequest", collectID);
        }

        public void OnStartCollectRespond(int collectID, int resultCode)
        {
            if (resultCode == (int)KCollectResult.crSuccess)
            {
                m_collectID = collectID;
            }
            else
            {
                ViewManager.GetInstance().CloseCollectPanel();
            }
        }

        public void OnFinishCollectRespond(int collectID, int resultCode)
        {
            m_collectID = 0;
            ViewManager.GetInstance().CloseCollectPanel();
        }

        public void OnInterruptCollectRespond(int collectID, int resultCode)
        {
            m_collectID = 0;
            ViewManager.GetInstance().CloseCollectPanel();
        }

        public object OnInterruptCollect(params object[] objs)
        {
            if (m_collectID != 0)
            {
                m_bAutoCollect = false;
                SendInterruptCollectObj(m_collectID);
            }
            return null;
        }

        public void ClearNeedCollectObjList()
        {
            needCollectIDs.Clear();
        }

        public void AddNeedCollectObjID(int nID, int missionID)
        {
            needCollectIDs.Add(nID, missionID);
        }

        public bool CheckNeedCollectID(int collectID)
        {
            return needCollectIDs.ContainsKey(collectID);
        }

        public int GetMissionIDByCollect(int collectID)
        {
            if (needCollectIDs.ContainsKey(collectID))
            {
                return needCollectIDs[collectID];
            }

            return 0;
        }
    }
}
