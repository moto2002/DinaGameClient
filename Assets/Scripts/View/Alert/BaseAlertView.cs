using UnityEngine;
using System.Collections;
using Assets.Scripts.View.UIDetail;
using Assets.Scripts.Logic.Item;
using Assets.Scripts.Manager;
using Assets.Scripts.Model.Player;

namespace Assets.Scripts.View.Alert
{
	public class BaseAlertView : IntensifyEffectUIDetail
	{
		
		public event AlertCallBackDelegate AlertCallBack;
		
		private bool bInited = false;
		
		private EquipInfo info;
		
		private int time;
		
		private bool isCountDown = false;
		
		
		private float startTime = 0f;
		private float currentTime;

        public BaseAlertView()
            : base(50, 50)
        {

        }

        protected override void Init()
        {
            base.Init();
            bInited = true;
            AlertWindows(info,time);
        }

        public void AlertWindows(EquipInfo info , int time )//, AlertCallBackDelegate alertCallBack
        {
//			 alertCallBack;
			this.info = info;
			this.time = time;
            if (!bInited)
                return;
			UIAtlas atlas = UIAtlasManager.GetInstance().GetUIAtlas("IconAtlas");
			EquipIcon.spriteName = info.Icon;
			EquipIcon.atlas = atlas;
			Text.text = "正在进行<9932CC>" + info.Name + "<->的<00BFFF>进阶<->";
//			
			startTime = this.time + Time.time;
			ShowProcess();
			if(this.time > 0)
				isCountDown = true;
        }
		
		
		private void ShowProcess()
		{
			HammerSprite.gameObject.SetActive(true);
			HammerTableSprite.gameObject.SetActive(true);
			Text.gameObject.SetActive(true);
			SuccessSprite.gameObject.SetActive(false);
			EquipIcon.gameObject.SetActive(false);
			ItemFrameSprite.gameObject.SetActive(false);
		}
		
		private void ShowSuccess ()
		{
			HammerSprite.gameObject.SetActive(false);
			HammerTableSprite.gameObject.SetActive(false);
			Text.gameObject.SetActive(false);
			SuccessSprite.gameObject.SetActive(true);
			EquipIcon.gameObject.SetActive(true);
			ItemFrameSprite.gameObject.SetActive(true);	
		}

		public override void FixedUpdate ()
		{
			base.FixedUpdate ();

			if(isCountDown && this.time >= 0)
			{
				currentTime = startTime - Time.time;
				if(currentTime <= 2)
					ShowSuccess();
				if(currentTime <= 0)
				{
					this.Hide();
					AlertCallBack();
					currentTime = 0;
					isCountDown = false;
				}
			}
		}

	}
}

