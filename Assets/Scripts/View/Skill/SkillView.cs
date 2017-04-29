using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Lib.View;
using Assets.Scripts.Controller;
using Assets.Scripts.Manager;

namespace Assets.Scripts.View.Skill
{
    public class SkillView : ViewScript
    {
        private GameObject activeSkillObj = null;
        private GameObject passiveSkillObj = null;
        private GameObject talentSkillObj = null;

        private SkillTabView currentView;

        private ActiveSkillView activeSkillView = null;
        private PassiveSkillView passiveSkillView = null;
        private TalentSkillView talentSkillView = null;

        private UIRadioButtonPanel buttonGroup = null;

        public SkillView(): base("SkillUI", 560, 414)
        {
        }


        protected override void Init()
        {
            activeSkillObj = FindGameObject("activeObj");
            activeSkillView = activeSkillObj.AddComponent<ActiveSkillView>();
            passiveSkillObj = FindGameObject("passiveObj");
            passiveSkillView = passiveSkillObj.AddComponent<PassiveSkillView>();
            talentSkillObj = FindGameObject("talentObj");
            talentSkillView = talentSkillObj.AddComponent<TalentSkillView>();
            ReplacementLayer();
            
            UIEventListener.Get(FindGameObject("closeBtn")).onClick += OnClickClose;

            buttonGroup = FindUIObject<UIRadioButtonPanel>("tabBtn");
            buttonGroup.OnRadioSelect += OnChangeTab;

            OnChangeTab(buttonGroup.SelectIndex);
        }

        protected override void InitEvent()
        {
            EventDispatcher.GameWorld.Regist(ControllerCommand.UPDATE_SKILL, OnUpdateSkill);
        }

        protected override void DestoryEvent()
        {
            EventDispatcher.GameWorld.Remove(ControllerCommand.UPDATE_SKILL, OnUpdateSkill);
        }

        private object OnUpdateSkill(params object[] objs)
        {
            int idx = (int)objs[0];
            if (buttonGroup.SelectIndex == idx && currentView != null)
            {
                currentView.UpdateView(objs);
            }
            ReplacementLayer();//UI做了修改， 需要重新调整子组件内部深度
            return null;
        }

        private void OnChangeTab(int index)
        {
            activeSkillObj.SetActive(false);
            passiveSkillObj.SetActive(false);
            talentSkillObj.SetActive(false);
            if (index == 0)
            {
                activeSkillObj.SetActive(true);
                currentView = activeSkillView;
            }
            else if (index == 1)
            {
                passiveSkillObj.SetActive(true);
                currentView = passiveSkillView;
            }
            else
            {
                talentSkillObj.SetActive(true);
                currentView = talentSkillView;
            }
        }
    }
}
