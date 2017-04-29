using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Lib.Resource;
using Assets.Scripts.Data;
using Assets.Scripts.Utils;
using Assets.Scripts.Model.Skill;
using Assets.Scripts.Manager;
using Assets.Scripts.Model.Player;
using Assets.Scripts.Logic.Mission;
using Assets.Scripts.Logic.Npc;
using Assets.Scripts.Logic.Intensify;

namespace Assets.Scripts.Lib.Loader
{
    public class KConfigFileManager
    {
        public delegate void LoadAllComplete();
        public event LoadAllComplete OnLoadAllComplete;
        private int ToBeLoadTotalCount = 0;
        private int LoadedCount = 0;
        
        public Dictionary<uint, KTabFile<KMapTriggerInfo>> mapMapsTrigger = null;

        public KTabFile<KLocalizationText> localizationTexts = null;
        public KTabFile<KTabLineShowInfo> showTabInfos = null;
        public KTabFile<KHeroSetting> heroSetting = null;
        public KTabFile<KMapListSetting> mapListSetting = null;
        public KTabFile<KActiveSkill> activeSkillList = null;
        public KTabFile<KPassiveSkill> passiveSkillList = null;
        public KTabFile<KPve> pveList  = null;
        public KTabFile<KMissionLoaclInfo> missionLoaclInfoList = null;
        public KTabFile<KMissionDialogue> missionDialogueList = null;
        public KTabFile<KPlayerLevelExpSetting> playerLevelSetting = null;
		public KTabFile<KBuff> buffs = null;
		public KTabFile<KParams> kparams = null;
		public KTabFile<KSkillDisplay> skillDisplay = null;
        public KTabFile<KGiftData> giftTabFile = null;
        public KTabFile<KCollectMissionInfo> collectMissionInfo = null;
        public KTabFile<KPlotInfo> plotInfo = null;

        public KTabFile<KTabLineItem> itemTabInfos = null;
		public KTabFile<KTabLineEquip> equipTabInfos = null;
		public KTabFile<KEquipStrengthen> equipStrengthenTab = null;
		public KTabFile<KEquipStep> equipSteptab = null;
		public KTabFile<KTabServerEquip> equipServerTab = null;
		public KTabFile<KTabClientEquip> equipClientTab = null;
		public KTabFile<KTabEquipSuit> equipSuitTab = null;

		public KTabFile<KNpcPos> npcPoss = null;
		public KTabFile<KAminEvent> animEvent = null;
        public KIniFile clientConfig = null;
        public KResourceVersion resourceVersion = null;

		
        private KConfigFileManager()
        {
            
        }
		
        public void LoadConfigFile()
        {
			kparams = new KTabFile<KParams>("Settings/params",  LoadComplet);
            ++ToBeLoadTotalCount;
			
			animEvent = new  KTabFile<KAminEvent>("Settings/skill/anim_event",  LoadComplet);
			++ToBeLoadTotalCount;
			
            localizationTexts = new KTabFile<KLocalizationText>("ClientSettings/localizationtext", LoadComplet);
            ++ToBeLoadTotalCount;

            itemTabInfos = new KTabFile<KTabLineItem>("ClientSettings/other", LoadComplet);
            ++ToBeLoadTotalCount;

            equipTabInfos = new KTabFile<KTabLineEquip>("ClientSettings/equip", LoadComplet);
            ++ToBeLoadTotalCount;
			
			equipStrengthenTab = new KTabFile<KEquipStrengthen>("Settings/item/equip_strengthen" , LoadComplet);
			++ToBeLoadTotalCount;
			
			equipSteptab = new KTabFile<KEquipStep>("Settings/item/equip_step" ,IntensifyLogic.GetInstance().EquipStepComplete , LoadComplet);
			++ToBeLoadTotalCount;
			
			equipServerTab = new KTabFile<KTabServerEquip>("Settings/item/equip" , LoadComplet);
			++ToBeLoadTotalCount;
			
			equipClientTab = new KTabFile<KTabClientEquip>("ClientSettings/equip_client" , LoadComplet);
			++ToBeLoadTotalCount;
			
			equipSuitTab = new KTabFile<KTabEquipSuit>("Settings/item/equip_suit" , LoadComplet);
			++ToBeLoadTotalCount;
			
            showTabInfos = new KTabFile<KTabLineShowInfo>("ClientSettings/show", LoadComplet);
            ++ToBeLoadTotalCount;

            skillDisplay = new KTabFile<KSkillDisplay>("Settings/skill/skill_display", LoadComplet);
            ++ToBeLoadTotalCount;

            giftTabFile = new KTabFile<KGiftData>("Settings/gift", LoadComplet);
            ++ToBeLoadTotalCount;

            heroSetting = new KTabFile<KHeroSetting>("Settings/hero/hero", LoadComplet);
            ++ToBeLoadTotalCount;

            plotInfo = new KTabFile<KPlotInfo>("Settings/mission/plot_info", LoadComplet);
            ++ToBeLoadTotalCount;

            mapListSetting = new KTabFile<KMapListSetting>("Settings/map_list", LoadMapListComplet);
            ++ToBeLoadTotalCount;

            buffs = new KTabFile<KBuff>("Settings/skill/buff_info", GameApplication.BuffderLoadComplete, LoadComplet);
            ++ToBeLoadTotalCount;
			
         //   activeSkillList = new KTabFile<KActiveSkill>("Settings/skill/active_skill", KSkillManager.GetInstance().ActiveSkillLoadComplete, LoadComplet);
            activeSkillList = new KTabFile<KActiveSkill>("Settings/skill/active_skill", LoadComplet);
            ++ToBeLoadTotalCount;

         //   passiveSkillList = new KTabFile<KPassiveSkill>("Settings/skill/passive_skill", KSkillManager.GetInstance().PassiveSkillLoadComplete, LoadComplet);
            passiveSkillList = new KTabFile<KPassiveSkill>("Settings/skill/passive_skill", LoadComplet);
            ++ToBeLoadTotalCount;

            missionLoaclInfoList = new KTabFile<KMissionLoaclInfo>("Settings/quests", MissionLogic.GetInstance().MissionLoadComplete, LoadComplet);
            ++ToBeLoadTotalCount;

            missionDialogueList = new KTabFile<KMissionDialogue>("Settings/mission/dialogue", MissionLogic.GetInstance().DialogueLoadComplete, LoadComplet);
            ++ToBeLoadTotalCount;

            playerLevelSetting = new KTabFile<KPlayerLevelExpSetting>("Settings/player_level_exp", PlayerLevelExp.GetInstance().PlayerLevelExpLoadComplete, LoadComplet);
            ++ToBeLoadTotalCount;

            npcPoss = new KTabFile<KNpcPos>("ClientSettings/npc_pos", NpcLogic.GetInstance().NpcPosLoadComplete, LoadComplet);
            ++ToBeLoadTotalCount;

            clientConfig = new KIniFile("ClientSettings/config", LoadComplet);
            ++ToBeLoadTotalCount;

            resourceVersion = new KResourceVersion(LoadComplet);
            ++ToBeLoadTotalCount;

            pveList = new KTabFile<KPve>("Settings/pve", LoadComplet);
            ++ToBeLoadTotalCount;

            collectMissionInfo = new KTabFile<KCollectMissionInfo>("Settings/mission/collect_mission", LoadComplet);
            ++ToBeLoadTotalCount;
        }

		public KSkillDisplay GetSkillDisplay(uint skillID, uint HeroID)
        {
            if (skillDisplay != null)
            {
                KSkillDisplay r = skillDisplay.getData(skillID + "_" + HeroID);
				if (null == r)
					r = skillDisplay.getData(skillID + "_0" );
				return r;
            }
            return null;
        }
		
        private void LoadComplet()
        {
            if (++LoadedCount == ToBeLoadTotalCount && OnLoadAllComplete != null)
            {
                OnLoadAllComplete.Invoke();
                OnLoadAllComplete = null;
            }
        }

        private void LoadMapListComplet()
        {
            LoadComplet();

            mapMapsTrigger = new Dictionary<uint, KTabFile<KMapTriggerInfo>>();
            foreach (KeyValuePair<string, KMapListSetting> kvp in mapListSetting.getAllData())
            {
                uint mapID = 0;
                if (!uint.TryParse(kvp.Key, out mapID))
                {
                    continue;
                }

                KTabFile<KMapTriggerInfo> info = new KTabFile<KMapTriggerInfo>("maps/" + kvp.Key + "/map_trigger", LoadComplet);
                ++ToBeLoadTotalCount;
                mapMapsTrigger.Add(mapID, info);
            }
        }

		public KParams GetParams()
		{
			if (kparams == null)
                return null;
			return kparams.getData("1");
		}

		public KAminEvent GetAnimEvent(uint id, string animName)
		{
			if (animEvent == null)
                return null;
            return animEvent.getData(id + "_" + animName);
		}

        public KActiveSkill GetActiveSkill(uint skillID, uint level)
        {
            if (activeSkillList == null)
                return null;

            return activeSkillList.getData(skillID + "_" + level);
        }

        public KPassiveSkill GetPassiveSkill(uint skillID, uint level)
        {
            if (passiveSkillList != null)
            {
                return passiveSkillList.getData(skillID + "_" + level);
            }
            return null;
        }

		public KHeroSetting GetHeroSetting(uint heroId)
		{
			return heroSetting.getData(heroId.ToString());
		}

		public HashSet<uint> GetRushSkillSet()
		{
			HashSet<uint> sets = new HashSet<uint>();
			foreach( KeyValuePair<string,KHeroSetting> kvp in heroSetting.getAllData())
			{
				if (kvp.Value.HeroType == Assets.Scripts.Define.KHeroObjectType.hotPlayer)
					sets.Add((uint)kvp.Value.RushSkill);
			}
			return sets;
		}

        public KMissionDialogue GetMissionDialogue(int dialogueID)
        {
            if (missionDialogueList != null)
            {
                return missionDialogueList.getData(dialogueID.ToString());
            }
            return null;
        }

        public KCollectMissionInfo GetCollectInfo(int collectID)
        {
            if (collectMissionInfo != null)
            {
                return collectMissionInfo.getData(collectID.ToString());
            }
            return null;
        }

        public KPlotInfo GetPlotInfo(int plotID)
        {
            if (plotInfo != null)
            {
                return plotInfo.getData(plotID.ToString());
            }
            return null;
        }

		public KEquipStrengthen GetForgeEquipStrengthen(int equipID , int equipLevel)
		{
			if(equipStrengthenTab != null)
			{
				return equipStrengthenTab.getData(equipID + "_" + equipLevel);
			}
			return null;
		}
		
		public KEquipStep GetForgeEquipStep(int equipLevel)
		{
			if(equipSteptab != null)
			{
				return equipSteptab.getData(equipLevel.ToString());
			}
			return null;
		}
		
		public KTabServerEquip GetEquipServerTab (int ID)
		{
			if(equipServerTab != null)
			{
				return equipServerTab.getData(ID.ToString());
			}
			return null;
		}
		
		public KTabClientEquip GetEquipClientTab (int ID)
		{
			if(equipClientTab != null)
			{
				return equipClientTab.getData(ID.ToString());
			}
			return null;
		}
		
		public KTabEquipSuit GetEquipSuitTab (int ID)
		{
			if(equipSuitTab != null)
			{
				return equipSuitTab.getData(ID.ToString());
			}
			return null;
		}

        private static KConfigFileManager instance = null;
        public static KConfigFileManager GetInstance()
        {
            if (instance == null)
            {
                instance = new KConfigFileManager();
            }
            return instance;
        }
    }
}
