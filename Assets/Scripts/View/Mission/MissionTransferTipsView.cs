using UnityEngine;
using System.Collections;
using Assets.Scripts.View.UIDetail;
using Assets.Scripts.Logic.Mission;

namespace Assets.Scripts.View.Mission
{
    public class MissionTransferTipsView : TaskTransferTipsUIDetail
    {
        public MissionTransferTipsView()
            : base(0, 0)
        {

        }

        private bool bInit = false;

        private static MissionTransferTipsView instance;
        public static MissionTransferTipsView GetInstance()
        {
            if (instance == null)
                instance = new MissionTransferTipsView();
            return instance;
        }

        protected override void PreInit()
        {
            base.PreInit();
        }

        protected override void Init()
        {
            base.Init();
            bInit = true;
            AnchorToMouse();
        }

        public void ShowTips()
        {
            AnchorToMouse();
            Show(true);
        }

        private void AnchorToMouse()
        {
            if (!bInit)
            {
                return;
            }

            Vector3 mPos = Input.mousePosition;
            mPos.x = Mathf.Clamp01(mPos.x / Screen.width);
            mPos.y = Mathf.Clamp01(mPos.y / Screen.height);

            Panel.transform.position = UICamera.currentCamera.ViewportToWorldPoint(mPos);
            Panel.transform.localPosition += new Vector3(-BackgroundSprite.width / 2, -BackgroundSprite.height / 2, 0);
        }
    }
}

