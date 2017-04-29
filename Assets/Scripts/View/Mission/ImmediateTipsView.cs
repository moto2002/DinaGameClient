using UnityEngine;
using System.Collections;
using Assets.Scripts.View.UIDetail;
using Assets.Scripts.Logic.Mission;

namespace Assets.Scripts.View.Mission
{
    public class ImmediateTipsView : OneKeyTips1UIDetail
    {
        public ImmediateTipsView()
            : base(0, 0)
        {

        }

        private bool bInit = false;
        private static ImmediateTipsView instance;
        public static ImmediateTipsView GetInstance()
        {
            if (instance == null)
                instance = new ImmediateTipsView();
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
            UnityEngine.Object.DontDestroyOnLoad(viewGo);
        }

        public void ShowTips(MissionInfo info)
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
