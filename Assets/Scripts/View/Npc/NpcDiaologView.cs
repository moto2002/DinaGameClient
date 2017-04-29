using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Lib.View;
using Assets.Scripts.Manager;
using Assets.Scripts.Data;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.View.Bag;
using Assets.Scripts.Logic.Item;
using Assets.Scripts.Logic.Mission;
using Assets.Scripts.Logic.Npc;
using Assets.Scripts.View.UIDetail;
using Assets.Scripts.Define;

namespace Assets.Scripts.View.Npc
{
    public class NpcDiaologView : TaskDialogueUIDetail
    {
		private const string OTHERATLAS = "OtherAtlas";
		private const string EQUIPATLAS = "EquipAtlas";
		private const string DEFAULTATLAS = "IconAtlas";
		private const int maxRewardNum = 5;
        private NpcPanelInfo pannelVO;
		private bool bPreInited = false;
        private int dialogueStep = 0;
        private int missionIndex = 0;
        private List<UISprite> ItemSpriteList = new List<UISprite>();
        private List<UILabel> ItemNumList = new List<UILabel>();
		private List<UISprite> ItemFrameSpriteList = new List<UISprite>();

        public NpcDiaologView()
            : base(485, 400)
        {

        }

        protected override void PreInit()
        {
            base.PreInit();
			bPreInited = true;
        }

        protected override void Init()
        {
            UIEventListener.Get(CloseButton.gameObject).onClick += OnClickCloseBtnHandler;
            UIEventListener.Get(AcceptButton.gameObject).onClick += OnClickAcceptBtnHandler;
            UIEventListener.Get(CompleteButton.gameObject).onClick += OnClickCompleteBtnHandler;
            UIEventListener.Get(Panel.gameObject).onClick += OnPanelHandler;
            UIEventListener.Get(ContinueButton.gameObject).onClick += OnClickNextBtnHandler;
            // UIEventListener.Get(content.gameObject).onURL += OnLinkHandler;
			
			//UI上固定有5个物品框
			ItemSpriteList.Add(ItemSprite_1);
			ItemSpriteList.Add(ItemSprite_2);
			ItemSpriteList.Add(ItemSprite_3);
			ItemSpriteList.Add(ItemSprite_4);
			ItemSpriteList.Add(ItemSprite_5);

            ItemNumList.Add(ItemNumber_1);
            ItemNumList.Add(ItemNumber_2);
            ItemNumList.Add(ItemNumber_3);
            ItemNumList.Add(ItemNumber_4);
            ItemNumList.Add(ItemNumber_5);

			ItemFrameSpriteList.Add(ItemFrameSprite_1);
			ItemFrameSpriteList.Add(ItemFrameSprite_2);
			ItemFrameSpriteList.Add(ItemFrameSprite_3);
			ItemFrameSpriteList.Add(ItemFrameSprite_4);
			ItemFrameSpriteList.Add(ItemFrameSprite_5);
				
            Clear();
            RenderInfo();
        }

        public NpcPanelInfo PannelVO
        {
            get
            {
                return pannelVO;
            }
            set
            {
                pannelVO = value;
                //处理头像
            }
        }

        public void Open()
        {
            Clear();
            RenderInfo();
            Show(true);
        }

        public void Close()
        {
            Clear();
            Hide();
        }

        private void RenderInfo()
        {
			SetAllRewardItemHide();
			
            if (bPreInited && PannelVO != null)
            {
                DialogueContentLabel.text = "      " + PannelVO.content;
                NpcNameLabel.text = PannelVO.npcName;
                string contentStr = "";

                for (int i = 0; i < pannelVO.missionLinks.Count; i++)
                {
                    string str = pannelVO.missionLinks[i].linkName;
                    int start = str.IndexOf("<a:");
                    if (start == -1)
                        return;
                    int end = str.IndexOf(">", start);
                    if (end == -1)
                        return;
                    str = str.Substring(start + 3, end - start - 3);

                    missionIndex = int.Parse(str.Replace(ControllerCommand.NPC_CLICK_MISSION_LINK, ""));
                    NpcLinkInfo linkVO = PannelVO.missionLinks[missionIndex];
                    MissionInfo missionVO = linkVO.data as MissionInfo;
                    if (missionVO.curStatus == MissionInfo.MisssionStatus.Finish)
                    {
                        contentStr += pannelVO.missionLinks[i].linkName;
                        break;
                    }

                    //contentStr += pannelVO.missionLinks[i].linkName + "\n\n";
                }

                if (contentStr == "")
                {
                    for (int i = 0; i < pannelVO.missionLinks.Count; i++)
                    {
                        string str = pannelVO.missionLinks[i].linkName;
                        int start = str.IndexOf("<a:");
                        if (start == -1)
                            return;
                        int end = str.IndexOf(">", start);
                        if (end == -1)
                            return;
                        str = str.Substring(start + 3, end - start - 3);

                        missionIndex = int.Parse(str.Replace(ControllerCommand.NPC_CLICK_MISSION_LINK, ""));
                        NpcLinkInfo linkVO = PannelVO.missionLinks[missionIndex];
                        MissionInfo missionVO = linkVO.data as MissionInfo;
                        if (missionVO.curStatus == MissionInfo.MisssionStatus.Accept)
                        {
                            contentStr += pannelVO.missionLinks[i].linkName;
                            break;
                        }
                    }
                }

                if (contentStr != "")
                {
                    AwardText.text = contentStr;
                    AwardText.gameObject.SetActive(true);
                    Highlighting.gameObject.SetActive(true);
                    TweenAlpha a = TweenAlpha.Begin(Highlighting.gameObject, 0.5f, 0.2f);
                    a.style = UITweener.Style.PingPong;
                }
            }
        }

        private void Clear()
        {
            if (bPreInited)
            {
                AwardText.text = "";
                AwardText.gameObject.SetActive(false);
                AcceptButton.gameObject.SetActive(false);
                CompleteButton.gameObject.SetActive(false);
                ContinueButton.gameObject.SetActive(false);
                AwardTitle.gameObject.SetActive(false);
                Highlighting.gameObject.SetActive(false);
                Normal.gameObject.SetActive(false);
            }
        }

        private void OnLinkHandler(GameObject go, string eventName)
        {
            Clear();
            NpcLinkInfo linkVO = PannelVO.missionLinks[missionIndex];
            MissionInfo missionVO = linkVO.data as MissionInfo;
            KMissionDialogue dialogue = null;
            if (missionVO.curStatus == MissionInfo.MisssionStatus.Accept)
            {
                dialogue = KConfigFileManager.GetInstance().GetMissionDialogue(missionVO.dialogue1[0]);
                if (missionVO.dialogue1.Length == 1)
                {
                    AcceptButton.gameObject.SetActive(true);
                    ShowRewardItem(missionVO);
                }
                else
                {
                    dialogueStep = 1;
                    ContinueButton.gameObject.SetActive(true);
                }
            }
            else if (missionVO.curStatus == MissionInfo.MisssionStatus.BeenAccepted)
            {
                dialogue = KConfigFileManager.GetInstance().GetMissionDialogue(missionVO.dialogue2);
            }
            else if (missionVO.curStatus == MissionInfo.MisssionStatus.Finish)
            {
                dialogue = KConfigFileManager.GetInstance().GetMissionDialogue(missionVO.dialogue3);
                CompleteButton.gameObject.SetActive(true);
                ShowRewardItem(missionVO);
            }
            if(dialogue != null)
                DialogueContentLabel.text = "      " + dialogue.Content;
			
        }

        private void OnClickAcceptBtnHandler(GameObject go)
        {
            NpcLinkInfo linkVO = PannelVO.missionLinks[missionIndex];
            MissionInfo missionVO = linkVO.data as MissionInfo;
            MissionLogic.GetInstance().SendAcceptQuestMsg(missionVO.id);
            missionIndex = 0;
            Hide();
        }

        private void OnClickCompleteBtnHandler(GameObject go)
        {
            NpcLinkInfo linkVO = PannelVO.missionLinks[missionIndex];
            MissionInfo missionVO = linkVO.data as MissionInfo;
            MissionLogic.GetInstance().SendFinishQuestMsg(missionVO.id);
            missionIndex = 0;
            Hide();
        }

        private void OnPanelHandler(GameObject go)
        {
            if (CompleteButton.gameObject.activeSelf)
            {
                this.OnClickCompleteBtnHandler(go);
            }
            else if (AcceptButton.gameObject.activeSelf)
            {
                this.OnClickAcceptBtnHandler(go);
            }
            else if (AwardText.gameObject.activeSelf)
            {
                this.OnLinkHandler(go, "");
            }
        }

        private void OnClickNextBtnHandler(GameObject go)
        {
            NpcLinkInfo linkVO = PannelVO.missionLinks[missionIndex];
            MissionInfo missionVO = linkVO.data as MissionInfo;
            if(dialogueStep < missionVO.dialogue1.Length)
            {
                KMissionDialogue dialogue = KConfigFileManager.GetInstance().GetMissionDialogue(missionVO.dialogue1[dialogueStep]);
                DialogueContentLabel.text = "      " + dialogue.Content;
                dialogueStep++;
            }
            else if (dialogueStep == missionVO.dialogue1.Length)
            {
                KMissionDialogue dialogue = KConfigFileManager.GetInstance().GetMissionDialogue(missionVO.dialogue1[dialogueStep]);
                DialogueContentLabel.text = "      " + dialogue.Content;
                ContinueButton.gameObject.SetActive(false);
                AcceptButton.gameObject.SetActive(true);
                ShowRewardItem(missionVO);
                dialogueStep = 0;
            }
        }

        private void OnClickCloseBtnHandler(GameObject go)
        {
            Hide();
        }

        private void ShowRewardItem(MissionInfo missionVO)
        {
            AwardTitle.gameObject.SetActive(true);

			int	index = 0;

            if (missionVO.money > 0)
            {
                SetItemData(index + 1, (int)KItemTableType.ittOther, "money", missionVO.money);
                index++;
            }

            if (missionVO.exp > 0)
            {
                SetItemData(index + 1, (int)KItemTableType.ittOther, "exp", missionVO.exp);
                index++;
            }

            if (missionVO.rewardTypes != null && missionVO.rewardTypes.Length != 0)
            {
                for (; index < missionVO.rewardTypes.Length; ++index)
                {
                    SetItemData(index + 1, missionVO.rewardTypes[index], missionVO.rewardItemIDs[index].ToString(), missionVO.rewardItemNums[index]);
                }
            }
			
			for (int i = index; i < maxRewardNum; ++i)
			{
				SetRewardItemHide(i);
			}
        }
		
		private void SetItemData(int itemPos, int itemType, string Icon, int itemNum)
		{
			string atlasName = null;
            string spriteName = Icon;
			switch (itemType)
			{
			case (int)KItemTableType.ittEquip:
				atlasName = EQUIPATLAS;
                KTabLineEquip equipData = KConfigFileManager.GetInstance().equipTabInfos.getData(Icon);
                if (equipData != null)
                {
                    spriteName = equipData.Icon;;
                }
				break;				
			case (int)KItemTableType.ittOther:
				atlasName = OTHERATLAS;
				break;
			default:
                atlasName = DEFAULTATLAS;
				break;
			}
			
			ItemFrameSpriteList[itemPos - 1].gameObject.SetActive(true);
			ItemSpriteList[itemPos - 1].depth = ItemFrameSpriteList[itemPos - 1].depth + 100;
            ItemSpriteList[itemPos - 1].atlas = UIAtlasManager.GetInstance().GetUIAtlas(atlasName);
            ItemSpriteList[itemPos - 1].spriteName = spriteName;

            ItemNumList[itemPos - 1].depth = ItemFrameSpriteList[itemPos - 1].depth + 105;

            if (itemNum > 9999)
            {
                float fNum = itemNum / 10000.0f;
                ItemNumList[itemPos - 1].text = fNum.ToString() + "万";
            }
			else
            {
				ItemNumList[itemPos - 1].text = itemNum.ToString();
            }
		}
			
		private void SetRewardItemHide(int index)
		{
			if (index < 0 || index >= maxRewardNum || ItemSpriteList.Count == 0)//UI上固定有5个物品框
				return;
			
			ItemFrameSpriteList[index].gameObject.SetActive(false);
		}
		
		private void SetAllRewardItemHide()
		{
			for (int i = 0; i < maxRewardNum; ++i)
			{
				SetRewardItemHide(i);
			}
		}

        public override void FixedUpdate()
        {
        }
    }
}
