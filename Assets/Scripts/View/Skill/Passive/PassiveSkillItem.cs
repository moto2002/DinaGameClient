using Assets.Scripts.Logic.Skill;
using UnityEngine;
using Assets.Scripts.Manager;
namespace Assets.Scripts.View.Skill
{
    public class PassiveSkillItem : MonoBehaviour
    {
        protected PassiveSkillData skillData = null;
        protected UISprite icon = null;
        public UIAtlas iconAtlas = null;

        protected virtual void ConfigUI()
        {
            iconAtlas = UIAtlasManager.GetInstance().GetUIAtlas("SkillAtlas");
            icon = NGUITools.AddChild<UISprite>(gameObject);
            icon.atlas = iconAtlas;
            icon.spriteName = "lock";
            icon.MakePixelPerfect();
        }

        public PassiveSkillData SkillData
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

        protected virtual void UpdateIcon()
        {
            icon.spriteName = "skill_p_" + skillData.SkillID;
            //if (skillData.learned)
            //{
            //    icon.color = Color.white;
            //}
            //else
            //{
            //    icon.color = gray;
            //}
        }

    }
}
