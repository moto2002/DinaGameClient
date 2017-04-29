using System;
using System.Collections.Generic;
using Assets.Scripts.Controller;
using Assets.Scripts.Lib.Net;
using Assets.Scripts.Proto;

namespace Assets.Scripts.Controller
{
    public class ChatController : Controller
    {
        private static ChatController instance;
        public static ChatController GetInstance()
        {
            if (instance == null)
                instance = new ChatController();
            return instance;
        }

        protected override void RegistListener()
        {
        } 

        public void SendGMMessage(string cmd)
        {
            C2S_GM_CMD msg = new C2S_GM_CMD();
            msg.protocolID = (byte)KC2S_Protocol.c2s_gm_cmd;
            msg.command = cmd;
            SendMessage(msg);
        }
    }
}
