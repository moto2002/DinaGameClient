using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Lib.View;
using Assets.Scripts.Manager;
using Assets.Scripts.Model.Scene;
using Assets.Scripts.Define;
using Assets.Scripts.Utils;
using Assets.Scripts.Model.Player;
using Assets.Scripts.View.UIDetail;

namespace Assets.Scripts.View.MainUI
{
    public class HeadView : HeadUIDetail
    {
        public HeadView()
            : base(350, 110)
        {

        }

        private static HeadView instance;
        public static HeadView GetInstance()
        {
            if (instance == null)
                instance = new HeadView();
            return instance;
        }

        protected override void PreInit()
        {
            base.PreInit();

            KGender gender = PlayerManager.GetInstance().MajorPlayer.Gender;
            if (gender == KGender.gFemale)
            {
                headSp.spriteName = "女主";
            }
            else
            {
                headSp.spriteName = "男主";
            }
        }

        protected override void Init()
        {
            base.Init();
            SetViewPosition(ViewPosition.TopLeft);

            UIEventListener.Get(VipBtn.gameObject).onClick += OnClickVipBtn;
        }

        protected override void InitEvent()
        {
            base.InitEvent();
            EventDispatcher.GameWorld.Regist(ControllerCommand.CHANGE_NICKNAME, OnChangeNickname);
            EventDispatcher.GameWorld.Regist(ControllerCommand.CHANGE_HEAD, OnChangeHead);
        }

        private void OnClickVipBtn(GameObject go)
        {

        }

        private object OnChangeNickname(params object[] objs)
        {
            string name = objs[0] as string;
            nicknameTxt.text = name;
            return null; 
        }

        private object OnChangeHead(params object[] objs)
        {
            KGender gender = EnumUtils.GetEnumIns<KGender>(objs[0]);
            if (gender == KGender.gFemale)
            {
                headSp.spriteName = "女主";
            }
            else
            {
                headSp.spriteName = "男主";
            }
            return null;
        }
    }
}
