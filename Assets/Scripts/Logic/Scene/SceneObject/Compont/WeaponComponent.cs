using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.Manager;
using Assets.Scripts.Utils;
using Assets.Scripts.Define;
using Assets.Scripts.Data;
using Assets.Scripts.Logic.Item;

namespace Assets.Scripts.Logic.Scene.SceneObject.Compont
{
	public class WeaponComponent : BaseComponent {
		public enum BIND_POINT
		{
			RIGHT_HAND,
			BEI,
			DEFAULT
		}
		public readonly static string  RIGHT_HAND = "wuqi_R";
		public readonly static string  LEFT_HAND = "wuqi_L";
		public readonly static string  BEI = "bei_wuqi";
		public  BIND_POINT bindPoint = BIND_POINT.RIGHT_HAND;
		bool backBindPoint = true;
		KTabLineEquip equipInfor;
		AssetInfo assetInfo = null;
		AssetInfo fxInfo = null;
		EditorDataMap data = null;
		AssetBundleRequest request = null;
		Transform tuowei1_1 = null;
		Transform tuowei1_2 = null;
		Transform tuowei2_1 = null;
		Transform tuowei2_2 = null;
		bool showWeaponTrail = false;
		MyTrail trail1;
		MyTrail trail2;
		
		public override string GetName()
        {
            return GetType().Name;
        }
		public override void OnAttachToEntity(SceneEntity ety)
        {
			BaseInit(ety);
            AssetLoader.GetInstance().PreLoad(URLUtil.GetRootPath() + "/ResourceLib/Effect/effect_weapon_badao01.res");
			
			Regist(ControllerCommand.WEAPON_TRAIL, OnWeaponTrailChange);
            Regist(ControllerCommand.EQUIP_CHANGE, OnEquipChange);
        }
		public override void OnDetachFromEntity(SceneEntity ety)
        {
			
			UnRegist(ControllerCommand.WEAPON_TRAIL, OnWeaponTrailChange);
            UnRegist(ControllerCommand.EQUIP_CHANGE, OnEquipChange);
            base.OnDetachFromEntity(ety);
        }
		private object OnWeaponTrailChange(params object[] objs)
        {
			showWeaponTrail = (bool)objs[0];
			return null;
		}
		private object OnEquipChange(params object[] objs)
        {
			uint[] equipIDs = null;
            if (objs.Length == 0)
            {
                return null;
            }
			equipIDs = (uint[])objs[0];
			SwitchWeapon(equipIDs[11]);
			return null;
		}
		void SwitchWeapon(uint id)
		{
			if( id == 0 )
			{
				id = Owner.heroSetting.DefaultWeapon;
			}
			if (equipInfor!= null && equipInfor.ID == id)
				return;
			equipInfor = ItemLocator.GetInstance().GetEquip((int)id);
			if (equipInfor == null)
			{
				Debug.LogError("武器 "+id+"不存在");
				return;
			}
			if (equipInfor.FBX.Length == 0)
			{
				Debug.LogError("武器 "+id+"没填写FBX数据");
				return;
			}
			GameObject.Destroy(Owner.property.weapon[0]);
			GameObject.Destroy(Owner.property.weapon[1]);
			Owner.property.weapon[0] = null;
			Owner.property.weapon[1] = null;
			assetInfo = AssetLoader.GetInstance().Load(URLUtil.GetResourceLibPath()+equipInfor.FBX);
			if (equipInfor.Fx.Length>0)
				fxInfo = AssetLoader.GetInstance().Load(URLUtil.GetResourceLibPath()+equipInfor.Fx);
		}
		
		/*private void GlobalLoadComplete(AssetInfo info)
		{
			Debug.Log(info.bundle);
		}*/
		
		public void SetWeaponPosition(BIND_POINT _bindPoint,bool reset = false)
		{
			if (bindPoint == _bindPoint && !reset)
			{
				return;
			}
			bindPoint = _bindPoint;
			if (null == Owner.property.weapon[0])
				return;
			
			string bindPointName = RIGHT_HAND;//默认挂右手.
			if (bindPoint == BIND_POINT.BEI)
			{
				if (data.data.ContainsKey(BEI + "@pos"))//如果存在背挂节点信息，挂背.
				{
					bindPointName = BEI;
				}
			}
			Transform t =  Owner.GetChildTransform(bindPointName);
			if(null!= t)
				Owner.property.weapon[0].transform.parent = t;
			Owner.property.weapon[0].transform.localPosition = data.GetVector3(bindPointName + "@pos") ;
			Owner.property.weapon[0].transform.localRotation = data.GetQuaternion(bindPointName + "@rot") ;
			
			if (null != Owner.property.weapon[1])
			{
				Owner.property.weapon[1].transform.parent = Owner.GetChildTransform(LEFT_HAND);
				Owner.property.weapon[1].transform.localPosition = data.GetVector3(LEFT_HAND + "@pos") ;
				Owner.property.weapon[1].transform.localRotation = data.GetQuaternion(LEFT_HAND + "@rot") ;
			}
			
			/*if (null != Owner.BodyGo)
			{
				Transform oldParent = Owner.property.weapon[0].transform.parent;
				Owner.property.weapon[0].transform.parent = Owner.BodyGo.transform.parent;
				Owner.property.weapon[0].transform.localScale = Owner.BodyGo.transform.localScale;
				Owner.property.weapon[0].transform.parent = oldParent;
			}
			else*/
			{
				Owner.property.weapon[0].transform.localScale = data.GetVector3(bindPointName + "@scal") ;
			}
			if (null != Owner.property.weapon[1])
			{
				Owner.property.weapon[1].transform.localScale = Owner.property.weapon[0].transform.localScale;
			}
			if (data.data.ContainsKey("bonesSize"))
			{
				int _len = data.GetInt("bonesSize");
				Transform [] _bones = new Transform[_len]; 
				SkinnedMeshRenderer _rd = Owner.property.weapon[0].GetComponent<SkinnedMeshRenderer>();
				for (int  i = 0 ; i < _len ; i++)
				{
					//mapdata.SetString("bone"+j,srd.bones[j].name);
					string _nam = data.GetString("bone"+i);
					Transform t2 =  Owner.GetChildTransform(_nam);
					_bones[i] = t2;
				}
				_rd.bones = _bones;
				_rd.rootBone = t;
				_len = data.GetInt("bindPosesSize");
				Matrix4x4 [] ms = new Matrix4x4[_len]; 
				for (int  j = 0 ; j < _len ; j++)
				{
					Matrix4x4 _m = new Matrix4x4();
					_m.SetColumn(0,data.GetVector4("bindPose0"+j));
					_m.SetColumn(1,data.GetVector4("bindPose1"+j));
					_m.SetColumn(2,data.GetVector4("bindPose2"+j));
					_m.SetColumn(3,data.GetVector4("bindPose3"+j));
					ms[j] = _m;
				}
				_rd.sharedMesh.bindposes = ms;
				
				/*_len = data.GetInt("boneWeightsSize");
				BoneWeight []_ws = new BoneWeight[_len];
				for (int  j = 0 ; j < _len ; j++)
				{
					BoneWeight _w = new BoneWeight();
					_w.boneIndex0 = data.GetInt("boneIndex0"+j);
					_w.weight0 = data.GetFloat("boneW0"+j);
					
					_w.boneIndex1 = data.GetInt("boneIndex1"+j);
					_w.weight1 = data.GetFloat("boneW1"+j);
					
					_w.boneIndex2 = data.GetInt("boneIndex2"+j);
					_w.weight2 = data.GetFloat("boneW2"+j);
					
					_w.boneIndex3 = data.GetInt("boneIndex3"+j);
					_w.weight3 = data.GetFloat("boneW3"+j);
					_ws[j] = _w;
				}
				rd.sharedMesh.boneWeights = _ws;*/
				
				
			}
		}
		public override void DoUpdate()
        {
			if (null != trail1)
			{
				trail1.showTrial = showWeaponTrail;	
			}
			if (null != trail2)
			{
				trail2.showTrial = showWeaponTrail;	
			}
			
			if (null != assetInfo && assetInfo.isDone(true))
			{
				EditorObjectData inf = assetInfo.subRequest.asset as EditorObjectData;
				data = new EditorDataMap();
				data.Load(inf.contain);
				Owner.property.weapon[0] = GameObject.Instantiate( assetInfo.bundle.mainAsset) as GameObject;
				Owner.property.weapon[0].layer = 10;	
				
				if (data.data.ContainsKey("wuqi_L@pos"))
				{
					Owner.property.weapon[1] = GameObject.Instantiate( assetInfo.bundle.mainAsset) as GameObject;
					Owner.property.weapon[1].layer = 10;
				}
				SetWeaponPosition(bindPoint,true);
				LoadTrail();
				LoadTrail2();
				assetInfo = null;
			}
			else if (null != Owner.property.weapon[0] && null != fxInfo && fxInfo.isDone())
			{
				GameObject fxobj = GameObject.Instantiate( fxInfo.bundle.mainAsset) as GameObject;
				fxobj.transform.parent = Owner.property.weapon[0].transform;
				fxobj.transform.localPosition = Vector3.zero;
				fxobj.transform.localScale = new Vector3(100f,100f,100f);
				fxobj.transform.localRotation  = Quaternion.identity;
				fxInfo = null;
			}
		}
		public void LoadTrail()
		{
			
			tuowei1_1 = Owner.property.weapon[0].transform.Find("tuowei1");
			tuowei1_2 = Owner.property.weapon[0].transform.Find("tuowei2");
			if (null != tuowei1_1 && null != tuowei1_2)
			{
				trail1 = tuowei1_1.gameObject.AddComponent<MyTrail>();
				trail1.p1 = tuowei1_1;
				trail1.p2 = tuowei1_2;
				//trail.time = 80;
				trail1.mat = WeaponTrailLoader.trailMat;
				KHeroSetting heroSetting = KConfigFileManager.GetInstance().heroSetting.getData(Owner.TabID.ToString());
				if (null != heroSetting)
				{
					trail1.time = heroSetting.WeaponTrailTime;
					trail1.startColor =  KingSoftCommonFunction.StringToColor(heroSetting.WeaponTrailBeginColor);
					trail1.endColor = KingSoftCommonFunction.StringToColor(heroSetting.WeaponTrailEndColor);
				}
			}
		}
		public void LoadTrail2()
		{
			if (null == Owner.property.weapon[1])
				return;
			tuowei2_1 = Owner.property.weapon[1].transform.Find("tuowei1");
			tuowei2_2 = Owner.property.weapon[1].transform.Find("tuowei2");
			if (null != tuowei2_1 && null != tuowei2_2)
			{
				trail2 = tuowei2_1.gameObject.AddComponent<MyTrail>();
				trail2.p1 = tuowei2_1;
				trail2.p2 = tuowei2_2;
				trail2.mat = WeaponTrailLoader.trailMat;
				KHeroSetting heroSetting = KConfigFileManager.GetInstance().heroSetting.getData(Owner.TabID.ToString());
				if (null != heroSetting)
				{
					trail2.time = heroSetting.WeaponTrailTime;
					trail2.startColor =  KingSoftCommonFunction.StringToColor(heroSetting.WeaponTrailBeginColor);
					trail2.endColor = KingSoftCommonFunction.StringToColor(heroSetting.WeaponTrailEndColor);
				}
			}
		}
	}
}


