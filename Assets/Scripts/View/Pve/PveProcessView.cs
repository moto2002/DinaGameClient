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
    class PveProcessView : PveProcessUIDetail
    {
        public PveProcessView()
            : base(0, 0)
        {

        }

        protected override void Init()
        {
            SetViewPosition(ViewPosition.TopRight);
            EventDispatcher.GameWorld.Regist(ControllerCommand.CHANGE_MAP, OnChangeMap);
            Panel.gameObject.transform.localPosition = new Vector3(-532, -442, 0);
            UIEventListener.Get(AutoFight.gameObject).onClick += OnAutoFight;
            UIEventListener.Get(ExitPve.gameObject).onClick += OnExitPve;

            base.Init();
            base.Hide();
        }


        protected override void InitEvent()
        {
            base.InitEvent();
        }

        public void ShowProcess()
        {
            base.viewGo.SetActive(true);
            MissionFollowView.GetInstance().Hide();
        }

        public void HideProcess()
        {
            Hide();
            MissionFollowView.GetInstance().Show();
        }
        //////////////////////////////////////////////////////////////////////////

        private string showText = "";
        private int completeTime = 0;
        private int exsistTime = 0;
        private int enterTime = 0;
        private int pveID = 0;

        private Dictionary<int, int> mapValue = new Dictionary<int, int>();

        public void OnValueModify(int nKey, int nValue)
        {
            mapValue[nKey] = nValue;
            UpdateValue();
        }

        private void UpdateValue()
        {
            MapText.text = showText;

            for (int i = 0; i < 9; i++)
            {
                int nValue = mapValue[i];
                MapText.text = MapText.text.Replace("V" + i, nValue.ToString());
            }
        }
        private void UpdateTime()
        {
            KPve pveInfo = KConfigFileManager.GetInstance().pveList.getData(pveID.ToString());

            if (pveInfo == null)
            {
                return;
            }
            int nCompleteTime = completeTime + (int)PlayerManager.GetInstance().MajorPlayer.onlineTime - enterTime;
            ProcessTime.text = GiftItem.FormatTime(nCompleteTime);
            ExitTime.text = "副本退出时间：<40b9ff>" + GiftItem.FormatTime(exsistTime - nCompleteTime) + "<->";

            Process1.gameObject.SetActive(false);
            Process2.gameObject.SetActive(false);
            Process3.gameObject.SetActive(false);


            if (nCompleteTime > pveInfo.tNormalTime)
            {
                Process3.gameObject.SetActive(true);
            }
            else if (nCompleteTime > pveInfo.tGoodTime)
            {
                Process2.gameObject.SetActive(true);
            }
            else
            {
                Process1.gameObject.SetActive(true);
            }
        }

        public object OnChangeMap(params object[] objs)
        {
            if (SceneView.GetInstance().setting.Type != (uint)KMapType.mapPVEMap)
            {
                Hide();
            }
            else
            {
                base.viewGo.SetActive(true);
            }

            return null;
        }

        public override void FixedUpdate()
        {
            if (SceneView.GetInstance().setting.Type != (uint)KMapType.mapPVEMap || !viewGo.activeSelf)
            {
                return;
            }

            UpdateTime();
        }

        public void InitPveMap(int nPveID, int nCompleteTime, RemoteTable pveValue)
        {   
            KPve pveInfo = KConfigFileManager.GetInstance().pveList.getData(nPveID.ToString());
            showText = pveInfo.Text;

            enterTime = (int)PlayerManager.GetInstance().MajorPlayer.onlineTime;
            MapName.text = pveInfo.Name;
            ProcessTime.text = GiftItem.FormatTime(nCompleteTime);
            completeTime = nCompleteTime;
            exsistTime = pveInfo.tExistTime;
            pveID = nPveID;

            UpdateTime();

            for (int i = 0; i < 9; i++)
            {
                int nValue = pveValue[i];
                mapValue[i] = nValue;
            }

            UpdateValue();

            ShowProcess();
        }

        private static PveProcessView instance = null;
        public static PveProcessView GetInstance()
        {
            if (instance == null)
            {
                instance = new PveProcessView();
            }
            return instance;
        }

        private void OnAutoFight(GameObject go)
        {
            SceneLogic.GetInstance().MainHero.property.CmdAutoAttack = !(SceneLogic.GetInstance().MainHero.property.CmdAutoAttack);
        }

        private void OnExitPve(GameObject go)
        {
            PveQuitView.GetInstance().Show();
        }
    }
}
