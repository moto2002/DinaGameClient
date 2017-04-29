using UnityEngine;
using Assets.Scripts.Manager;

namespace Assets.Scripts.View.Skill
{
    public class SkillItem : MonoBehaviour
    {
        public static readonly Color gray = new Color(33f / 255, 33f / 255, 33f / 255);
        protected UISprite icon = null;
        protected System.Object data = null;
        public UIAtlas iconAtlas = null;


        public void Awake()
        {
            ConfigUI();
        }

        protected virtual void ConfigUI()
        {
            AddIcon();
        }

        protected virtual void AddIcon()
        {
            iconAtlas = UIAtlasManager.GetInstance().GetUIAtlas("SkillAtlas");
            icon = NGUITools.AddChild<UISprite>(gameObject);
            icon.atlas = iconAtlas;
            icon.spriteName = "lock";
            icon.MakePixelPerfect();
        }


        public System.Object Data
        {
            get { return data; }
            set
            {
                if (value != data)
                {
                    data = value;
                    UpdateIcon();
                }
            }
        }

        protected virtual void UpdateIcon()
        {
            
        }
    }
}
