using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Manager;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.Lib.Log;
using Assets.Scripts.Data;
using Assets.Scripts.Logic.Role;
using Assets.Scripts.Utils;
using Assets.Scripts.Logic.Scene.SceneObject.Compont;
using UnityEngine;
using Assets.Scripts.Logic.Scene;

namespace Assets.Scripts
{
    class GameProgress
    {
        protected static Logger log = LoggerFactory.GetInstance().GetLogger(typeof(GameProgress));
        public delegate void LoadComplete();
        public event LoadComplete OnLoadComplete;
        private float progressValue;

        public void StartLoad()
        {
            progressValue = 0.0f;
            LoadFont();

        }

        //装载字体
        void LoadFont()
        {
            FontManager.GetInstance().OnLoadComplete += LoadFont_OnLoadComplete;
            FontManager.GetInstance().LoadDefault();
        }
		

        void LoadFont_OnLoadComplete()
        {
            FontManager.GetInstance().OnLoadComplete -= LoadFont_OnLoadComplete;
            progressValue = 0.1f;
			LoadWeaponTrail();
        }
		void LoadWeaponTrail()
		{
			WeaponTrailLoader.OnLoadCompleteFun = WeaponTrail_OnLoadComplete;
			WeaponTrailLoader.StartLoad();
		}
		void WeaponTrail_OnLoadComplete()
		{
			LoadMinMapInfor();
		}
		void LoadMinMapInfor()
		{
			AssetLoader.GetInstance().Load(URLUtil.GetResourceLibPath() + "Scene/MinMap.xml", LoadMinMapComplete, AssetType.BINARY);
		}
		private void LoadMinMapComplete(AssetInfo info)
		{
			string xmlText =  System.Text.Encoding.Default.GetString( info.binary );
			MinMapDataManager.instance.LoadXml(xmlText);
			LoadShader();
		}
		
		void LoadShader()
		{
			AssetLoader.GetInstance().Load(URLUtil.GetEffectPath("ShaderManager"), LoadShaderComplete, AssetType.BUNDLER);
		}
		private void LoadShaderComplete(AssetInfo info)
		{
			GameObject glowGameObject = GameObject.Instantiate(info.bundle.mainAsset) as GameObject;
			GameObject.DontDestroyOnLoad(glowGameObject);
			LoadNameLabelObj();
		}
		
		
		void LoadNameLabelObj()
		{
			NumTipComponent.OnLoadComplete += NumTipsLoadComplete;
			NumTipComponent.Load();
		}
		void NumTipsLoadComplete()
		{
			NumTipComponent.OnLoadComplete -= NumTipsLoadComplete;
			LoadResource();
		}

        //装载Atlas资源
        void LoadResource()
        {
			UIAtlasManager.GetInstance().AddResource("OtherAtlas");
			UIAtlasManager.GetInstance().AddResource("EquipAtlas");
            UIAtlasManager.GetInstance().AddResource("SkillAtlas");
            UIAtlasManager.GetInstance().AddResource("IconAtlas");
            UIAtlasManager.GetInstance().AddResource("CommonAtlas");
            UIAtlasManager.GetInstance().AddResource("Common2Atlas");
            UIAtlasManager.GetInstance().AddResource("ChatsystemAtlas");
            UIAtlasManager.GetInstance().OnLoadComplete += UIAtlas_OnLoadComplete;
            UIAtlasManager.GetInstance().Load();
        }

        void UIAtlas_OnLoadComplete()
        {
            UIAtlasManager.GetInstance().OnLoadComplete -= UIAtlas_OnLoadComplete;
            progressValue = 0.2f;
            LoadRoleBaseData();
        }

        void LoadRoleBaseData()
        {
            EventDispatcher.GameWorld.Regist(ControllerCommand.ROLE_DATA_BASE_LOADED, OnLoadBaseComplete);
            RoleGenerator.LoadRoleBaseData();
        }

        object OnLoadBaseComplete(object obj)
        {
            EventDispatcher.GameWorld.Remove(ControllerCommand.ROLE_DATA_BASE_LOADED, OnLoadBaseComplete);
            progressValue = 0.2f;
            LoadTabFile();
            return null;
        }
		//装载TAB表
        void LoadTabFile()
        {
            KConfigFileManager.GetInstance().OnLoadAllComplete += LoadTabFile_OnLoadAllComplete;
            KConfigFileManager.GetInstance().LoadConfigFile();
        }

        void LoadTabFile_OnLoadAllComplete()
        {
            log.Debug("44");
            KConfigFileManager.GetInstance().OnLoadAllComplete -= LoadTabFile_OnLoadAllComplete;
            progressValue = 0.2f;
            CompleteLoad();

            log.Debug("LoadTabFile_OnLoadAllComplete:" +
                KConfigFileManager.GetInstance().clientConfig.GetString("Client", "logLevel") + ", " +
                KConfigFileManager.GetInstance().clientConfig.GetInt("Gateway", "port")
            );
        }

        void CompleteLoad()
        {
            progressValue = 1;
            if (OnLoadComplete != null)
            {
                OnLoadComplete();
            }
        }
    }
}
