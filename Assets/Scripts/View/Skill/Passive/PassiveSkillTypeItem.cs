using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.View.Skill
{
    public class PassiveSkillTypeItem : SkillItem
    {

        protected override void UpdateIcon()
        {
            if (data == null)
            {
                icon.spriteName = "lock";
            }
            else
            {
                icon.spriteName = "skill_t_" + data;
            }
            icon.MakePixelPerfect();
        }
    }
}
