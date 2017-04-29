using System;
using System.Collections.Generic;
using Assets.Scripts.Lib.Resource;

namespace Assets.Scripts.Data
{
    public class KMissionLoaclInfo : AKTabFileObject
    {
        public int nID;
        public int nLevelLimt;
        public int nSubmitLv;
        public int nPreID;
        public int nType;
        public int nSubType;
        public int nPlotID;
		public bool bScript;
        public string QuestName;
        public string Condition;
        public string ConditionNums;
        public string NeedItemIDs;
        public string NeedItemNums;
        public int nRewardExp;
        public string RewardItemTypes;
        public string RewardItemIDs;
		public string RewardItemNums;
        public int nRewardMoney;
        public int nRewardGold;
        public int nNpcID;
        public int nSubmitNpcID;
        public string Dialogue1;
        public int nDialogue2;
        public int nDialogue3;
        public string Describe;
        //public string Tips;
        public string Position;
        public string PathType;
        public int PathAIType;
        public bool bAutoComplete;
        public int nTimes;

        public override string getKey()
        {
            return nID.ToString();
        }
    }
}
