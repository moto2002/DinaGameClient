using UnityEngine;
using System;
using System.Collections.Generic;
using Assets.Scripts.Define;
using Assets.Scripts.Controller;
using Assets.Scripts.Lib.View;
using Assets.Scripts.Manager;
using Assets.Scripts.Utils;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.View.UIDetail;
using Assets.Scripts.Logic.Scene.SceneObject;
using Assets.Scripts.Logic.Scene;
using Assets.Scripts.Data;

namespace Assets.Scripts.Module.View
{
    class LoginView : CreateRoleUIDetail
    {
        private UITexture texture  = null;
        private int curJob = 1;
        private Dictionary<int, SceneEntity> modelDict = new Dictionary<int, SceneEntity>();

        private GameObject createZhanjiEffect;
        private GameObject createBadaoEffect;
        private GameObject createJuchuiEffect;
		private GameObject createQuanrenEffect;
		
		static Camera loginCamera = null;
		private string poseAction = "pose";
		SceneEntity entity = null;
		public LoginView()
            : base(0, 0)
        {
        }
		
        protected override void PreInit()
        {
            base.PreInit();
			Front();
        }
		
		 public override void Show()
        {
			Front();
			base.Show();
        }
		bool resetPose = false;
		public override void Update()
        {
			if(null != entity)
			{
				if(null != entity.BodyGo)
				{
					entity.BodyGo.layer = 8;
					if(entity.AnimCmp.IsPlayingEx("pose"))
					{
						resetPose = true;
					}
					else
					{
						if(resetPose)
							entity.AnimCmp.CrossFadeAnimation("idle2");
					}	
				}
			}
			if (Input.GetKeyDown(KeyCode.Return))
			{
				ClickEnterGameHandler(null);
			}
        }
        override protected void Init()
        {
            log.Debug("begin to init");
			
            UIEventListener.Get(JobBadao.gameObject).onClick += ClickJobHandler;
            UIEventListener.Get(JobZhanji.gameObject).onClick += ClickJobHandler;
            UIEventListener.Get(JobJuchui.gameObject).onClick += ClickJobHandler;
			UIEventListener.Get(JobQuanren.gameObject).onClick += ClickJobHandler;
            UIEventListener.Get(diceBtn.gameObject).onClick += ClickDiceHandler;
            UIEventListener.Get(enterGameBtn.gameObject).onClick += ClickEnterGameHandler;

            createBadaoEffect = FindGameObject("JobBadao_effect");
			createZhanjiEffect = FindGameObject("JobZhanji_effect");
            createJuchuiEffect = FindGameObject("JobJuchui_effect");
			createQuanrenEffect = FindGameObject("JobQuanren_effect");
			
			JobBadao_static.gameObject.SetActive(false);
			JobZhanji_static.gameObject.SetActive(true);
			JobJuchui_static.gameObject.SetActive(true);
			JobQuanren_static.gameObject.SetActive(true);
      //      创建角色的底图 ( Sprite )").transform.localScale = new Vector3(1450, 1020, 1);
            ClickDiceHandler(null);
			if (null == loginCamera)
			{
				GameObject go = new GameObject("_loginCamera",typeof(Camera));
				loginCamera = go.camera;
				loginCamera.CopyFrom(ViewCameraManager.GetInstance().GetUI2DCamera());
				loginCamera.cullingMask = 1 << 10 | 1 << 11;
				loginCamera.depth = loginCamera.depth+1;
				loginCamera.clearFlags = CameraClearFlags.Nothing;
				
				/*BloomAndLensFlares bloom0 = go.AddComponent<BloomAndLensFlares>();
				BloomAndLensFlares bloom1 = Camera.main.gameObject.AddComponent<BloomAndLensFlares>();
				bloom0.tweakMode = bloom1.tweakMode;
				bloom0.screenBlendMode = bloom1.screenBlendMode;
				bloom0.hdr = bloom1.hdr;
				bloom0.lensflareIntensity = bloom1.lensflareIntensity;
				bloom0.lensflareThreshhold = bloom1.lensflareThreshhold;
				bloom0.bloomBlurIterations = bloom1.bloomBlurIterations;
				bloom0.sepBlurSpread = bloom1.sepBlurSpread;*/
			}
            ChangeShowPlayer();
        }

        //添加模块事件,如Socket或者游戏世界等通知事件,并不包括UI事件
        protected override void InitEvent()
        {
            //ScreenManager.GetInstance().OnChangeSize += ChangeBGSize;
        }
		public void SetLayer(GameObject o,int index)
		{
			Transform [] ts = o.GetComponents<Transform>();
			foreach (Transform t in ts)
			{
				t.gameObject.layer = index;
			}
		}
        private void ChangeShowPlayer()
        {
			if (null != entity)
				entity.gameObject.SetActive(false);
			if (modelDict.TryGetValue(curJob,out entity))
			{
				entity.gameObject.SetActive(true);
			}
			else
			{
				KHeroSetting heroSetting = KConfigFileManager.GetInstance().heroSetting.getData(""+curJob);
				entity = SceneLogic.GetInstance().CreateSceneObject((uint)curJob, KSceneObjectType.sotHero, heroSetting.HeroType, KDoodadType.dddInvalid, new Vector3(0, -0.7f, -1), 1);
                entity.heroSetting = heroSetting;
                entity.Init();
				entity.AnimCmp.pause = true;
				entity.transform.rotation = new Quaternion(0, 180, 0,0);
	            entity.transform.localScale = new Vector3(0.78f, 0.78f, 0.78f);
				SceneLogic.GetInstance().RemoveSceneObjInfor(1);
				//SetLayer(entity.gameObject,LayerMask.NameToLayer("2D"));
				modelDict[curJob] = entity;
			}
			resetPose = false;
			entity.AnimCmp.CrossFadeAnimation(poseAction);
        }
        private void ClickJobHandler(GameObject go)
        {
            if (createBadaoEffect != null)
				createBadaoEffect.SetActive(go == JobBadao.gameObject);
            if (createZhanjiEffect != null)
				createZhanjiEffect.SetActive(go == JobZhanji.gameObject);
            if (createJuchuiEffect != null)
				createJuchuiEffect.SetActive(go == JobJuchui.gameObject);
			if (createQuanrenEffect != null)
				createQuanrenEffect.SetActive(go == JobQuanren_static.gameObject);
			
			JobBadao_static.gameObject.SetActive(go != JobBadao.gameObject);
			JobZhanji_static.gameObject.SetActive(go != JobZhanji.gameObject);
			JobJuchui_static.gameObject.SetActive(go != JobJuchui.gameObject);
			JobQuanren_static.gameObject.SetActive(go != JobQuanren_static.gameObject);
			
            if (go == JobBadao.gameObject)
				curJob = 1;
            else if (go == JobZhanji.gameObject)
				curJob = 2;
            else if (go == JobJuchui.gameObject)
				curJob = 3;
			else
				curJob = 4;
			ChangeShowPlayer();
        }
        private void ClickDiceHandler(GameObject go)
        {
            nicknameTxt.text = "name" + (new System.Random()).Next(100, 100000);
        }
		
		bool isEnteringGame = false;
        private void ClickEnterGameHandler(GameObject go)
        {
			if (isEnteringGame)
				return;
			if (!true)//判断是否符合登陆条件...
				return;
			isEnteringGame = true;
			LoginController.GetInstance().OnCreateRoleRequest(nicknameTxt.text, (sbyte)KGender.gFemale, curJob);		
        	
		}

        private void ChangeBGSize(uint width, uint height)
        {
            texture.transform.localScale = new Vector3(width, height, 0);
        }
    }
}
