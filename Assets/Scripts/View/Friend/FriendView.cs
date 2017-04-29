using System;
using UnityEngine;
using Assets.Scripts.Lib.View;
using Assets.Scripts.Model.Player;
using Assets.Scripts.Define;
using Assets.Scripts.Utils;
using Assets.Scripts.Manager;
using Assets.Scripts.Logic.Friend;
using System.Collections;

namespace Assets.Scripts.View.Friend
{
    public class FriendView : ViewScript
    {
        private static FriendView instance;
        private UIAtlas headAtlas = null, friendAtlas = null;
        public UISprite bg = null;
        public UISprite down = null;
        public UILabel enemyLab = null;
        public UISprite enemySprite = null;
        public UILabel friendLab = null;
        public UISprite friendSprite = null;
        public UISprite[] image = null;
        public UISprite[] infoBg = null;
        public UILabel[] infoLab = null;
        public UIGrid grid = null;
        public UISprite imageSign = null;
        public UISprite inside = null;
        public UILabel levelLab = null;
        public UISprite myImage = null;
        public UISprite myTitleBg = null;
        public UILabel nameLab = null;
        public UISlider slider = null;
        public UISprite sortBg = null;
        public UILabel sortLab = null;
        public UISprite title = null;
        public UISprite up = null;
        public UIImageButton addBtn = null;
        public UIImageButton closeBtn = null;

        public static FriendView getInstance()
        {
            if (instance == null)
                instance = new FriendView();
            return instance;
        }

        public FriendView()
            : base("FriendListTestUI", 300, 500)
        {
        }

        protected override void PreInit()
        {
            headAtlas = UIAtlasManager.GetInstance().GetUIAtlas("HeadAtlas");
            friendAtlas = UIAtlasManager.GetInstance().GetUIAtlas("FriendAtlas");
            bg = FindUIObject<UISprite>("bg");
            enemyLab = FindUIObject<UILabel>("enemyLab");
            enemySprite = FindUIObject<UISprite>("enemySprite");
            friendLab = FindUIObject<UILabel>("friendLab");
            friendSprite = FindUIObject<UISprite>("friendSprite");
            grid = GameObject.Find("Panel").GetComponent<UIGrid>();
            imageSign = FindUIObject<UISprite>("imageSign");
            inside = FindUIObject<UISprite>("inside");
            levelLab = FindUIObject<UILabel>("levelLab");
            myImage = FindUIObject<UISprite>("myImage");
            myTitleBg = FindUIObject<UISprite>("myTitleBg");
            nameLab = FindUIObject<UILabel>("nameLab");
            slider = FindUIObject<UISlider>("slider");
            slider.value = 1;
            sortBg = FindUIObject<UISprite>("sortBg");
            sortLab = FindUIObject<UILabel>("sortLab");
            title = FindUIObject<UISprite>("title");
            //up = FindUIObject<UISprite>("up");
            addBtn = FindUIObject<UIImageButton>("addBtn");
            closeBtn = FindUIObject<UIImageButton>("closeBtn");

            SetMyInfo();
            createFriendLab();
        }

        protected override void Init()
        {
            base.Init();
        }
        

        //设置我自己相关信息
        private void SetMyInfo()
        {
            FriendLogic logic = FriendLogic.getInstance();
            ArrayList friendList = logic.GetFriendList();//好友集合
            ArrayList enemyList = logic.getEnemyList();//敌人集合
            //设置我image
            MajorPlayer majorPlayer = PlayerManager.GetInstance().MajorPlayer;
            imageSign.atlas = headAtlas;
            KGender gender = EnumUtils.GetEnumIns<KGender>(majorPlayer.Gender);
            if (gender == KGender.gFemale)
            {
                imageSign.spriteName = "女主";
            }
            else
            {
                imageSign.spriteName = "男主";
            }
            //show my nickname level
            nameLab.text = majorPlayer.PlayerName;
            levelLab.text = "等级：" + majorPlayer.level;
            //show my sort
            sortLab.text = "我的好友 [" + friendList.Count + "/" + logic.Friend_MaxNum + "]       你等级排名：100";
        }

        private void createFriendLab()
        {
            FriendLogic logic = FriendLogic.getInstance();
            FriendInfo info = null;
            ArrayList friendList = logic.GetFriendList();//好友集合
            image = new UISprite[friendList.Count];//好友头像
            infoLab = new UILabel[friendList.Count];//好友介绍
            infoBg = new UISprite[friendList.Count];//好友背景
            for (int i = 0; i < friendList.Count; i++)
            {

                info = friendList[i] as FriendInfo;
                image[i] = NGUITools.AddChild<UISprite>(GameObject.Find("friend"));
                image[i].transform.localPosition = new Vector3(-170.3591f, -35.93111f + (-55) * i, 0);
                image[i].transform.localScale = new Vector3(1, 1, 1);
                image[i].atlas = headAtlas;
                image[i].width = 46;
                image[i].height = 44;
                if (info.sex == KGender.gFemale)
                {
                    image[i].spriteName = "女主";
                }
                else
                {
                    image[i].spriteName = "男主";
                }
                image[i].depth = 10 + i;

                //先设置label
                infoLab[i] = NGUITools.AddChild<UILabel>(GameObject.Find("friend"));
                infoLab[i].transform.localPosition = new Vector3(-140f, -35.93111f + (-55) * i, 0);
                infoLab[i].font = FontManager.GetInstance().Font;
                infoLab[i].text = info.nickName + "   " + info.level + "级";
                infoLab[i].transform.localScale = new Vector3(1, 1, 1);
                infoLab[i].pivot = UIWidget.Pivot.BottomLeft;
                infoLab[i].depth =30 + i;
                infoLab[i].MakePixelPerfect();

                infoBg[i] = NGUITools.AddChild<UISprite>(GameObject.Find("friend"));
                infoBg[i].transform.localPosition = new Vector3(-140f, -35.93111f + (-55) * i, 0);
                infoBg[i].transform.localScale = new Vector3(1, 1, 1);
                infoBg[i].pivot = UIWidget.Pivot.BottomLeft;
                infoBg[i].atlas = friendAtlas;
                infoBg[i].spriteName = "friendbg";
                infoBg[i].width = infoLab[i].width;
                infoBg[i].height = infoLab[i].height;
                infoBg[i].depth = 20 + i;
              }
        }
    }
}
