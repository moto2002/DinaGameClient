using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Lib.Resource;


namespace Assets.Scripts.Data
{
    public class KTabLineRoleBaseInfoClient : AKTabFileObject
    {
        public int id;
        public string title;
        public int hp;
        public int mp;
        public long exp;
        public int attack;
        public int defend;
        public int hit;
        public int dodge;
        public int crite;
        public int criteResistance;
        public int armor;
        public int sunderArmor;
        public int speed;
        public int attackSpeed;

        public override string getKey()
        {
            return id.ToString();
        }
    }
}
