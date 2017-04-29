using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Assets.Scripts.Manager;
using Assets.Scripts.Utils;
using Assets.Scripts.View.Scene;
using Assets.Scripts.Controller;
using Assets.Scripts.Model.Scene;
using Assets.Scripts.Lib.Loader;
using UObject = UnityEngine.Object;
using Assets.Scripts.Lib.Log;
using Assets.Scripts.Logic.Scene.SceneObject;
using Assets.Scripts.Logic.Scene;

namespace Assets.Scripts.View.Scene.Map
{
    public class SceneMap
    {
        public static Logger log = LoggerFactory.GetInstance().GetLogger(typeof(SceneMap));
        private uint currentMapId = 0;
        private bool isLoading = false;
        private IList<GameObjectAsset> sceneUnitList  = new List<GameObjectAsset>();
		private  static GameObject rootParent;
        private  GameObject sceneParent;

        private Dictionary<string, List<GameObjectAsset>> loadingDict = new Dictionary<string, List<GameObjectAsset>>();

        public GameObject SceneLight;
		int totalCount = 1;
        public SceneMap()
        {
			if(null == rootParent)
			{
				rootParent = new GameObject("Scene");
				GameObject.DontDestroyOnLoad(rootParent);
			}
            
        }
		public void DestroyOldScene()
		{
			if(null != sceneParent)
			{
				GameObject.DestroyImmediate(sceneParent);
				sceneParent = null;
			}
			foreach(GameObjectAsset goa in sceneUnitList)
			{
                if (goa != null)
                {
                    if (null != goa.infor)
                        AssetLoader.GetInstance().DestroyImmediate(goa.infor);
                }
            }
            sceneUnitList.Clear();
		}

        public void Build(uint mapId)
        {
            log.Debug("开始构建地图");
            this.currentMapId = mapId;
            AssetLoader.GetInstance().Load(URLUtil.GetScenePath(mapId), LoadSceneCompleteHandler, AssetType.SCENE);
        }

        private void LoadSceneCompleteHandler(AssetInfo info)
        {
			DestroyOldScene();
            sceneParent = new GameObject("SubScene_" + currentMapId);
			sceneParent.transform.parent = rootParent.transform;
            log.Debug("构建基础scene场景");
            UObject.DontDestroyOnLoad(GameObject.Find("GameObject"));
            Application.LoadLevel(currentMapId.ToString());

            MoveTo(Vector3.zero);
            KingSoftCommonFunction.ResetGoundMesh();
            log.Debug("装载场景unit完成");

			//Assets.Scripts.View.Npc.LoadingView.GetInstance().SetValue(0.2f);
            SceneView.GetInstance().BuildComplete();
			
			/*Assets.Scripts.Manager.EventDispatcher.GameWorld.Dispath(
				Assets.Scripts.Manager.ControllerCommand.CLOSE_LOADING_PANEL);*/
        }

        private void LoadSceneUnit()
        {
        }

        private System.Object loadLock = new System.Object();

        private void InitObject(GameObject obj, GameObjectAsset asset)
        {
            GameObject go = GameObject.Instantiate(obj) as GameObject;
            go.transform.position = asset.pos;
            go.transform.rotation = asset.rotation;
            go.transform.localScale = asset.scale;
            if (go.renderer != null)
            {
                go.transform.parent = sceneParent.transform;
                go.AddComponent<MeshCollider>();
                go.renderer.lightmapIndex = asset.lightmapIndex;
                go.renderer.lightmapTilingOffset = asset.lightmapTilingOffset;
            }
            if (go.animation != null)
            {
                go.transform.parent = sceneParent.transform;
            }
            if (go.name.IndexOf("PlayerLight_") >= 0)
            {
                SceneLight = go;
                go.transform.localPosition = new Vector3(0, asset.pos.y, 0);
                go.transform.localRotation = UnityEngine.Quaternion.identity;
                go.transform.localScale = UnityEngine.Vector3.one;
                if (go.transform.localPosition.y < 2 || go.transform.localPosition.y > 10)
                {
                    go.transform.localPosition = new Vector3(0, 6, 0);
                }
                go.light.cullingMask = CameraLayerManager.GetInstance().GetSceneObjectLightCullingMask();
                SceneView.GetInstance().SetSceneLight();
            }
            if (go.name.IndexOf("SunLight_") >= 0)
            {
                go.light.cullingMask = CameraLayerManager.GetInstance().GetSceneObjectSunLightCullingMask();
                go.transform.parent = sceneParent.transform;
            }
        }

        public void MoveTo(Vector3 rolePos)
        {
        }

        private bool CheckSceneUnit(GameObjectAsset asset, Vector3 rolePos)
        {
            return true;
        }

        public void Dispose()
        {

        }
    }
}