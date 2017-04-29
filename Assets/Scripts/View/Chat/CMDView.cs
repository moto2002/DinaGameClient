using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Lib.View;
using Assets.Scripts.Controller;
using Assets.Scripts.View.UIDetail;

namespace Assets.Scripts.View.Chat
{
    public class CMDView : ChatUIDetail
    {
        public CMDView()
            : base(200, 30)
        {

        }

        private static CMDView instance;
        public static CMDView GetInstance()
        {
            if (instance == null)
                instance = new CMDView();
            return instance;
        }

        protected override void PreInit()
        {
            base.PreInit();
        }

        protected override void Init()
        {
            SetViewPosition(ViewPosition.BottomLeft);
            uiPanel.transform.localPosition += new Vector3(5, 0, 0);
            
            UIEventListener.Get(Input.gameObject).onSubmit += OnSubmitHandler;
        }

        public bool IsEditing()
        {
            if (Input == null)
                return false;

            return Input.selected;
        }

        private void OnSubmitHandler(GameObject go)
        {
            string str = inputTxt.text;
            str = str.Substring(0,str.LastIndexOf('|'));
            inputTxt.text = "";
            ChatController.GetInstance().SendGMMessage(str);
        }
    }
}
