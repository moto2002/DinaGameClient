using System;
using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.View.Bag;
using Assets.Scripts.View.Mission;
using Assets.Scripts.Manager;
using Assets.Scripts.Controller;
using Assets.Scripts.View.MainUI;
using Assets.Scripts.View.Chat;
using Assets.Scripts.View.Scene.MinMap;
using Assets.Scripts.View.Scene;
using Assets.Scripts.View.Npc;
using Assets.Scripts.Logic.Scene.SceneObject;
using Assets.Scripts.Logic.Scene;
using Assets.Scripts.Logic.Npc;
using Assets.Scripts.Logic;
using Assets.Scripts.Model.Player;
using Assets.Scripts.View;
using Assets.Scripts.View.MainMenu;
using Assets.Scripts.View.Gift;
using Assets.Scripts.View.Pve;
using Assets.Scripts.View.Other;
using Assets.Scripts.View.Info;
using Assets.Scripts.Logic.Item;
using Assets.Scripts.View.ButtonBar;
using Assets.Scripts.Logic.Intensify;
using Assets.Scripts.Logic.Mission;
using Assets.Scripts.Lib.View;

namespace Assets.Scripts.View
{
    public class ViewManager
    {
        private BagView bagView;
        private NpcDiaologView npcView;
        private BagItemClickView bagItemClickView;
        private BagItemPartView bagItemPartView;
        private BagItemSaleView bagItemSaleView;
        private BagItemUseView bagItemUseView;
        private CollectView collectView;
        private PlotMissionView plotView;
		private ChangeEquipPromptView changeEquipPromptView;

        private static ViewManager instance;
        public static ViewManager GetInstance()
        {
            if (instance == null)
                instance = new ViewManager();
            return instance;
        }

        public ViewManager()
        {
            InitListeners();
        }

        private void InitListeners()
        {
            EventDispatcher.GameWorld.Regist(ControllerCommand.OPEN_BAGITEMCLICK_PANEL, OnOpenBagItemClickView);
            EventDispatcher.GameWorld.Regist(ControllerCommand.CLOSE_BAGITEMCLICK_PANEL, OnCloseBagItemClickView);
            EventDispatcher.GameWorld.Regist(ControllerCommand.OPEN_BAGITEMPART_PANEL, OnOpenBagItemPartView);
            EventDispatcher.GameWorld.Regist(ControllerCommand.OPEN_BAGITEMSALE_PANEL, OnOpenBagItemSaleView);
            EventDispatcher.GameWorld.Regist(ControllerCommand.OPEN_BAGITEMUSE_PANEL, OnOpenBagItemUseView);
            EventDispatcher.GameWorld.Regist(ControllerCommand.OPEN_BAG_PANEL, OnOpenBagView);
            EventDispatcher.GameWorld.Regist(ControllerCommand.INIT_SCENE_UI, OnInitSceneUI);
            EventDispatcher.GameWorld.Regist(ControllerCommand.INIT_SCENE_VIEW, OnInitSceneView);
            EventDispatcher.GameWorld.Regist(ControllerCommand.OPEN_NPC_PANEL, OnOpenNpcPanel);
			EventDispatcher.GameWorld.Regist(ControllerCommand.OPEN_LOADING_PANEL, OnOpenLoadingPanel);
			EventDispatcher.GameWorld.Regist(ControllerCommand.CLOSE_LOADING_PANEL, OnCloseLoadingPanel);
            EventDispatcher.GameWorld.Regist(ControllerCommand.OPEN_NPC_PANEL_BYID, OnOpenNpcPanelByID);
            EventDispatcher.GameWorld.Regist(ControllerCommand.OPEN_COLLECT_PANEL, OnOpenCollectPanelByID);
            EventDispatcher.GameWorld.Regist(ControllerCommand.OPEN_PLOT_PANEL, OnOpenPlotPanelByID);
            EventDispatcher.GameWorld.Regist(ControllerCommand.PLAYER_LEVEL_UP, OnPlayerLevelUp);
			EventDispatcher.GameWorld.Regist(ControllerCommand.OPEN_CHANGEEQUIP_PANEL, OnOpenChangeEquipPromptView);
            EventDispatcher.GameWorld.Regist(ControllerCommand.OPEN_FORGE_PANEL, OnOpenIntensiftView);
        }

        public object OnInitSceneView(params object[] objs)
        {
            SceneView.GetInstance();
            return null;
        }

        public object OnOpenIntensiftView(params object[] objs)
        {
            IntensifyLogic.GetInstance().InitIntensifyUI();
            return null;
        }

        public object OnInitSceneUI(params object[] objs)
        {
            //--------- menu begin--------------------------
            ButtonBarView.GetInstance().viewType = ViewType.Menu; //ShortcutView.GetInstance(); //old view

            CMDView.GetInstance().viewType = ViewType.Menu;
            HitPanelView.GetInstance().viewType = ViewType.Menu;
            HeroMenuView.GetInstance().viewType = ViewType.Menu;   //HeadView.GetInstance();//old view
            MinMapView.GetInstance().viewType = ViewType.Menu;

            if (PlayerManager.GetInstance().MajorPlayer.level >= 14)
                MissionFollowListView.GetInstance().viewType = ViewType.Menu;
            else
                MissionFollowView.GetInstance().viewType = ViewType.Menu;

            TopRightMenuView.GetInstance().viewType = ViewType.Menu;
            PveProcessView.GetInstance().viewType = ViewType.Menu;
            //-----------menu end--------------------

            //-----------window begin-----------------
            PveView.GetInstance();
            PveQuitView.GetInstance();
            PveAutoFight.GetInstance();
            //-----------window end-------------------

            //----------Tip begin-----------
            PveFailView.GetInstance();
            ReliveView.GetInstance();
            //----------Tip end-------------


			return null;
        }

        public Transform GetMissionViewEffectPos()
        {
            if (PlayerManager.GetInstance().MajorPlayer.level >= 14)
            {
                return MissionFollowListView.GetInstance().GetEffectPos();
            }
            else
            {
                return MissionFollowView.GetInstance().GetEffectPos();
            }
        }

        public object OnOpenBagView(params object[] objs)
        {
            if (bagView == null)
                bagView = new BagView();
            else
                bagView.Show();
            return null;
        }

        public object OnOpenBagItemClickView(params object[] objs)
        {
            if (bagItemClickView == null)
            {
                bagItemClickView = BagItemClickView.GetInstance();
            }
            bagItemClickView.ChangeItemClickView(Int32.Parse(objs[0].ToString()), objs[1]);
            return null;
        }

        public object OnCloseBagItemClickView(params object[] objs)
        {
            if (bagItemClickView != null)
                bagItemClickView.Hide();
            return null;
        }

        public object OnOpenBagItemPartView(params object[] objs)
        {
            if (bagItemPartView == null)
            {
                bagItemPartView = BagItemPartView.GetInstance();
            }
            bagItemPartView.OpenView(Int32.Parse(objs[0].ToString()));
            return null;
        }

        public object OnOpenBagItemSaleView(params object[] objs)
        {
            if (bagItemSaleView == null)
            {
                bagItemSaleView = BagItemSaleView.GetInstance();
            }
            bagItemSaleView.OpenView(Int32.Parse(objs[0].ToString()), Int32.Parse(objs[1].ToString()));
            return null;
        }

        public object OnOpenBagItemUseView(params object[] objs)
        {
            if (bagItemUseView == null)
            {
                bagItemUseView = BagItemUseView.GetInstance();
            }
            bagItemUseView.OpenView(Int32.Parse(objs[0].ToString()));
            return null;
        }
		
	
		public object OnCloseLoadingPanel(params object[] objs)
        {
            LoadingView.GetInstance().Hide();
			return null;
		}

		public object OnOpenLoadingPanel(params object[] objs)
        {
            LoadingView.GetInstance().Show();
            return null;
        }
		
        public object OnOpenNpcPanel(params object[] objs)
        {
            uint npcSceneId = Convert.ToUInt32(objs[0]);
            SceneEntity npc = SceneLogic.GetInstance().GetSceneObject(npcSceneId);
            if (npcView == null)
            {
                npcView = new NpcDiaologView();
            }
            npcView.PannelVO = NpcLogic.GetInstance().GetNPCPanelInfo((int)npc.TabID);
            npcView.Open();
            return null;
        }

        public object OnOpenNpcPanelByID(params object[] objs)
        {
            int npcID = Convert.ToInt32(objs[0]);
            if (npcView == null)
            {
                npcView = new NpcDiaologView();
            }
            npcView.PannelVO = NpcLogic.GetInstance().GetNPCPanelInfo(npcID);
            npcView.Open();
            return null;
        }

        public object OnOpenCollectPanelByID(params object[] objs)
        {
            if (collectView == null)
            {
                collectView = new CollectView();
            }

            if (!collectView.isOpen())
            {
                collectView.Open(Convert.ToInt32(objs[0]), Convert.ToInt32(objs[1]));
            }
            return null;
        }

        public void InterruptCollectObj()
        {
            if (collectView != null && collectView.isOpen())
            {
                CollectObjLogic.GetInstance().OnInterruptCollect(null);
                collectView.Close();
            }
        }

        public void CloseCollectPanel()
        {
            if (collectView != null && collectView.isOpen())
            {
                collectView.Close();
            }
        }

        public object OnOpenPlotPanelByID(params object[] objs)
        {
            if (plotView == null)
            {
                plotView = new PlotMissionView();
            }

            if (!plotView.isOpen())
            {
                plotView.Open(Convert.ToInt32(objs[0]));
            }
            return null;
        }

        public void AutoCloseNpcPanel()
        {
            if (npcView != null && npcView.isOpen())
            {
                if (!NpcLogic.GetInstance().CheckNpcNearby(npcView.PannelVO.npcID, 5))
                    npcView.Close();
            }
        }

        public void CloseNpcPanel()
        {
            if (npcView != null && npcView.isOpen())
            {
                npcView.Close();
            }
        }

        private object OnPlayerLevelUp(params object[] objs)
        {
            byte level = (byte)objs[0];
            MajorPlayer player = PlayerManager.GetInstance().MajorPlayer;
            player.levelCurrent = level;
         
            if (level >= 14)
            {
                MissionFollowView.GetInstance().DestroyObject();
                MissionFollowListView.GetInstance();
            }
            GiftHallView.GetInstance().UpdateUIOnDataChanged();
            return null;
        }
		
		private object OnOpenChangeEquipPromptView(params object[] objs)
		{
			EquipInfo itemInfo = (EquipInfo)objs[0];
			if (changeEquipPromptView == null)
			{
				changeEquipPromptView = new ChangeEquipPromptView();
			}
			changeEquipPromptView.SetEquipInfo(itemInfo);
			changeEquipPromptView.Show();
			return null;
		}	
    }
}
