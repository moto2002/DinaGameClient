using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.Utils;

public class AudioManager  
{
	public static  readonly AudioManager instance = new AudioManager();
	public float volume = 1.0f;
	public Dictionary<string,AudioClip> audios = new Dictionary<string, AudioClip>();
	public Dictionary<AssetInfo,string> urls = new Dictionary<AssetInfo, string>();
	private AudioManager()
	{
		
	}
	
	public void PlaySound3d(string res_path,Vector3 pos)
	{
		AudioClip clip = null;
		if (audios.TryGetValue(res_path, out clip))
		{
			if (clip != null)
			{
				GameObject g = new GameObject("audio_" + res_path);
				//g.transform.position = pos;
				g.transform.position = Camera.main.transform.position;
				AudioSource _as = g.AddComponent<AudioSource>();
				_as.volume = volume;
				_as.clip = clip;
				_as.Play();
				DestoryObject dot = g.AddComponent<DestoryObject>();
				dot.delta = 5f;
			}
			
			
		}
		else
		{
			AssetInfo infor =  AssetLoader.GetInstance().Load(
				URLUtil.GetResourceLibPath() + "Audio/"  +res_path, 
				LoadAudioClipComplete, AssetType.BUNDLER, false,
                AssetLoaderLevel.IMMEDIATELY
			);
			urls[infor] = res_path;
		}
	}
	private void LoadAudioClipComplete(AssetInfo info)
	{
		AudioClip clip = null;
		if (null != info.bundle && null != info.bundle.mainAsset)
			clip = info.bundle.mainAsset as AudioClip;
		string res_path = urls[info];
		audios[res_path] = clip;
		if (null == clip)
			Debug.LogError("Load audio "+res_path+ " false");
	}
	
}
