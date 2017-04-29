using UnityEngine;
using System.Collections.Generic;


namespace Assets.Scripts
{
    class ViewCameraManager
    {
        private UICamera ui2DCamera;
        private UICamera ui3DCamera;
        private Camera camera;
        private Camera mainCamera;


        public void Init()
        {
            if (Camera.main != null)
            {
                GameObject go = Camera.main.gameObject;
                Object.DontDestroyOnLoad(go);
            }
            InitSceneCamera();
            InitUI2DCamera();
        }

        public Camera UICamera
        {
            get
            {
                return camera;
            }
        }
		public Camera GetUI2DCamera()
		{
			if (null == ui2DCamera)
				InitUI2DCamera();
			return ui2DCamera.camera;
		}
	
        public void InitUI2DCamera()
        {
			if (null!= ui2DCamera)
				return;
            GameObject go = new GameObject("_UI2DCamera");
            Object.DontDestroyOnLoad(go);
            ui2DCamera = go.AddComponent<UICamera>();
            ui2DCamera.debug = true;
            ui2DCamera.eventReceiverMask = CameraLayerManager.GetInstance().Get2DTag();

            camera = go.GetComponent<Camera>();
            if (camera == null)
            {
                camera = go.AddComponent<Camera>();
            }
            camera.orthographic = true;
            camera.orthographicSize = 1;
            camera.nearClipPlane = -10;
            camera.farClipPlane = 3000;
            camera.depth = 3;
            camera.clearFlags = CameraClearFlags.Depth;
            camera.cullingMask = CameraLayerManager.GetInstance().Get2DTag();
        }


        public void InitSceneCamera()
        {
            mainCamera = Camera.main;
            mainCamera.cullingMask = CameraLayerManager.GetInstance().GetSceneTag();
        }

        private static ViewCameraManager instance;
        public static ViewCameraManager GetInstance()
        {
            if (instance == null)
            {
                instance = new ViewCameraManager();
            }
            return instance;
        }
    }
}
