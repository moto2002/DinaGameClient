using System;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Lib.Net;
using Assets.Scripts.Lib.Log;
using Assets.Scripts.Proto;
using Assets.Scripts.Manager;
using System.IO;


namespace Assets.Scripts.Logic
{
    public class BaseLogic
    {
        protected Logger log = null;
        protected NetClient gatewaySocket = null;
        protected NetClient gameSocket = null;

        public BaseLogic()
        {
            PreInit();
            Init();
            InitListeners();
        }

        protected virtual void PreInit()
        {
            gatewaySocket = NetClientFactory.GetGatewaySocket();
            gameSocket = NetClientFactory.GetGameSocket();
            log = LoggerFactory.GetInstance().GetLogger(this.GetType());
        }

        protected virtual void Init()
        {

        }

        protected virtual void InitListeners()
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

        protected void RegistProctect(KG2C_Protocol protocolId, Type type)
        {
            gatewaySocket.RegisterProtocol((int)protocolId, null, type);
        }

        protected void RegistProctect(KS2C_Protocol protocolId, Type type)
        {
            gameSocket.RegisterProtocol((int)protocolId, null, type);
        }

        public void SendMessage(KGC_PROTOCOL_HEADER buf)
        {
            gatewaySocket.SendMessage(buf);
        }

        public void SendMessage(C2S_HEADER buf)
        {
            gameSocket.SendMessage(buf);
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
