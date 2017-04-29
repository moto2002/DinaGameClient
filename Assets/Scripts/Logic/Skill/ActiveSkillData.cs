using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Logic.Skill
{
    public class ActiveSkillData
    {
        public uint SkillID;
        public uint Level;
        public uint SkillExp;
        public Ticker cdTicket = new Ticker(6000);
		public ActiveSkillData()
		{
			cdTicket.SetActive();
		}
    }
}
