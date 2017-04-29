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

public class WeaponTrailLoader : MonoBehaviour {
	
	public static void StartLoad()
	{
		AssetLoader.GetInstance().Load(URLUtil.GetResourceLibPath() + "Actor/Weapon/WeaponTrailMat.res",ImageLoadImgComplete,AssetType.BUNDLER);
	}
	public delegate void LoadComplete();
    public static  LoadComplete OnLoadCompleteFun = null;
	public static Texture2D texture;
	public static Material trailMat = null;
	
	private static void ImageLoadImgComplete(AssetInfo info)
	{
		GameObject g = info.bundle.mainAsset as GameObject;
		trailMat = g.renderer.material;
		texture = g.renderer.material.mainTexture as Texture2D;
		if (null!=OnLoadCompleteFun)
			OnLoadCompleteFun();
	}
}
