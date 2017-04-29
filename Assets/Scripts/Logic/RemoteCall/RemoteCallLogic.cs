using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Logic.Friend;
using UnityEngine;
using Assets.Scripts.Model.Player;
using Assets.Scripts.View.Gift;
using Assets.Scripts.View.MainUI;
using Assets.Scripts.Utils;
using Assets.Scripts.Logic.Mission;
using Assets.Scripts.View.Pve;
using Assets.Scripts.Manager;
using Assets.Scripts.Logic.Scene;
using Assets.Scripts.View.Chat;
using Assets.Scripts.View.ButtonBar;
using Assets.Scripts.Logic.Bag;
using Assets.Scripts.Logic.Item;
using Assets.Scripts.Logic.Intensify;
using Assets.Scripts.Lib.View;
using Assets.Scripts.Controller;


namespace Assets.Scripts.Logic.RemoteCall
{
    class RemoteCallLogic : RemoteCall
    {
		private static RemoteCallLogic instance;
        public static RemoteCallLogic GetInstance()
        {
            if (instance == null)
            {
                instance = new RemoteCallLogic();
            }
            return instance;
        }
		
        private void OnTest(RemoteTable tab)
        {
            string xxx = tab[111];
            int aaaa = tab["xxx"];
            RemoteTable tab2 = tab[222] as RemoteTable;
            string x2 = tab2["ooo"];
   
			CallGS("OnTest", 123, "zxc");
        }


        private void OnAddDoodad(int objID, int objType, int posX, int posY, RemoteTable table)
        {
            SceneLogic.GetInstance().OnAddDoodad((uint)objID, objType, posX, posY, table);
        }

        //private void OnHeartBeat(int nCurFrame, ulong uid, RemoteTable tab)
        //{
        //    log.Debug("xxxxxxxx" + uid + "   "+ nCurFrame + tab[1] + tab["xxx"]);
        //}
        //增加好友
        private void OnAddFriend(ulong friendID, string friendName, int friendLevel, bool onLine)
        {
            FriendLogic.getInstance().OnAddFriend(friendID, friendName, friendLevel, onLine);
        }
        //提示已经是好友
        private void OnAlreadyMyFriend(string friendName)
        {
            FriendLogic.getInstance().OnAlreadyMyFriend(friendName);
        }
        //删除好友
        private void OnRemoveFriend(ulong friendID)
        {
            FriendLogic.getInstance().OnRemoveFriend(friendID);
        }

        //好友达到上限
        private void OnMaxFriendNotify()
        {
            FriendLogic.getInstance().OnMaxFriendNotify();
        }

        //仇人达到上限
        private void OnMaxEnemyNotify()
        {
            FriendLogic.getInstance().OnMaxEnemyNotify();
        }

        //增加好友申请消息
        private void OnAddApplyFriendMsg(ulong senderID, string nickName, int level)
        {
            FriendLogic.getInstance().OnAddApplyFirendMsg(senderID, nickName, level);
        }
        //增加仇人
        private void OnAddEnemy(ulong roleID, string roleName, int roleLevel, bool onLine)
        {
            FriendLogic.getInstance().OnAddEnemy(roleID, roleName, roleLevel, onLine);
        }
        //删除仇人
        private void OnRemoveEnemy(ulong roleID)
        {
            FriendLogic.getInstance().OnRemoveEnemy(roleID);
        }
        //提示已经是我仇人
        private void OnAlreadyMyEnemy(string enemyName)
        {
            FriendLogic.getInstance().OnAlreadyMyEnemy(enemyName);
        }

        //同步好友
        private void OnSyncFriends(RemoteTable tab)
        {
            FriendLogic.getInstance().OnSyncFriends(tab);
        }

        //同步好友的等级
        private void OnUpdateFriendLevel(ulong roleID, int roleLevel)
        {
            FriendLogic.getInstance().OnUpdateFriendLevel(roleID, roleLevel);
        }

        //同步仇人的等级
        private void OnUpdateEnemyLevel(ulong roleID, int roleLevel)
        {
            FriendLogic.getInstance().OnUpdateEnemyLevel(roleID, roleLevel);
        }

        //同步好友在线状态
        private void OnFriendOnLineStatus(ulong roleID, bool status)
        {
            FriendLogic.getInstance().OnFriendOnLineStatus(roleID, status);
        }
        //同步仇人在线状态
        private void OnEnemyOnLineStatus(ulong roleID, bool status)
        {
            FriendLogic.getInstance().OnEnemyOnLineStatus(roleID, status);
        }

        private void OnGetReward(int rewardID)
        {
            Debug.Log(rewardID);
        }

        private void UpdatePlayerGiftBagData(int levelCurrent, int combat, int onlineTime, RemoteTable rewardData)
        {
            MajorPlayer player = PlayerManager.GetInstance().MajorPlayer;
            player.levelCurrent = levelCurrent;//当前等级
            player.combat = combat; //战力
            player.onlineTime = (float)onlineTime / 10; //1s为单位
            
            foreach (KeyValuePair<object,object> item in rewardData.dictKV)
            {
                Int32 key = (Int32)item.Key;
                RemoteBool value = item.Value as RemoteBool;
                player.rewardData[key] = value.GetBool();
                //key=reward tab ID value = isGained
            }

            GiftItem.curCountingGiftItem = null;
            OnlineGiftView.GetInstance().UpdateUIOnDataChanged();
            GiftHallView.GetInstance().UpdateUIOnDataChanged();
        }


        private void GainGift(int giftID)
        {
            MajorPlayer player = PlayerManager.GetInstance().MajorPlayer;
			if(player.rewardData !=null && player.rewardData[giftID] != null) 
			{
           		player.rewardData[giftID] = true;

           		GiftItem.curCountingGiftItem = null;
            	EventDispatcher.GameWorld.DispatchEvent(ControllerCommand.GIFT_ITEM_UPDATE);
			}
        }

        private void UpdateCombat(int combat)
        {
            MajorPlayer player = PlayerManager.GetInstance().MajorPlayer;
            if (player.combat != combat)
            {
                player.combat = combat; //战力
                EventDispatcher.GameWorld.DispatchEvent(ControllerCommand.UPDATE_COMBAT);
            }
        }

		
		private void UpdatePlayerShortCutList(RemoteTable shortCutList)
		{
			RemoteTable shortCut = shortCutList["shortCut"] as RemoteTable;
			int c = shortCut.Count;
			RemoteTable _items = shortCutList["shortCut"] as RemoteTable;
			for (int i = 0 ; i < 8 ; i++)
			{
				int _index = i+1;
				if (_items.ContainsKey(_index))
				{
					int _type   = _items[_index]["type"];
					int _itemId = _items[_index]["itemId"];
					SHORTCUTITEM _sctype = SHORTCUTITEM.NONE;
					if (_type == 2)
					{
						_sctype = SHORTCUTITEM.SKILL;
					}
					else if( _type == 1 )
					{
						_sctype = SHORTCUTITEM.ITEM;
					}
					else
					{
						_sctype = SHORTCUTITEM.NONE;
					}
					ButtonBarView.GetInstance().SetShortCut(i,_sctype,_itemId,false);
				}
			}
			
			//shortCutList["shortCut"][0]["index"]
		}
		//装备强化
		private void EquipStrengResult(bool isSuccess, int itemPos, int newLevel)
		{
			log.Debug("!!!!!!!!!!!!EquipStrengResult," + isSuccess + ",pos:" + itemPos + ",lev:" + newLevel);
            ItemInfo info;
            if (isSuccess)
            {
                info = BagLogic.GetInstance().GetEquipByPos(itemPos);
                (info as EquipInfo).CurStrengthenLv = newLevel;
                if(IntensifyLogic.GetInstance().GetIntensifyUI() != null && IntensifyLogic.GetInstance().GetIntensifyUI().isOpen())
                    IntensifyLogic.GetInstance().UpdataIntensify(itemPos , true);
            }
		}
		
		

			
		private void OnSyncQuestState(RemoteTable questState)
		{
			MissionLogic.GetInstance().OnSyncQuestState(questState);
		}
		
		private void OnSyncQuestList(RemoteTable acceptQuests)
		{
			MissionLogic.GetInstance().OnSyncQuestList(acceptQuests);	
		}
		
		private void OnSyncQuestValue(int questID, int valueIndex, int newValue)
		{
			MissionLogic.GetInstance().OnSyncQuestValue(questID, valueIndex, newValue);
		}	
		
		private void OnSyncAcceptQuestRespond(int missionID, int resultCode)
		{
			MissionLogic.GetInstance().OnSyncAcceptQuestRespond(missionID, resultCode);
		}
		
		private void OnSyncFinishQuestRespond(int missionID, int resultCode)
		{
			MissionLogic.GetInstance().OnSyncFinishQuestRespond(missionID, resultCode);
		}

        private void OnSyncQuickFinishQuestRespond(int missionID, int resultCode)
        {
            MissionLogic.GetInstance().OnSyncQuickFinishQuestRespond(missionID, resultCode);
        }

		private void OnSyncCancelQuestRespond(int missionID, int resultCode)
		{
			MissionLogic.GetInstance().OnSyncCancelQuestRespond(missionID, resultCode);
		}

        private void OnStartCollectRespond(int collectID, int resultCode)
        {
            CollectObjLogic.GetInstance().OnStartCollectRespond(collectID, resultCode);
        }

        private void OnFinishCollectRespond(int collectID, int resultCode)
        {
            CollectObjLogic.GetInstance().OnFinishCollectRespond(collectID, resultCode);
        }

        private void OnInterruptCollectRespond(int collectID, int resultCode)
        {
            CollectObjLogic.GetInstance().OnInterruptCollectRespond(collectID, resultCode);
        }

//////////////////////////////////////////////////////////////////////////

//副本
        private void OnPVESucess(int nPveID, int completeTime, RemoteTable award)
        {
            float fCompleteTime = (float)completeTime / 10.0f;
            PveView.GetInstance().ShowPveView(nPveID, fCompleteTime, award);
        }

        private void OnPVEFail()
        {
            PveProcessView.GetInstance().HideProcess();
            PveFailView.GetInstance().ShowView();
        }

        private void OnValueModify(int nKey, int nValue)
        {
            PveProcessView.GetInstance().OnValueModify(nKey, nValue);
        }
    
        private void OnPveMapInit(int pveID, int completeTime, RemoteTable pveValue)
        {
            PveAutoFight.GetInstance().Show();
            PveProcessView.GetInstance().InitPveMap(pveID, completeTime / 10, pveValue);
        }

        private void test()
        {
            PveFailView.GetInstance().ShowView();
        }
//////////////////////////////////////////////////////////////////////////
		
		private void SyncMoney(int nMoney, int nCoin, int nMenterPoint)
		{
			log.Debug("!!!!!!!!!!!!SyncMoney,nMoney:" + nMoney + ",nCoin:" + nCoin + ",nMenterPoint:" + nMenterPoint);
            MajorPlayer player = PlayerManager.GetInstance().MajorPlayer;
            player.money = nMoney;
            player.coin = nCoin;
            player.menterPoint = nMenterPoint;
            PlayerController.GetInstance().UpdateMoney(nMoney, nCoin, nMenterPoint);
            EventDispatcher.GameWorld.DispatchEvent(ControllerCommand.SYNC_MONEY);
		}

 	}
   

}
