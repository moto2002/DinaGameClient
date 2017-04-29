using UnityEngine;
using System.Collections;
using Assets.Scripts.Data;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.Model.Player;
using Assets.Scripts.View.UIDetail;
using Assets.Scripts.Logic.Mission;

namespace Assets.Scripts.View.Mission
{
    public class CollectView : CollectUIDetail
    {
        GameObject collectButton;
        GameObject collectProgressBar;
        float m_fCollectTime;
        bool m_bBeCollect;

        int m_collectID;
        int m_objID;

        private bool bInited = false;
        private static CollectView instance;
        public static CollectView GetInstance()
        {
            if (instance == null)
                instance = new CollectView();
            return instance;
        }

        public CollectView()
            : base(0, 0)
        {
        }

        protected override void PreInit()
        {
            base.PreInit();
            collectButton = FindGameObject("CollectButton");
            collectProgressBar = FindGameObject("CollectProgressBar");
            Reset();
        }

        protected override void Init()
        {
            GameObject.DontDestroyOnLoad(viewGo);
            UIEventListener.Get(collectButton).onClick += OnCollectBtnHandler;

            bInited = true;

            if (CollectObjLogic.GetInstance().m_bAutoCollect)
            {
                OnCollectBtnHandler(null);
            }
        }

        public void Open(int collectID, int objID)
        {
            Reset();
            m_collectID = collectID;
            m_objID = objID;
            Show(true);

            if (CollectObjLogic.GetInstance().m_bAutoCollect && bInited)
            {
                OnCollectBtnHandler(null);
            }
        }

        public void Close()
        {
            Reset();
            Hide();
        }

        private void Reset()
        {
            if (bInited)
            {
                m_bBeCollect = false;
                m_fCollectTime = 0;
                collectButton.SetActive(true);
                collectProgressBar.SetActive(false);
            }
        }

        private void OnCollectBtnHandler(GameObject go)
        {
            MajorPlayer player = PlayerManager.GetInstance().MajorPlayer;
            m_fCollectTime = player.onlineTime;
            m_bBeCollect = true;
            collectButton.SetActive(false);
            collectProgressBar.SetActive(true);
            ForegroundSprite.fillAmount = 0;

            CollectObjLogic.GetInstance().SendStartCollectObj(m_collectID, m_objID);
        }

        public override void FixedUpdate()
        {
            if (m_bBeCollect)
            {
                KCollectMissionInfo info = KConfigFileManager.GetInstance().GetCollectInfo(m_collectID);
                if (info != null)
                {
                    MajorPlayer player = PlayerManager.GetInstance().MajorPlayer;
                    float timeUse = player.onlineTime - m_fCollectTime;
                    if (timeUse < info.nNeedTime+0.3f)
                    {
                        ForegroundSprite.fillAmount = timeUse / (info.nNeedTime+0.3f);
                    }
                    else
                    {
                        CollectObjLogic.GetInstance().SendFinishCollectObj(m_collectID, CollectObjLogic.GetInstance().GetMissionIDByCollect(m_collectID));
                        m_bBeCollect = false;
                    }
                }
            }
        }
    }

}