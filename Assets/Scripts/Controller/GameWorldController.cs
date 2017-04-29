using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Lib.Net;
using System.Net;
using Assets.Scripts.Lib.Log;
using Assets.Scripts.Proto;
using Assets.Scripts.Model.Scene;
using Assets.Scripts.View.Scene;
using Assets.Scripts.Manager;
using UnityEngine;
using Assets.Scripts.Model.Player;
using System.IO;

/************************************************************************/
/* 
c2s_handshake_request
s2c_sync_player_base_info
S2C_SWITCH_MAP
C2S_APPLY_SCENE_OBJ
S2C_SYNC_SCENE_OBJ_END
c2s_apply_scene_hero_data
S2C_SYNC_NEW_HERO
S2C_SYNC_SCENE_HERO_DATA_END
c2s_apply_scene_hero_data
C2S_LOADING_COMPLETE
S2C_BATTLE_STARTEDFRAME
 */
/************************************************************************/
namespace Assets.Scripts.Controller
{
    class GameWorldController : Controller
    {
        private static GameWorldController instance = null;

        private NetClient gameWordClient = null;

        protected override void Init()
        {
            gameWordClient = NetClientFactory.GetGameSocket();
            InitListener();
        }

        private void InitListener()
        {
        }

        public void Connect(string host, short port)
        {
            gameWordClient.OnConected += OnConected;
            gameWordClient.Connect(host, port, 0, "");
			log.Debug("连接GameWord"+"host:"+host+"   port:"+port);
        }

        private void OnConected()
        {
            log.Debug("连接上游戏服务");
            HandShakeRequest();
        }


        public void HandShakeRequest()
        {
            C2S_HANDSHAKE_REQUEST request = new C2S_HANDSHAKE_REQUEST();
            request.protocolID = (byte)KC2S_Protocol.c2s_handshake_request;
            request.uRoleID = PlayerManager.GetInstance().MajorPlayer.PlayerID;
            request.guid = PlayerManager.GetInstance().MajorPlayer.guid;
            request.procotolversion = (byte)KProcotolVersion.eProcotolVersion;
            gameWordClient.SendMessage(request);
        }




        public static GameWorldController GetInstance()
        {
            if (instance == null)
            {
                instance = new GameWorldController();
				instance.Init();
            }
            return instance;
        }

    }
}
