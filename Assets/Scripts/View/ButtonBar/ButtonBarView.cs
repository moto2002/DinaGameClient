using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Lib.View;
using Assets.Scripts.Controller;
using Assets.Scripts.View.UIDetail;
using Assets.Scripts.View.MainUI;
using Assets.Scripts.Manager;
using Assets.Scripts.Logic.RemoteCall;
using Assets.Scripts.Logic.Scene;
using Assets.Scripts.Logic.Skill;
using Assets.Scripts.Logic.Scene.SceneObject;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.Data;
using Assets.Scripts.Model.Player;
using Assets.Scripts.View.MainMenu;
using Assets.Scripts.Utils;

namespace Assets.Scripts.View.ButtonBar
{
	public class ButtonBarView : ButtonBarUIDetail
	{
		public const int ITEM_COUNT = 8;
		public ShortcutItem [] shortcuts = new ShortcutItem[8];
		public GameObject dragObject = null;
		public ButtonBarView()
            : base(Screen.width, 0)
        {
            
        }
		
		private static ButtonBarView instance;
        public static ButtonBarView GetInstance()
        {
            if (instance == null)
                instance = new ButtonBarView();
            return instance;
        }

        private BooldController HeroHP = null;
        private BooldController HeroMP = null;

		protected override void Init()
        {
            SetViewPosition(ViewPosition.Bottom);

            GameObject HeroHPGO = FindGameObject("effect_ui_red_xuetiao");
            if (HeroHPGO != null)
            {
                HeroHP = HeroHPGO.AddComponent<BooldController>();
            }

            GameObject HeroMPGO = FindGameObject("effect_ui_blue_xuetiao");
            if (HeroHPGO != null)
            {
                HeroMP = HeroMPGO.AddComponent<BooldController>();
            }
            

            EventDispatcher.GameWorld.Regist(ControllerCommand.CHANGE_PLAYER_HP_MP, OnChangeHpMP);
            EventDispatcher.GameWorld.Regist(ControllerCommand.ADD_EXP, OnAddExp);
            EventDispatcher.GameWorld.Regist(ControllerCommand.PLAYER_LEVEL_UP, OnAddExp);
			
			UIEventListener.Get(BagButton.gameObject).onClick += ClickBagHandler;
            UIEventListener.Get(ForgeButton.gameObject).onClick += ClickForgeHandler;
            UIEventListener.Get(FriendButton.gameObject).onClick += ClickFriendHandler;
            UIEventListener.Get(GuildButton.gameObject).onClick += ClickGeneralHandler;
            UIEventListener.Get(HeroButton.gameObject).onClick += ClickHorseHandler;
            UIEventListener.Get(RoleButton.gameObject).onClick += ClickRoleHandler;
            UIEventListener.Get(SkillButton.gameObject).onClick += ClickSkillHandler;
            UIEventListener.Get(TeamButton.gameObject).onClick += ClickTeamHandler;
		}

        private object OnChangeHpMP(params object[] objs)
        {
            if (null != HeroHP && null != HeroMP)
            {
                SceneEntity hero = SceneLogic.GetInstance().MainHero;
                HeroHP.SetRate(1f - (float)hero.Hp / (float)hero.MaxHp);
                HeroMP.SetRate(1f - (float)hero.Mp / (float)hero.MaxMp);
            }
            return null;
        }

        private object OnAddExp(params object[] objs)
        {
            MajorPlayer player = PlayerManager.GetInstance().MajorPlayer;
            base.Exp.fillAmount = (float)player.Exp / (float)player.maxExp;

            if (player.addExp > 0)
            {
                HeroMenuView.GetInstance().PlayerMessageList.Add(Util.ADD_EXP + player.addExp);
                player.addExp = 0;
            }

            //Debug.LogError(player.maxExp + " " + player.Exp + " " + base.Exp.fillAmount + " " + player.level);

            return null;
        }
		
		public void OnShortCutChange(int _index, SHORTCUTITEM _type,int _itemId)
		{
			if (_type == SHORTCUTITEM.ITEM)
			{
				
				RemoteCallLogic.GetInstance().CallGS("OnShortCutChange", _index+1, 1,_itemId);
			}
			else if (_type == SHORTCUTITEM.SKILL)
			{
				RemoteCallLogic.GetInstance().CallGS("OnShortCutChange", _index+1, 2,_itemId);
			}
			else
			{
				RemoteCallLogic.GetInstance().CallGS("OnShortCutChange", _index+1, 0,0);
			}
		}
		public void SetShortCut(int index,SHORTCUTITEM type, int id , bool save = true)
		{
			if (index<0||index>7)
			{
				return;
			}
			ShortCutData data =  ShortCutDataManager.Instance.datas[index];
			data.id = id;
			data.type = type;
			
			ShortcutItem item = shortcuts[index];
			if (null != item)
				item.Refreash();
			if (save)
			{
				OnShortCutChange(index,type,id);
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
				UISprite _icon = FindUIObject<UISprite>("Icon"+(i+1));
				UISprite _mark = FindUIObject<UISprite>("Button"+(i+1)+"Mark");
				UIImageButton _button = FindUIObject<UIImageButton>("Button"+(i+1));
				Transform _frame = FindUIObject<Transform>("Frame"+(i+1));
				_mark.type = UISprite.Type.Filled;
				_mark.fillDirection = UISprite.FillDirection.Radial360;
				_icon.fillAmount = 1f;
				 
				ShortcutItem _shortcut  = _icon.gameObject.AddComponent<ShortcutItem>();
				_shortcut.icon = _icon;
				_shortcut.index = i;
				_shortcut.mark = _mark;
				_shortcut.button = _button;
				shortcuts[i] = _shortcut;
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
		public override void Update()
        {
			if ( null != SceneLogic.GetInstance().MainHero && Input.GetKeyDown(KeyCode.Space))
			{
				SkillLogic.GetInstance().OnSkill((ushort)SceneLogic.GetInstance().MainHero.heroSetting.RushSkill);
			}
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
		

        
	}
}
