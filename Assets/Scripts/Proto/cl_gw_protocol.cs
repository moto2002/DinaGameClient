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
	public enum KC2G_Protocol
	{
		c2g_protocol_begin = 1,
		c2g_ping_signal = 2,
		c2g_handshake_request = 3,
		c2g_account_verify_request = 4,
		c2g_check_role_exist_request = 5,
		c2g_create_role_request = 6,
		c2g_delete_role_request = 7,
		c2g_rename_request = 8,
		c2g_mibao_verify_request = 9,
		c2g_protocol_total = 10,
	}

	public enum KG2C_Protocol
	{
		g2c_protocol_begin = 1,
		g2c_ping_respond = 2,
		g2c_handshake_respond = 3,
		g2c_account_locked_notify = 4,
		g2c_account_verify_respond = 5,
		g2c_kick_account_notify = 6,
		g2c_sync_role_list = 7,
		g2c_check_role_exist_respond = 8,
		g2c_create_role_notify = 9,
		g2c_create_role_respond = 10,
		g2c_delete_role_respond = 11,
		g2c_sync_login_key = 12,
		g2c_webgateway_verify_notify = 13,
		g2c_protocol_total = 14,
	}

	public class KGC_PROTOCOL_HEADER: KProtoBuf
	{
		public byte byProtocol;

		public KGC_PROTOCOL_HEADER()
		{
			Register("byProtocol", typeof(uint).ToString() , 1, 1);
		}
	}

	public enum KG_NET_TYPE
	{
		NET_TYPE_DEFAULT = 0,
		NET_TYPE_TELECOM = 1,
		NET_TYPE_NETCOM = 2,
		NET_TYPE_UNICOM = 3,
		NET_TYPE_MOBILE = 4,
		NET_TYPE_CRC = 5,
		NET_TYPE_SATCOM = 6,
	}

	public class KC2G_HandshakeRequest: KGC_PROTOCOL_HEADER
	{
		public int nProtocolVersion;
		public int nGroupID;
		public byte uNetType;

		public KC2G_HandshakeRequest()
		{
			Register("nProtocolVersion", typeof(int).ToString() , 4, 1);
			Register("nGroupID", typeof(int).ToString() , 4, 1);
			Register("uNetType", typeof(uint).ToString() , 1, 1);
		}
	}

	public class KC2G_PingSignal: KGC_PROTOCOL_HEADER
	{
		public uint dwTime;

		public KC2G_PingSignal()
		{
			Register("dwTime", typeof(uint).ToString() , 4, 1);
		}
	}

	public class KC2G_AccountVerifyRequest: KGC_PROTOCOL_HEADER
	{
		public int nGroupID;
		public string account = "";
		public string password = "";

		public KC2G_AccountVerifyRequest()
		{
			Register("nGroupID", typeof(int).ToString() , 4, 1);
			Register("account", typeof(string).ToString() , 32, 1);
			Register("password", typeof(string).ToString() , 64, 1);
		}
	}

	public class KC2G_Check_Role_Exist_Request: KGC_PROTOCOL_HEADER
	{
		public string szRoleName = "";

		public KC2G_Check_Role_Exist_Request()
		{
			Register("szRoleName", typeof(string).ToString() , 32, 1);
		}
	}

	public class KC2G_CreateRoleRequest: KGC_PROTOCOL_HEADER
	{
		public string szRoleName = "";
		public sbyte byGender;
		public int nMainHeroTemplateID;

		public KC2G_CreateRoleRequest()
		{
			Register("szRoleName", typeof(string).ToString() , 32, 1);
			Register("byGender", typeof(int).ToString() , 1, 1);
			Register("nMainHeroTemplateID", typeof(int).ToString() , 4, 1);
		}
	}

	public class KC2G_MibaoVerifyRequest: KGC_PROTOCOL_HEADER
	{
		public string szMibaoPassword = "";

		public KC2G_MibaoVerifyRequest()
		{
			Register("szMibaoPassword", typeof(string).ToString() , 10, 1);
		}
	}

	public class KG2C_PingRespond: KGC_PROTOCOL_HEADER
	{
		public uint time;

		public KG2C_PingRespond()
		{
			Register("time", typeof(uint).ToString() , 4, 1);
		}
	}

	public class KG2C_AccountLockedNotify: KGC_PROTOCOL_HEADER
	{
		public byte nothing;

		public KG2C_AccountLockedNotify()
		{
			Register("nothing", typeof(uint).ToString() , 1, 1);
		}
	}

	public class KG2C_AccountVerifyRespond: KGC_PROTOCOL_HEADER
	{
		public int code;
		public int loginTime;
		public int lastLoginTime;
		public uint lastLoginIP;
		public int zoneID;
		public uint limitPlayTimeFlag;
		public uint limitPlayOnlineSeconds;
		public uint limitPlayOfflineSeconds;

		public KG2C_AccountVerifyRespond()
		{
			Register("code", typeof(int).ToString() , 4, 1);
			Register("loginTime", typeof(int).ToString() , 4, 1);
			Register("lastLoginTime", typeof(int).ToString() , 4, 1);
			Register("lastLoginIP", typeof(uint).ToString() , 4, 1);
			Register("zoneID", typeof(int).ToString() , 4, 1);
			Register("limitPlayTimeFlag", typeof(uint).ToString() , 4, 1);
			Register("limitPlayOnlineSeconds", typeof(uint).ToString() , 4, 1);
			Register("limitPlayOfflineSeconds", typeof(uint).ToString() , 4, 1);
		}
	}

	public class KG2C_SyncLoginKey: KGC_PROTOCOL_HEADER
	{
		public byte code;
		public ulong uRoleID;
		public uint ip;
		public int port;
		public MemoryStream guid = new MemoryStream();

		public KG2C_SyncLoginKey()
		{
			Register("code", typeof(uint).ToString() , 1, 1);
			Register("uRoleID", typeof(uint).ToString() , 8, 1);
			Register("ip", typeof(uint).ToString() , 4, 1);
			Register("port", typeof(int).ToString() , 4, 1);
			Register("guid", typeof(MemoryStream).ToString() , 16, 1);
		}
	}

	public class KG2C_KickAccountNotify: KGC_PROTOCOL_HEADER
	{
		public byte nothing;

		public KG2C_KickAccountNotify()
		{
			Register("nothing", typeof(uint).ToString() , 1, 1);
		}
	}

	public class KG2C_CheckRoleNameExistRespond: KGC_PROTOCOL_HEADER
	{
		public byte byExist;
		public string szRoleName = "";

		public KG2C_CheckRoleNameExistRespond()
		{
			Register("byExist", typeof(uint).ToString() , 1, 1);
			Register("szRoleName", typeof(string).ToString() , 32, 1);
		}
	}

	public class KG2C_CreateRoleNotify: KGC_PROTOCOL_HEADER
	{

		public KG2C_CreateRoleNotify()
		{
		}
	}

	public class KG2C_CreateRoleRespond: KGC_PROTOCOL_HEADER
	{
		public byte code;
		public ulong uRoleID;

		public KG2C_CreateRoleRespond()
		{
			Register("code", typeof(uint).ToString() , 1, 1);
			Register("uRoleID", typeof(uint).ToString() , 8, 1);
		}
	}

	public class KG2C_HandshakeRespond: KGC_PROTOCOL_HEADER
	{
		public byte code;
		public byte gameEdition;
		public string szZoneName = "";
		public string szGroupName = "";
		public uint uPlayerIndex;

		public KG2C_HandshakeRespond()
		{
			Register("code", typeof(uint).ToString() , 1, 1);
			Register("gameEdition", typeof(uint).ToString() , 1, 1);
			Register("szZoneName", typeof(string).ToString() , 48, 1);
			Register("szGroupName", typeof(string).ToString() , 32, 1);
			Register("uPlayerIndex", typeof(uint).ToString() , 4, 1);
		}
	}

	public class KG2C_Webgateway_Verify_Notify: KGC_PROTOCOL_HEADER
	{
		public byte code;

		public KG2C_Webgateway_Verify_Notify()
		{
			Register("code", typeof(uint).ToString() , 1, 1);
		}
	}

}
