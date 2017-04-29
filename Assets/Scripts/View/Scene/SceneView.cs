using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Scripts.Model.Scene;
using Assets.Scripts.View.Scene;
using Assets.Scripts.View.Scene.Map;
using Assets.Scripts.Controller;
using Assets.Scripts.Lib.Log;

using SObject = System.Object;
using UObject = UnityEngine.Object;
using Assets.Scripts.Manager;
using Assets.Scripts.Define;
using Assets.Scripts.Model.Player;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.Data;
using Assets.Scripts.Model.Skill;
using Assets.Scripts.Utils;
using Assets.Scripts.Logic.Scene.SceneObject;
using Assets.Scripts.Logic.Scene.SceneObject.Compont;
using Assets.Scripts.Logic.Scene;

namespace Assets.Scripts.View.Scene
{
    public class SceneView : MonoBehaviour
	{
        private static Logger log = LoggerFactory.GetInstance().GetLogger(typeof(SceneView));
		private SceneMap sceneMap;
        private ArrayList addPlayerList;
        private SceneCamera sceneCamera;
        private MouseClickScene clickScene;
        public bool IsLock = true;
        public delegate void Build(SObject sender);
        public delegate void BuildCompleteDelegate();
        public KMapListSetting setting = null;
        private bool isFirstLogin = true;
        public uint mapId;

        public SceneMap SceneMap{get { return sceneMap; }}

        protected void Init()
        {
            EventDispatcher.GameWorld.Regist(ControllerCommand.SET_MAIN_HERO, OnSetMainHero);
        }

		void Update () 
        {
            EventDispatcher.GameWorld.DispatchEvent(ControllerCommand.UPDATE_MINI_MAP);
		}

        void Awake()
        {
            sceneMap = new SceneMap();

            sceneCamera = this.gameObject.AddComponent<SceneCamera>();
            clickScene = MouseClickScene.Create(this);
            CursorManager.GetInstance().SetCursor(CursorType.ctNormal);
        }

        public void buildScene(uint mapId)
        {
			SceneLogic.GetInstance().Clear();
            setting = KConfigFileManager.GetInstance().mapListSetting.getData(mapId.ToString());

            if (null != setting)
            {
                SceneLogic.GetInstance().backgroundType = setting.BackgroundType;
            }

            BuildHandler(mapId);
        }

        public void BuildComplete()
        {
            // BuildCompleteLoadTriggerObject();
            OnSceneBuildComplete();
            EventDispatcher.GameWorld.Dispath(ControllerCommand.CHANGE_MAP);
        }

        private void BuildHandler(uint mapId)
        {
            addPlayerList = new ArrayList();
            EventDispatcher.GameWorld.DispatchEvent(ControllerCommand.LOAD_MINI_MAP, mapId);
            sceneMap.Build(mapId);
        }

        private void OnSceneBuildComplete()
        {
            if (isFirstLogin)
            {
                LoginController.GetInstance().CloseLoginView();
                EventDispatcher.GameWorld.DispatchEvent(ControllerCommand.INIT_SCENE_UI);
                isFirstLogin = false;
            }

            SceneLogic.GetInstance().SendApplySceneObj();
        }

        public void SwitchMap(uint Id)
        {
            mapId = Id;
            buildScene(Id);
        }

        private void Dispose()
        {
            
        }

       


        //攻击场景
        private void AttackSkillScene()
        {
            //检查技能是否为方向攻击技能， 是则直接攻击场景， 释放技能, 不是则移动到最近点攻击
        }

        private void RoleAttackScene()
        {

        }

        private bool isRequestFight;

        public void CancelRoleRequestFight()
        {
            isRequestFight = false;
        }
		

		public uint wTargetHeroID;
		public ushort wSkillID;
		public ushort wDamage;
		public byte byAttackEvent;
		



        private static SceneView mInstance;
        public static SceneView GetInstance()
        {
            if (mInstance == null)
            {
                mInstance = UObject.FindObjectOfType(typeof(SceneView)) as SceneView;

                if (mInstance == null)
                {
                    GameObject go = new GameObject("_SceneView");
                	//go.hideFlags = HideFlags.HideAndDontSave;
                    DontDestroyOnLoad(go);
                    mInstance = go.AddComponent<SceneView>();
                }
                mInstance.Init();
            }
            return mInstance;
        }

        public void SetSceneLight()
        {
            if (SceneLogic.GetInstance().MainHero == null)
            {
                return;
            }
            GameObject lightGo = SceneMap.SceneLight;
            if (lightGo != null)
            {
                lightGo.transform.parent = SceneLogic.GetInstance().MainHero.transform;
            }
        }

        public object OnSetMainHero(params object[] objs)
        {
            SetSceneLight();
            sceneCamera.SetTarget(SceneLogic.GetInstance().MainHero);
            IsLock = false;
            return null;
        }

        public SceneCamera GetSceneCamera()
        {
            return sceneCamera;
        }
	}
}
