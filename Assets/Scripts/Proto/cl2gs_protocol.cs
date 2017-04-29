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
	public enum KProcotolVersion
	{
		eProcotolVersion = 1,
	}

	public enum KC2S_Protocol
	{
		client_gs_connection_begin = 0,
		c2s_handshake_request = 1,
		c2s_loading_complete = 2,
		c2s_apply_scene_obj = 3,
		c2s_apply_scene_hero_data = 4,
		c2s_player_logout = 5,
		c2s_ping_signal = 6,
		c2s_hero_move = 7,
		c2s_apply_create_room = 8,
		c2s_apply_join_room = 9,
		c2s_invite_player_join_room = 10,
		c2s_apply_leave_room = 11,
		c2s_auto_joinroom = 12,
		c2s_accept_or_refuse_join_room = 13,
		c2s_gm_cmd = 14,
		c2s_talk_message = 15,
		c2s_click_hero = 16,
		c2s_use_item = 17,
		c2s_use_item_by_num = 18,
		c2s_sale_item_to_sys = 19,
		c2s_exchange_item = 20,
		c2s_part_item = 21,
		c2s_sort_out_item = 22,
		c2s_upgrade_skill_request = 23,
		c2s_cast_skill = 24,
		c2s_relive_request = 25,
		c2s_exit_pvemap_request = 26,
		c2s_pickup_item_request = 27,
		c2s_call_gs = 28,
		c2s_call_ls = 29,
		c2s_call_player = 30,
		client_gs_connection_end = 31,
	}

	public class C2S_HEADER: KProtoBuf
	{
		public ushort protocolID;

		public C2S_HEADER()
		{
			Register("protocolID", typeof(uint).ToString() , 2, 1);
		}
	}

	public class C2S_HANDSHAKE_REQUEST: C2S_HEADER
	{
		public ulong uRoleID;
		public byte procotolversion;
		public MemoryStream guid = new MemoryStream();

		public C2S_HANDSHAKE_REQUEST()
		{
			Register("uRoleID", typeof(uint).ToString() , 8, 1);
			Register("procotolversion", typeof(uint).ToString() , 1, 1);
			Register("guid", typeof(MemoryStream).ToString() , 16, 1);
		}
	}

	public class C2S_LOADING_COMPLETE: C2S_HEADER
	{

		public C2S_LOADING_COMPLETE()
		{
		}
	}

	public class C2S_APPLY_SCENE_OBJ: C2S_HEADER
	{

		public C2S_APPLY_SCENE_OBJ()
		{
		}
	}

	public class C2S_APPLY_SCENE_HERO_DATA: C2S_HEADER
	{

		public C2S_APPLY_SCENE_HERO_DATA()
		{
		}
	}

	public class C2S_PLAYER_LOGOUT: C2S_HEADER
	{

		public C2S_PLAYER_LOGOUT()
		{
		}
	}

	public class C2S_PING_SIGNAL: C2S_HEADER
	{
		public uint time;

		public C2S_PING_SIGNAL()
		{
			Register("time", typeof(uint).ToString() , 4, 1);
		}
	}

	public class C2S_HERO_MOVE: C2S_HEADER
	{
		public uint heroID;
		public ushort posX;
		public ushort posY;
		public short posZ;

		public C2S_HERO_MOVE()
		{
			Register("heroID", typeof(uint).ToString() , 4, 1);
			Register("posX", typeof(uint).ToString() , 2, 1);
			Register("posY", typeof(uint).ToString() , 2, 1);
			Register("posZ", typeof(int).ToString() , 2, 1);
		}
	}

	public class C2S_GM_CMD: C2S_HEADER
	{
		public string command = "";

		public C2S_GM_CMD()
		{
			Register("command", typeof(string).ToString() , 0, 1);
		}
	}

	public class C2S_TALK_MESSAGE: C2S_HEADER
	{
		public byte byMsgType;
		public string szReceiver = "";
		public string talkdata = "";

		public C2S_TALK_MESSAGE()
		{
			Register("byMsgType", typeof(uint).ToString() , 1, 1);
			Register("szReceiver", typeof(string).ToString() , 32, 1);
			Register("talkdata", typeof(string).ToString() , 0, 1);
		}
	}

	public class C2S_CLICK_HERO: C2S_HEADER
	{
		public uint heroID;

		public C2S_CLICK_HERO()
		{
			Register("heroID", typeof(uint).ToString() , 4, 1);
		}
	}

	public class C2S_USE_ITEM: C2S_HEADER
	{
		public ushort byPackageType;
		public ushort byPackageIndex;
		public ushort byPos;

		public C2S_USE_ITEM()
		{
			Register("byPackageType", typeof(uint).ToString() , 2, 1);
			Register("byPackageIndex", typeof(uint).ToString() , 2, 1);
			Register("byPos", typeof(uint).ToString() , 2, 1);
		}
	}

	public class C2S_USE_ITEM_BY_NUM: C2S_HEADER
	{
		public ushort byPackageType;
		public ushort byPackageIndex;
		public ushort byPos;
		public ushort stackNum;

		public C2S_USE_ITEM_BY_NUM()
		{
			Register("byPackageType", typeof(uint).ToString() , 2, 1);
			Register("byPackageIndex", typeof(uint).ToString() , 2, 1);
			Register("byPos", typeof(uint).ToString() , 2, 1);
			Register("stackNum", typeof(uint).ToString() , 2, 1);
		}
	}

	public class C2S_PART_ITEM: C2S_HEADER
	{
		public ushort byPackageType;
		public ushort byPackageIndex;
		public ushort byPos;
		public ushort stackNum;

		public C2S_PART_ITEM()
		{
			Register("byPackageType", typeof(uint).ToString() , 2, 1);
			Register("byPackageIndex", typeof(uint).ToString() , 2, 1);
			Register("byPos", typeof(uint).ToString() , 2, 1);
			Register("stackNum", typeof(uint).ToString() , 2, 1);
		}
	}

	public class C2S_SALE_ITEM_TO_SYS: C2S_HEADER
	{
		public ushort byPackageType;
		public ushort byPackageIndex;
		public ushort byPos;

		public C2S_SALE_ITEM_TO_SYS()
		{
			Register("byPackageType", typeof(uint).ToString() , 2, 1);
			Register("byPackageIndex", typeof(uint).ToString() , 2, 1);
			Register("byPos", typeof(uint).ToString() , 2, 1);
		}
	}

	public class C2S_SORT_OUT_ITEM: C2S_HEADER
	{
		public ushort byPackageIndex;
		public MemoryStream itemPositionDataList = new MemoryStream();

		public C2S_SORT_OUT_ITEM()
		{
			Register("byPackageIndex", typeof(uint).ToString() , 2, 1);
			Register("itemPositionDataList", typeof(MemoryStream).ToString() , 0, 1);
		}
	}

	public class C2S_EXCHANGE_ITEM: C2S_HEADER
	{
		public ushort bySrcPackageType;
		public ushort bySrcPackageIndex;
		public ushort bySrcPos;
		public ushort byTargetPackageType;
		public ushort byTargetPackageIndex;
		public ushort byTargetPos;

		public C2S_EXCHANGE_ITEM()
		{
			Register("bySrcPackageType", typeof(uint).ToString() , 2, 1);
			Register("bySrcPackageIndex", typeof(uint).ToString() , 2, 1);
			Register("bySrcPos", typeof(uint).ToString() , 2, 1);
			Register("byTargetPackageType", typeof(uint).ToString() , 2, 1);
			Register("byTargetPackageIndex", typeof(uint).ToString() , 2, 1);
			Register("byTargetPos", typeof(uint).ToString() , 2, 1);
		}
	}

	public class C2S_UPGRADE_SKILL_REQUEST: C2S_HEADER
	{
		public byte IsActiveSkill;
		public ushort SkillID;

		public C2S_UPGRADE_SKILL_REQUEST()
		{
			Register("IsActiveSkill", typeof(uint).ToString() , 1, 1);
			Register("SkillID", typeof(uint).ToString() , 2, 1);
		}
	}

	public class C2S_CAST_SKILL: C2S_HEADER
	{
		public uint casterID;
		public uint targetID;
		public ushort skillID;
		public ushort x;
		public ushort y;
		public short z;

		public C2S_CAST_SKILL()
		{
			Register("casterID", typeof(uint).ToString() , 4, 1);
			Register("targetID", typeof(uint).ToString() , 4, 1);
			Register("skillID", typeof(uint).ToString() , 2, 1);
			Register("x", typeof(uint).ToString() , 2, 1);
			Register("y", typeof(uint).ToString() , 2, 1);
			Register("z", typeof(int).ToString() , 2, 1);
		}
	}

	public class C2S_EXIT_PVEMAP_REQUEST: C2S_HEADER
	{

		public C2S_EXIT_PVEMAP_REQUEST()
		{
		}
	}

	public class C2S_RELIVE_REQUEST: C2S_HEADER
	{
		public byte reliveSelect;

		public C2S_RELIVE_REQUEST()
		{
			Register("reliveSelect", typeof(uint).ToString() , 1, 1);
		}
	}

	public class C2S_PICKUP_ITEM_REQUEST: C2S_HEADER
	{
		public uint dwItemID;

		public C2S_PICKUP_ITEM_REQUEST()
		{
			Register("dwItemID", typeof(uint).ToString() , 4, 1);
		}
	}

	public class C2S_CALL_GS: C2S_HEADER
	{
		public MemoryStream data = new MemoryStream();

		public C2S_CALL_GS()
		{
			Register("data", typeof(MemoryStream).ToString() , 0, 1);
		}
	}

	public class C2S_CALL_LS: C2S_HEADER
	{
		public MemoryStream data = new MemoryStream();

		public C2S_CALL_LS()
		{
			Register("data", typeof(MemoryStream).ToString() , 0, 1);
		}
	}

	public class C2S_CALL_PLAYER: C2S_HEADER
	{
		public ulong uPlayerID;
		public MemoryStream data = new MemoryStream();

		public C2S_CALL_PLAYER()
		{
			Register("uPlayerID", typeof(uint).ToString() , 8, 1);
			Register("data", typeof(MemoryStream).ToString() , 0, 1);
		}
	}

}
