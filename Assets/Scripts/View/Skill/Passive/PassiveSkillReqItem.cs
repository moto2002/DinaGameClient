using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Logic.Item;
using Assets.Scripts.Manager;

namespace Assets.Scripts.View.Skill
{
    public class PassiveSkillReqItem : MonoBehaviour
    {
        protected UISprite icon = null;
        public UIAtlas iconAtlas = null;

        private ItemInfo itemData = null;
        private UISprite background = null;
        private UILabel numTxt = null;

        void Start()
        {  
        }

        protected void AddIcon()
        {
            background = NGUITools.AddChild<UISprite>(gameObject);
            background.atlas = UIAtlasManager.GetInstance().GetUIAtlas();
            background.MakePixelPerfect();
            iconAtlas = UIAtlasManager.GetInstance().GetUIAtlas("IconAtlas");
            icon = NGUITools.AddChild<UISprite>(gameObject);
            icon.atlas = iconAtlas;
            icon.MakePixelPerfect();
            numTxt = NGUITools.AddChild<UILabel>(gameObject);
            numTxt.font = FontManager.GetInstance().Font;
        }

        public ItemInfo ItemData
        {
            get
            {
                return itemData;
            }
            set
            {
                if (value != itemData)
                {
                    itemData = value;
                    UpdateIcon();
                }
            }
        }

        protected void UpdateIcon()
        {
            if (itemData == null)
            {
                icon.spriteName = "lock";
            }
            else
            {
                icon.spriteName = itemData.Icon;
                numTxt.text = itemData.CurNum.ToString();
            }
            icon.MakePixelPerfect();
        }
    }
}
