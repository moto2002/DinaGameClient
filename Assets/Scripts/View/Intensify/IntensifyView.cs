using System;
using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.View.UIDetail;
using Assets.Scripts.Utils;
using Assets.Scripts.Manager;
using Assets.Scripts.View.Drag;
using Assets.Scripts.Logic.Item;
using Assets.Scripts.Controller;
using Assets.Scripts.Logic.Scene;
using Assets.Scripts.Define;
using Assets.Scripts.View;
using Assets.Scripts.UIComponent;

/************************************************************************/
/* 装备强化
 * author@linfeng*/
/************************************************************************/
namespace Assets.Scripts.View.Intensify
{
	public class IntensifyView : IntensifyUIDetail
	{
		public const string FORGE_UPDATE_EQUIP_STENG = "FORGE_UPDATE_EQUIP_STENG";
		
		public static int TOTALEQUIP = 12;
		
		private UISprite itemIcon = null;

		public List<IntensifyItem> itemList;
		public List<EquipAttribute> attributeList;

		private string wrapStr = "\n";
		private string greenColorStr = "<8ad931>";
		private string yelloColorStr = "<e4aa44>";
		private string redColorStr = "<FF0000>";
		private string colorEnd = "<->";
		private string colorText = "";

		private int i;
		private int itemIndex = -1;

		private List<GameObject> canUsePropertyList;
		private List<GameObject> canUseItemList;
		
		private ThumbnailView thumView;
		
		public IntensifyView () :base(400 , 400)
		{
			
		}
		
		protected override void Init ()
		{
			base.Init ();
			itemList = new List<IntensifyItem>();
			attributeList = new List<EquipAttribute>();
			UpSprite.gameObject.SetActive(false);
			DrawThum();
		}
		
		private void DrawThum ()
		{
			thumView =  Panel.gameObject.AddComponent<ThumbnailView>();
			thumView.CreateThumbnailView(Panel.gameObject , new Vector3(0f,0f,0) , new Vector3(146f ,382f , 0f) , EquipBigSprite);
		}
		
		public void ShowItems (int itemCount)
		{
			itemList.Clear();
			canUseItemList = ItemListTool.GetCanUseItemList(GridItem.gameObject,"Item");
			for(i=0; i<itemCount; i++)
			{
				GameObject go =	ItemListTool.GetNewItemObj(canUseItemList , GridItem.gameObject , Item0.gameObject);
				go.transform.name = "Item" + i.ToString();
				IntensifyItem itemView;
				if(go.GetComponent<IntensifyItem>() == null)
					itemView = go.AddComponent<IntensifyItem>();
				else
					itemView = go.GetComponent<IntensifyItem>();
				itemView.Init();
				itemList.Add(itemView);
			}
			GridItem.Reposition();
			ItemListTool.ActiveUnuseItem(canUseItemList);
		}
		
		public void ShowPropertys (int propertyCount)
		{
			attributeList.Clear();
			canUsePropertyList = ItemListTool.GetCanUseItemList(AttributeList.gameObject,"Attribute");
			for(i=0;i<propertyCount;i++)
			{
				GameObject go =	ItemListTool.GetNewItemObj(canUsePropertyList , AttributeList.gameObject , Attribute0.gameObject);
				go.transform.name = "Attribute" + i.ToString();
				EquipAttribute attriView;
				if(go.GetComponent<EquipAttribute>() == null)
					attriView = go.AddComponent<EquipAttribute>();
				else
					attriView = go.GetComponent<EquipAttribute>();
				attriView.Init();
				attributeList.Add(attriView);
			}
			AttributeList.Reposition();
			ItemListTool.ActiveUnuseItem(canUsePropertyList);
		}
		
		public void SetPropertyData (List<KAttributeType> propertyName , List<int> propertyValue)
		{
			for(i=0;i<propertyName.Count;i++)
			{
				attributeList[i].SetPropertyData(Util.GetAttributeText(propertyName[i]) + propertyValue[i]);
			}
		}

		public void SetNextPropertyData (List<KAttributeType> propertyName,List<int> propertyValue,List<KAttributeType> propertyNextName , List<int> propertyNextValue)
		{
			for(i=0;i<propertyNextName.Count;i++)
			{
				if(propertyName.Count-1 <i)
				{
					attributeList[i].SetPropertyData(Util.GetAttributeText(propertyNextName[i]) + "0");
					attributeList[i].SetNextPropertyData(greenColorStr + propertyNextValue[i] + colorEnd);
				}
				else
					attributeList[i].SetNextPropertyData(greenColorStr + (propertyNextValue[i] - propertyValue[i]) + colorEnd);
			}
		}
		
		public void SetMaxPropertyData (List<int> propertyValue)
		{
			for(i=0;i<propertyValue.Count;i++)
			{
				attributeList[i].SetMaxPropertyData(yelloColorStr + "最大" + propertyValue[i] + colorEnd);
			}
		}
		
		protected override void InitEvent ()
		{
			base.InitEvent ();
			EventDispatcher.GameWorld.Dispath(FORGE_UPDATE_EQUIP_STENG ,new object());
		}
		private UISprite itemClick;
		public void SelectItem (int index)
		{
			if(index >= 0)
			{
				if(itemIndex >= 0)
				{
					itemIcon = itemList[itemIndex].Bg;
					itemClick = itemList[itemIndex].clickBg;
					itemIcon.enabled = true;
					itemClick.enabled = false;
				}
				itemIcon = itemList[index].Bg;
				itemClick = itemList[index].clickBg;
				itemIcon.enabled = false;
				itemClick.enabled = true;
				itemIndex = index;	
			}
		}
		
		public void ClearAllItem()
		{
			for(i=0;i<itemList.Count;i++)
			{
				itemClick = itemList[i].clickBg;
				itemClick.enabled = false;
			}
			if(itemIndex > 0)
			{
				itemIcon = itemList[itemIndex].Bg;
				itemIcon.enabled = true;
			}
			itemIndex = -1;
		}
		
		public void SetLeveltextData(int CurStrengthenLv, int needRoleLevel, byte level)
		{
			CurrentLevelText.text = "当前强化等级" + CurStrengthenLv;
			NeedLevelText.text = SetText("需要等级 : ",needRoleLevel,needRoleLevel>=level);
		}

        public void SetNeedMaterialData(string itemName, int itemCount , string bagName , int bagNum)
		{
            NeedMaterialText.text = "需要材料 ： " + itemName + " *" + itemCount;
			HaveMaterialText.text = SetText("背包材料 ：" + bagName + " *" ,bagNum , bagNum>=itemCount);
		}
		
		public void SetNeedMaterialStrengthData(int count , int bagCount)
		{
			NeedMaterialText.text = "需要材料 : 银两" + count;
			HaveMaterialText.text = SetText("背包材料 : 银两" , bagCount, bagCount>= count);
		}
		/// <summary>
		/// 设置文字
		/// </summary>
		/// <returns>
		/// 字符串
		/// </returns>
		/// <param name='title'>
		/// 头文字
		/// </param>
		/// <param name='number'>
		/// 数值
		/// </param>
		/// <param name='isCondition'>
		/// 颜色（TRUE 绿  FALSE 红）
		/// </param>
		private string SetText ( string title , int number , bool isCondition = true )
		{
			string values = "";
			if(isCondition)
				colorText = greenColorStr;
			else
				colorText = redColorStr;
			values = title + colorText + number + colorEnd;
			return values;
		}
	
		public void clearUI ()
		{
			CurrentLevelText.text = null;
			GetMaterialBtn.text = null;
			SoulText.text = null;
			NeedMaterialText.text = null;
			HaveMaterialText.text = null;
			NeedLevelText.text = null;
		}
		
		public void ClearThum()
		{
//			Object.DestroyObject(thumView.GetCamera());
		}
		
		

	}
}

