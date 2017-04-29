using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Utils;
using Assets.Scripts.Data;

namespace Assets.Scripts.Model.Skill
{
    class Attribute
    {
        public AttributeType key = 0;
		public int value1 = 0;
		public int value2 = 0;
		
		public Attribute()
		{
		}
		
		public void copy(Attribute attr)
		{
			key = attr.key;
			value1 = attr.value1;
			value2 = attr.value2;
		}
				
        public string getAttributeText()
        {
            string descText = null;
			
            string value1Str = " " + Util.formatSignedInteger(value1);
            string value2Str = " " + Util.formatSignedInteger(value2);
			
            string text = "";
            switch(key)
            {
                case AttributeType.atInvalid:
                    break;
                case AttributeType.atMaxEndurance:
                    text = KLocalizationText.getValueByKey("attribute.endurance");
                    descText = text + value1Str;
                    break;
                case AttributeType.atEnduranceReplenish:
                    text = KLocalizationText.getValueByKey("attribute.endurance_replenish");
                    descText = text + value1Str;
                    break;
                case AttributeType.atEnduranceReplenishPercent:
                    text = KLocalizationText.getValueByKey("attribute.endurance_replenish");
                    descText = text + value1Str + "%";
                    break;
                case AttributeType.atMaxStamina:
                    text = KLocalizationText.getValueByKey("attribute.stamina");
                    descText = text + value1Str;
                    break;
                case AttributeType.atStaminaReplenish:
                    text = KLocalizationText.getValueByKey("attribute.stamina_replenish");
                    descText = text + value1Str;
                    break;
                case AttributeType.atStaminaReplenishPercent:
                    text = KLocalizationText.getValueByKey("attribute.stamina_replenish");
                    descText = text + value1Str + "%";
                    break;
                case AttributeType.atMaxAngry:
                    text = KLocalizationText.getValueByKey("attribute.max_angry");
                    descText = text + value1Str;
                    break;
                case AttributeType.atAdditionalRecoveryAngry:
                    text = KLocalizationText.getValueByKey("attribute.additional_angry");
                    descText = text + value1Str + "%";
                    break;
                case AttributeType.atWillPower:
                    text = KLocalizationText.getValueByKey("attribute.willpower");
                    descText = text + value1Str;
                    break;
                case AttributeType.atInterference:
                    text = KLocalizationText.getValueByKey("attribute.interference");
                    descText = text + value1Str;
                    break;
                case AttributeType.atInterferenceRange:
                    text = KLocalizationText.getValueByKey("attribute.interference_range");
                    descText = text + value1Str;
                    break;
                case AttributeType.atAddInterferenceRangePercent:
                    text = KLocalizationText.getValueByKey("attribute.interference_range");
                    descText = text + value1Str + "%";
                    break;
                case AttributeType.atAttackPoint:
                    text = KLocalizationText.getValueByKey("attribute.attackpoint");
                    descText = text + value1Str;
                    break;
                case AttributeType.atAttackPointPercent:
                    text = KLocalizationText.getValueByKey("attribute.attackpoint");
                    descText = text + value1Str + "%";
                    break;
                case AttributeType.atAgility:
                    text = KLocalizationText.getValueByKey("attribute.agility");
                    descText = text + value1Str + "%";
                    break;
                case AttributeType.atCritPoint:
                    text = KLocalizationText.getValueByKey("attribute.crit_point");
                    descText = text + value1Str;
                    break;
                case AttributeType.atCritRate:
                    text = KLocalizationText.getValueByKey("attribute.crit_rate");
                    descText = text + value1Str + "%";
                    break;
                case AttributeType.atDefense:
                    text = KLocalizationText.getValueByKey("attribute.defense");
                    descText = text + value1Str;
                    break;
                case AttributeType.atDefensePercent:
                    text = KLocalizationText.getValueByKey("attribute.defense");
                    descText = text + value1Str + "%";
                    break;
                case AttributeType.atRunSpeedBase:
                    text = KLocalizationText.getValueByKey("attribute.run_speed");
                    descText = text + value1Str;
                    break;
                case AttributeType.atAddMoveSpeedPercent:
                    text = KLocalizationText.getValueByKey("attribute.run_speed");
                    descText = text + value1Str + "%";
                    break;
                case AttributeType.atJumpSpeedBase:
                    text = KLocalizationText.getValueByKey("attribute.jump_speed");
                    descText = text + value1Str;
                    break;	
                case AttributeType.atJumpSpeedPercent:
                    text = KLocalizationText.getValueByKey("attribute.jump_speed");
                    descText = text + value1Str + "%";
                    break;	
                case AttributeType.atShootBallHitRate:
                    text = KLocalizationText.getValueByKey("attribute.shoot_ball_hit_rate");
                    descText = text + value1Str + "%";
                    break;
                case AttributeType.atNomalShootForce:
                    text = KLocalizationText.getValueByKey("attribute.normal_shoot_force");
                    descText = text + value1Str;
                    break;
                case AttributeType.atSkillShootForce:
                    text = KLocalizationText.getValueByKey("attribute.skill_shoot_force");
                    descText = text + value1Str;
                    break;
                case AttributeType.atSlamDunkForce:
                    text = KLocalizationText.getValueByKey("attribute.slamdunk_force");
                    descText = text + value1Str;
                    break;
                default:
                    text = KLocalizationText.getValueByKey("attribute.unknow");
                    descText = text + key + value1Str;
                    break;
            }			
            return descText;
        }
    }
}
