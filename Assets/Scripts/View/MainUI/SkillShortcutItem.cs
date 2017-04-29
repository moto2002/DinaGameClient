using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Scripts.Manager;
using Assets.Scripts.View.Skill;
using Assets.Scripts.View.Drag;
using Assets.Scripts.Logic.Item;
using Assets.Scripts.Model.Skill;

namespace Assets.Scripts.View.MainUI
{
    public class SkillShortcutItem : ActiveSkillItem
    {
        public const string DRAG_TYPE = "SKILL_SHORTCUT_TYPE";

        //private UISprite coolSp = null;
        private UIAtlas coolAtlas = null;
        private Cooldown cool = null;

        void Update()
        {
            if (cool != null)
            {
                //Time.realtimeSinceStartup
                //ChangeCD(cool.GetLastTime() / cool.cdTime);
            }
        }
        protected override void ConfigUI()
        {
            base.ConfigUI();
            icon.transform.localScale = new Vector3(1,1,1);
            drag.DragType = DRAG_TYPE;
            drag.DropEvent += DoDrop;
        }

        protected void UpdateIcon()
        {
            if (skillData == null)
            {
                icon.spriteName = "";
                icon.enabled = false;
                //coolSp.enabled = true;
                //coolSp.fillAmount = 1.0f;
            }
            else
            {
                icon.enabled = true;
                base.UpdateIcon();
                //coolSp.enabled = false;
                icon.transform.localScale = new Vector3(1, 1, 1);
            }
        }

        public void ChangeCD(Cooldown cool)
        {
            this.cool = cool;
            //coolSp.enabled = true;
        }

        private void ChangeCD(float value)
        {
            //coolSp.fillAmount = value;
            if (value <= 0)
            {
                //coolSp.enabled = false;
                cool = null;
            }
        }

        public void DoDrop(GameObject go)
        {
            //if (go == null)
            //{
            //    return;
            //}
            //IDragable dragObj = go.GetComponent<DragItem>();
            //string type = dragObj.DragType;
            //if (type.Equals(DRAG_TYPE))
            //{
            //    SkillShortcutItem shortcoutItem = go.GetComponent<SkillShortcutItem>();
            //    if (skillData == null)
            //    {
            //        SkillData = dragObj.DragItemVO;
            //        dragObj.DragItemVO = null;
            //        shortcoutItem.Data = null;
            //    }
            //    else
            //    {
            //        IDragInfo vo = SkillData;
            //        SkillData = dragObj.DragItemVO;
            //        dragObj.DragItemVO = vo;
            //        shortcoutItem.Data = vo;
            //    }
            //}
            //else if(type.Equals(ActiveSkillItem.DRAG_TYPE))
            //{
            //    SkillData = dragObj.DragItemVO;
            //}
        }

        public UIAtlas CoolAtlas
        {
            get { return coolAtlas; }
            set
            {
                coolAtlas = value;
                //if (coolSp != null)
                //{
                //    DestroyImmediate(coolSp);
                //}
                //coolSp = NGUITools.AddWidget<UISprite>(gameObject);
                //coolSp.atlas = value;
                //coolSp.spriteName = "低圈";
                //coolSp.type = UISprite.Type.Filled;
                //coolSp.MakePixelPerfect();
            }
        }
    }
}
