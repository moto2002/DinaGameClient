using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Lib.View;
using Assets.Scripts.Define;
using Assets.Scripts.View.UIDetail;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.Lib.Resource;
using Assets.Scripts.Data;
using Assets.Scripts.Logic;
using UnityEngine;
using Assets.Scripts.Manager;
using Assets.Scripts.Logic.RemoteCall;
using Assets.Scripts.Logic.Item;
using Assets.Scripts.View.Mission;
using Assets.Scripts.Model.Player;
using Assets.Scripts.Logic.Scene;


namespace Assets.Scripts.View.Other
{
    class ReliveView : YouDeadUIDetail
    {
        public ReliveView()
            : base(0, 0)
        {

        }

        private int showTime = 0;
        protected override void Init()
        {
            base.Init();
            base.Hide();
            SetViewPosition(ViewPosition.Center);
            Panel.gameObject.transform.localPosition = new Vector3(-400, 150, 0);
            UIEventListener.Get(ReliveHero.gameObject).onClick += OnReliveHere;
            UIEventListener.Get(Relive.gameObject).onClick += OnRelive;
            UIEventListener.Get(Exit.gameObject).onClick += OnExit;
          
        }

        public void ShowView()
        {
            base.viewGo.SetActive(true);
            showTime = (int)PlayerManager.GetInstance().MajorPlayer.onlineTime;
        }

        public override void FixedUpdate()
        {
            if (!viewGo.activeSelf)
            {
                return;
            }

            int reliveTime = 20 - (int)PlayerManager.GetInstance().MajorPlayer.onlineTime + showTime;

            if (reliveTime >= 0)
            {
                ReliveTime.text = reliveTime + "秒";
            }

            if (reliveTime == 0)
            {
                SceneLogic.GetInstance().MainHero.Net.SendReliveRequest(false);
                Hide();
            }
        }

        private void OnExit(GameObject go)
        {
            Hide();
        }

        private void OnReliveHere(GameObject go)
        {
            SceneLogic.GetInstance().MainHero.Net.SendReliveRequest(true);
            Hide();
        }

        private void OnRelive(GameObject go)
        {
            SceneLogic.GetInstance().MainHero.Net.SendReliveRequest(false);
            Hide();
        }

        protected override void InitEvent()
        {
            base.InitEvent();
        }

        private static ReliveView instance = null;
        public static ReliveView GetInstance()
        {
            if (instance == null)
            {
                instance = new ReliveView();
            }
            return instance;
        }
    }
}
