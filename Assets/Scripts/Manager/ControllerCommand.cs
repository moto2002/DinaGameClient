using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Manager
{
	public class ControllerCommand
	{
		public const string OPEN_CHANGEEQUIP_PANEL = "OPEN_CHANGEEQUIP_PANEL";
        public const string OPEN_BAG_PANEL = "OPEN_BAG_PANEL";
        public const string OPEN_ROLE_INFO_PANEL = "OPEN_ROLE_INFO_PANEL";
        public const string OPEN_SKILL_PANEL = "OPEN_SKILL_PANEL";
        public const string OPEN_GENERAL_PANEL = "OPEN_GENERAL_PANEL";
        public const string OPEN_TEAM_PANEL = "OPEN_TEAM_PANEL";
        public const string OPEN_FRIEND_PANEL = "OPEN_FRIEND_PANEL";
        public const string INIT_SCENE_UI = "INIT_SCENE_UI";
        public const string INIT_SCENE_VIEW = "INIT_SCENE_VIEW";
        public const string OPEN_HORSE_PANEL = "OPEN_HORSE_PANEL";
        public const string OPEN_FORGE_PANEL = "OPEN_FORGE_PANEL";
        public const string OPEN_NPC_PANEL = "OPEN_NPC_PANEL";
		public const string OPEN_LOADING_PANEL = "OPEN_LOADING_PANEL";
		public const string CLOSE_LOADING_PANEL = "CLOSE_LOADING_PANEL";
        public const string OPEN_NPC_PANEL_BYID = "OPEN_NPC_PANEL_BYID";
        public const string OPEN_BAGITEMCLICK_PANEL = "OPEN_BAGITEMCLICK_PANEL";
        public const string CLOSE_BAGITEMCLICK_PANEL = "CLOSE_BAGITEMCLICK_PANEL";
        public const string OPEN_BAGITEMPART_PANEL = "OPEN_BAGITEMPART_PANEL";
        public const string OPEN_BAGITEMUSE_PANEL = "OPEN_BAGITEMUSE_PANEL";
        public const string OPEN_BAGITEMSALE_PANEL = "OPEN_BAGITEMSALE_PANEL";
        public const string OPEN_COLLECT_PANEL = "OPEN_COLLECT_PANEL";
        public const string OPEN_PLOT_PANEL = "OPEN_PLOT_PANEL";

        public const string USE_SKILL_SHORTCUT = "USE_SKILL_SHORTCUT";

        public const string KEYCODE_A_DOWN = "KEYCODE_A_DOWN";

        public const string CHANGE_PLAYER_HP_MP = "CHANGE_PLAYER_HP_MP";
        public const string CHANGE_NICKNAME = "CHANGE_NICKNAME";
        public const string CHANGE_MONEY = "CHANGE_MONEY";
        public const string CHANGE_HEAD = "CHANGE_HEAD";
        public const string CHANGE_MAP = "CHANGE_MAP";

        public const string UPGRADE_ACTIVE_SKILL = "UPGRADE_ACTIVE_SKILL";
        public const string UPGRADE_PASSIVE_SKILL = "UPGRADE_PASSIVE_SKILL";
        public const string UPGRADE_TALENT_SKILL = "UPGRADE_TALENT_SKILL";
        public const string UPDATE_SKILL_POINT = "UPDATE_SKILL_POINT";
        public const string UPDATE_SKILL = "UPDATE_SKILL";

        public const string UPDATE_ROLE_AVATAR = "UPDATE_ROLE_AVATAR";
        public const string UPDATE_ROLE_ATTRIBUTE = "UPDATE_ROLE_ATTRIBUTE";

        public const string SHOW_PLAYER_PANEL = "SHOW_PLAYER_PANEL";
        public const string SHOW_MONSTER_PANEL = "SHOW_MONSTER_PANEL";
        public const string SCENE_DOUBLE_CLICK = "SCENE_DOUBLE_CLICK";
        public const string SCENE_CLICK = "SCENE_CLICK";
        public const string SCENE_CLICK_OBJECT = "SCENE_CLICK_OBJECT";
        public const string SCENE_PLAYER_ATTR = "SCENE_PLAYER_ATTR";

        public const string NPC_CLICK_MISSION_LINK = "NPC_CLICK_MISSION_LINK";
        public const string PLAYER_LEVEL_UP = "PLAYER_LEVEL_UP";

        public const string UPDATE_BAG_GOODS = "UPDATE_BAG_GOODS";
        public const string LOAD_MINI_MAP = "LOAD_MINI_MAP";
        public const string UPDATE_MINI_MAP = "UPDATE_MINI_MAP";
        public const string SET_MAIN_HERO = "SET_MAIN_HERO";
        public const string UPDATE_ROLE_EQUIP = "UPDATE_ROLE_EQUIP";

        public const string ROLE_DATA_BASE_LOADED = "ROLE_DATA_BASE_LOADED";
        
        public const string SET_GLOW = "SET_GLOW";
        public const string CrossFadeAnimation  = "CrossFadeAnimation";
        public const string SetActiveAction = "SetActiveAction";
        public const string TryFinishAction = "TryFinishAction";
        public const string FinishImmediate = "FinishImmediate";
        public const string ActionMoveToDistance = "ActionMoveToDistance";
        public const string PlayAnimation = "PlayAnimation";
        public const string LookAtPos = "LookAtPos";
        public const string IsPlayingActionFinish = "IsPlayingActionFinish";
        public const string SET_SPEED = "SET_SPEED";
        public const string SET_DESTINATION = "SET_DESTINATION";
        public const string MOVE_TO_DES = "MOVE_TO_DES";
        public const string AUTO_MOVE = "AUTO_MOVE";
		public const string FuKong = "FuKong";
        public const string Back = "Back";
        public const string Drag = "Drag";
        public const string Jump = "Jump";
        public const string Idle = "Idle";
        public const string CLEAR_BUFF = "CLEAR_BUFF";
        public const string ADD_BUFF = "ADD_BUFF";
        public const string REMOVE_BUFF = "REMOVE_BUFF";
        public const string CHANGE_NAME = "CHANGE_NAME";
		public const string BE_HIT = "BE_HIT";
		public const string HIT_SLOW = "HIT_SLOW";
		public const string SLOW = "SLOW";
        public const string CHANGE_TITLE = "CHANGE_TITLE";
        public const string LOAD_NAME_LABEL = "LOAD_NAME_LABEL";
        public const string LOAD_RES = "LOAD_RES";
        public const string UPDATE_MISSION_SIGN = "UPDATE_MISSION_SIGN";
        public const string UPDATE_MISSION = "UPDATE_MISSION";
        public const string CONTINUE_MISSION = "CONTINUE_MISSION";
        public const string EQUIP_CHANGE = "EQUIP_CHANGE";
		public const string WEAPON_TRAIL = "WEAPON_TRAIL";
		public const string GHOST_SHADOW = "GHOST_SHADOW";
        public const string SHOW_PVE_VIEW = "SHOW_PVE_VIEW";
        public const string GIFT_ITEM_UPDATE = "GIFT_ITEM_UPDATE";
        public const string SYNC_MONEY = "SYNC_MONEY";
        public const string UPDATE_COMBAT = "UPDATE_COMBAT";

        public const string HERO_MOVE = "HERO_MOVE";
		public const string HERO_DEAD = "HERO_DEAD";
        public const string HERO_PLAYSKILL = "HERO_PLAYSKILL";
        public const string ATTACK_BOSS = "ATTACK_BOSS";
        public const string CHANGE_TARGET = "CHANGE_TARGET";
        public const string ADD_EXP = "ADD_EXP";
        public const string ADD_ITEM = "ADD_ITEM";
		
		public const string MOVE_TO_NPC = "MOVE_TO_NPC";
		public const string REACH_NPC = "REACH_NPC";
		
		public const string STOP_MOVE = "STOP_MOVE";
	}
}
