using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Model.Scene;
using Assets.Scripts.Lib.Net;
using Assets.Scripts.Proto;
using Assets.Scripts.Define;
using Assets.Scripts.Utils;
using Assets.Scripts.Lib.Log;
using Assets.Scripts.View.Scene;
using Assets.Scripts.Module;
using Assets.Scripts.Module.View;
using Assets.Scripts.Manager;
using UnityEngine;
using Assets.Scripts.Model.Player;


namespace Assets.Scripts.Controller
{
	class LoginController:Controller
	{
        private static LoginController instance;
        private string password = "aa";
        private LoginView loginView;

        protected override void RegistListener()
        {
            RegistSocketListener(KG2C_Protocol.g2c_ping_respond, OnPingRespond, typeof(KG2C_PingRespond));
            RegistSocketListener(KG2C_Protocol.g2c_handshake_respond, OnHandShakeRespond, typeof(KG2C_HandshakeRespond));
            RegistSocketListener(KG2C_Protocol.g2c_account_locked_notify, OnAccountLockedNotify, typeof(KG2C_AccountLockedNotify));
            RegistSocketListener(KG2C_Protocol.g2c_account_verify_respond, OnAccountVerifyRespond, typeof(KG2C_AccountVerifyRespond));
            RegistSocketListener(KG2C_Protocol.g2c_kick_account_notify, OnKickAccountNotify, typeof(KG2C_KickAccountNotify));
            RegistSocketListener(KG2C_Protocol.g2c_check_role_exist_respond, OnCheckRoleNameExistRespond, typeof(KG2C_CheckRoleNameExistRespond));
            RegistSocketListener(KG2C_Protocol.g2c_create_role_notify, OnCreateRoleNotify, typeof(KG2C_CreateRoleNotify));
            RegistSocketListener(KG2C_Protocol.g2c_create_role_respond, OnCreateRoleRespond, typeof(KG2C_CreateRoleRespond));
            RegistSocketListener(KG2C_Protocol.g2c_sync_login_key, OnLoginKeySync, typeof(KG2C_SyncLoginKey));
        }

        protected override void Init()
        {

        }

        public void Connect(string host, short port)
        {
            gatewaySocket.OnConected += OnConected;
            gatewaySocket.Connect(host, port, 10, "");
        }


        private void OnConected()
        {
            log.Debug("网关连接成功");
            KC2G_HandshakeRequest handSkakeRequest = new KC2G_HandshakeRequest();
            handSkakeRequest.byProtocol = (byte)KC2G_Protocol.c2g_handshake_request;
            handSkakeRequest.nProtocolVersion = (int)KProcotolVersion.eProcotolVersion;
            handSkakeRequest.nGroupID = ConfigManager.GetInstance().GroupID;
            handSkakeRequest.uNetType = (byte)0;
            SendMessage(handSkakeRequest);
            log.Debug("发送网关握手");
        }

        private void OnPingRespond(KProtoBuf buf)
        {
        }

        public void OnHandShakeRespond(KProtoBuf buf)
        {
            
            KG2C_HandshakeRespond respond = buf as KG2C_HandshakeRespond;
            KGateWayHandShakeCode code = EnumUtils.GetEnumIns<KGateWayHandShakeCode>(respond.code);
            log.Debug("网关握手返回:" + code);
            if (code == KGateWayHandShakeCode.ghcHandshakeSucceed)
            {
                KC2G_AccountVerifyRequest request = new KC2G_AccountVerifyRequest();
                request.byProtocol = (byte)KC2G_Protocol.c2g_account_verify_request;
                log.Debug("Account=" + ConfigManager.GetInstance().Account);
                request.account = ConfigManager.GetInstance().Account;
                request.nGroupID = ConfigManager.GetInstance().GroupID;
                request.password = password;
                SendMessage(request);
            }
            else if (code == KGateWayHandShakeCode.ghcGatewayVersionError)
            {

            }
        }

        private void OnAccountLockedNotify(KProtoBuf buf)
        {
        
        }

        public void OnAccountVerifyRespond(KProtoBuf buf)
        {
            KG2C_AccountVerifyRespond respond = buf as KG2C_AccountVerifyRespond;
        }

        private void OnKickAccountNotify(KProtoBuf buf)
        {
        
        }

        private void OnCheckRoleNameExistRespond(KProtoBuf buf)
        {
        
        }


        //账号验证通过,玩家已经创建角色,返回登录成功
        public void onSyncLoginKey(KProtoBuf buf)
        {
            log.Debug("账号验证成功,登录成功");

            DisposeGatewayConnect();

            KG2C_SyncLoginKey respond = buf as KG2C_SyncLoginKey;
            KGameLoginRespondCode code = EnumUtils.GetEnumIns<KGameLoginRespondCode>(respond.code);
            log.Debug("----------" + code);
            if (code == KGameLoginRespondCode.eGameLoginSucceed)
            {
                MajorPlayer player = PlayerManager.GetInstance().MajorPlayer;
                player.PlayerID = respond.uRoleID;
                player.guid = respond.guid;
                IPAddress add = new IPAddress(respond.ip);
                GameWorldController.GetInstance().Connect(add.ToString(), (short)respond.port);
            }
        }


        public void onKickAccount(KProtoBuf buf)
        {

        }

        public void onAccountLockedNotify(KProtoBuf buf)
        {

        }

        public void onPingRespond(KProtoBuf buf)
        {

        }

        //创建角色,角色名已经存在
        public void onCheckRoleNameExistRespond(KProtoBuf buf)
        {
            log.Debug("创建角色失败,角色名已存在");
        }

        //创建角色成功
        public void onCreateRoleRespond(KProtoBuf buf)
        {
            KG2C_CreateRoleRespond respond = buf as KG2C_CreateRoleRespond;
            KCreateRoleRespondCode code = EnumUtils.GetEnumIns<KCreateRoleRespondCode>(respond.code);

            if (code == KCreateRoleRespondCode.eCreateRoleSucceed)
            {
                log.Debug("创建角色成功");
            }
            else if (code == KCreateRoleRespondCode.eCreateRoleNameAlreadyExist)
            {
                log.Debug("昵称已存在");
            }
            else if (code == KCreateRoleRespondCode.eCreateRoleInvalidRoleName)
            {
                log.Debug("无效的昵称");
            }
            else if (code == KCreateRoleRespondCode.eCreateRoleNameTooLong)
            {
                log.Debug("太长");
            }
            else if (code == KCreateRoleRespondCode.eCreateRoleNameTooShort)
            {
                log.Debug("太短");
            }
            else
            {
                log.Debug("不能创建");
            }
        }

        public void onWebGatewayVerifyNotify(KProtoBuf buf)
        {

        }


        public void OnCreateRoleRequest(string nickname, sbyte gender, int nMainHeroTemplateID)
        {
            KC2G_CreateRoleRequest request = new KC2G_CreateRoleRequest();
            request.byProtocol = (byte)KC2G_Protocol.c2g_create_role_request;
            request.szRoleName = nickname;
            request.byGender = gender;
            request.nMainHeroTemplateID = nMainHeroTemplateID;
            SendMessage(request);
        }

        public void OpenLoginView()
        {
            if (loginView == null)
            {
                loginView = new LoginView();
            }
        }

        public void CloseLoginView()
        {
            if (loginView != null)
                loginView.DestroyObject();
            loginView = null;
        }

        private void OnLoginKeySync(KProtoBuf proto)
        {
            DisposeGatewayConnect();

            KG2C_SyncLoginKey respond = proto as KG2C_SyncLoginKey;
            KGameLoginRespondCode code = EnumUtils.GetEnumIns<KGameLoginRespondCode>(respond.code);
            log.Debug("账号验证成功,登录成功" + code);
            if (code == KGameLoginRespondCode.eGameLoginSucceed)
            {
                MajorPlayer player = PlayerManager.GetInstance().MajorPlayer;
                player.PlayerID = respond.uRoleID;
                player.guid = respond.guid;
                IPAddress add = new IPAddress(respond.ip);
                GameWorldController.GetInstance().Connect(add.ToString(), (short)respond.port);
            }
        }

        private void OnCreateRoleNotify(KProtoBuf proto)
        {
            log.Debug("展示创建角色界面");
            OpenLoginView();
        }

        private void CheckRoleNameExistHandler(KProtoBuf proto)
        {

        }

        private void OnCreateRoleRespond(KProtoBuf proto)
        {
            KG2C_CreateRoleRespond respond = proto as KG2C_CreateRoleRespond;
            KCreateRoleRespondCode code = EnumUtils.GetEnumIns<KCreateRoleRespondCode>(respond.code);

            if (code == KCreateRoleRespondCode.eCreateRoleSucceed)
            {
                log.Debug("创建角色成功");
            }
            else
            {
                log.Debug("创建角色失败=" + respond.code);
            }
        }



        public static LoginController GetInstance()
        {
            if (instance == null)
                instance = new LoginController();
            return instance;
        }

	}
}
