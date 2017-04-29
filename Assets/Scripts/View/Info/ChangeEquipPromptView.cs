using System;
using Assets.Scripts.View.UIDetail;
using Assets.Scripts.Manager;
using UnityEngine;
using Assets.Scripts.Logic.Item;
using Assets.Scripts.Logic.Bag;
using Assets.Scripts.Define;
using Assets.Scripts.Data;
using Assets.Scripts.Lib.Loader;
using System.Collections.Generic;
using Assets.Scripts.Utils;

namespace Assets.Scripts.View.Info
{
	public class ChangeEquipPromptView : ChangeEquipPromptUIDetail
	{
		protected int equipIndex = 0;
		protected int equipStrengthenLevel = 1;
		protected int equipBagPos = 0;
		protected int equipLoadPos = 0;
		protected KAttributeType attributeType = KAttributeType.atInvalid;
		protected int attributeValue = 0;
		protected int extValue = 0;
		protected int capacityValue = 0;
		
		public ChangeEquipPromptView ()
			: base(320, 188)
		{
		}
		
		protected override void PreInit()
        {
            base.PreInit();
        }

        protected override void Init()
        {
            base.Init();
			UIEventListener.Get(CloseButton.gameObject).onClick += OnClickClose;
			UIEventListener.Get(ChangeButton.gameObject).onClick += OnClickChange;
			UIAtlas atlas = UIAtlasManager.GetInstance().GetUIAtlas("IconAtlas");
            EquipSprite.atlas = atlas;
            EquipSprite.type = UISprite.Type.Sliced;
            EquipSprite.spriteName = "ItemBg";
			UpdateEquipInfo();
        }
		
		private void OnClickClose(GameObject go)
        {
			Hide();
		}
		
		private void OnClickChange(GameObject go)
		{
			ChangeEquip();
			Hide();
		}
		
		private void ChangeEquip()
		{
			//先检测背包位置是不是原来的道具...
			ItemInfo itemInfo = BagLogic.GetInstance().GetItemByPos(equipBagPos);
			if (itemInfo != null && itemInfo.typeId == equipIndex)
			{
				BagLogic.GetInstance().LoadEquip(equipBagPos, equipLoadPos);
			}
		}
		
		private void UpdateEquipInfo()
		{
			if (EquipSprite != null)
			{
				KTabLineEquip equipData = KConfigFileManager.GetInstance().equipTabInfos.getData(equipIndex.ToString());
				if (equipData!=null)
				{
					EquipSprite.spriteName = equipData.Icon;
				}
			}
			if (EquipAttributeLbel != null)
			{
				string attributeText = Util.GetAttributeText(attributeType);
				EquipAttributeLbel.text = attributeText + "：" + attributeValue.ToString() + "<01f700>(+" + extValue.ToString() + ")";
			}
			
			if (FightingCapacityNumber != null)
			{
				FightingCapacityNumber.text = capacityValue.ToString();
			}
		}
		
		public void SetEquipInfo(EquipInfo itemInfo)
		{
			equipIndex = itemInfo.typeId;
			equipStrengthenLevel = itemInfo.CurStrengthenLv;
			equipBagPos = itemInfo.Position;
			equipLoadPos = itemInfo.PutWhere;
			attributeType = KAttributeType.atInvalid;
			attributeValue = 0;
			extValue = 0;
			capacityValue = 0;
			
			string dataKey = equipIndex.ToString() + "_" + equipStrengthenLevel.ToString();
			KEquipStrengthen equipData = KConfigFileManager.GetInstance().equipStrengthenTab.getData(dataKey);
			if (equipData != null)
			{
				foreach(KeyValuePair<KAttributeType , int> tempData in equipData.AttributeDict)
				{
					attributeType = tempData.Key;
					attributeValue = tempData.Value;
					break;
				}
				capacityValue =  Util.GetFightCalculate(equipData.AttributeDict);
			}
			UpdateEquipInfo();
		}
		
	}
}

