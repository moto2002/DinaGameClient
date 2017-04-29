using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.Utils;

namespace Assets.Scripts.UIComponent
{
	public class ThumbnailView : MonoBehaviour
	{
		private RenderTexture renderTexture;
		
		private const int width = 512;
		
		private const int height = 512;
		
		private Camera camera;

		private GameObject parent;
		private Vector3 position;
		private Vector3 scale;
		private UITexture uiTexture;
		private bool isStartRotate = false;
		private Transform modelTran;
		private int speed;
				
		public ThumbnailView ()
		{
		}

		public void CreateThumbnailView (GameObject parent , Vector3 position , Vector3 scale , UITexture uiTexture)
		{
			this.parent = parent;
			this.position = position;
			this.scale = scale;
			this.uiTexture = uiTexture;
			CreateTexture ();
			CreateCamera ();
		}
		
		private void CreateTexture ()
		{
			renderTexture = new RenderTexture(width,height,24,RenderTextureFormat.ARGB32,RenderTextureReadWrite.sRGB);
        	renderTexture.name = parent.name + "Texture";
        	renderTexture.isPowerOfTwo = false;
			uiTexture.mainTexture = renderTexture;
		}
		
		private void CreateCamera()
		{
			GameObject go = new GameObject(parent.name + "Camera");
            camera = go.AddComponent<Camera>();
			camera.orthographicSize = 100;
			camera.depth = 4;
			camera.clearFlags = CameraClearFlags.SolidColor;
			camera.cullingMask = CameraLayerManager.GetInstance().Get3DUITag();
			camera.targetTexture = renderTexture;
			camera.transform.localPosition = position;
		}
		
		public void StartRotate (GameObject modelObject , int speed)
		{
			isStartRotate = true;
			this.modelTran = modelObject.transform;
			this.speed = speed;
		}
		
		void FixedUpdate ()
		{
			if(isStartRotate)
			{
				modelTran.Rotate(Vector3.forward*Time.deltaTime*-speed);
			}
		}
		
		public void StopRotate ()
		{
			isStartRotate = false;
		}
		
		public Camera GetCamera()
		{
			if(null != camera)
				return camera;
			return null;
		}

	}
}


