using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Manager;
using Assets.Scripts.Model.Scene;
using Assets.Scripts.Define;

namespace Assets.Scripts.Model.Player
{
    class Player
    {
        public ulong PlayerID = GameWorldSettings.INVALID_ID;
        public string PlayerName = null;
        public KGender Gender;              //性别
        public int groupID = 0;
		public uint hero = 0;

        public ushort uJob;
        public byte vipLevel;
        public uint vipExp;
        public uint vipEndTime;
        public byte level = 0;
        public uint Exp = 0;

        public string spouseName;
        public string factionName;
		
		public int money;
		public int coin;
		public int menterPoint;

        public uint maxExp;//当前升级所需经验值
    }
}
