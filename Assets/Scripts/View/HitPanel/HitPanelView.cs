using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Lib.View;
using Assets.Scripts.Controller;
using Assets.Scripts.View.UIDetail;

namespace Assets.Scripts.View.Chat
{
	
    public class HitPanelView : HitPanelUIDetail
    {
		Ticker ticker = new Ticker(3000);
		int count = 0;
		
		public void hit()
		{
			Panel.gameObject.SetActive(true);
			count++;
			hitLabel.text = System.Convert.ToString(count)+"+";
			ticker.Restart();
		}
        public HitPanelView()
            : base(200, 30)
        {

        }
        private static HitPanelView instance;
        public static HitPanelView GetInstance()
        {
            if (instance == null)
                instance = new HitPanelView();
            return instance;
        }
		
		public override void FixedUpdate()
        {
			if (ticker.IsActiveOneTime())
			{
				Panel.gameObject.SetActive(false);
				count = 0;
			}
        }

        protected override void PreInit()
        {
            base.PreInit();
        }

        protected override void Init()
        {
            GameObject.DontDestroyOnLoad(viewGo);
            SetViewPosition(ViewPosition.BottomLeft);
			Panel.gameObject.SetActive(false);
            uiPanel.transform.localPosition += new Vector3(0, 300, 0);
        }
    }
}
