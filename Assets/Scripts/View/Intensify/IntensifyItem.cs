using UnityEngine;
using System.Collections;
using Assets.Scripts.View.UIDetail;
using Assets.Scripts.Manager;
using Assets.Scripts.View.Drag;
using Assets.Scripts.Logic.Item;

namespace Assets.Scripts.View.Intensify
{
	public class IntensifyItem : MonoBehaviour
	{
		private const string DEFAULTATLAS = "IconAtlas";
		public UILabel ItemLevelText = null;
		public UILabel ItemNameText = null;
		public UISprite Bg = null;
		public UISprite Icon = null;
		public UISprite clickBg = null;
		
		public DragItem drag = null;
		
		private UIAtlas atlas;
		
		public void Init ()
		{
			UIComplete();
		}
		
		public void UIComplete()
		{
			ItemLevelText = FindUIObject<UILabel>("ItemLevelText");
			ItemNameText = FindUIObject<UILabel>("ItemNameText");
			Bg = FindUIObject<UISprite>("ItemFrameSprite");
			clickBg = FindUIObject<UISprite>("ItemFrameClickSprite");
			Icon = FindUIObject<UISprite>("EquipIcon");
			if(drag == null)
				drag = gameObject.AddComponent<DragItem>();
			else
				drag = gameObject.GetComponent<DragItem>();
            drag.DragIcon = Bg;
            drag.ToolTipEvent += DoToolTip;
		}
		
		public void SetItemData (string name , int level , string IconName)
		{
			Icon.atlas = GetAtlas(DEFAULTATLAS);
			Icon.spriteName = IconName;
			ItemLevelText.text = level.ToString();
			ItemNameText.text = name;
		}
		
		public void DoToolTip(bool show)
		{
//			if(show &&  drag.DragItemVO != null)
//			{
//				TooltipsManager.GetInstance().ShowEquipTip(drag.DragItemVO as EquipInfo);
//			}
//			else
//			{
//				TooltipsManager.GetInstance().Hide();
//			}
		}
		
		private UIAtlas GetAtlas (string UIAtlasName)
		{
			atlas = UIAtlasManager.GetInstance().GetUIAtlas(UIAtlasName);
			return atlas;
		}
		
		public T FindUIObject<T>(string name) where T : Component
	    {
	        T[] coms = this.GetComponentsInChildren<T>(true);
	        int count = coms.Length;
	        for (int i = 0; i < count; i++)
	        {
	            if (coms[i].gameObject.name.Equals(name))
	            {
	                return coms[i];
	            }
	        }
	        return null;
	    }

	}
}

