using System;
using System.Collections.Generic;
using Assets.Scripts.Lib.Net;
using Assets.Scripts.Proto;
using Assets.Scripts.Utils;
using Assets.Scripts.Define;
using Assets.Scripts.Model.Player;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.View.Scene;
using Assets.Scripts.Manager;
using Assets.Scripts.Data;
using Assets.Scripts.Model.Scene;
using UnityEngine;
using NetMessage;
using ProtoBuf;
using Assets.Scripts.View.Skill;
using Assets.Scripts.Logic.Scene;
using Assets.Scripts.View.MainUI;
using Assets.Scripts.Logic.Scene.SceneObject.Compont;
using Assets.Scripts.View.ButtonBar;

namespace Assets.Scripts.Logic.Skill
{
    public enum SkillType
    {
        stActive,
        stPassive,
        stTalent,
    }

    public class SkillLogic : BaseLogic
    {
		public uint [] activeSkillList = new uint[0];
        private SkillView skillView = null;
        public Dictionary<uint, ActiveSkillData> ActiveSkillDict = new Dictionary<uint, ActiveSkillData>();
        public Dictionary<uint, PassiveSkillData> PassiveSkillDict = new Dictionary<uint, PassiveSkillData>();
        public Dictionary<uint, PassiveSkillData> TalentSkillDict = new Dictionary<uint, PassiveSkillData>();

        public uint SkillPoint = 10;

        protected override void InitListeners()
        {
            RegistSocketListener(KS2C_Protocol .s2c_upgrade_skill_respond, OnUpgradeSkill, typeof(S2C_UPGRADE_SKILL_RESPOND));
            RegistSocketListener(KS2C_Protocol.s2c_sync_skill_data_list, OnSyncSkillDataList, typeof(S2C_SYNC_SKILL_DATA_LIST));
            RegistSocketListener(KS2C_Protocol.s2c_sync_skill_point, OnSyncSkillPoint, typeof(S2C_SYNC_SKILL_POINT));
          	EventDispatcher.GameWorld.Regist(ControllerCommand.OPEN_SKILL_PANEL, OpenSkillView);
			//
		}

        public void SendUpgradeSkill(byte isActiveSkill, ushort skillID)
        {
            C2S_UPGRADE_SKILL_REQUEST request = new C2S_UPGRADE_SKILL_REQUEST();
            request.protocolID = (byte)KC2S_Protocol.c2s_upgrade_skill_request;
            request.IsActiveSkill = isActiveSkill;
            request.SkillID = skillID;
            SendMessage(request);
        }

        private void OnUpgradeSkill(KProtoBuf buf)
        {
            S2C_UPGRADE_SKILL_RESPOND respond = buf as S2C_UPGRADE_SKILL_RESPOND;
            if (respond.Sucess == 1)
            {
                log.Info("升级成功");
                UpgradeSkill(respond.SkillID, respond.IsActiveSkill);
            }
        }

        private void OnSyncSkillDataList(KProtoBuf buf)
        {
            S2C_SYNC_SKILL_DATA_LIST respond = buf as S2C_SYNC_SKILL_DATA_LIST;
            KNMSkillDataList skillList = Serializer.Deserialize<KNMSkillDataList>(respond.SkillDataList);
            SetSkillList(skillList);
        }

        private void OnSyncSkillPoint(KProtoBuf buf)
        {
            //天赋技能用到的天赋
            S2C_SYNC_SKILL_POINT respond = buf as S2C_SYNC_SKILL_POINT;
            SkillPoint = respond.SkillPoint;
        }

        private object OpenSkillView(params object[] objs)
        {
            if (skillView == null)
            {
                skillView = new SkillView();
            }
            else
            {
                skillView.Show();
            }
            return null;
        }

        //升级主动技能
        public void UpgradeSkill(uint skillID, int type)
        {
            int skillIdx = 0;
            if (type == 1)
            {
                skillIdx = 0;
                ActiveSkillData activeSkillData;
                if (ActiveSkillDict.TryGetValue(skillID, out activeSkillData) == false)
                {
                    activeSkillData = new ActiveSkillData();
					activeSkillData.SkillID = skillID;
                    ActiveSkillDict.Add(skillID, activeSkillData);
					uint [] oldList = activeSkillList;
					activeSkillList = new uint[activeSkillList.Length+1];
					Array.Copy(oldList,activeSkillList,oldList.Length);
					activeSkillList[oldList.Length] = skillID;
					
					HashSet<uint> rushSkillList = KConfigFileManager.GetInstance().GetRushSkillSet();
					if ( !rushSkillList.Contains(skillID) )
					{
						for (int i = 0 ; i < ShortCutDataManager.Instance.Count ;i++ )
						{
							ShortCutData data = ShortCutDataManager.Instance.datas[i];
							if (data.type == SHORTCUTITEM.NONE)
							{
								ButtonBarView.GetInstance().SetShortCut(i,SHORTCUTITEM.SKILL,(int)skillID,true);
								break;
							}
						}
					}
                }
                activeSkillData.Level += 1;
				KActiveSkill skill = KConfigFileManager.GetInstance().GetActiveSkill( activeSkillData.SkillID ,activeSkillData.Level);
				activeSkillData.cdTicket.SetCD(skill.SkillCD);
				
            }
            else
            {
                KPassiveSkill passiveSkillSetting = KConfigFileManager.GetInstance().GetPassiveSkill(skillID, 1);
                PassiveSkillData passiveSkillData;
                if (passiveSkillSetting.SkillType > 0)
                {
                    if (PassiveSkillDict.TryGetValue(skillID, out passiveSkillData) == false)
                    {
                        passiveSkillData = new PassiveSkillData();
                        PassiveSkillDict.Add(skillID, passiveSkillData);
                    }
                    passiveSkillData.Level += 1;
                    skillIdx = 1;
                } 
                else
                {
                    if (TalentSkillDict.TryGetValue(skillID, out passiveSkillData) == false)
                    {
                        passiveSkillData = new PassiveSkillData();
                        TalentSkillDict.Add(skillID, passiveSkillData);
                    }
                    passiveSkillData.Level += 1;
                    skillIdx = 2;
                }
            }
            EventDispatcher.GameWorld.Dispath(ControllerCommand.UPDATE_SKILL, skillIdx, skillID);
        }

        public void SetSkillList(KNMSkillDataList skillList)
        {
            ActiveSkillDict.Clear();
			activeSkillList = new uint[skillList.ActiveSkillData.Count];
			int index = 0;
            foreach (KNMActiveSkillData nmactiveSkillData in skillList.ActiveSkillData)
            {
                ActiveSkillData activeSkillData = new ActiveSkillData();
                activeSkillData.Level = nmactiveSkillData.SkillLevel;
                activeSkillData.SkillExp = nmactiveSkillData.SkillExp;
                activeSkillData.SkillID = nmactiveSkillData.SkillID;
                ActiveSkillDict.Add(activeSkillData.SkillID, activeSkillData);
				activeSkillList[index++] =  nmactiveSkillData.SkillID;
				KActiveSkill skill = KConfigFileManager.GetInstance().GetActiveSkill( activeSkillData.SkillID ,activeSkillData.Level);
				activeSkillData.cdTicket.SetCD(skill.SkillCD);
            }
			

            PassiveSkillDict.Clear();
            foreach (KNMPassiveSkillData nmapassiveSkillData in skillList.PassiveSkillData)
            {
                PassiveSkillData passiveSkillData = new PassiveSkillData();
                passiveSkillData.Level = nmapassiveSkillData.SkillLevel;
                passiveSkillData.SkillID = nmapassiveSkillData.SkillID;

                KPassiveSkill passiveSkillSetting = KConfigFileManager.GetInstance().GetPassiveSkill(passiveSkillData.SkillID, 1);
                if (passiveSkillSetting.SkillType > 0)
                {
                    PassiveSkillDict.Add(passiveSkillData.SkillID, passiveSkillData);
                }
                else
                {
                    TalentSkillDict.Add(passiveSkillData.SkillID, passiveSkillData);
                }
            }
        }

        //天赋是否能学习
        public bool CanLearnTalent(uint skillID)
        {
            KPassiveSkill skill = KConfigFileManager.GetInstance().GetPassiveSkill(skillID, 1); //得到需要学习的技能
            
            foreach (KPassiveSkill s in skill.GetReqSkillList()) //得到此技能需要学习的技能
            {
                bool canLearn = false;
                foreach (PassiveSkillData skillData in TalentSkillDict.Values)
                {
                    if (skillData.SkillID == s.SkillID)
                    {

                    }
                }
            }
            return true;
        }

        Ticker ticker = new Ticker(500);
		public float GetAllSkillCDNormal()
		{
			return ticker.GetTimeNormal();
		}
        public bool RequestSkill(uint skillId)
        {
			KActiveSkill _skill = KConfigFileManager.GetInstance().GetActiveSkill(skillId, 1);
			ticker.SetCD(_skill.AllSkillCD);
            if (ticker.IsEnable())
            {
                ActiveSkillData skill = GetActiveSkillVOByID(skillId);
                if( null == skill)
					return false;
				if (skill.cdTicket.IsEnable())
                {
                    return true;
                }
            }
            return false;
        }

        public void ResetSkillCD(uint skillId)
        {
            ticker.Restart();
            ActiveSkillData skill = GetActiveSkillVOByID(skillId);
            if (null == skill)
            {
                return;
            }
            if (skill.cdTicket.IsEnable())
            {
                skill.cdTicket.Restart();
            }
        }

        public ActiveSkillData GetActiveSkillVOByID(uint skillID)
        {
            if (ActiveSkillDict.ContainsKey(skillID))
                return ActiveSkillDict[skillID];

            return null;
        }

        public void DefaultSkill()
        {
            ushort job = PlayerManager.GetInstance().MajorPlayer.Job;
            uint defaultSkillId = GetDefaultSkillId(job);
            ActiveSkillData skillData = new ActiveSkillData();
            KActiveSkill skillSetting = KConfigFileManager.GetInstance().GetActiveSkill(defaultSkillId, 1);
            skillData.SkillID = (ushort)defaultSkillId;
            ActiveSkillDict.Add(skillData.SkillID, skillData);
			skillData.cdTicket.SetCD(skillSetting.SkillCD);
        }

        //得到职业默认的技能ID
        public uint GetDefaultSkillId(ushort job)
        {
            if (job == 1)
            {
                return 1;
            }
            else if (job == 2)
            {
                return 2;
            }
            else if (job == 3)
            {
                return 3;
            }
			else if (job == 4)
            {
                return 4;
            }
            return 0;
        }

        private static SkillLogic instance;
        public static SkillLogic GetInstance()
        {
            if (instance == null)
                instance = new SkillLogic();
            return instance;
        }
		public void OnSkill(ushort skillId)
		{
			AnimationComponent.OperaWalking = false;
			if(SkillLogic.GetInstance().RequestSkill(skillId)){
				KSkillDisplay skillDisplay = KConfigFileManager.GetInstance().GetSkillDisplay(skillId,(uint)SceneLogic.GetInstance().MainHero.Job);
				
				if(skillDisplay.Opera.CompareTo("NONE")==0)
				{
	                SceneLogic.GetInstance().MainHero.Action.SendSkill(skillId);
				}
				else if(skillDisplay.Opera.CompareTo("TARGET")==0)
				{
	                if (SceneLogic.GetInstance().MainHero.property.target != null && SceneLogic.GetInstance().MainHero.property.target.property.isCanAttack && !SceneLogic.GetInstance().MainHero.property.target.property.isDeaded)
					{
	                    SceneLogic.GetInstance().MainHero.Action.MoveAndSkill(skillId, SceneLogic.GetInstance().MainHero.property.target);
					}
				}
				else if(skillDisplay.Opera.CompareTo("DIR_RAND")==0)
				{
	                EventRet ret = SceneLogic.GetInstance().MainHero.DispatchEvent(ControllerCommand.TryFinishAction);
	                bool bRet = (bool)ret.GetReturn<AnimationComponent>();
	                if (!bRet)
						return;
					ActiveSkillData skillVO = SkillLogic.GetInstance().GetActiveSkillVOByID(skillId);
					KActiveSkill skill = KConfigFileManager.GetInstance().GetActiveSkill((uint)skillId,skillVO.Level);
					if(null == skill)
						return;
					float  CastMinRange = ((float)skill.CastMinRange) /100f;
					float CastRange = ((float)skill.CastRange) / 100f;
					Vector3 currMousePoint = KingSoftCommonFunction.ScreenMouseToGround(SceneLogic.GetInstance().MainHero.Position);
					currMousePoint = new Vector3(currMousePoint.x,SceneLogic.GetInstance().MainHero.Position.y,currMousePoint.z);
					Vector3 dir = currMousePoint - SceneLogic.GetInstance().MainHero.Position;
					dir = new Vector3(dir.x,0,dir.z);
					float f = Vector3.Distance(Vector3.zero,dir);
					if(f < CastMinRange){
						Vector3 dir2 = dir.normalized*CastMinRange;
						currMousePoint = SceneLogic.GetInstance().MainHero.Position + dir2; 
					}
					else if(f>CastRange)
					{
						Vector3 dir2 = dir.normalized*CastRange;
						currMousePoint = SceneLogic.GetInstance().MainHero.Position + dir2; 
					}
					Vector3 p = KingSoftCommonFunction.NearPosition(currMousePoint);
	                Vector3 midPoint = SceneLogic.GetInstance().MainHero.Position + (p-SceneLogic.GetInstance().MainHero.Position)*0.5f;
					if(KingSoftCommonFunction.IsPointCanWalk(midPoint))
					{
						SceneLogic.GetInstance().MainHero.Action.SendSkill(skillId, p);
					}
					
				}
				else if(skillDisplay.Opera.CompareTo("TARGET_DIR")==0)
				{
	                EventRet ret = SceneLogic.GetInstance().MainHero.DispatchEvent(ControllerCommand.TryFinishAction);
	                bool bRet = (bool)ret.GetReturn<AnimationComponent>();
					if(!bRet)
						return;
					ActiveSkillData skillVO = SkillLogic.GetInstance().GetActiveSkillVOByID(skillId);
					KActiveSkill skill = KConfigFileManager.GetInstance().GetActiveSkill((uint)skillId,skillVO.Level);
					if(null == skill)
						return;
					float CastRange = 2f;
					Vector3 currMousePoint;
					if (SceneLogic.GetInstance().MainHero.property.target != null && SceneLogic.GetInstance().MainHero.property.target.property.isCanAttack && !SceneLogic.GetInstance().MainHero.property.target.property.isDeaded)
					{
	                    currMousePoint = SceneLogic.GetInstance().MainHero.property.target.Position;
					}
					else
					{
						currMousePoint = KingSoftCommonFunction.ScreenMouseToGround(SceneLogic.GetInstance().MainHero.Position);
					}
					
					Vector3 dir = currMousePoint - SceneLogic.GetInstance().MainHero.Position;
					dir = new Vector3(dir.x,0,dir.z);
					if(dir.x==0&&dir.z==0)
					{
						dir = new Vector3(0f,1f,0f);
					}
					float f = Vector3.Distance(Vector3.zero,dir);
					SceneLogic.GetInstance().MainHero.Forward = dir;
					Vector3 dir2 = dir.normalized*CastRange;
					currMousePoint = SceneLogic.GetInstance().MainHero.Position + dir2; 
					Vector3 p = KingSoftCommonFunction.NearPosition(currMousePoint);
	                SceneLogic.GetInstance().MainHero.Action.SendSkill(skillId, p);
				}	
			}
		}
    }
}
