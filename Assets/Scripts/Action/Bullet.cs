using UnityEngine;
using System.Collections;
using Assets.Scripts.View.Scene;
using Assets.Scripts.Logic.Scene.SceneObject;
using Assets.Scripts.Logic.Scene;
using Assets.Scripts.Data;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.Utils;
public class Bullet : MonoBehaviour {
	public SceneEntity hero;
	public SceneEntity target = null;
	public Vector3 endPosition;
	public Vector3 beginPosition;
	public float speed = 5f;
	public string hitFx = "";
	public MessageCase  msg_case ;
	public KSkillDisplay displayInfor;
	// Use this for initialization
	void Start () {
		if (hitFx.Length>0)
            AssetLoader.GetInstance().PreLoad(URLUtil.GetResourceLibPath() + hitFx);
		RefreadEndPosition();
	}
	
	void RefreadEndPosition()
	{
		if (null != target)
		{
			beginPosition = transform.position;
			endPosition = target.transform.position;
		}
	}
	
	void Shake()
	{
		if (hero.property.isMainHero)
		{
			if (displayInfor.CameraShakeScale > 0 && displayInfor.CameraShakeTime > 0)
			{
				if (displayInfor.CameraShakeFile.Length > 0)
					SceneCamera.Shake(displayInfor.CameraShakeFile,displayInfor.CameraShakeTime,displayInfor.CameraShakeSpeed,displayInfor.CameraShakeScale);
				else
					SceneCamera.Shake(displayInfor.CameraShakeTime,displayInfor.CameraShakeScale,SHAKE_TYPE.SUDDENLY);
			}
		}
	}
	// Update is called once per frame
	void Update () {
		RefreadEndPosition();
		Vector3 p = transform.position;
		if ( /* null== target || */KingSoftMath.MoveTowards( ref p,target.transform.position,speed*Time.deltaTime))
		{
			
			if (displayInfor != null && displayInfor.CameraEffect.CompareTo("SHAKE_BULLET_HIT")==0)
			{
				Shake();
			}
			if (displayInfor.SoundType == KSkillDisplay.ACTION_AUIDO_TYPE.Hit)
			{
				if( displayInfor.Sound.Length>0 )
				{
					AudioManager.instance.PlaySound3d(displayInfor.Sound,hero.Position);
				}
			}
			
			msg_case.PopMessage(hero);
			gameObject.AddComponent<DestoryObject>();
		}
		else
		{
			KingSoftCommonFunction.NearPosition(p);
			transform.position = p;
		}
	}
}
