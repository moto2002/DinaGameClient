using System;
using System.Collections.Generic;
using Assets.Scripts.Proto;

namespace Assets.Scripts.Lib.Net
{
	class NetClientFactory
	{
        private static bool unitSocketLoginSuccess = false;
		

		public static bool GetUnitSocketLoginSuccess()
		{
			return unitSocketLoginSuccess;
		}
		
		/**
		 * 设置公共线socket的可用状态
		 */		
		public static void SetUnitSocketLoginSuccess(bool value)
		{
			unitSocketLoginSuccess = value;
		}
		
		
		private static NetClient socket;

        public static NetClient GetGameSocket()
		{
            if (socket == null)
			{
                NetClient.PingDelegate pingDelegate = new NetClient.PingDelegate(PingGameServer);
                socket = NetClient.NewInstance("_GameSocket", pingDelegate, 20000);
			}
            return socket;
		}
		
		private static NetClient gatewaySocket;

        public static NetClient GetGatewaySocket()
		{
            if (gatewaySocket == null)
			{
                NetClient.PingDelegate pingDelegate = new NetClient.PingDelegate(PingGateway);
                gatewaySocket = NetClient.NewInstance("_GameWaySocket", PingGateway, 20000);
			}
            return gatewaySocket;
		}

        private static void PingGateway(long currentTime)
        {
            KC2G_PingSignal ping = new KC2G_PingSignal();
            ping.byProtocol = (byte)KC2G_Protocol.c2g_ping_signal;
            ping.dwTime = (uint)currentTime;
            gatewaySocket.SendMessage(ping);
        }

        private static void PingGameServer(long currentTime)
        {
            C2S_PING_SIGNAL ping = new C2S_PING_SIGNAL();
            ping.protocolID = (byte)KC2S_Protocol.c2s_ping_signal;
            ping.time = (uint)currentTime;
            socket.SendMessage(ping);
        }
	}
}
