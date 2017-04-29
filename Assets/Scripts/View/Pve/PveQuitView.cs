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
    class PveQuitView : PointOutUIDetail
    {
        public PveQuitView()
            : base(0, 0)
        {

        }

        protected override void Init()
        {
            base.Init();
            base.Hide();
            EventDispatcher.GameWorld.Regist(ControllerCommand.CHANGE_MAP, OnChangeMap);
            SetViewPosition(ViewPosition.Center);
            Panel.gameObject.transform.localPosition = new Vector3(200, 50, 0);
            UIEventListener.Get(ExitMap.gameObject).onClick += OnExitPve;
            UIEventListener.Get(Cancle.gameObject).onClick += OnCalcle;

        }

        private void OnCalcle(GameObject go)
        {
            Hide();
        }

        private void OnExitPve(GameObject go)
        {
            RemoteCallLogic.GetInstance().CallGS("OnExitPveMap");
            PveProcessView.GetInstance().HideProcess();
            PveAutoFight.GetInstance().Hide();
            Hide();
        }

        public object OnChangeMap(params object[] objs)
        {
            if (SceneView.GetInstance().setting.Type != (uint)KMapType.mapPVEMap)
            {
                Hide();
            }

            return null;
        }

        protected override void InitEvent()
        {
            base.InitEvent();
        }

        private static PveQuitView instance = null;
        public static PveQuitView GetInstance()
        {
            if (instance == null)
            {
                instance = new PveQuitView();
            }
            return instance;
        }
    }
}
