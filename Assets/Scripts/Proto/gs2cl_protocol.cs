/*
compile by protobuf, please don't edit it manually. 
Any problem please contact tongxuehu@gmail.com, thx.
*/

using System;
using System.Collections.Generic;
using System.IO;
using Assets.Scripts.Lib.Net;

namespace Assets.Scripts.Proto
{
	public enum KS2C_Protocol
	{
		gs_client_connection_begin = 0,
		s2c_ping_signal = 1,
		s2c_sync_player_base_info = 2,
		s2c_sync_player_state_info = 3,
		s2c_account_kickout = 4,
		s2c_switch_gs = 5,
		s2c_switch_map = 6,
		s2c_talk_message = 7,
		s2c_hero_move = 8,
		s2c_battle_startedframe = 9,
		s2c_sync_scene_obj = 10,
		s2c_sync_scene_obj_end = 11,
		s2c_sync_scene_hero_data_end = 12,
		s2c_sync_new_hero = 13,
		s2c_sync_hero_data = 14,
		s2c_cast_skill = 15,
		s2c_skill_effect = 16,
		s2c_add_buff_notify = 17,
		s2c_del_buff_notify = 18,
		s2c_hero_death = 19,
		s2c_remove_scene_obj = 20,
		s2c_sync_hero_hpmp = 21,
		s2c_pvp_game_over = 22,
		s2c_sync_add_hp = 23,
		s2c_sync_add_mp = 24,
		s2c_sync_move_speed = 25,
		s2c_sync_max_hp = 26,
		s2c_sync_max_mp = 27,
		s2c_sync_attack_speed = 28,
		s2c_cast_skill_fail_notify = 29,
		s2c_move_fail_notify = 30,
		s2c_scene_obj_change_pos = 31,
		s2c_click_hero_respond = 32,
		s2c_downward_notify = 33,
		s2c_add_exp = 34,
		s2c_sync_item = 35,
		s2c_package_size = 36,
		s2c_update_item_amount = 37,
		s2c_destroy_item = 38,
		s2c_use_item = 39,
		s2c_part_item = 40,
		s2c_exchange_item = 41,
		s2c_sale_item_to_sys = 42,
		s2c_sync_equips = 43,
		s2c_sync_skill_data_list = 44,
		s2c_upgrade_skill_respond = 45,
		s2c_sync_skill_point = 46,
		s2c_sync_new_player_info = 47,
		s2c_enter_fight_notify = 48,
		s2c_leave_fight_notify = 49,
		s2c_start_hero_force_move = 50,
		s2c_stop_hero_force_move = 51,
		s2c_hero_relive = 52,
		s2c_sync_damage = 53,
		s2c_update_level = 54,
		s2c_hero_pos = 55,
		s2c_call_script = 56,
		s2c_sync_all_attribute = 57,
		s2c_sync_one_attribute = 58,
		s2c_sync_self_attribute = 59,
		gs_client_connection_end = 60,
	}

	public class S2C_HEADER: KProtoBuf
	{
		public ushort protocolID;

		public S2C_HEADER()
		{
			Register("protocolID", typeof(uint).ToString() , 2, 1);
		}
	}

	public class S2C_PING_SIGNAL: S2C_HEADER
	{
		public uint time;

		public S2C_PING_SIGNAL()
		{
			Register("time", typeof(uint).ToString() , 4, 1);
		}
	}

	public class S2C_SYNC_PLAYER_BASE_INFO: S2C_HEADER
	{
		public ulong uPlayerID;
		public int nLastSaveTime;
		public int nLastLoginTime;
		public int nTotalGameTime;
		public int nCreateTime;
		public sbyte byGender;
		public sbyte byCanRename;
		public string szAccountName = "";
		public string szPlayerName = "";
		public int nServerTime;
		public ulong uClubID;
		public sbyte byHeroJob;
		public int nGroupID;

		public S2C_SYNC_PLAYER_BASE_INFO()
		{
			Register("uPlayerID", typeof(uint).ToString() , 8, 1);
			Register("nLastSaveTime", typeof(int).ToString() , 4, 1);
			Register("nLastLoginTime", typeof(int).ToString() , 4, 1);
			Register("nTotalGameTime", typeof(int).ToString() , 4, 1);
			Register("nCreateTime", typeof(int).ToString() , 4, 1);
			Register("byGender", typeof(int).ToString() , 1, 1);
			Register("byCanRename", typeof(int).ToString() , 1, 1);
			Register("szAccountName", typeof(string).ToString() , 32, 1);
			Register("szPlayerName", typeof(string).ToString() , 32, 1);
			Register("nServerTime", typeof(int).ToString() , 4, 1);
			Register("uClubID", typeof(uint).ToString() , 8, 1);
			Register("byHeroJob", typeof(int).ToString() , 1, 1);
			Register("nGroupID", typeof(int).ToString() , 4, 1);
		}
	}

	public class S2C_SYNC_PLAYER_STATE_INFO: S2C_HEADER
	{
		public ushort uJob;
		public byte byVipLevel;
		public uint nVIPExp;
		public uint nVIPEndTime;
		public byte byLevel;
		public uint nExp;

		public S2C_SYNC_PLAYER_STATE_INFO()
		{
			Register("uJob", typeof(uint).ToString() , 2, 1);
			Register("byVipLevel", typeof(uint).ToString() , 1, 1);
			Register("nVIPExp", typeof(uint).ToString() , 4, 1);
			Register("nVIPEndTime", typeof(uint).ToString() , 4, 1);
			Register("byLevel", typeof(uint).ToString() , 1, 1);
			Register("nExp", typeof(uint).ToString() , 4, 1);
		}
	}

	public class S2C_ACCOUNT_KICKOUT: S2C_HEADER
	{

		public S2C_ACCOUNT_KICKOUT()
		{
		}
	}

	public class S2C_SWITCH_GS: S2C_HEADER
	{
		public ulong uPlayerID;
		public byte[] guid = new byte[16];
		public uint IPAddr;
		public ushort port;

		public S2C_SWITCH_GS()
		{
			Register("uPlayerID", typeof(uint).ToString() , 8, 1);
			Register("guid", typeof(uint).ToString() , 1, 16);
			Register("IPAddr", typeof(uint).ToString() , 4, 1);
			Register("port", typeof(uint).ToString() , 2, 1);
		}
	}

	public class S2C_SWITCH_MAP: S2C_HEADER
	{
		public uint mapID;
		public int copyIndex;

		public S2C_SWITCH_MAP()
		{
			Register("mapID", typeof(uint).ToString() , 4, 1);
			Register("copyIndex", typeof(int).ToString() , 4, 1);
		}
	}

	public class S2C_HERO_MOVE: S2C_HEADER
	{
		public uint heroID;
		public ushort posX;
		public ushort posY;
		public short posZ;
		public byte moveType;

		public S2C_HERO_MOVE()
		{
			Register("heroID", typeof(uint).ToString() , 4, 1);
			Register("posX", typeof(uint).ToString() , 2, 1);
			Register("posY", typeof(uint).ToString() , 2, 1);
			Register("posZ", typeof(int).ToString() , 2, 1);
			Register("moveType", typeof(uint).ToString() , 1, 1);
		}
	}

	public class KMailListInfo: KProtoBuf
	{
		public byte byType;
		public uint dwMailID;
		public string szSender = "";
		public string szTitle = "";
		public int nSendTime;
		public int nRecvTime;
		public byte bReadFlag;
		public ushort wMailFlag;
		public byte byItemType;
		public ushort wItemIndex;
		public byte byQuality;

		public KMailListInfo()
		{
			Register("byType", typeof(uint).ToString() , 1, 1);
			Register("dwMailID", typeof(uint).ToString() , 4, 1);
			Register("szSender", typeof(string).ToString() , 32, 1);
			Register("szTitle", typeof(string).ToString() , 64, 1);
			Register("nSendTime", typeof(int).ToString() , 4, 1);
			Register("nRecvTime", typeof(int).ToString() , 4, 1);
			Register("bReadFlag", typeof(uint).ToString() , 1, 1);
			Register("wMailFlag", typeof(uint).ToString() , 2, 1);
			Register("byItemType", typeof(uint).ToString() , 1, 1);
			Register("wItemIndex", typeof(uint).ToString() , 2, 1);
			Register("byQuality", typeof(uint).ToString() , 1, 1);
		}
	}

	public class KClubListInfo: KProtoBuf
	{
		public ulong uClubID;
		public string szName = "";
		public byte byLevel;
		public byte byMemberCount;
		public uint dwHonour;

		public KClubListInfo()
		{
			Register("uClubID", typeof(uint).ToString() , 8, 1);
			Register("szName", typeof(string).ToString() , 32, 1);
			Register("byLevel", typeof(uint).ToString() , 1, 1);
			Register("byMemberCount", typeof(uint).ToString() , 1, 1);
			Register("dwHonour", typeof(uint).ToString() , 4, 1);
		}
	}

	public class KMemberListInfo: KProtoBuf
	{
		public uint dwMemberID;
		public string szName = "";
		public byte byLevel;
		public uint dwTotalContribute;
		public byte byStatus;

		public KMemberListInfo()
		{
			Register("dwMemberID", typeof(uint).ToString() , 4, 1);
			Register("szName", typeof(string).ToString() , 32, 1);
			Register("byLevel", typeof(uint).ToString() , 1, 1);
			Register("dwTotalContribute", typeof(uint).ToString() , 4, 1);
			Register("byStatus", typeof(uint).ToString() , 1, 1);
		}
	}

	public class S2C_BATTLE_STARTEDFRAME: S2C_HEADER
	{
		public int startFrame;
		public int startedFrame;
		public int totalFrame;
		public int leftFrame;

		public S2C_BATTLE_STARTEDFRAME()
		{
			Register("startFrame", typeof(int).ToString() , 4, 1);
			Register("startedFrame", typeof(int).ToString() , 4, 1);
			Register("totalFrame", typeof(int).ToString() , 4, 1);
			Register("leftFrame", typeof(int).ToString() , 4, 1);
		}
	}

	public class S2C_SYNC_SCENE_OBJ_END: S2C_HEADER
	{

		public S2C_SYNC_SCENE_OBJ_END()
		{
		}
	}

	public class S2C_SYNC_SCENE_HERO_DATA_END: S2C_HEADER
	{

		public S2C_SYNC_SCENE_HERO_DATA_END()
		{
		}
	}

	public class KHeroBuffList: KProtoBuf
	{
		public byte bufferID;
		public byte overlapCount;

		public KHeroBuffList()
		{
			Register("bufferID", typeof(uint).ToString() , 1, 1);
			Register("overlapCount", typeof(uint).ToString() , 1, 1);
		}
	}

	public class S2C_SYNC_NEW_HERO: S2C_HEADER
	{
		public uint id;
		public ulong uOwnerID;
		public byte faceDir;
		public ushort posX;
		public ushort posY;
		public short posZ;
		public ushort desX;
		public ushort desY;
		public short desZ;
		public ushort wNpcIDOrJob;
		public ushort moveSpeed;
		public ushort attackSpeed;
		public byte moveState;
		public byte bNewHero;
		public uint HP;
		public uint MP;
		public uint MaxHP;
		public uint MaxMP;
		public uint[] equipIDs = new uint[16];
		public List<KHeroBuffList> bufferList = new List<KHeroBuffList>();

		public S2C_SYNC_NEW_HERO()
		{
			Register("id", typeof(uint).ToString() , 4, 1);
			Register("uOwnerID", typeof(uint).ToString() , 8, 1);
			Register("faceDir", typeof(uint).ToString() , 1, 1);
			Register("posX", typeof(uint).ToString() , 2, 1);
			Register("posY", typeof(uint).ToString() , 2, 1);
			Register("posZ", typeof(int).ToString() , 2, 1);
			Register("desX", typeof(uint).ToString() , 2, 1);
			Register("desY", typeof(uint).ToString() , 2, 1);
			Register("desZ", typeof(int).ToString() , 2, 1);
			Register("wNpcIDOrJob", typeof(uint).ToString() , 2, 1);
			Register("moveSpeed", typeof(uint).ToString() , 2, 1);
			Register("attackSpeed", typeof(uint).ToString() , 2, 1);
			Register("moveState", typeof(uint).ToString() , 1, 1);
			Register("bNewHero", typeof(uint).ToString() , 1, 1);
			Register("HP", typeof(uint).ToString() , 4, 1);
			Register("MP", typeof(uint).ToString() , 4, 1);
			Register("MaxHP", typeof(uint).ToString() , 4, 1);
			Register("MaxMP", typeof(uint).ToString() , 4, 1);
			Register("equipIDs", typeof(uint).ToString() , 4, 16);
			Register("bufferList", typeof(KHeroBuffList).ToString() , 0, 0);
		}
	}

	public class S2C_SYNC_HERO_DATA: S2C_HEADER
	{
		public ushort wJob;

		public S2C_SYNC_HERO_DATA()
		{
			Register("wJob", typeof(uint).ToString() , 2, 1);
		}
	}

	public class S2C_SKILL_EFFECT: S2C_HEADER
	{
		public uint wTargetHeroID;
		public uint wCasterID;
		public ushort wSkillID;
		public uint wDamage;
		public byte byAttackEvent;

		public S2C_SKILL_EFFECT()
		{
			Register("wTargetHeroID", typeof(uint).ToString() , 4, 1);
			Register("wCasterID", typeof(uint).ToString() , 4, 1);
			Register("wSkillID", typeof(uint).ToString() , 2, 1);
			Register("wDamage", typeof(uint).ToString() , 4, 1);
			Register("byAttackEvent", typeof(uint).ToString() , 1, 1);
		}
	}

	public class S2C_SYNC_DAMAGE: S2C_HEADER
	{
		public uint wTargetHeroID;
		public uint wDamage;

		public S2C_SYNC_DAMAGE()
		{
			Register("wTargetHeroID", typeof(uint).ToString() , 4, 1);
			Register("wDamage", typeof(uint).ToString() , 4, 1);
		}
	}

	public class S2C_CAST_SKILL: S2C_HEADER
	{
		public uint casterID;
		public ushort skillID;
		public uint targetID;
		public ushort x;
		public ushort y;
		public short z;

		public S2C_CAST_SKILL()
		{
			Register("casterID", typeof(uint).ToString() , 4, 1);
			Register("skillID", typeof(uint).ToString() , 2, 1);
			Register("targetID", typeof(uint).ToString() , 4, 1);
			Register("x", typeof(uint).ToString() , 2, 1);
			Register("y", typeof(uint).ToString() , 2, 1);
			Register("z", typeof(int).ToString() , 2, 1);
		}
	}

	public class S2C_ADD_BUFF_NOTIFY: S2C_HEADER
	{
		public uint herID;
		public ushort wBuffID;
		public byte byOverLap;

		public S2C_ADD_BUFF_NOTIFY()
		{
			Register("herID", typeof(uint).ToString() , 4, 1);
			Register("wBuffID", typeof(uint).ToString() , 2, 1);
			Register("byOverLap", typeof(uint).ToString() , 1, 1);
		}
	}

	public class S2C_DEL_BUFF_NOTIFY: S2C_HEADER
	{
		public uint herID;
		public ushort wBuffID;

		public S2C_DEL_BUFF_NOTIFY()
		{
			Register("herID", typeof(uint).ToString() , 4, 1);
			Register("wBuffID", typeof(uint).ToString() , 2, 1);
		}
	}

	public class S2C_DOWNWARD_NOTIFY: S2C_HEADER
	{
		public uint messageID;
		public MemoryStream param = new MemoryStream();

		public S2C_DOWNWARD_NOTIFY()
		{
			Register("messageID", typeof(uint).ToString() , 4, 1);
			Register("param", typeof(MemoryStream).ToString() , 0, 1);
		}
	}

	public class S2C_AUTOMATCH_RESPOND: S2C_HEADER
	{
		public byte eRetCode;
		public uint punishingMemberID;
		public ushort wLeftSeconds;

		public S2C_AUTOMATCH_RESPOND()
		{
			Register("eRetCode", typeof(uint).ToString() , 1, 1);
			Register("punishingMemberID", typeof(uint).ToString() , 4, 1);
			Register("wLeftSeconds", typeof(uint).ToString() , 2, 1);
		}
	}

	public class S2C_ENTER_LADDER_ROOM_NOTIFY: S2C_HEADER
	{
		public uint ladderRoomID;
		public uint mapID;
		public ushort countDownFrames;

		public S2C_ENTER_LADDER_ROOM_NOTIFY()
		{
			Register("ladderRoomID", typeof(uint).ToString() , 4, 1);
			Register("mapID", typeof(uint).ToString() , 4, 1);
			Register("countDownFrames", typeof(uint).ToString() , 2, 1);
		}
	}

	public class S2C_LEAVE_AUTOMATCH_NOTIFY: S2C_HEADER
	{
		public byte byLeaveReason;

		public S2C_LEAVE_AUTOMATCH_NOTIFY()
		{
			Register("byLeaveReason", typeof(uint).ToString() , 1, 1);
		}
	}

	public class S2C_LADDER_ROOM_DISSOVE_NOTIFY: S2C_HEADER
	{
		public byte byLeaveReason;
		public ulong uLeaverID;

		public S2C_LADDER_ROOM_DISSOVE_NOTIFY()
		{
			Register("byLeaveReason", typeof(uint).ToString() , 1, 1);
			Register("uLeaverID", typeof(uint).ToString() , 8, 1);
		}
	}

	public class S2C_HERO_DEATH: S2C_HEADER
	{
		public uint heroID;
		public uint KillerID;

		public S2C_HERO_DEATH()
		{
			Register("heroID", typeof(uint).ToString() , 4, 1);
			Register("KillerID", typeof(uint).ToString() , 4, 1);
		}
	}

	public class S2C_REMOVE_SCENE_OBJ: S2C_HEADER
	{
		public uint objID;

		public S2C_REMOVE_SCENE_OBJ()
		{
			Register("objID", typeof(uint).ToString() , 4, 1);
		}
	}

	public class S2C_SCENE_OBJ_CHANGE_POS: S2C_HEADER
	{
		public uint objID;
		public ushort posX;
		public ushort posY;
		public short posZ;

		public S2C_SCENE_OBJ_CHANGE_POS()
		{
			Register("objID", typeof(uint).ToString() , 4, 1);
			Register("posX", typeof(uint).ToString() , 2, 1);
			Register("posY", typeof(uint).ToString() , 2, 1);
			Register("posZ", typeof(int).ToString() , 2, 1);
		}
	}

	public class S2C_SYNC_HERO_HPMP: S2C_HEADER
	{
		public uint heroID;
		public int hp;
		public int mp;

		public S2C_SYNC_HERO_HPMP()
		{
			Register("heroID", typeof(uint).ToString() , 4, 1);
			Register("hp", typeof(int).ToString() , 4, 1);
			Register("mp", typeof(int).ToString() , 4, 1);
		}
	}

	public class S2C_PVP_GAME_OVER: S2C_HEADER
	{
		public byte byWinSide;
		public int nWinScore;
		public int nLoseScore;
		public sbyte byAddWinScore;
		public sbyte byAddLoseScore;

		public S2C_PVP_GAME_OVER()
		{
			Register("byWinSide", typeof(uint).ToString() , 1, 1);
			Register("nWinScore", typeof(int).ToString() , 4, 1);
			Register("nLoseScore", typeof(int).ToString() , 4, 1);
			Register("byAddWinScore", typeof(int).ToString() , 1, 1);
			Register("byAddLoseScore", typeof(int).ToString() , 1, 1);
		}
	}

	public class S2C_SYNC_ADD_HP: S2C_HEADER
	{
		public uint heroID;
		public int hp;

		public S2C_SYNC_ADD_HP()
		{
			Register("heroID", typeof(uint).ToString() , 4, 1);
			Register("hp", typeof(int).ToString() , 4, 1);
		}
	}

	public class S2C_SYNC_ADD_MP: S2C_HEADER
	{
		public uint heroID;
		public int mp;

		public S2C_SYNC_ADD_MP()
		{
			Register("heroID", typeof(uint).ToString() , 4, 1);
			Register("mp", typeof(int).ToString() , 4, 1);
		}
	}

	public class S2C_SYNC_MOVE_SPEED: S2C_HEADER
	{
		public uint heroID;
		public ushort moveSpeed;

		public S2C_SYNC_MOVE_SPEED()
		{
			Register("heroID", typeof(uint).ToString() , 4, 1);
			Register("moveSpeed", typeof(uint).ToString() , 2, 1);
		}
	}

	public class S2C_SYNC_MAX_HP: S2C_HEADER
	{
		public uint heroID;
		public uint MaxHP;

		public S2C_SYNC_MAX_HP()
		{
			Register("heroID", typeof(uint).ToString() , 4, 1);
			Register("MaxHP", typeof(uint).ToString() , 4, 1);
		}
	}

	public class S2C_SYNC_MAX_MP: S2C_HEADER
	{
		public uint heroID;
		public uint MaxMP;

		public S2C_SYNC_MAX_MP()
		{
			Register("heroID", typeof(uint).ToString() , 4, 1);
			Register("MaxMP", typeof(uint).ToString() , 4, 1);
		}
	}

	public class S2C_CAST_SKILL_FAIL_NOTIFY: S2C_HEADER
	{
		public ushort skillID;

		public S2C_CAST_SKILL_FAIL_NOTIFY()
		{
			Register("skillID", typeof(uint).ToString() , 2, 1);
		}
	}

	public class S2C_MOVE_FAIL_NOTIFY: S2C_HEADER
	{
		public uint heroID;

		public S2C_MOVE_FAIL_NOTIFY()
		{
			Register("heroID", typeof(uint).ToString() , 4, 1);
		}
	}

	public class S2C_TALK_MESSAGE: S2C_HEADER
	{
		public byte byMsgType;
		public ulong uTalkerID;
		public uint uHeroID;
		public MemoryStream talkdata = new MemoryStream();

		public S2C_TALK_MESSAGE()
		{
			Register("byMsgType", typeof(uint).ToString() , 1, 1);
			Register("uTalkerID", typeof(uint).ToString() , 8, 1);
			Register("uHeroID", typeof(uint).ToString() , 4, 1);
			Register("talkdata", typeof(MemoryStream).ToString() , 0, 1);
		}
	}

	public class S2C_SYNC_SCENE_OBJ: S2C_HEADER
	{
		public uint id;
		public byte type;
		public ushort x;
		public ushort y;
		public ushort z;
		public int customData0;
		public int customData1;
		public int customData2;
		public int customData3;

		public S2C_SYNC_SCENE_OBJ()
		{
			Register("id", typeof(uint).ToString() , 4, 1);
			Register("type", typeof(uint).ToString() , 1, 1);
			Register("x", typeof(uint).ToString() , 2, 1);
			Register("y", typeof(uint).ToString() , 2, 1);
			Register("z", typeof(uint).ToString() , 2, 1);
			Register("customData0", typeof(int).ToString() , 4, 1);
			Register("customData1", typeof(int).ToString() , 4, 1);
			Register("customData2", typeof(int).ToString() , 4, 1);
			Register("customData3", typeof(int).ToString() , 4, 1);
		}
	}

	public class S2C_CLICK_HERO_RESPOND: S2C_HEADER
	{
		public ushort wTemplateID;

		public S2C_CLICK_HERO_RESPOND()
		{
			Register("wTemplateID", typeof(uint).ToString() , 2, 1);
		}
	}

	public class S2C_ADD_EXP: S2C_HEADER
	{
		public uint addExp;

		public S2C_ADD_EXP()
		{
			Register("addExp", typeof(uint).ToString() , 4, 1);
		}
	}

	public class S2C_SYNC_ITEM: S2C_HEADER
	{
		public ulong dwPlayerID;
		public ushort byPackageType;
		public ushort byPackageIndex;
		public ushort byPos;
		public ushort byTabType;
		public uint dwTabIndex;
		public ushort byBind;
		public uint uGenTime;
		public uint uStackNum;
		public uint uLevel;
		public uint uStrengthenLevel;
		public uint uHole;

		public S2C_SYNC_ITEM()
		{
			Register("dwPlayerID", typeof(uint).ToString() , 8, 1);
			Register("byPackageType", typeof(uint).ToString() , 2, 1);
			Register("byPackageIndex", typeof(uint).ToString() , 2, 1);
			Register("byPos", typeof(uint).ToString() , 2, 1);
			Register("byTabType", typeof(uint).ToString() , 2, 1);
			Register("dwTabIndex", typeof(uint).ToString() , 4, 1);
			Register("byBind", typeof(uint).ToString() , 2, 1);
			Register("uGenTime", typeof(uint).ToString() , 4, 1);
			Register("uStackNum", typeof(uint).ToString() , 4, 1);
			Register("uLevel", typeof(uint).ToString() , 4, 1);
			Register("uStrengthenLevel", typeof(uint).ToString() , 4, 1);
			Register("uHole", typeof(uint).ToString() , 4, 1);
		}
	}

	public class S2C_PACKAGE_SIZE: S2C_HEADER
	{
		public ushort byPackageType;
		public ushort byPackageSize;

		public S2C_PACKAGE_SIZE()
		{
			Register("byPackageType", typeof(uint).ToString() , 2, 1);
			Register("byPackageSize", typeof(uint).ToString() , 2, 1);
		}
	}

	public class S2C_UPDATE_ITEM_AMOUNT: S2C_HEADER
	{
		public ushort byPackageType;
		public ushort byPackageIndex;
		public ushort byPos;
		public uint dwStack;

		public S2C_UPDATE_ITEM_AMOUNT()
		{
			Register("byPackageType", typeof(uint).ToString() , 2, 1);
			Register("byPackageIndex", typeof(uint).ToString() , 2, 1);
			Register("byPos", typeof(uint).ToString() , 2, 1);
			Register("dwStack", typeof(uint).ToString() , 4, 1);
		}
	}

	public class S2C_DESTROY_ITEM: S2C_HEADER
	{
		public ushort byPackageType;
		public ushort byPackageIndex;
		public ushort byPos;

		public S2C_DESTROY_ITEM()
		{
			Register("byPackageType", typeof(uint).ToString() , 2, 1);
			Register("byPackageIndex", typeof(uint).ToString() , 2, 1);
			Register("byPos", typeof(uint).ToString() , 2, 1);
		}
	}

	public class S2C_USE_ITEM: S2C_HEADER
	{
		public ushort byPackageType;
		public ushort byPackageIndex;
		public ushort byPos;
		public byte bResult;

		public S2C_USE_ITEM()
		{
			Register("byPackageType", typeof(uint).ToString() , 2, 1);
			Register("byPackageIndex", typeof(uint).ToString() , 2, 1);
			Register("byPos", typeof(uint).ToString() , 2, 1);
			Register("bResult", typeof(uint).ToString() , 1, 1);
		}
	}

	public class S2C_PART_ITEM: S2C_HEADER
	{
		public ushort byPackageType;
		public ushort byPackageIndex;
		public ushort byPos;
		public byte bResult;

		public S2C_PART_ITEM()
		{
			Register("byPackageType", typeof(uint).ToString() , 2, 1);
			Register("byPackageIndex", typeof(uint).ToString() , 2, 1);
			Register("byPos", typeof(uint).ToString() , 2, 1);
			Register("bResult", typeof(uint).ToString() , 1, 1);
		}
	}

	public class S2C_EXCHANGE_ITEM: S2C_HEADER
	{
		public byte bResultCode;
		public ushort bySrcPackageType;
		public ushort bySrcPackageIndex;
		public ushort bySrcPos;
		public ushort byTargetPackageType;
		public ushort byTargetPackageIndex;
		public ushort byTargetPos;
		public ushort byExchangeResult;

		public S2C_EXCHANGE_ITEM()
		{
			Register("bResultCode", typeof(uint).ToString() , 1, 1);
			Register("bySrcPackageType", typeof(uint).ToString() , 2, 1);
			Register("bySrcPackageIndex", typeof(uint).ToString() , 2, 1);
			Register("bySrcPos", typeof(uint).ToString() , 2, 1);
			Register("byTargetPackageType", typeof(uint).ToString() , 2, 1);
			Register("byTargetPackageIndex", typeof(uint).ToString() , 2, 1);
			Register("byTargetPos", typeof(uint).ToString() , 2, 1);
			Register("byExchangeResult", typeof(uint).ToString() , 2, 1);
		}
	}

	public class S2C_SALE_ITEM_TO_SYS: S2C_HEADER
	{
		public ushort byPackageType;
		public ushort byPackageIndex;
		public ushort byPos;
		public byte bResult;
		public uint nMoneyValue;

		public S2C_SALE_ITEM_TO_SYS()
		{
			Register("byPackageType", typeof(uint).ToString() , 2, 1);
			Register("byPackageIndex", typeof(uint).ToString() , 2, 1);
			Register("byPos", typeof(uint).ToString() , 2, 1);
			Register("bResult", typeof(uint).ToString() , 1, 1);
			Register("nMoneyValue", typeof(uint).ToString() , 4, 1);
		}
	}

	public class S2C_SYNC_EQUIPS: S2C_HEADER
	{
		public uint uHeroID;
		public uint[] equips = new uint[16];

		public S2C_SYNC_EQUIPS()
		{
			Register("uHeroID", typeof(uint).ToString() , 4, 1);
			Register("equips", typeof(uint).ToString() , 4, 16);
		}
	}

	public class S2C_UPGRADE_SKILL_RESPOND: S2C_HEADER
	{
		public byte IsActiveSkill;
		public ushort SkillID;
		public byte Sucess;

		public S2C_UPGRADE_SKILL_RESPOND()
		{
			Register("IsActiveSkill", typeof(uint).ToString() , 1, 1);
			Register("SkillID", typeof(uint).ToString() , 2, 1);
			Register("Sucess", typeof(uint).ToString() , 1, 1);
		}
	}

	public class S2C_SYNC_SKILL_DATA_LIST: S2C_HEADER
	{
		public MemoryStream SkillDataList = new MemoryStream();

		public S2C_SYNC_SKILL_DATA_LIST()
		{
			Register("SkillDataList", typeof(MemoryStream).ToString() , 0, 1);
		}
	}

	public class S2C_SYNC_SKILL_POINT: S2C_HEADER
	{
		public ushort SkillPoint;

		public S2C_SYNC_SKILL_POINT()
		{
			Register("SkillPoint", typeof(uint).ToString() , 2, 1);
		}
	}

	public class S2C_SYNC_NEW_PLAYER_INFO: S2C_HEADER
	{
		public ulong playerID;
		public string szPlayerName = "";
		public byte byGender;

		public S2C_SYNC_NEW_PLAYER_INFO()
		{
			Register("playerID", typeof(uint).ToString() , 8, 1);
			Register("szPlayerName", typeof(string).ToString() , 32, 1);
			Register("byGender", typeof(uint).ToString() , 1, 1);
		}
	}

	public class S2C_START_HERO_FORCE_MOVE: S2C_HEADER
	{
		public uint heroID;

		public S2C_START_HERO_FORCE_MOVE()
		{
			Register("heroID", typeof(uint).ToString() , 4, 1);
		}
	}

	public class S2C_STOP_HERO_FORCE_MOVE: S2C_HEADER
	{
		public uint heroID;

		public S2C_STOP_HERO_FORCE_MOVE()
		{
			Register("heroID", typeof(uint).ToString() , 4, 1);
		}
	}

	public class S2C_SYNC_ATTACK_SPEED: S2C_HEADER
	{
		public uint heroID;
		public ushort attackSpeed;

		public S2C_SYNC_ATTACK_SPEED()
		{
			Register("heroID", typeof(uint).ToString() , 4, 1);
			Register("attackSpeed", typeof(uint).ToString() , 2, 1);
		}
	}

	public class S2C_HERO_RELIVE: S2C_HEADER
	{
		public uint heroID;

		public S2C_HERO_RELIVE()
		{
			Register("heroID", typeof(uint).ToString() , 4, 1);
		}
	}

	public class S2C_UPDATE_LEVEL: S2C_HEADER
	{
		public ushort nlevel;

		public S2C_UPDATE_LEVEL()
		{
			Register("nlevel", typeof(uint).ToString() , 2, 1);
		}
	}

	public class S2C_HERO_POS: S2C_HEADER
	{
		public uint heroID;
		public ushort posX;
		public ushort posY;
		public short posZ;

		public S2C_HERO_POS()
		{
			Register("heroID", typeof(uint).ToString() , 4, 1);
			Register("posX", typeof(uint).ToString() , 2, 1);
			Register("posY", typeof(uint).ToString() , 2, 1);
			Register("posZ", typeof(int).ToString() , 2, 1);
		}
	}

	public class S2C_ENTER_FIGHT_NOTIFY: S2C_HEADER
	{

		public S2C_ENTER_FIGHT_NOTIFY()
		{
		}
	}

	public class S2C_LEAVE_FIGHT_NOTIFY: S2C_HEADER
	{

		public S2C_LEAVE_FIGHT_NOTIFY()
		{
		}
	}

	public class S2C_CALL_SCRIPT: S2C_HEADER
	{
		public MemoryStream data = new MemoryStream();

		public S2C_CALL_SCRIPT()
		{
			Register("data", typeof(MemoryStream).ToString() , 0, 1);
		}
	}

	public class S2C_SYNC_ALL_ATTRIBUTE: S2C_HEADER
	{
		public int MaxHP;
		public int MaxMP;
		public int Attack;
		public int Defence;
		public int Miss;
		public int Crit;
		public int ReduceCrit;
		public int CritHurt;
		public int ReduceCritHurt;
		public int AttackSpeed;
		public int MoveSpeed;
		public int HpRecover;
		public int MpRecover;
		public int Reflex;
		public int ReduceDefence;
		public int DamageMore;
		public int DamageLess;
		public int ReduceDamage;
		public int ExtDamage;
		public int DamageBack;
		public int AttackRecover;
		public int UpAttack;

		public S2C_SYNC_ALL_ATTRIBUTE()
		{
			Register("MaxHP", typeof(int).ToString() , 4, 1);
			Register("MaxMP", typeof(int).ToString() , 4, 1);
			Register("Attack", typeof(int).ToString() , 4, 1);
			Register("Defence", typeof(int).ToString() , 4, 1);
			Register("Miss", typeof(int).ToString() , 4, 1);
			Register("Crit", typeof(int).ToString() , 4, 1);
			Register("ReduceCrit", typeof(int).ToString() , 4, 1);
			Register("CritHurt", typeof(int).ToString() , 4, 1);
			Register("ReduceCritHurt", typeof(int).ToString() , 4, 1);
			Register("AttackSpeed", typeof(int).ToString() , 4, 1);
			Register("MoveSpeed", typeof(int).ToString() , 4, 1);
			Register("HpRecover", typeof(int).ToString() , 4, 1);
			Register("MpRecover", typeof(int).ToString() , 4, 1);
			Register("Reflex", typeof(int).ToString() , 4, 1);
			Register("ReduceDefence", typeof(int).ToString() , 4, 1);
			Register("DamageMore", typeof(int).ToString() , 4, 1);
			Register("DamageLess", typeof(int).ToString() , 4, 1);
			Register("ReduceDamage", typeof(int).ToString() , 4, 1);
			Register("ExtDamage", typeof(int).ToString() , 4, 1);
			Register("DamageBack", typeof(int).ToString() , 4, 1);
			Register("AttackRecover", typeof(int).ToString() , 4, 1);
			Register("UpAttack", typeof(int).ToString() , 4, 1);
		}
	}

	public class S2C_SYNC_ONE_ATTRIBUTE: S2C_HEADER
	{
		public uint HeroID;
		public byte AttributeType;
		public int AttributeValue;

		public S2C_SYNC_ONE_ATTRIBUTE()
		{
			Register("HeroID", typeof(uint).ToString() , 4, 1);
			Register("AttributeType", typeof(uint).ToString() , 1, 1);
			Register("AttributeValue", typeof(int).ToString() , 4, 1);
		}
	}

	public class S2C_SYNC_SELF_ATTRIBUTE: S2C_HEADER
	{
		public byte AttributeType;
		public int AttributeValue;

		public S2C_SYNC_SELF_ATTRIBUTE()
		{
			Register("AttributeType", typeof(uint).ToString() , 1, 1);
			Register("AttributeValue", typeof(int).ToString() , 4, 1);
		}
	}

}
