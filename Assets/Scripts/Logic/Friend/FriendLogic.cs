using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Logic.RemoteCall;
using Assets.Scripts.Model.Player;
using Assets.Scripts.Define;

namespace Assets.Scripts.Logic.Friend
{
    class FriendLogic : BaseLogic
    {
        private static FriendLogic instance;
        private ArrayList friendList = null; //好友
        private ArrayList enemyList = null;//仇人
        private ArrayList applyFriendMsg = null;//好友申请消息

        public  int Friend_MaxNum = 100;//好友数目上限
        public  int Enemy_MaxNum = 100;//仇人数目上限
        public  int ApplyMsg_MaxNum = 100;//好友申请消息数目上限

        public static FriendLogic getInstance()
        {
            if (instance == null)
                instance = new FriendLogic();
            return instance;
        }
        public ArrayList GetFriendList()
        {
            return friendList;
        }
        public ArrayList getEnemyList()
        {
            return enemyList;
        }

        private FriendLogic()
        {
            friendList = new ArrayList();
            friendList.Insert(0, new FriendInfo(1, "aa", 1, true));
            friendList.Insert(1, new FriendInfo(2, "谭小明aa", 2, true));
            friendList.Insert(2, new FriendInfo(3, "谭小明aaa", 3, true));
            friendList.Insert(3, new FriendInfo(4, "谭小明aaaa", 4, true));
            friendList.Insert(4, new FriendInfo(5, "asdfsfeaefaea", 1, true));
            friendList.Insert(5, new FriendInfo(6, "谭小eaeaf明aa", 2, true));
            friendList.Insert(6, new FriendInfo(7, "谭小asdfasdf明aaa", 3, true));
            friendList.Insert(7, new FriendInfo(8, "adfafeaaaa", 4, true));
            friendList.Insert(8, new FriendInfo(10, "adfafesdfdsfaaaa", 4, true));
            enemyList = new ArrayList();
            applyFriendMsg = new ArrayList();
        }
        //发送申请好友消息通过昵称
        public void SendAddFriendByName(string nickName)
        {
            MajorPlayer player = PlayerManager.GetInstance().MajorPlayer;
            RemoteCallLogic.GetInstance().CallLS("OnAddFriendByName", player.PlayerID, nickName);
        }

        //通过好友申请消息增加好友
        public void SendAddFriendByMsg(ulong playerID)
        {
            MajorPlayer player = PlayerManager.GetInstance().MajorPlayer;
            RemoteCallLogic.GetInstance().CallLS("OnAddFriendByMsg", player.PlayerID, playerID);
        }
        //删除好友
        public void SendRemoveFriend(ulong playerID)
        {
            MajorPlayer player = PlayerManager.GetInstance().MajorPlayer;
            RemoteCallLogic.GetInstance().CallLS("OnRemoveFriend", player.PlayerID, playerID);
        }
        //增加仇人
        public void SendAddEnemy(ulong playerID)
        {
            MajorPlayer player = PlayerManager.GetInstance().MajorPlayer;
            RemoteCallLogic.GetInstance().CallLS("OnAddEnemy", player.PlayerID, playerID);
        }
        //删除仇人
        public void SendRemoveEnemy(ulong playerID)
        {
            MajorPlayer player = PlayerManager.GetInstance().MajorPlayer;
            RemoteCallLogic.GetInstance().CallLS("OnRemoveEnemy", player.PlayerID, playerID);
        }

        //增加好友
        public void OnAddFriend(ulong friendID, string friendName, int friendLevel, bool onLine)
        {
            FriendInfo info = new FriendInfo(friendID, friendName, friendLevel, onLine);
            friendList.Add(info);
            //刷新好友列表UI
        }
        //提示已经是好友
        public void OnAlreadyMyFriend(string friendName)
        {

        }

        //删除好友
        public void OnRemoveFriend(ulong friendID)
        {
            foreach (FriendInfo info in friendList)
            {
                if (info != null && info.playerID == friendID)
                {
                    friendList.Remove(info);
                    break;
                }
            }
            //刷新好友列表UI
        }

        //好友数目达到上限提示
        public void OnMaxFriendNotify()
        {
            //提示
        }

        //仇人数目达到上限提升
        public void OnMaxEnemyNotify()
        {
            //提示
        }

        //增加申请好友消息info
        public void OnAddApplyFirendMsg(ulong senderID, string nickName, int level)
        {
            //对好友申请大小判断

            FriendMsgInfo msgInfo = new FriendMsgInfo(senderID, nickName, level);
            applyFriendMsg.Add(msgInfo);
            //刷新好友申请UI
        }

        //增加仇人
        public void OnAddEnemy(ulong roleID, string roleName, int roleLevel, bool onLine)
        {
            FriendInfo info = new FriendInfo(roleID, roleName, roleLevel, onLine);
            enemyList.Add(info);
            //刷新仇人列表UI
        }
        //删除仇人
        public void OnRemoveEnemy(ulong roleID)
        {
            foreach (FriendInfo info in enemyList)
            {
                if (info != null && info.playerID == roleID)
                {
                    enemyList.Remove(info);
                    break;
                }
            }
            //刷新角色仇人列表
        }
        //提示已经是仇人
        public void OnAlreadyMyEnemy(string enemyName)
        {

        }

        //同步好友
        public void OnSyncFriends(RemoteTable dataTab)
        {
            //to be continue
            RemoteObject remoteObj = null;
            FriendInfo info = null;
            RemoteTable friendTab = (RemoteTable)dataTab["friend"];
            RemoteTable enemyTab = (RemoteTable)dataTab["enemy"];
            if (friendTab != null)
            {
                foreach (KeyValuePair<object, object> value in friendTab.dictKV)
                {
                    remoteObj = value.Value as RemoteObject;
                    if (remoteObj == null)
                        continue;
                    info = new FriendInfo((ulong)(remoteObj["uID"].GetInt()), remoteObj["nickName"].GetString(), remoteObj["level"].GetInt(), remoteObj["onLine"].GetBool());
                    friendList.Add(info);
                }
            }

            if (enemyTab != null)
            {
                foreach (KeyValuePair<object, object> value in enemyTab.dictKV)
                {
                    remoteObj = value.Value as RemoteObject;
                    if (remoteObj == null)
                        continue;
                    info = new FriendInfo((ulong)(remoteObj["uID"].GetInt()), remoteObj["nickName"].GetString(), remoteObj["level"].GetInt(), remoteObj["onLine"].GetBool());
                    enemyList.Add(info);
                }
            }
        }

        //更新好友的等级
        public void OnUpdateFriendLevel(ulong friendID, int level)
        {
            foreach (FriendInfo info in friendList)
            {
                if (info != null && info.playerID == friendID)
                {
                    info.level = level;
                    break;
                }
            }
        }

        //更新仇人的等级
        public void OnUpdateEnemyLevel(ulong roleID, int level)
        {
            foreach (FriendInfo info in enemyList)
            {
                if (info != null && info.playerID == roleID)
                {
                    info.level = level;
                    break;
                }
            }
        }
        //更新好友在线状态
        public void OnFriendOnLineStatus(ulong roleID, bool status)
        {
            foreach (FriendInfo info in friendList)
            {
                if (info != null && info.playerID == roleID)
                {
                    info.onLine = status;
                    break;
                }
            }
        }
        //更新仇人在线状态
        public void OnEnemyOnLineStatus(ulong roleID, bool status)
        {
            foreach (FriendInfo info in enemyList)
            {
                if (info != null && info.playerID == roleID)
                {
                    info.onLine = status;
                    break;
                }
            }
        }
    }

    //对角色好友集合进行排序，根据level排序
    public class FriendListSortComparer : System.Collections.IComparer
    {
        public int Compare(Object object1, Object object2)
        {
            FriendInfo info1 = (FriendInfo)object1;
            FriendInfo info2 = (FriendInfo)object2;
            return info2.level.CompareTo(info1.level);
        }
    }

    class FriendInfo
    {
        public ulong playerID;//玩家ID
        public string nickName;//玩家昵称
        public bool onLine;//玩家是否在线
        public int level;//玩家等级
        public KGender sex;//玩家性别
        //public ulong onLineTime;//上线时间
        // public int serverID; //玩家所在服务
        public FriendInfo(ulong friendID, string friendName, int friendLevel, bool isOnLine)
        {
            playerID = friendID;
            nickName = friendName;
            level = friendLevel;
            onLine = isOnLine;
        }
    }
    class FriendMsgInfo
    {
        public ulong playerID;
        public string nickName;
        public int level;
        public FriendMsgInfo(ulong senderID, string name, int nLevel)
        {
            playerID = senderID;
            nickName = name;
            level = nLevel;
        }
    }
}
