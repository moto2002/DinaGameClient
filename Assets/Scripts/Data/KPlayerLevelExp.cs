using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Lib.Resource;

namespace Assets.Scripts.Data
{
    public class KPlayerLevelExp : AKTabFileObject
    {
        public int LevelIndex;
        public long Exp;

        public override string getKey()
        {
            return LevelIndex.ToString();
        }
    }
}
