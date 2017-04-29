using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Define;
using System.IO;
using Assets.Scripts.Model.Scene;
using Assets.Scripts.Logic.RemoteCall;

namespace Assets.Scripts.Model.Player
{
    class MajorPlayer : Player
    {
        public int LastSaveTime;            //上一次保存时间
        public int LastLoginTime;           //上一次登录时间
        public int TotalGameTime;           //总计游戏时间
        public int CreateTime;              //创建账户的时间
        //public KGender Gender;              //性别
        public sbyte CanRename;             //是否重新命名过角色名
        public string AccountName;          //账户名
        public int ServerTime;              //服务器时间
        public ulong ClubID;
        public int GroupID;
        public MemoryStream guid = null;

        public KGender gender;
        public ushort Job;

        public int levelCurrent = 0; //当前等级
        public int combat = 0; //战力
        public float onlineTime = 0; //1s为单位
        public Dictionary<int,bool> rewardData = new Dictionary<int,bool>();//key=reward tab ID value = isGained

        public int money;//金钱
        public int coin;//元宝
        public int menterPoint;//点卷

        public int addMoney;//金钱
        public int addCoin;//元宝
        public int addMenterPoint;//点卷
        public uint addExp;

        private PlayerHeroData heroData = new PlayerHeroData();

        public PlayerHeroData HeroData
        {
            get { return heroData; }
        }
    }
}
