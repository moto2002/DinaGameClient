using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.View.UIDetail;
using Assets.Scripts.Logic.Intensify;
using Assets.Scripts.Logic.Item;
using Assets.Scripts.Data;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.Utils;
using Assets.Scripts.Define;

namespace Assets.Scripts.View.Intensify
{
	public class EquipSuitTipView : SuitTipsUIDetail
	{
		List<ItemInfo> item;
		private bool bInited = false;
		private List<int> levelList = new List<int>();
		private int number = 1;
		
		private int level = 1;
		
		private KTabEquipSuit equipSuittab;
		
		private List<GameObject> canUseItemList;
		
		private List<GameObject> canUseEquipNameList;
		
		private List<string> properTitelList = new List<string>();
		private List<int> propertyValueList = new List<int>();
		
		private List<string> levelNameList = new List<string>();
		
		private KEquipStrengthen equipTab;
		
//		protected KAttributeType attributeType = KAttributeType.atInvalid;
		
		public EquipSuitTipView()
            : base(50, 50)
        {
        }
		
		protected override void Init ()
		{
			base.Init ();
			bInited = true;
            if (item != null)
                showTips(item);
		}
		public void showTips (List<ItemInfo> roleEquip)
		{
			int max;
			IntensifyLogic logic = IntensifyLogic.GetInstance();
			item = roleEquip;
            if (!bInited)
                return;
			if(item.Count <= 0 )
				return;
			level = IntensifyLogic.GetInstance().GetStepLevel(GetEquip(0).CurStrengthenLv);
			for(int i=0;i<item.Count;i++)
			{
				number = IntensifyLogic.GetInstance().GetStepLevel(GetEquip(i).CurStrengthenLv);
				if(number<=level)
				{
					level = number;
				}
				max = logic.maxStrngthenLevList[logic.GetStepLevel(GetEquip(i).CurStrengthenLv)-1];
				equipTab = KConfigFileManager.GetInstance().GetForgeEquipStrengthen(GetEquip(i).ID, max + 1);
				levelNameList.Add(equipTab.EquipName);
			}
			ShowEquipName(levelNameList.Count);
			equipSuittab = KConfigFileManager.GetInstance().GetEquipSuitTab(level);
			ShowProperty(equipSuittab.AttributeDict3);
			UpdatePosition();
			levelNameList.Clear();
			
		}
		
		private void ShowProperty(Dictionary<KAttributeType , int> attribute)
		{
			foreach(KeyValuePair<KAttributeType , int > dict in attribute)
			{
				properTitelList.Add(Util.GetAttributeText(dict.Key));
				propertyValueList.Add(dict.Value);
			}
			ShowItems(attribute.Count);
			properTitelList.Clear();
			propertyValueList.Clear();
		}
		
		private void ShowEquipName (int itemCount)
		{
			canUseEquipNameList = ItemListTool.GetCanUseItemList(EquipList.gameObject,"Equip");
			for(int i=0;i<itemCount;i++)
			{
				GameObject go =	ItemListTool.GetNewItemObj(canUseEquipNameList , EquipList.gameObject , Equip0.gameObject);
				go.transform.name = "Equip" + i.ToString();
				go.GetComponent<UILabel>().text = levelNameList[i];
			}
			EquipList.Reposition();
			ItemListTool.ActiveUnuseItem(canUseEquipNameList);
			float addPosition;
			Vector3 listPos = AttributeList.transform.position;
			addPosition = EquipList.cellHeight * EquipList.gameObject.transform.childCount;
			listPos.y = AttributeList.transform.position.y - addPosition;
			AttributeList.transform.localPosition = listPos;
		}
		
		public void ShowItems (int itemCount)
		{
			canUseItemList = ItemListTool.GetCanUseItemList(AttributeList.gameObject,"Attribute");
			for(int i=0;i<itemCount;i++)
			{
				GameObject go =	ItemListTool.GetNewItemObj(canUseItemList , AttributeList.gameObject , Attribute0.gameObject);
				go.transform.name = "Attribute" + i.ToString();
				go.GetComponent<UILabel>().text = properTitelList[i] + ":" + propertyValueList[i];
			}
			AttributeList.Reposition();
			ItemListTool.ActiveUnuseItem(canUseItemList);
		}
		
		private EquipInfo GetEquip(int index)
		{
			return item[index] as EquipInfo;
		}
		
		private int GetEquipFormRoleCount ()
		{
			return item.Count;
		}
		
		private void UpdatePosition ()
		{
			Vector3 mPos = Input.mousePosition;
			mPos.x = Mathf.Clamp01(mPos.x / Screen.width);
			mPos.y = Mathf.Clamp01(mPos.y / Screen.height);
			Panel.transform.position = UICamera.currentCamera.ViewportToWorldPoint(mPos);
		}          
		
		public void Close ()
		{
			Hide();
		}

	}
}

