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
using Assets.Scripts.View.Scene;

namespace Assets.Scripts.View.Pve
{
    class PveAutoFight : HangUpUIDetail
    {
        public PveAutoFight()
            : base(0, 0)
        {

        }

        protected override void Init()
        {
            base.Init();
            base.Hide();
            SetViewPosition(ViewPosition.Center);
            EventDispatcher.GameWorld.Regist(ControllerCommand.CHANGE_MAP, OnChangeMap);
            Panel.gameObject.transform.localPosition = new Vector3(200, 300, 0);
            UIEventListener.Get(Exit.gameObject).onClick += OnExit;
            UIEventListener.Get(FIght.gameObject).onClick += OnAutoFight;
        }

        private void OnExit(GameObject go)
        {
            Hide();
        }

        private void OnAutoFight(GameObject go)
        {
            SceneLogic.GetInstance().MainHero.property.CmdAutoAttack = true;
            Hide();
        }

        protected override void InitEvent()
        {
            base.InitEvent();
        }

        public object OnChangeMap(params object[] objs)
        {
            if (SceneView.GetInstance().setting.Type != (uint)KMapType.mapPVEMap)
            {
                Hide();
            }

            return null;
        }

        public override void FixedUpdate()
        {

        }

        private static PveAutoFight instance = null;
        public static PveAutoFight GetInstance()
        {
            if (instance == null)
            {
                instance = new PveAutoFight();
            }
            return instance;
        }
    }
}
