using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Data;
using Assets.Scripts.Logic.Item;
using Assets.Scripts.Logic.Bag;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.Utils;
using Assets.Scripts.UIComponent;
using Assets.Scripts.Model.Player;

namespace Assets.Scripts.Manager 
{
	public class EquipManager
	{
		public const int EQUIPCOUNT = 12;
		
		private List<ItemInfo> roleEquip;
		private BagLogic blogic;
		private int equipModelKey = 0;
		private int selectKey = 0;
		private string equipName;

		private GameObject panel;
		
		private List<int> maxStrngthenLevList = new List<int>();
		private List<int> minStrngthenLevList = new List<int>();
		private Dictionary<int, GameObject> loadModelList = new Dictionary<int, GameObject>();

		public void LoadEquipModel (int key , string equipModelName , GameObject panel)
		{
			this.equipName = equipModelName;
			this.equipModelKey = key;
			this.panel = panel;
			if (loadModelList.ContainsKey(equipModelKey))
            {
				DestroyModel();
				CreateView(loadModelList[equipModelKey]);
				return;
			}
			AssetLoader.GetInstance().Load(URLUtil.GetEquipModelPath(equipModelName) , LoadModelComplete , AssetType.BUNDLER);
		}
		
		private void DestroyModel()
		{
			if(loadModelList.Count > 0)
			{
				loadModelList[selectKey].SetActive(false);
			}
			selectKey = equipModelKey;
		}
		
		private void LoadModelComplete(AssetInfo info)
		{
			DestroyModel();
			GameObject modelObject = GameObject.Instantiate(info.bundle.mainAsset) as GameObject;
			modelObject.renderer.material.shader = Shader.Find("Kingsoft/DiffuseCullOff");
			modelObject.layer = LayerMask.NameToLayer("3DUI");
			modelObject.name = equipName;
			modelObject.transform.localPosition = (info.bundle.mainAsset as GameObject).transform.localPosition;
			loadModelList.Add(equipModelKey , modelObject);
			CreateView(modelObject);
			
		}
		
		private void CreateView (GameObject go)
		{
			loadModelList[equipModelKey].SetActive(true);
			panel.GetComponent<ThumbnailView>().StartRotate(go , -100);
		}
		
		public void GetEquipStepTab ()
		{
			
		}
		
		public void GetEquipFromRole ()
		{
			blogic = BagLogic.GetInstance();
			EquipInfo info;
			for(int i = 0 ; i<EQUIPCOUNT ; i++)
			{
				if(blogic.GetEquipByPos(i) != null)
				{
					info =  blogic.GetEquipByPos(i) as EquipInfo;
					roleEquip.Add(info);
				}
			}
		}
		
		public List<ItemInfo> GetEquipListFromRole ()
		{
			return roleEquip;
		}
		/// <summary>
		/// Sets the equip on role.
		/// </summary>
		/// <param name='SubType'>
		/// 装备的位置
		/// </param>
		public EquipInfo SetEquipOnRole (int SubType)
		{
			foreach(EquipInfo info in roleEquip)
			{
				if(info.PutWhere == SubType)
				{
					return info;
				}
			}
			return null;
		}
		
		public void GetEquipFromBag ()
		{
			
		}
		
		public int GetEquipStepLevel (int level)
		{
			for(int i=0;i<maxStrngthenLevList.Count;i++)
			{
				if(minStrngthenLevList[i] <= level && level <= maxStrngthenLevList[i])
				{
					return i+1;
				}
			}
			return 0;
		}
		
		public void GetNextSuitName ()
		{
			
		}
		
		public void ChangeEquipOnRole ()
		{
			
		}
		
		public void GetSuitLevel ()
		{
			
		}
		
		public void EquipStepComplete()
		{
			Dictionary<string , KEquipStep> stepList = KConfigFileManager.GetInstance().equipSteptab.getAllData();
			foreach(KeyValuePair<string , KEquipStep> dict in stepList)
			{
				maxStrngthenLevList.Add(dict.Value.nMaxStrengthenLev);
				minStrngthenLevList.Add(dict.Value.nMinStrengthenLev);
			}
		}
		
		private static EquipManager instance;
        public static EquipManager GetInstance()
        {
            if (instance == null)
                instance = new EquipManager();
            return instance;
        }
	}
}


