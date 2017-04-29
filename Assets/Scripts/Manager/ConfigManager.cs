using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using Assets.Scripts.Define;

namespace Assets.Scripts.Manager
{
    class ConfigManager
    {
        private string account;
        public bool DebugMode = false;
        public int GroupID = (int)KConstDefine.cdDefaultGroupID;
        public string Account
        {
            set
            {
                account = value;
            }
            get
            {
                if (Application.isWebPlayer)
                {
                    //return "account_" + new Random().Next(1001, 10000);
                    return account;
                }
                else
                {
					//return "lchtest003";
                    return "account_" + new Random().Next(1001, 10000);
                    //return "account_22224";
                    //return "account_3234121";
                    //return "dfsgsdgfb";
                }
            }
        }

        public static uint INVALID_ID = 0;

        private static ConfigManager instance;
        public static ConfigManager GetInstance()
        {
            if (instance == null)
            {
                instance = new ConfigManager();
            }
            return instance;
        }
    }
}
