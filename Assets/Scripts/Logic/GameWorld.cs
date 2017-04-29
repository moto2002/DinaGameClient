using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Proto;

namespace Assets.Scripts.Logic
{
    class GameWorld : BaseLogic
    {
        private static GameWorld instance;
        public static GameWorld GetInstance()
        {
            if (instance == null)
                instance = new GameWorld();
            return instance;
        }

        protected override void Init()
        {
            
        }

        protected override void InitListeners()
        {
//rpc register label begin do not touch me
            RegistProctect(KS2C_Protocol.s2c_ping_signal, typeof(S2C_PING_SIGNAL));
            RegistProctect(KS2C_Protocol.s2c_sync_player_base_info, typeof(S2C_SYNC_PLAYER_BASE_INFO));
            RegistProctect(KS2C_Protocol.s2c_sync_player_state_info, typeof(S2C_SYNC_PLAYER_STATE_INFO));
            RegistProctect(KS2C_Protocol.s2c_account_kickout, typeof(S2C_ACCOUNT_KICKOUT));
            RegistProctect(KS2C_Protocol.s2c_switch_gs, typeof(S2C_SWITCH_GS));
            RegistProctect(KS2C_Protocol.s2c_switch_map, typeof(S2C_SWITCH_MAP));
            RegistProctect(KS2C_Protocol.s2c_talk_message, typeof(S2C_TALK_MESSAGE));
            RegistProctect(KS2C_Protocol.s2c_hero_move, typeof(S2C_HERO_MOVE));
            RegistProctect(KS2C_Protocol.s2c_battle_startedframe, typeof(S2C_BATTLE_STARTEDFRAME));
            RegistProctect(KS2C_Protocol.s2c_sync_scene_obj, typeof(S2C_SYNC_SCENE_OBJ));
            RegistProctect(KS2C_Protocol.s2c_sync_scene_obj_end, typeof(S2C_SYNC_SCENE_OBJ_END));
            RegistProctect(KS2C_Protocol.s2c_sync_scene_hero_data_end, typeof(S2C_SYNC_SCENE_HERO_DATA_END));
            RegistProctect(KS2C_Protocol.s2c_sync_new_hero, typeof(S2C_SYNC_NEW_HERO));
            RegistProctect(KS2C_Protocol.s2c_sync_hero_data, typeof(S2C_SYNC_HERO_DATA));
            RegistProctect(KS2C_Protocol.s2c_cast_skill, typeof(S2C_CAST_SKILL));
            RegistProctect(KS2C_Protocol.s2c_skill_effect, typeof(S2C_SKILL_EFFECT));
            RegistProctect(KS2C_Protocol.s2c_add_buff_notify, typeof(S2C_ADD_BUFF_NOTIFY));
            RegistProctect(KS2C_Protocol.s2c_del_buff_notify, typeof(S2C_DEL_BUFF_NOTIFY));
            RegistProctect(KS2C_Protocol.s2c_hero_death, typeof(S2C_HERO_DEATH));
            RegistProctect(KS2C_Protocol.s2c_remove_scene_obj, typeof(S2C_REMOVE_SCENE_OBJ));
            RegistProctect(KS2C_Protocol.s2c_sync_hero_hpmp, typeof(S2C_SYNC_HERO_HPMP));
            RegistProctect(KS2C_Protocol.s2c_pvp_game_over, typeof(S2C_PVP_GAME_OVER));
            RegistProctect(KS2C_Protocol.s2c_sync_add_hp, typeof(S2C_SYNC_ADD_HP));
            RegistProctect(KS2C_Protocol.s2c_sync_add_mp, typeof(S2C_SYNC_ADD_MP));
            RegistProctect(KS2C_Protocol.s2c_sync_move_speed, typeof(S2C_SYNC_MOVE_SPEED));
            RegistProctect(KS2C_Protocol.s2c_sync_max_hp, typeof(S2C_SYNC_MAX_HP));
            RegistProctect(KS2C_Protocol.s2c_sync_max_mp, typeof(S2C_SYNC_MAX_MP));
            RegistProctect(KS2C_Protocol.s2c_sync_attack_speed, typeof(S2C_SYNC_ATTACK_SPEED));
            RegistProctect(KS2C_Protocol.s2c_cast_skill_fail_notify, typeof(S2C_CAST_SKILL_FAIL_NOTIFY));
            RegistProctect(KS2C_Protocol.s2c_move_fail_notify, typeof(S2C_MOVE_FAIL_NOTIFY));
            RegistProctect(KS2C_Protocol.s2c_scene_obj_change_pos, typeof(S2C_SCENE_OBJ_CHANGE_POS));
            RegistProctect(KS2C_Protocol.s2c_click_hero_respond, typeof(S2C_CLICK_HERO_RESPOND));
            RegistProctect(KS2C_Protocol.s2c_downward_notify, typeof(S2C_DOWNWARD_NOTIFY));
            RegistProctect(KS2C_Protocol.s2c_add_exp, typeof(S2C_ADD_EXP));
            RegistProctect(KS2C_Protocol.s2c_sync_item, typeof(S2C_SYNC_ITEM));
            RegistProctect(KS2C_Protocol.s2c_package_size, typeof(S2C_PACKAGE_SIZE));
            RegistProctect(KS2C_Protocol.s2c_update_item_amount, typeof(S2C_UPDATE_ITEM_AMOUNT));
            RegistProctect(KS2C_Protocol.s2c_destroy_item, typeof(S2C_DESTROY_ITEM));
            RegistProctect(KS2C_Protocol.s2c_use_item, typeof(S2C_USE_ITEM));
            RegistProctect(KS2C_Protocol.s2c_part_item, typeof(S2C_PART_ITEM));
            RegistProctect(KS2C_Protocol.s2c_exchange_item, typeof(S2C_EXCHANGE_ITEM));
            RegistProctect(KS2C_Protocol.s2c_sale_item_to_sys, typeof(S2C_SALE_ITEM_TO_SYS));
            RegistProctect(KS2C_Protocol.s2c_sync_equips, typeof(S2C_SYNC_EQUIPS));
            RegistProctect(KS2C_Protocol.s2c_sync_skill_data_list, typeof(S2C_SYNC_SKILL_DATA_LIST));
            RegistProctect(KS2C_Protocol.s2c_upgrade_skill_respond, typeof(S2C_UPGRADE_SKILL_RESPOND));
            RegistProctect(KS2C_Protocol.s2c_sync_skill_point, typeof(S2C_SYNC_SKILL_POINT));
            RegistProctect(KS2C_Protocol.s2c_sync_new_player_info, typeof(S2C_SYNC_NEW_PLAYER_INFO));
            RegistProctect(KS2C_Protocol.s2c_enter_fight_notify, typeof(S2C_ENTER_FIGHT_NOTIFY));
            RegistProctect(KS2C_Protocol.s2c_leave_fight_notify, typeof(S2C_LEAVE_FIGHT_NOTIFY));
            RegistProctect(KS2C_Protocol.s2c_start_hero_force_move, typeof(S2C_START_HERO_FORCE_MOVE));
            RegistProctect(KS2C_Protocol.s2c_stop_hero_force_move, typeof(S2C_STOP_HERO_FORCE_MOVE));
            RegistProctect(KS2C_Protocol.s2c_hero_relive, typeof(S2C_HERO_RELIVE));
            RegistProctect(KS2C_Protocol.s2c_sync_damage, typeof(S2C_SYNC_DAMAGE));
            RegistProctect(KS2C_Protocol.s2c_update_level, typeof(S2C_UPDATE_LEVEL));
            RegistProctect(KS2C_Protocol.s2c_hero_pos, typeof(S2C_HERO_POS));
            RegistProctect(KS2C_Protocol.s2c_call_script, typeof(S2C_CALL_SCRIPT));
            RegistProctect(KS2C_Protocol.s2c_sync_all_attribute, typeof(S2C_SYNC_ALL_ATTRIBUTE));
            RegistProctect(KS2C_Protocol.s2c_sync_one_attribute, typeof(S2C_SYNC_ONE_ATTRIBUTE));
            RegistProctect(KS2C_Protocol.s2c_sync_self_attribute, typeof(S2C_SYNC_SELF_ATTRIBUTE));
//rpc register label end do not touch me
        }

    }
}
