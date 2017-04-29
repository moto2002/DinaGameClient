using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Lib.Resource;
using Assets.Scripts.Define;

namespace Assets.Scripts.Data
{
    public class KHeroSetting : AKTabFileObject
    {
        public uint ID;
        public string Name;
        public KHeroObjectType HeroType;
        public uint RepresentID;
		public string FbxName;
		public uint AttackSkill;
		public uint DefaultWeapon;
        public KGender Gender;
		public float TipPos0;
        public uint AI;
        public uint AttributeID;
        public uint RefreshSec;
        public string SkillList;
        public ulong Exp;
        public uint Level;
        public KMonsterGrade MonsterGrade;
        public uint ViewRange;
        public uint WalkRange;
        public string Caption;
        public string Title;
        public float Scale;
		public int WeaponTrailTime;
		public string WeaponTrailBeginColor;
		public string WeaponTrailEndColor;
		public int RushSkill;
		
		
        public override string getKey()
        {
            return ID.ToString();
        }
    }
}
