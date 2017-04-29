

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Lib.Log;
using Assets.Scripts.Data;
using Assets.Scripts.View.Scene;
using Assets.Scripts.Utils;
using Assets.Scripts.Define;
using UnityEngine;


namespace Assets.Scripts.Model.Scene
{
    public class PlayerHeroData
    {
        private static Logger log = LoggerFactory.GetInstance().GetLogger(typeof(PlayerHeroData));

        public PlayerHeroData()
        {
        }

        //public int Level = 0;                           //等级
        //public int Experience = 0;                      //经验
        //public int Hp = 0;
        public string Name { get; set; }							//名字
        public int Level { get; set; }						//等级
        public string Icon { get; set; }                        //头像

        public sbyte Sex { get; set; }
        private Dictionary<KAttributeType, int> dictAttributeValue = new Dictionary<KAttributeType, int>();

        public int this[KAttributeType eType]
        {
            get
            {
                if (!dictAttributeValue.ContainsKey(eType))
                {
                    return 0;
                }
                return dictAttributeValue[eType];
            }
            set
            {
                dictAttributeValue[eType] = value;
            }
        }
    }
}
