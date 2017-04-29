using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Scripts.Manager;
using Assets.Scripts.Logic.Skill;
using Assets.Scripts.View.ButtonBar;

namespace Assets.Scripts.View.MainUI
{
	public class DraggignSkill
	{
		public int  skillIndex;
		public int shorCutIndex = -1;
	}
	public enum SHORTCUTITEM
	{
		NONE,
		SKILL,
		ITEM
	}
	public class ShortCutData
	{
		public int id = 0;
		public SHORTCUTITEM type = SHORTCUTITEM.NONE;
	}
	public class ShortCutDataManager
	{
		public static readonly ShortCutDataManager Instance = new ShortCutDataManager();
		public  int Count 
		{
			get
			{
				return 8;
			}
		}
		public ShortCutData [] datas = null;
		private ShortCutDataManager()
		{
			datas = new ShortCutData[Count];
			for(int i = 0 ; i < Count ; i++)
			{
				datas[i] = new ShortCutData();
			}
		}
	}
    public class ShortcutItem :MonoBehaviour
    {
        public UISprite icon;
		public UISprite mark;
		public UIImageButton button;
        public string mText = "";
		public int index = 0;
		public KeyCode keyCode;
		float cdValue = 0f;
		public SHORTCUTITEM itemType
		{
			get
			{
				return ShortCutDataManager.Instance.datas[index].type;
			}
		}
		public int itemId
		{
			get
			{
				return ShortCutDataManager.Instance.datas[index].id;
			}
		}
       	void Start()
		{
			icon = GetComponent<UISprite>();
			Refreash();
			cdValue = 1f;
			mark.fillAmount = 1f - cdValue;
			UIEventListener.Get(button.gameObject).onClick += ClickItemHandler;
			UIEventListener.Get(button.gameObject).onDrag = OnDrag;
			UIEventListener.Get(button.gameObject).onDrop = OnDrop;
		}
		public void Refreash()
		{
			ShortCutData data = ShortCutDataManager.Instance.datas[index];
			if (null == icon)
			{
				return;
			}
			if (data.type == SHORTCUTITEM.SKILL)
			{
				
				icon.atlas = UIAtlasManager.GetInstance().GetUIAtlas("SkillAtlas");
				icon.spriteName = "" + itemId;
			}
			else if(data.type ==  SHORTCUTITEM.ITEM)
			{
				icon.atlas = null;
				icon.spriteName = "";
				
			}
			else
			{
				if (icon.atlas != null)
				{
					icon.atlas = null;
					icon.spriteName = "";
				}
					
			}
		}
		
        void Update()
        {
			if( Input.GetKeyDown(keyCode) )
			{
				ClickItemHandler(button.gameObject);		
			}
			if (itemType == SHORTCUTITEM.SKILL)
			{
				ActiveSkillData data =  SkillLogic.GetInstance().GetActiveSkillVOByID((ushort)itemId);
				float _v = data.cdTicket.GetTimeNormal();
				if (_v>=1)
					_v = SkillLogic.GetInstance().GetAllSkillCDNormal();
				if (_v>1)
					_v = 1;
				
				
				if (cdValue != _v)
				{
					if (null != mark)
						mark.fillAmount = 1f - _v;
					cdValue = _v;
				}	
			}
        }
		private void ClickItemHandler(GameObject go)
        {
			if (itemType == SHORTCUTITEM.SKILL)
			{
				Debug.Log("try play skill "+itemId);
				SkillLogic.GetInstance().OnSkill((ushort)itemId);
			}
        }
		
		public void OnDrag(GameObject go, Vector2 delta)
        {
           	DraggignSkill _skill = new DraggignSkill();
			_skill.skillIndex = itemId;
			_skill.shorCutIndex = index;
            CursorManager.GetInstance().SetDragingCur(icon.atlas,icon.spriteName,DrawDataType.SKILL,_skill);
        }

        public void OnDrop(GameObject go, GameObject draggedObject)
        {
			if (CursorManager.GetInstance().GetDraggingDataType() == DrawDataType.SKILL)
			{
				DraggignSkill _skill = CursorManager.GetInstance().getDraggingData() as DraggignSkill;
				if (null == _skill)
					return;
				
				int oldItemId = itemId;
				SHORTCUTITEM oldItemType =  itemType;
				
				ButtonBarView.GetInstance().SetShortCut(index,SHORTCUTITEM.SKILL,(int)_skill.skillIndex,true);
				
				if (_skill.shorCutIndex >= 0)
				{
					if (oldItemType == SHORTCUTITEM.SKILL)
					{
						ButtonBarView.GetInstance().SetShortCut(_skill.shorCutIndex,SHORTCUTITEM.SKILL,(int)oldItemId,true);
					}
					else
					{
						ButtonBarView.GetInstance().SetShortCut(_skill.shorCutIndex,SHORTCUTITEM.NONE,(int)oldItemId,true);
						cdValue = 0;
						mark.fillAmount = 1f ;
					}
				}
				
			}
            CursorManager.GetInstance().ClearDragCursor();
        }
    }
}
