using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Data;
using Assets.Scripts.Lib.Loader;

namespace Assets.Scripts.Model.Player
{
    public class PlayerLevelExp
    {
        public Dictionary<uint, byte> m_playerExpToLevelDictionary = new Dictionary<uint, byte>();
        public byte GetPlayerLevelByExp(uint nExp)
        {
            byte byLevel = 0;
            if (m_playerExpToLevelDictionary.TryGetValue(nExp, out byLevel))
                return byLevel;

            uint nLastExp = 0;
            for (byLevel = 1; byLevel <= m_nMaxLevel; ++byLevel)
            {
                KPlayerLevelExpSetting playerLevelExpSetting = KConfigFileManager.GetInstance().playerLevelSetting.getData(byLevel.ToString());
                if (playerLevelExpSetting.Exp >= nExp && nExp > nLastExp)
                    return (byte)(byLevel - 1);

                nLastExp = playerLevelExpSetting.Exp;
            }
            return 0;
        }

        public int m_nMaxLevel = 0;
        public void PlayerLevelExpLoadComplete()
        {
            Dictionary<string, KPlayerLevelExpSetting> playerLevelSettingDictionary = 
                KConfigFileManager.GetInstance().playerLevelSetting.getAllData();

            foreach (KeyValuePair<string, KPlayerLevelExpSetting> pair in playerLevelSettingDictionary)
            {
                m_playerExpToLevelDictionary.Add(pair.Value.Exp, pair.Value.LevelIndex);
                if (pair.Value.LevelIndex > m_nMaxLevel)
                    m_nMaxLevel = pair.Value.LevelIndex;
            }
        }
		
		
        
        private static PlayerLevelExp instance;
        public static PlayerLevelExp GetInstance()
        {
            if (instance == null)
            {
                instance = new PlayerLevelExp();
            }
            return instance;
        }
    }
}
