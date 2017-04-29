using System;
using UnityEngine;
using Assets.Scripts.View.Bag;
using Assets.Scripts.View.Drag;
using Assets.Scripts.Logic.Item;
using Assets.Scripts.Logic.Skill;
using Assets.Scripts.Manager;

namespace Assets.Scripts.View.Skill
{
    public class ActiveSkillItem : MonoBehaviour
    {
        public static readonly Color gray = new Color(33f / 255, 33f / 255, 33f / 255);

        public const string DRAG_TYPE = "ACTIVE_SKILL_TYPE";
        protected UISprite icon = null;
        public UIAtlas iconAtlas = null;
        protected DragItem drag = null;

        protected ActiveSkillData skillData = null;

        void Awake()
        {
            ConfigUI();
        }

        protected virtual void ConfigUI()
        {
            iconAtlas = UIAtlasManager.GetInstance().GetUIAtlas("SkillAtlas");
            icon = NGUITools.AddChild<UISprite>(gameObject);
            icon.atlas = iconAtlas;
            icon.spriteName = "lock";
            icon.depth = 100;
            icon.MakePixelPerfect();

            drag = gameObject.AddComponent<DragItem>();
            drag.DragType = DRAG_TYPE;
        }


        public ActiveSkillData SkillData
        {
            get 
            { 
                return skillData; 
            }
            set
            {
                if (value != skillData)
                {
                    skillData = value;
                    UpdateIcon();
                }
            }
        }

        protected void UpdateIcon()
        {
            if (skillData == null)
            {
                icon.spriteName = "lock";
            }
            else
            {
                icon.spriteName = "skill_a_" + skillData.SkillID;
                icon.MakePixelPerfect();
                drag.DragIcon = icon;
           //     drag.DragItemVO = skillData;
            }
            icon.MakePixelPerfect();
        }

    }
}
