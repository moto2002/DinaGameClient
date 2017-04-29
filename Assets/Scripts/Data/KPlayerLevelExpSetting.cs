using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Lib.Resource;

namespace Assets.Scripts.Data
{
    public class KPlayerLevelExpSetting : AKTabFileObject
    {
        public byte LevelIndex = 0;
        public uint Exp = 0;

        public override string getKey()
        {
            return LevelIndex.ToString();
        }

        public override void onComplete()
        {
        }
    }
}
