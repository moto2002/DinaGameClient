using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Manager;
using Assets.Scripts.Model.Skill;
using Assets.Scripts.Data;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.Logic.Skill;

namespace Assets.Scripts.View.Skill
{
    public class TalentSkillItem : PassiveSkillItem
    {
        private GameObject addBtn = null;
        private UILabel numTxt = null;

        protected override void ConfigUI()
        {
            base.ConfigUI();
            numTxt = NGUITools.AddWidget<UILabel>(gameObject);
            numTxt.font = FontManager.GetInstance().Font;
            numTxt.transform.localPosition = new Vector3(0, -20, 0);
        }

        public void AddPointBtn(GameObject btn)
        {
            addBtn = btn;
            addBtn.transform.parent = gameObject.transform;
            addBtn.transform.localPosition = new Vector3(18, 18, 0);
            addBtn.transform.localScale = Vector3.one;
        }

        protected override void UpdateIcon()
        {
            base.UpdateIcon();
            KPassiveSkill nextSkillSetting = KConfigFileManager.GetInstance().GetPassiveSkill(skillData.SkillID, skillData.Level + 1);
            if (SkillLogic.GetInstance().CanLearnTalent(skillData.SkillID) && SkillLogic.GetInstance().SkillPoint > 0 && nextSkillSetting != null)
            {
                addBtn.SetActive(true);
            }
            else
            {
                addBtn.SetActive(false);
            }
        }
    }
}
