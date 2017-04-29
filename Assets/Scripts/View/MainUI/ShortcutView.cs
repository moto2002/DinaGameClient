using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.Lib.View;
using Assets.Scripts.Model.Scene;
using Assets.Scripts.View.Scene;
using Assets.Scripts.Manager;
using Assets.Scripts.View.Skill;
using Assets.Scripts.Model.Skill;
using Assets.Scripts.View.UIDetail;
using Assets.Scripts.Logic.RemoteCall;
using Assets.Scripts.Logic.Scene;
using Assets.Scripts.Logic.Skill;

namespace Assets.Scripts.View.MainUI
{
    public class ShortcutView : ShortcutUIDetail
    {
        public const int SKILL_COUNT = 6;
        public static readonly Vector3 colliderSize = new Vector3(40, 40, 0);
        public static readonly Vector3[] skillPos = { new Vector3(-265.19f, -36.78f, 0), new Vector3(-206.40f, -36.78f, 0), new Vector3(-150.45f, -36.78f, 0), new Vector3(-93.288f, -36.78f, 0), new Vector3(100.288f, -36.78f, 0), new Vector3(159.0f, -36.78f, 0) };
		public ShortcutItem [] shortcuts = new ShortcutItem[8];
        private static ShortcutView instance;
        public static ShortcutView GetInstance()
        {
            if (instance == null)
                instance = new ShortcutView();
            return instance;
        }

        private SkillShortcutItem[] skillItems = null;

        public ShortcutView():base(662, 160)
        {
            skillItems = new SkillShortcutItem[SKILL_COUNT];
			for(int i = 0 ; i < 8 ; i++)
			{
				shortcuts[i] = new ShortcutItem();
			}
        }
		
		public void SetShortCut(int index,SHORTCUTITEM type, int id , bool save = true)
		{
			if (index<0&&index>7)
			{
				return;
			}
			ShortCutData data =  ShortCutDataManager.Instance.datas[index];
			data.id = id;
			data.type = type;
			ShortcutItem item = shortcuts[index];
			item.Refreash();
			if (save)
			{
				//OnShortCutChange(index,type,id);
			}
		}
		public int[] GetActiveSkill()
		{
			List<int> _skills = new List<int>();
			foreach (ShortcutItem item in shortcuts)
			{
				_skills.Add(item.itemId);
			}
			return _skills.ToArray();
		}

        protected override void PreInit()
        {
            base.PreInit();
			for (int i = 0; i < 8 ; i++)
			{
				UISprite s = FindUIObject<UISprite>("Item"+(i+1));
				s.type = UISprite.Type.Filled;
				s.fillDirection = UISprite.FillDirection.Radial360;
				s.fillAmount = 1f;
				shortcuts[i] = s.gameObject.AddComponent<ShortcutItem>();
				shortcuts[i].index = i;
			}
			shortcuts[0].keyCode = KeyCode.Alpha1;
			shortcuts[1].keyCode = KeyCode.Alpha2;
			shortcuts[2].keyCode = KeyCode.Alpha3;
			shortcuts[3].keyCode = KeyCode.Alpha4;
			
			shortcuts[4].keyCode = KeyCode.Alpha5;
			shortcuts[5].keyCode = KeyCode.Alpha6;
			shortcuts[6].keyCode = KeyCode.Alpha7;
			shortcuts[7].keyCode = KeyCode.Alpha8;
        }
		
		
        protected override void Init()
        {
            SetViewPosition(ViewPosition.Bottom);

            CreateSkillItem();

            UIEventListener.Get(bagBtn.gameObject).onClick += ClickBagHandler;
            UIEventListener.Get(forgeBtn.gameObject).onClick += ClickForgeHandler;
            UIEventListener.Get(friendBtn.gameObject).onClick += ClickFriendHandler;
            UIEventListener.Get(generalBtn.gameObject).onClick += ClickGeneralHandler;
            UIEventListener.Get(horseBtn.gameObject).onClick += ClickHorseHandler;
            UIEventListener.Get(roleBtn.gameObject).onClick += ClickRoleHandler;
            UIEventListener.Get(skillBtn.gameObject).onClick += ClickSkillHandler;
            UIEventListener.Get(teamBtn.gameObject).onClick += ClickTeamHandler;

        }
		
		public override void Update()
        {
			if ( null != SceneLogic.GetInstance().MainHero && Input.GetKeyDown(KeyCode.Space))
			{
				SkillLogic.GetInstance().OnSkill((ushort)SceneLogic.GetInstance().MainHero.heroSetting.RushSkill);
			}
        }
		
        private void CreateSkillItem()
        {
            
        }

        private void ClickBagHandler(GameObject go)
        {
            EventDispatcher.GameWorld.Dispath(ControllerCommand.OPEN_BAG_PANEL, new object());
        }

        private void ClickForgeHandler(GameObject go)
        {
            EventDispatcher.GameWorld.Dispath(ControllerCommand.OPEN_FORGE_PANEL, new object());
        }

        private void ClickFriendHandler(GameObject go)
        {
            EventDispatcher.GameWorld.Dispath(ControllerCommand.OPEN_FRIEND_PANEL, new object());
        }

        private void ClickGeneralHandler(GameObject go)
        {
            EventDispatcher.GameWorld.Dispath(ControllerCommand.OPEN_GENERAL_PANEL, new object());
        }

        private void ClickHorseHandler(GameObject go)
        {
            EventDispatcher.GameWorld.Dispath(ControllerCommand.OPEN_HORSE_PANEL, new object());
        }

        private void ClickRoleHandler(GameObject go)
        {
            EventDispatcher.GameWorld.Dispath(ControllerCommand.OPEN_ROLE_INFO_PANEL, new object());
        }

        private void ClickSkillHandler(GameObject go)
        {
            EventDispatcher.GameWorld.Dispath(ControllerCommand.OPEN_SKILL_PANEL, new object());
        }

        private void ClickTeamHandler(GameObject go)
        {
            EventDispatcher.GameWorld.Dispath(ControllerCommand.OPEN_TEAM_PANEL, new object());
        }

        

        private void ExpChangeHandler()
        {
        }

        private void HPChangeHandler()
        {
        }

        private void MPChangeHandler()
        {
        }

        private void AngerChangeHandler()
        {

        }

        

    }
}
