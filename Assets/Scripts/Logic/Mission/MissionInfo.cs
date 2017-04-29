using System;
using System.Collections.Generic;

namespace Assets.Scripts.Logic.Mission
{
    public class MissionInfo
    {
        public enum MisssionStatus
        {
            None,
            Accept,
            BeenAccepted,
            Finish
        };

        public enum MissionType
        {
            MainMission = 1,
            SubLineMission,
            DaliyMission,
        }

        public enum MissionSubType
        {
            Normal = 1,
            Monster,
            Collect,
        }

        public int id;
        public int levelLimt;
        public int submitLv;
        public int preID;
        public int type;
        public int subType;
        public int nPlotID;
		public bool bScript;
        public string questName;
        public string condition;
        public int[] conditionNums;
        public int[] curConditionNums;
        public int[] needItemIDs;
        public int[] needItemNums;
        public int exp;
        public int[] rewardTypes;
        public int[] rewardItemIDs;
        public int[] rewardItemNums;
        public int money;
        public int gold;
        public int npcID;
        public int submitNpcID;
        public int[] dialogue1;
        public int dialogue2;
        public int dialogue3;
        public string desc;
        public string tips;
        public int[] position;
        public string pathType;
        public int pathAiType;
        public bool isAutoComplete;
        public int totalTimes;
        public int curTimes;
        public MisssionStatus curStatus;
    }
}

