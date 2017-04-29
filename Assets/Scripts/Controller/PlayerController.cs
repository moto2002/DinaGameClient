using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Proto;
using Assets.Scripts.Lib.Net;
using Assets.Scripts.Model.Player;
using Assets.Scripts.Define;
using Assets.Scripts.Manager;
using Assets.Scripts.View.Role;
using Assets.Scripts.Model.Skill;
using Assets.Scripts.Model.Scene;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.Data;
using Assets.Scripts.Logic.Scene;
using Assets.Scripts.Logic.Skill;
using Assets.Scripts.Logic;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Controller
{
    class PlayerController : Controller
    {
        private RoleView roleView = null;
        protected override void RegistListener()
        {
            RegistSocketListener(KS2C_Protocol.s2c_ping_signal, OnPingSignal, typeof(S2C_PING_SIGNAL));
            RegistSocketListener(KS2C_Protocol.s2c_sync_new_player_info, SyncNewPlayer, typeof(S2C_SYNC_NEW_PLAYER_INFO));
            RegistSocketListener(KS2C_Protocol.s2c_sync_player_base_info, SyncPlayerBaseInfo, typeof(S2C_SYNC_PLAYER_BASE_INFO));
            RegistSocketListener(KS2C_Protocol.s2c_sync_player_state_info, OnSyncPlayerStateInfo, typeof(S2C_SYNC_PLAYER_STATE_INFO));
            RegistSocketListener(KS2C_Protocol.s2c_add_exp, OnAddExp, typeof(S2C_ADD_EXP));
            RegistSocketListener(KS2C_Protocol.s2c_sync_hero_data, OnSyncHeroData, typeof(S2C_SYNC_HERO_DATA));
            RegistSocketListener(KS2C_Protocol.s2c_update_level, OnRoleUpdateLevel, typeof(S2C_UPDATE_LEVEL));
            RegistGameListener(ControllerCommand.OPEN_ROLE_INFO_PANEL, OnOpenRolePanel);
            RegistGameListener(ControllerCommand.PLAYER_LEVEL_UP, OnPlayerLevelUp);
        }

        private void SyncPlayerBaseInfo(KProtoBuf buf)
        {
            S2C_SYNC_PLAYER_BASE_INFO respond = buf as S2C_SYNC_PLAYER_BASE_INFO;
            MajorPlayer majorPlayer = PlayerManager.GetInstance().MajorPlayer;
            majorPlayer.PlayerID = respond.uPlayerID;
            majorPlayer.LastSaveTime = respond.nLastSaveTime;
            majorPlayer.LastLoginTime = respond.nLastLoginTime;
            majorPlayer.TotalGameTime = respond.nTotalGameTime;
            majorPlayer.CreateTime = respond.nCreateTime;
            majorPlayer.Gender = (KGender)respond.byGender;
            majorPlayer.CanRename = respond.byCanRename;
            majorPlayer.AccountName = respond.szAccountName;
            majorPlayer.PlayerName = respond.szPlayerName;
            majorPlayer.ServerTime = respond.nServerTime;
            majorPlayer.ClubID = respond.uClubID;
            majorPlayer.Job = (ushort)respond.byHeroJob;
            majorPlayer.GroupID = respond.nGroupID;
            SkillLogic.GetInstance().DefaultSkill();
            SceneLogic.GetInstance().InitScene();

            KGender gender = EnumUtils.GetEnumIns<KGender>(respond.byGender);
            majorPlayer.gender = gender;

            EventDispatcher.GameWorld.Dispath(ControllerCommand.CHANGE_HEAD, respond.byGender);
			PreLoad.GetInstance().OnLoadMajorPlayer();
        }

        private void OnPingSignal(KProtoBuf buf)
        {
        }

        private void OnSyncPlayerStateInfo(KProtoBuf buf)
        {
            S2C_SYNC_PLAYER_STATE_INFO respond = buf as S2C_SYNC_PLAYER_STATE_INFO;
            MajorPlayer majorPlayer = PlayerManager.GetInstance().MajorPlayer;
            majorPlayer.uJob = respond.uJob;
            majorPlayer.vipLevel = respond.byVipLevel;
            majorPlayer.vipExp = respond.nVIPExp;
            majorPlayer.vipEndTime = respond.nVIPEndTime;
            majorPlayer.level = respond.byLevel;
            majorPlayer.Exp = respond.nExp;

            KPlayerLevelExpSetting playerLevelExpSetting = KConfigFileManager.GetInstance().playerLevelSetting.getData((majorPlayer.level + 1).ToString());
            majorPlayer.maxExp = playerLevelExpSetting.Exp;

        }
		/// <summary>
		/// 玩家金钱.
		/// </summary>
		public void UpdateMoney (int nMoney,int nCoin,int nMenterPoint)
		{
			MajorPlayer majorPlayer = PlayerManager.GetInstance().MajorPlayer;
            majorPlayer.addMoney = nMoney - majorPlayer.money;
            majorPlayer.addCoin = nCoin - majorPlayer.coin;
            majorPlayer.addMenterPoint = nMenterPoint - majorPlayer.menterPoint;

			majorPlayer.money = nMoney;
			majorPlayer.coin = nCoin;
			majorPlayer.menterPoint = nMenterPoint;
		}
		
		
        private void SyncNewPlayer(KProtoBuf buf)
        {
            S2C_SYNC_NEW_PLAYER_INFO respond = buf as S2C_SYNC_NEW_PLAYER_INFO;

            PlayerManager.GetInstance().AddPlayer(respond.playerID, respond.szPlayerName, respond.byGender);
        }

        private void OnAddExp(KProtoBuf buf)
        {
            S2C_ADD_EXP respond = buf as S2C_ADD_EXP;
            MajorPlayer majorPlayer = PlayerManager.GetInstance().MajorPlayer;

            majorPlayer.Exp += respond.addExp;
            majorPlayer.addExp = respond.addExp;
			
			if (null != SceneLogic.GetInstance().MainHero)
			{
				SceneLogic.GetInstance().MainHero.TipsCmp.CreateTip( new Vector3(0,SceneLogic.GetInstance().MainHero.heroSetting.TipPos0,0), "+" + respond.addExp, "ExpFont","effect_ui_shuzitanchu_putong.res");
			}
            EventDispatcher.GameWorld.DispatchEvent(ControllerCommand.ADD_EXP);
        }
		private void LoadLevelUpComplete(AssetInfo info)
		{
			try
			{
				if (null != SceneLogic.GetInstance().MainHero && null != info.bundle.mainAsset)
				{
					GameObject fx = GameObject.Instantiate(info.bundle.mainAsset) as GameObject;
					fx.transform.parent = SceneLogic.GetInstance().MainHero.transform;
					fx.transform.localPosition = Vector3.zero;
					KingSoftCommonFunction.SetLayer(fx,11);
					DestoryObject d = fx.AddComponent<DestoryObject>();
					d.delta = 5f;
				}
			}
			catch(System.Exception e)
			{
				
			}
		}
        private void OnRoleUpdateLevel(KProtoBuf buf)
        {
            S2C_UPDATE_LEVEL respond = buf as S2C_UPDATE_LEVEL;
            MajorPlayer majorPlayer = PlayerManager.GetInstance().MajorPlayer;
            
            byte level = (byte)respond.nlevel;
            if(level != majorPlayer.level) {
                majorPlayer.level = level;
                majorPlayer.Exp = 0;
                KPlayerLevelExpSetting playerLevelExpSetting = KConfigFileManager.GetInstance().playerLevelSetting.getData((majorPlayer.level + 1).ToString());
                majorPlayer.maxExp = playerLevelExpSetting.Exp;
				if (null != SceneLogic.GetInstance().MainHero)
				{
					AssetLoader.GetInstance().Load(URLUtil.GetEffectPath("effect_levelup"), LoadLevelUpComplete, AssetType.BINARY);

					//effect_levelup
					//SceneLogic.GetInstance().MainHero.TipsCmp.CreateTip( new Vector3(0,SceneLogic.GetInstance().MainHero.heroSetting.TipPos0,0), "+" + respond.addExp, "ExpFont","effect_ui_shuzitanchu_putong.res");
				}
                EventDispatcher.GameWorld.Dispath(ControllerCommand.PLAYER_LEVEL_UP, level);
            }
        }

        private void OnSyncHeroData(KProtoBuf buf)
        {
            S2C_SYNC_HERO_DATA respond = buf as S2C_SYNC_HERO_DATA;
            MajorPlayer majorPlayer = PlayerManager.GetInstance().MajorPlayer;
            log.Debug("同步场景英雄数据 " + majorPlayer.PlayerID);

            majorPlayer.Job = respond.wJob;
 
            //if (majorPlayer.PotencyPoint > 0)
            //{
            //    PlayerController.GetInstance().SetHeroPoint();
            //}
        }

        private object OnOpenRolePanel(params object[] objs)
        {
            if (roleView == null)
            {
                roleView = new RoleView();
            }
            else
            {
                roleView.Show();
            }
            return null;
        }

        private object OnPlayerLevelUp(System.Object obj)
        {
            //byte byOldLevel = (byte)obj;
            //log.Debug("OnPlayerLevelUp byNewLevel=" + PlayerManager.GetInstance().MajorPlayer.level + ", oldLevel=" + byOldLevel);

            if (roleView == null)
                return null;
            return null;
        }


        private static PlayerController instance;
        public static PlayerController GetInstance()
        {
            if (instance == null)
            {
                instance = new PlayerController();
            }
            return instance;
        }
    }
}
