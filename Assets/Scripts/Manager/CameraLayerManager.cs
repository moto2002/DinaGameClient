using UnityEngine;
using System.Collections.Generic;


namespace Assets.Scripts
{
    class CameraLayerManager
    {
        private int sceneTag = (1 << 0) | (1 << 1) | (1 << 2) | (1 << 4) | (1<<10)| (1<<11)|(1<<30);

        private static string D2Layer = "2D";
		private static string D3UILayer = "3DUI";
        private static string SceneObjectLayer = "SceneObject";
        private static string SceneEffectLayer = "SceneEffect";
		private static string MissionSign = "MissionSign";

        public LayerMask Get2DTag()
        {
            int layerId = LayerMask.NameToLayer(D2Layer);
            return 1 << layerId;
        }
		
		public LayerMask Get3DUITag()
		{
			int layerId = LayerMask.NameToLayer(D3UILayer);
			return 1 << layerId;
		}

        public int Get2DLayTag()
        {
            return LayerMask.NameToLayer(D2Layer);
        }
		
		public int Get3DUILayTag()
		{
			return LayerMask.NameToLayer(D3UILayer);
		}

        public LayerMask GetSceneTag()
        {
            return sceneTag;
        }

        public LayerMask GetSceneObjectTag()
        {
            return LayerMask.NameToLayer(SceneObjectLayer);
        }

        public LayerMask GetSceneObjectLightCullingMask()
        {
            int layerId = LayerMask.NameToLayer(SceneObjectLayer);
            return -1 - (1 << layerId);
        }

        public LayerMask GetSceneObjectSunLightCullingMask()
        {
            int layerId = LayerMask.NameToLayer(SceneEffectLayer);
            return -1 - (1 << layerId);
        }
		
		public string GetMissionSignName()
        {
            return MissionSign;
        }
        public LayerMask GetSceneEffectTag()
        {
            return LayerMask.NameToLayer(SceneEffectLayer);
        }


        private static CameraLayerManager instance;
        public static CameraLayerManager GetInstance()
        {
            if (instance == null)
            {
                instance = new CameraLayerManager();
            }
            return instance;
        }
    }
}
