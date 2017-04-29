using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Lib.View;
using Assets.Scripts.Manager;
using Assets.Scripts.Data;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.View.Bag;
using Assets.Scripts.Logic.Item;
using Assets.Scripts.Logic.Mission;
using Assets.Scripts.Logic.Npc;
using Assets.Scripts.View.UIDetail;

namespace Assets.Scripts.View.Npc
{
    public class LoadingView : LoadingUIDetail
    {
	
		float loadingValue = 0f;
		UITexture loading = null;
		
		
		public LoadingView()
            : base(0, 0)
        {

        }

        private static LoadingView instance;
        public static LoadingView GetInstance()
        {
            if (instance == null)
                instance = new LoadingView();
            return instance;
        }

        protected override void PreInit()
        {
            base.PreInit();
			SetValue(loadingValue);
			Front();
		}
		public override void Update()
        {
			//Front();
        }
		protected override void Init()
        {
            SetValue(0f);
        }
		public void SetValue(float t)
		{
			
			loadingValue = t;
			if (null != loading)
				loading.material.SetFloat("_MarkX",1-loadingValue);
		}

	}
}
