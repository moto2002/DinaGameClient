using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Logic.Item;
using Assets.Scripts.Data;
using Assets.Scripts.Logic.Bag;
using Assets.Scripts.Manager;
using Assets.Scripts.View.Intensify;
using Assets.Scripts.Logic.RemoteCall;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.View.Drag;
using Assets.Scripts.Model.Player;
using Assets.Scripts.Define;
using Assets.Scripts.Utils;
using Assets.Scripts.UIComponent;

namespace Assets.Scripts.Logic.Intensify
{
	/// <summary>
	/// 锻造  by linfeng 2014.3.10
	/// </summary>
	public class IntensifyLogic : BaseLogic
	{
		public List<int> maxStrngthenLevList = new List<int>();
		public List<int> minStrngthenLevList = new List<int>();
		
		private List<ItemInfo> roleEquip;
		private BagLogic blogic;
		private IntensifyView ui = null;
		
		private int selectIndex = -1;//选中的索引
		private int recordIndex = -1;//记录要强化的装备
		private int i = 0;
		
		private KEquipStrengthen equipTab;
		private KEquipStep steptab;
		private KTabServerEquip serverEquipTab;
		
		private List<KAttributeType> propertyNameList;
		private List<int> curPropertyList;
		private List<KAttributeType> propertyNextNameList;
		private List<int> nextPropertyList;
		private List<int> maxPropertyList;
		
		private bool isStrengthen = false;

        private static IntensifyLogic instance;
		public static IntensifyLogic GetInstance()
        {
            if (instance == null)
                instance = new IntensifyLogic();
            return instance;
        }
		protected override void Init()
   	 	{
			roleEquip = new List<ItemInfo>();
			propertyNameList = new List<KAttributeType>();
			curPropertyList = new List<int>();
			propertyNextNameList = new List<KAttributeType>();
			nextPropertyList = new List<int>();
			maxPropertyList = new List<int>();
    	}
		protected override void InitListeners ()
		{
			base.InitListeners ();
			EventDispatcher.GameWorld.Regist(IntensifyView.FORGE_UPDATE_EQUIP_STENG,IntensifyUICompelethandler);
		}
		/// <summary>
		/// UI完成
		/// </summary>
		private object IntensifyUICompelethandler(params object[] objs)
		{
			UIEventListener.Get(ui.IntensifyBtn.gameObject).onClick += OnIntensifyClickHandler;
			UIEventListener.Get(ui.CloseBtn.gameObject).onClick += OnClosePanelHandler;
			UpdataIntensify();
			CreateSoulDrag();
			CreateItemDrag();
			return null;
		}
		
		public void GetEquipFormRole ()
		{
			blogic = BagLogic.GetInstance();
			EquipInfo info;
			for(i = 0;i<12;i++)
			{
				if(blogic.GetEquipByPos(i) != null)
				{
					info =  blogic.GetEquipByPos(i) as EquipInfo;
					roleEquip.Add(info);
				}
			}
		}
		
		private void SetRoleEquipOnUI ()
		{
			for(i=0;i<roleEquip.Count;i++)
			{
				SetEquipItemData(i,roleEquip[i] as EquipInfo);
			}
		}
		private void CreateItemDrag ()
		{
			for(int i = 0; i<roleEquip.Count;i++)
			{
				ui.itemList[i].drag.DragItemVO = roleEquip[i];
			}
		}
		
		private void CreateSoulDrag ()
		{
			DragItem drag = null;
			drag = ui.Soul.gameObject.AddComponent<DragItem>();
            drag.DragIcon = ui.Soul;
            drag.ToolTipEvent += OnShowSuitTipHandler;
		}
		
		private void SetEquipItemData (int index , EquipInfo info)
		{
			ui.itemList[index].SetItemData(info.Name, info.CurStrengthenLv, info.Icon);
		}
		
		private void ShowItems ()
		{
			ui.ShowItems(roleEquip.Count);
			for(i = 0; i<ui.itemList.Count;i++)
			{
				UIEventListener.Get(ui.itemList[i].gameObject).onClick += OnSelectEquipItemHandler;
			}
		}
		
		private void OnIntensifyClickHandler(GameObject go)
		{
			recordIndex = selectIndex;
            if (GetEquipInfo().CurStrengthenLv >= maxStrngthenLevList[maxStrngthenLevList.Count-1])
			{
                Debug.Log("已经强化到最高");
			}
            else
			{
			    RemoteCallLogic.GetInstance().CallGS("OnEquipStrengthen",GetEquipInfo().PutWhere);
			}
		}
		private void OnClosePanelHandler(GameObject go)
		{
			ui.ClearAllItem();
			ui.ClearThum();
			ui.clearUI();
			ui.Hide();
		}
		
		private void OnShowSuitTipHandler(bool isShow)
		{
			if(isShow)
			{
				TooltipsManager.GetInstance().ShowEquipSuitTip(roleEquip);
			}
			else
			{
				TooltipsManager.GetInstance().Hide();
			}
		}

		private void OnSelectEquipItemHandler(GameObject go)
		{
			selectIndex = int.Parse( go.name.Remove(0,4) );
			ShowSelectItemProperty(selectIndex);
		}
		
		private void ShowSelectItemProperty(int index = 0 ,bool isIntensify = false)
		{
			if(roleEquip.Count <= 0 )
				return ;
			if(isIntensify)
			{
				for(i=0;i<roleEquip.Count;i++)
				{
					if(index == (roleEquip[i] as EquipInfo).PutWhere)
					{
						selectIndex = i;
						break;
					}
					else
					{
						selectIndex = index;
					}
				}
				ui.ClearAllItem();
			}
			else
			{
				selectIndex = index;
				ReadEquipModeltab();
			}
			ui.SelectItem(selectIndex);
			SetUpPropertyWithItem();
			SetLeveltextData();
			ChangeBtnText();
		}
		
		private void SetUpPropertyWithItem ()
		{
			GetItemPropertyData();
			ui.ShowPropertys(propertyNextNameList.Count);
			SetPropertyData();
			ClearPropertyData();
		}

		private void GetItemPropertyData ()
		{
			GetPropertyByCurLevel(GetEquipInfo().ID , GetEquipInfo().CurStrengthenLv);
			GetPropertyByNextLevel(GetEquipInfo().ID , GetEquipInfo().CurStrengthenLv+1);
			int maxlevel = maxStrngthenLevList[GetStepLevel(GetEquipInfo().CurStrengthenLv)-1];
            GetPropertyByMaxLevel(GetEquipInfo().ID, maxlevel);
		}
		private void GetPropertyByCurLevel (int equipID , int equipLevel)
		{
			equipTab = KConfigFileManager.GetInstance().GetForgeEquipStrengthen(equipID,equipLevel);
			foreach(KeyValuePair<KAttributeType , int > dict in equipTab.AttributeDict)
			{
				propertyNameList.Add(dict.Key);
				curPropertyList.Add(dict.Value);
			}
		}
		private void GetPropertyByNextLevel (int equipID , int equipLevel)
		{
			equipTab = KConfigFileManager.GetInstance().GetForgeEquipStrengthen(equipID,equipLevel);
			foreach(KeyValuePair<KAttributeType , int > dict in equipTab.AttributeDict)
			{
				propertyNextNameList.Add(dict.Key);
				nextPropertyList.Add(dict.Value);
			}
		}
		private void GetPropertyByMaxLevel (int equipID , int equipLevel)
		{
			equipTab = KConfigFileManager.GetInstance().GetForgeEquipStrengthen(equipID,equipLevel);
			foreach(KeyValuePair<KAttributeType , int > dict in equipTab.AttributeDict)
			{
				maxPropertyList.Add(dict.Value);
			}
		}
		private void SetPropertyData()
		{
			ui.SetPropertyData(propertyNameList,curPropertyList);
			ui.SetNextPropertyData(propertyNameList,curPropertyList,propertyNextNameList,nextPropertyList);
			ui.SetMaxPropertyData(maxPropertyList);
		}
		private void ClearPropertyData ()
		{
			propertyNameList.Clear();
			curPropertyList.Clear();
			propertyNextNameList.Clear();
			nextPropertyList.Clear();
			maxPropertyList.Clear();
		}
		private void ReadEquipModeltab()
		{
			EquipManager.GetInstance().LoadEquipModel(GetEquipInfo().PutWhere , GetEquipInfo().FBX , ui.Panel.gameObject);
		}
		
		private void SetLeveltextData ()
		{
			MajorPlayer player = PlayerManager.GetInstance().MajorPlayer;
			ui.SetLeveltextData(GetEquipInfo().CurStrengthenLv, ReadStepTab().nNeedLevel, player.level);
		}
		//入口
		public void UpdataIntensify (int index = 0 , bool isIntensify = false)
		{
			if(isIntensify)
			{//强化成功
				if(isStrengthen)
				{
					AlertWindowManager window = AlertWindowManager.GetInstance();
					window.AlertEquipWindow(GetEquipInfo() , 6 , alertCallBack);
					ui.IntensifyBtn.isEnabled = false;
					isStrengthen = false;
				}
				else
				{
					Debug.Log("强化完成");
				}
			}
			clearAll();
			GetEquipFormRole();
			if(roleEquip.Count <= 0)
			{
				ui.Hide();
				return ;
			}
			ShowItems();
			ui.ShowItems(roleEquip.Count);
			SetRoleEquipOnUI();
			ui.ClearAllItem();
			ShowSelectItemProperty(index , isIntensify);
		}
		
		private void alertCallBack()
		{
			ui.IntensifyBtn.isEnabled = true;
		}
		
		private void ChangeBtnText ()
		{
			if(GetEquipInfo().CurStrengthenLv == ReadStepTab().nMaxStrengthenLev)
			{
				isStrengthen = true;
				ui.UpSprite.gameObject.SetActive(true);
				ui.IntensifySprite.gameObject.SetActive(false);
				SetNeedMaterialData();
			}
			else
			{
				isStrengthen = false;
				ui.UpSprite.gameObject.SetActive(false);
				ui.IntensifySprite.gameObject.SetActive(true);
				SetNeedMaterialStrengthData();
			}
		}
		
		private void SetNeedMaterialData ()
		{
			string itemName = ""; 
            int itemCount = 0;
            if (ReadStepTab().nItemIndex != 0)
            {
                KTabLineItem otherItem = ItemLocator.GetInstance().GetOtherItem(ReadStepTab().nItemIndex);
                itemName = otherItem.Name;
                itemCount = ReadStepTab().nItemCount;
            }
			ItemInfo info;
			info = GetGoodFormBag(itemName);
			if(info != null)
        		ui.SetNeedMaterialData(itemName, itemCount ,info.Name , info.CurNum);
			else
				ui.SetNeedMaterialData(itemName, itemCount ,itemName , 0);
		}
		
		private void SetNeedMaterialStrengthData ()
		{
			MajorPlayer player = PlayerManager.GetInstance().MajorPlayer;
            ui.SetNeedMaterialStrengthData(ReadStrengthenTab().nMoney, player.money);
		}
		
		public void InitIntensifyUI ()
		{
            if (ui == null)
            {
                ui = new IntensifyView();
            }
            else
            {
                ui.Show();
                UpdataIntensify();
            }
		}
		/// <summary>
		/// 获得强化的UI
		/// </summary>
		/// <returns>
		/// The intensift U.
		/// </returns>
		public IntensifyView GetIntensifyUI ()
		{
			return ui;
		}
		
		private void clearAll()
		{
			ui.clearUI();
			roleEquip.Clear();
			selectIndex = -1;
			recordIndex = -1;
			for(i = 0; i<ui.itemList.Count;i++)
			{
				UIEventListener.Get(ui.itemList[i].gameObject).onClick -= OnSelectEquipItemHandler;
			}
		}
		private KEquipStep ReadStepTab()
        {
            steptab = KConfigFileManager.GetInstance().GetForgeEquipStep(GetStepLevel(GetEquipInfo().CurStrengthenLv));
            return steptab;
        }
        private KEquipStrengthen ReadStrengthenTab()
        {
            equipTab = KConfigFileManager.GetInstance().GetForgeEquipStrengthen(GetEquipInfo().ID, GetEquipInfo().CurStrengthenLv);
            return equipTab;
        }
		private KTabServerEquip ReadServerEquipTab ()
		{
			serverEquipTab = KConfigFileManager.GetInstance().GetEquipServerTab(GetEquipInfo().ID);
			return serverEquipTab;
		}
		
		private EquipInfo GetEquipInfo()
		{
			return roleEquip[selectIndex] as EquipInfo;
		}
		
		public int GetStepLevel(int level)
		{
			for(i=0;i<maxStrngthenLevList.Count;i++)
			{
				if(minStrngthenLevList[i] <= level && level <= maxStrngthenLevList[i])
				{
					return i+1;
				}
			}
			return 0;
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

		public ItemInfo GetGoodFormBag (string name)
		{
			blogic = BagLogic.GetInstance();
			ItemInfo info;
			for(i=0;i<blogic.bagItems.Length;i++)
			{
				info = blogic.GetItemByPos(i);
				if(info != null && info.Name == name)
					return info;
			}
			return null;
		}
	}

}