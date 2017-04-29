using UnityEngine;
using System.Collections;
using Assets.Scripts.Logic.Scene;
public class GhostShadow : MonoBehaviour {
	static GhostShadowManager gsmgr = null;
	static Camera gsCamera = null;
	float tick = 0f;
	public GameObject [] subMeshs  = null;
	static void CheckCamera()
	{
		if (null == gsmgr)
		{
			GameObject mgr = new GameObject ("GhostShadewCamra", typeof(Camera)); 
			mgr.camera.depth = -1000;
			gsCamera = mgr.camera;
			gsCamera.hideFlags = HideFlags.DontSave;
			gsmgr = mgr.AddComponent<GhostShadowManager>();
			GameObject.DontDestroyOnLoad(mgr);	
		}	
	}
	// Use this for initialization
	void Start () {
		CheckCamera();
	}
	public static void CopySceneObjToTexture(float f)
	{
		CheckCamera();
	}
	void LateUpdate()
	{
		tick += Time.deltaTime;
		if (tick < 0.05f)
			return;
		tick = 0f;
		gsmgr.AddShadow(transform, subMeshs);
	}
}
