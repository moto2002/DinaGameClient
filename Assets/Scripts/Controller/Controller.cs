using UnityEngine;
using System;
using System.Collections;
using Assets.Scripts.Lib.Net;
using Assets.Scripts.Proto;
using Assets.Scripts.Lib.Log;
using Assets.Scripts.Manager;
using System.IO;

namespace Assets.Scripts.Controller
{
    /**
     * 所有的委派事件， 都在这里定义
     * */
    public class Controller
    {
        protected Logger log = null;

        protected NetClient gatewaySocket = null;
        protected NetClient gameSocket = null;

        public Controller()
        {

            PreInit();
            RegistListener();
            Init();
        }

        protected virtual void PreInit()
        {
            gatewaySocket = NetClientFactory.GetGatewaySocket();
            gameSocket = NetClientFactory.GetGameSocket();
            log = LoggerFactory.GetInstance().GetLogger(this.GetType());
        }

        protected virtual void RegistListener()
        {

        }

        protected virtual void Init()
        {

        }

        protected void RegistSocketListener(KG2C_Protocol protocolId, ProtocolHandler handler, Type type)
        {
            gatewaySocket.RegisterProtocol((int)protocolId, handler, type);
        }

        protected void RegistSocketListener(KS2C_Protocol protocolId, ProtocolHandler handler, Type type)
        {
            gameSocket.RegisterProtocol((int)protocolId, handler, type);
        }

        protected void SendMessage(KGC_PROTOCOL_HEADER buf)
        {
            gatewaySocket.SendMessage(buf);
        }

        protected void SendMessage(C2S_HEADER buf)
        {
            gameSocket.SendMessage(buf);
        }

        private void PingGameServer(long currentTime)
        {
            C2S_PING_SIGNAL ping = new C2S_PING_SIGNAL();
            ping.protocolID = (byte)KC2S_Protocol.c2s_ping_signal;
            ping.time = (uint)currentTime;
            SendMessage(ping);
        }

        private void PingGateway(long currentTime)
        {
            KC2G_PingSignal ping = new KC2G_PingSignal();
            ping.byProtocol = (byte)KC2G_Protocol.c2g_ping_signal;
            ping.dwTime = (uint)currentTime;
            SendMessage(ping);
        }
        protected void DisposeGatewayConnect()
        {
            gatewaySocket.DisposeConnect();
        }

        protected void DisposeGameConnect()
        {
            gameSocket.DisposeConnect();
        }

        protected void RegistGameListener(string type, MyEventHandler handler)
        {
            EventDispatcher.GameWorld.Regist(type, handler);
        }

        protected void RemoveGameListener(string type, MyEventHandler handler)
        {
            EventDispatcher.GameWorld.Remove(type, handler);
        }

        protected void Dispatch(string type, System.Object obj)
        {
            EventDispatcher.GameWorld.Dispath(type, obj);
        }

    }
}
