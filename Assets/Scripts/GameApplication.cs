using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Controller;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.Lib.Log;
using Assets.Scripts;
using Assets.Scripts.Manager;
using System.Collections;
using Assets.Scripts.Utils;
using Assets.Scripts.View.Scene;
using Assets.Scripts.Logic.Bag;
using Assets.Scripts.View;
using Assets.Scripts.Logic;
using Assets.Scripts.Logic.RemoteCall;
using Assets.Scripts.Logic.Scene.SceneObject.Compont;
using Assets.Scripts.Logic.Scene;
using Assets.Scripts.Logic.Npc;
using Assets.Scripts.Model.Player;
using Assets.Scripts.Logic.Scene.SceneObject;

class GameApplication : MonoBehaviour
{
    private static Logger log = LoggerFactory.GetInstance().GetLogger(typeof(GameApplication));
    private string webData="";

    private GameProgress progress;

    void Awake()
    {
        Logger.Log("Application Awake");
		string[] names = QualitySettings.names;
        QualitySettings.SetQualityLevel(names.Length - 1);
        Logger.Log("Game Quality:" + QualitySettings.names[QualitySettings.names.Length - 1]);
        LoggerFactory.GetInstance().StartLog();
        ColorSpace space = QualitySettings.activeColorSpace;
    }

    void Start()
    {
        Logger.Log("Application Start");
        
        if (Application.isEditor)
        {
            InitCoreManager();
        }
        else
        {
            getGameParameter();
        }

        Application.RegisterLogCallback(LogCallback);
    }

    private void LogCallback(string condition, string stackTrace, LogType type)
    {
        Logger.Log(condition, stackTrace, type);
    }
	
	/*string str = "";
	void OnGUI()
	{
		str = GUI.TextField(new Rect(0,0,200,30),str);
	}*/
	
	int testHeroId = 0;
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.F12))
		{
			ConfigManager.GetInstance().DebugMode = true;
			SceneEntity[] ses =  SceneLogic.GetInstance().GetSceneObjs();
			foreach (SceneEntity e in ses)
			{
				e.CreateTraceComponent();
			}
		}
		if (Input.GetKeyDown(KeyCode.F11)|Input.GetKeyDown(KeyCode.Insert))
		{
			ChatController.GetInstance().SendGMMessage("ChangeMap(2,0)");
		}
		
		if (Input.GetKeyDown(KeyCode.F10))
		{
			ChatController.GetInstance().SendGMMessage("player:AddExp(2000000)");
		}
		if (Input.GetKeyDown(KeyCode.F8))
		{
			{
				Object [] objs =  Object.FindObjectsOfType(typeof(Material));
				Debug.LogWarning("Material count = "+objs.Length);
			}
			//QualitySettings.SetQualityLevel(names.Length - 1);
		}
		
		if (Input.GetKeyDown(KeyCode.F7))
		{
			ChatController.GetInstance().SendGMMessage("AddNpc(2013)");
		}
		if (Input.GetKeyDown(KeyCode.F4))
		{
			ChatController.GetInstance().SendGMMessage("SetAttack(1000000)");
		}
		
		if (Input.GetKeyDown(KeyCode.F5))
		{
			testHeroId++;
			TestComponent.CreateNewHero((uint)testHeroId);
			testHeroId %=3;
		}
		DelayCallManager.instance.Update();
		UIManager.GetInstance().Update();
	}
	
	
    void FixedUpdate()
    {
        PlayerManager.GetInstance().MajorPlayer.onlineTime += Time.deltaTime;
        UIManager.GetInstance().FixedUpdate();
    }

    private void InitCoreManager()
    {
        RegisterLog();
		DontDestroyOnLoad(this.gameObject);
        log.Debug("Start game application...");
        GameWorld.GetInstance();
        LoggerView.GetInstance();
        CameraLayerManager.GetInstance();
        AssetLoader.GetInstance();
        ViewCameraManager.GetInstance().Init();
        LayerManager.GetInstance().Init();
        RemoteCallLogic.GetInstance();
        NGUIManager.GetInstance();

        LoginController.GetInstance();
        SceneLogic.GetInstance();
        PlayerController.GetInstance();
        BagLogic.GetInstance();
        NpcLogic.GetInstance();
		TimeManager.GetInstance();
        ViewManager.GetInstance();
		gameObject.AddComponent("CronJob");
        LoadResource();
		
		//ActiveRush.InitFx();
		
		
		
		AssetLoader.GetInstance().PreLoad(URLUtil.GetEffectPath("effect_skill_badao03_xuanfengzhan_g2"));
		AssetLoader.GetInstance().PreLoad(URLUtil.GetEffectPath("effect_skill_badao01_lipihuashantexiao"));
		AssetLoader.GetInstance().PreLoad(URLUtil.GetEffectPath("effect_skill_badao03_xuanfengzhan"));
		AssetLoader.GetInstance().PreLoad(URLUtil.GetEffectPath("effect_skill_badao03_xuanfengzhan_mingzhong2"));
		
		AssetLoader.GetInstance().PreLoad(URLUtil.GetEffectPath("effect_skill_badao04_yuanyuewandao_mingzhong"));
		AssetLoader.GetInstance().PreLoad(URLUtil.GetEffectPath("effect_skill_badao04_yuanyuewandao_mingzhong_g"));
		AssetLoader.GetInstance().PreLoad(URLUtil.GetEffectPath("effect_skill_badao04_yuanyuewandao_wuqifeixing"));
		AssetLoader.GetInstance().PreLoad(URLUtil.GetEffectPath("effect_skill_badao04_yuanyuewandao"));
		
		AssetLoader.GetInstance().PreLoad(URLUtil.GetEffectPath("effect_skill_badao02_nuzhanbahuang_gongji"));
		AssetLoader.GetInstance().PreLoad(URLUtil.GetEffectPath("effect_skill_daobin_06_nuhou_shifa"));
		
		AssetLoader.GetInstance().PreLoad(URLUtil.GetEffectPath("effect_skill_daobin_05_zhenfei"));
		AssetLoader.GetInstance().PreLoad(URLUtil.GetEffectPath("effect_skill_daobin_04_chongfeng"));
		AssetLoader.GetInstance().PreLoad(URLUtil.GetEffectPath("effect_skill_daobin_04_chongfeng_mingzhong"));
		
		AssetLoader.GetInstance().PreLoad(URLUtil.GetEffectPath("effect_guaiwusiwang2"));
		
		AssetLoader.GetInstance().PreLoad(URLUtil.GetEffectPath("effect_levelup"));
		
		AssetLoader.GetInstance().Load(URLUtil.GetEffectPath("effect_guangquan_yellow"), PLAYER_SEL_LoadComplete, AssetType.BUNDLER);
        AssetLoader.GetInstance().Load(URLUtil.GetEffectPath("effect_guangquan2_red"), NPC_SEL_LoadComplete, AssetType.BUNDLER);

        PreLoad.GetInstance().OnEnterScene();
		SelecterManager.GetInstance();
    }

	public static void BuffderLoadComplete()
	{
		BuffComponent.InitBuff(1);
        BuffComponent.InitBuff(2);
        BuffComponent.InitBuff(3);
        BuffComponent.InitBuff(4);
        BuffComponent.InitBuff(5);
	}
	
	private void PLAYER_SEL_LoadComplete(AssetInfo info)
    {
        GlowComponent.globalPlayerSelectGameObject = Instantiate(info.bundle.mainAsset) as GameObject;
        GlowComponent.globalPlayerSelectGameObject.name = "effect_guangquan_yellow";
        GlowComponent.globalPlayerSelectGameObject.transform.localPosition = Vector3.zero;
        GlowComponent.globalPlayerSelectGameObject.transform.localScale = Vector3.one;
        GlowComponent.globalPlayerSelectGameObject.SetActive(true);
		//GlowComponent.globalPlayerSelectGameObject.hideFlags = HideFlags.HideAndDontSave;
        GameObject.DontDestroyOnLoad(GlowComponent.globalPlayerSelectGameObject);
 
    }
	private void NPC_SEL_LoadComplete(AssetInfo info)
    {
        GlowComponent.globalSelectGameObject = Instantiate(info.bundle.mainAsset) as GameObject;
        GlowComponent.globalSelectGameObject.transform.name = "effect_guangquan2_red";
        GlowComponent.globalSelectGameObject.transform.localPosition = transform.position;
        GlowComponent.globalSelectGameObject.transform.localScale = Vector3.one;
        GlowComponent.globalSelectGameObject.SetActive(true);
		//GlowComponent.globalSelectGameObject.hideFlags = HideFlags.HideAndDontSave;
        GameObject.DontDestroyOnLoad(GlowComponent.globalSelectGameObject);
 
    }

    private void RegisterLog()
    {
    //    LoggerFactory.GetInstance().IsDefaultStart = false;
    //    LoggerFactory.GetInstance().StartLog("");
    }

    private void LoadResource()
    {
        log.Debug("11");
        progress = new GameProgress();
        progress.OnLoadComplete += Progress_OnLoadComplete;
        progress.StartLoad();
    }

    void Progress_OnLoadComplete()
    {
        progress.OnLoadComplete -= Progress_OnLoadComplete;
        InitNetwork();
    }

    private void InitNetwork()
    {
        LoginController.GetInstance().Connect(KConstants.IP_HOST, KConstants.IP_PORT);
        log.Debug("连接服务器  IP： " + KConstants.IP_HOST + "PROT：" + KConstants.IP_PORT);
        log.Debug("END");
    }

    private void getGameParameter()
    {
        Application.ExternalCall("getGameParameter");
    }

    public void InitGameParameter(string msg)
    {
        webData = msg;
        string[] parameters;
        parameters = msg.Split('|');
        KConstants.IP_HOST = parameters[0];
        KConstants.IP_PORT = (short)int.Parse(parameters[1]);
        ConfigManager.GetInstance().Account = parameters[2];
        ConfigManager.GetInstance().DebugMode = bool.Parse(parameters[3]);
        ConfigManager.GetInstance().GroupID = int.Parse(parameters[4]);
        log.Debug("接受到参数" + msg);
        InitCoreManager();
    }
}
