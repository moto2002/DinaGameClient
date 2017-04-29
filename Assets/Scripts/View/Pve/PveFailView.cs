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
using Assets.Scripts.View.Scene;

namespace Assets.Scripts.View.Pve
{
    class PveFailView : PveFailUIDetail
    {
        public PveFailView()
            : base(0, 0)
        {

        }

        protected override void Init()
        {
            base.Init();
            base.Hide();
            SetViewPosition(ViewPosition.Center);
            Panel.gameObject.transform.localPosition = new Vector3(200, -200, 0);
        }

        protected override void InitEvent()
        {
            base.InitEvent();
        }

        private int showTime = 0;

        public void ShowView()
        {
            Show();
            showTime = 200;
        }

        public override void FixedUpdate()
        {
            if (SceneView.GetInstance().setting.Type != (uint)KMapType.mapPVEMap)
            {
                Hide();
                return;
            }

            if (!viewGo.activeSelf)
            {
                return;
            }

            if (showTime > 0)
            {
                showTime--;
                if (showTime == 0)
                {
                    RemoteCallLogic.GetInstance().CallGS("OnExitPveMap");
                    Hide();
                }
            }
        }

        private static PveFailView instance = null;
        public static PveFailView GetInstance()
        {
            if (instance == null)
            {
                instance = new PveFailView();
            }
            return instance;
        }
    }
}
