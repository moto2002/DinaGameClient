using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Lib.View;
using Assets.Scripts.Controller;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.Utils;
using Assets.Scripts.Manager;
using Assets.Scripts.View.Scene;
using Assets.Scripts.Define;
using Assets.Scripts.Logic.Scene;
using System;
using Assets.Scripts.View.UIDetail;
using Assets.Scripts.View.Gift;
using Assets.Scripts.View.MainMenu;
using Assets.Scripts.Logic.Scene.SceneObject;

namespace Assets.Scripts.View.Scene.MinMap
{
    public class MinMapView : NewSmallMapUIDetail
    {
        enum POINTER
        {
            NPC,
            TEAMMATE,
            ENEMY,
        }
        private Renderer minMapRender;
        private Transform mainHeroPointer;
		private UISprite mainHeroSprite;
        private GameObject blissButtonEffect;
        public MinMapInfor infor = null;
        public float proportion = 60f;//屏幕显示范围
        public float panelSize = 10f;
        private float mapSize;
        private float tiling;
		
		
		public UIDrawCall enemyDC = null;
		public UIDrawCall npcDC = null;
		public UIDrawCall teammateDC = null;
		
        private List<UIDrawCall> enemy_ary = new List<UIDrawCall>();
        private List<UIDrawCall> teammate_ary = new List<UIDrawCall>();
        private List<UIDrawCall> npc_ary = new List<UIDrawCall>();

        private List<UIDrawCall> act_enemy_ary = new List<UIDrawCall>();
        private List<UIDrawCall> act_teammate_ary = new List<UIDrawCall>();
        private List<UIDrawCall> act_npc_ary = new List<UIDrawCall>();
		
        public MinMapView()
            : base(10, 10)
        {

        }

        private static MinMapView instance;
        public static MinMapView GetInstance()
        {
            if (instance == null)
                instance = new MinMapView();
            return instance;
        }
		
		public UIDrawCall GetDrawCall(UISprite uisprite)
		{
			GameObject obj = new GameObject();
			obj.transform.parent = minMapRender.transform;
			obj.transform.localPosition = Vector3.zero;
			obj.transform.localScale = new Vector3(0.1f,0.1f,0.1f);
			UIDrawCall dc = obj.AddComponent<UIDrawCall>();
			obj.layer = LayerMask.NameToLayer("2D");
			BetterList<Vector3> verts = new BetterList<Vector3>();
			BetterList<Vector2> uvs = new BetterList<Vector2>();
			BetterList<Color32> cols = new BetterList<Color32>();
			dc.material = uisprite.material;
			uisprite.UpdateUVs(true);
			uisprite.OnFill(verts,uvs,cols);
			dc.Set(verts,null,null,uvs,cols);
			dc.renderQueue = mainHeroSprite.renderQueue;
			return dc;
		}
        protected override void PreInit()
        {
            base.PreInit();

            minMapRender = FindUIObject<Renderer>("minmap");
            mainHeroPointer = FindUIObject<Transform>("mainHero");
            blissButtonEffect = GameObject.Find("effect_ui_NewSmallMapUI_yuanquan");
			
			mainHeroSprite = mainHeroPointer.GetComponentInChildren<UISprite>();
			
			enemyDC = GetDrawCall(enemy);
			npcDC = GetDrawCall(npc);
			teammateDC = GetDrawCall(teammate);
			
			enemyDC.gameObject.SetActive(false);
			npcDC.gameObject.SetActive(false);
			teammateDC.gameObject.SetActive(false);
			
            enemy.gameObject.SetActive(false);
            npc.gameObject.SetActive(false);
            teammate.gameObject.SetActive(false);
            loadMap(SceneLogic.GetInstance().mapId);

            base.delayBad.gameObject.SetActive(false);
            base.delayNormal.gameObject.SetActive(false); 
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (blissButtonEffect != null)
            {
                bool isGiftHallAvailable = TopRightMenuView.GetInstance().IsGiftHallAvailable();
                
                if (isGiftHallAvailable)
                {
                    if (!blissButtonEffect.activeSelf)
                    {
                        blissButtonEffect.SetActive(true);
                    }
                }
                else
                {
                    if (blissButtonEffect.activeSelf)
                    {
                        blissButtonEffect.SetActive(false);
                    }
                }
            }
        }

        private void LoadImgComplete(AssetInfo info)
        {
            minMapRender.material.mainTexture = info.bundle.mainAsset as Texture2D;
        }

        public object OnLoadMap(params object[] objs)
        {
            uint mapid = Convert.ToUInt32(objs[0]);
            loadMap(mapid);
            return null;
        }

        public object OnUpdateMiniMap(params object[] objs)
        {
            if (null != SceneLogic.GetInstance().MainHero)
            {
                SetPosition(SceneLogic.GetInstance().MainHero.transform);
            }
            return null;
        }
        public void loadMap(uint mapid)
        {
            AssetLoader.GetInstance().Load(URLUtil.GetResourceLibPath() + "Scene/" + mapid + "/min.img", LoadImgComplete, AssetType.BUNDLER);
            infor = MinMapDataManager.instance.GetMinMap(mapid);
        }

        //      private List<UISprite> enemy_ary = new List<UISprite>();
        // 		private List<UISprite> teammate_ary = new List<UISprite>();
        // 		private List<UISprite> npc_ary = new List<UISprite>();

        void Clear()
        {
            foreach (UIDrawCall s in act_enemy_ary)
            {
                s.gameObject.SetActive(false);
                enemy_ary.Insert(0,s);
            }
            act_enemy_ary.Clear();

            foreach (UIDrawCall s in act_teammate_ary)
            {
                s.gameObject.SetActive(false);
                teammate_ary.Insert(0,s);
            }
            act_teammate_ary.Clear();

            foreach (UIDrawCall s in act_npc_ary)
            {
                s.gameObject.SetActive(false);
                npc_ary.Insert(0,s);
            }
            act_npc_ary.Clear();
        }
		
        void AddPointer(SceneEntity obj, POINTER type, Vector3 center)
        {
			Vector3 position = obj.Position;
            float deltaX = position.x - center.x;
            float deltaZ = position.z - center.z;
            float dis = Vector3.Distance(new Vector3(position.x, 0, position.z), new Vector3(center.x, 0, center.z));
            if (dis * 2.5f > proportion)
            {
				if (null != obj.ui.mark && obj.ui.mark.gameObject.activeSelf)
					obj.ui.mark.gameObject.SetActive(false);
                return;
            }
            
            if (type == POINTER.ENEMY)
            {
				if (null == obj.ui.mark)
				{
					obj.ui.mark = GetDrawCall(enemy);
					obj.ui.mark.material = enemyDC.material;
                    obj.ui.mark.transform.parent = enemy.transform.parent;
                    obj.ui.mark.transform.localRotation = enemy.transform.localRotation;
                    obj.ui.mark.transform.localPosition = enemy.transform.localPosition;
                    obj.ui.mark.transform.localScale = enemy.transform.localScale;
				}
                obj.ui.mark.gameObject.name = "enemy_pointer";
            }
            else if (type == POINTER.NPC)
            {
                if (null == obj.ui.mark)
				{
                    obj.ui.mark = GetDrawCall(npc);
					obj.ui.mark.material = npcDC.material;
                    obj.ui.mark.transform.parent = npc.transform.parent;
                    obj.ui.mark.transform.localRotation = npc.transform.localRotation;
                    obj.ui.mark.transform.localPosition = npc.transform.localPosition;
                    obj.ui.mark.transform.localScale = npc.transform.localScale;
                }
                obj.ui.mark.gameObject.name = "npc_pointer";
            }
            else if (type == POINTER.TEAMMATE)
            {
                if (null == obj.ui.mark)
                {
                    obj.ui.mark = GetDrawCall(enemy);
					obj.ui.mark.material = teammateDC.material;
                    obj.ui.mark.transform.parent = teammate.transform.parent;
                    obj.ui.mark.transform.localRotation = teammate.transform.localRotation;
                    obj.ui.mark.transform.localPosition = teammate.transform.localPosition;
                    obj.ui.mark.transform.localScale = teammate.transform.localScale;
                }
                obj.ui.mark.gameObject.name = "teamate_pointer";
            }


            obj.ui.mark.transform.localPosition = new Vector3(
                -panelSize * deltaX / proportion,
                obj.ui.mark.transform.localPosition.y,
                -panelSize * deltaZ / proportion);
            if (!obj.ui.mark.gameObject.activeSelf)
				obj.ui.mark.gameObject.SetActive(true);
        }

        public void SetPosition(Transform t)
        {
            if (null != minMapRender && null != infor)
            {
                mapSize = infor.maxX - infor.minX;
                tiling = proportion / mapSize;
                float offsetX = (t.position.x - infor.minX) / mapSize - tiling / 2;
                float offsetY = (t.position.z - infor.minZ) / mapSize - tiling / 2;
                minMapRender.material.mainTextureOffset = new Vector2(offsetX, offsetY);
                minMapRender.material.mainTextureScale = new Vector2(tiling, tiling);
                mainHeroPointer.localEulerAngles = new Vector3(0, t.rotation.eulerAngles.y, 0);

                uint[] ids = SceneLogic.GetInstance().GetSceneObjIds();
                Clear();
                foreach (uint _id in ids)
                {
                    SceneEntity obj = SceneLogic.GetInstance().GetSceneObject(_id);
                    if (obj.HeroType == KHeroObjectType.hotNpc)
                    {
                        AddPointer(obj, POINTER.NPC, t.position);
                    }
                    else if (obj.HeroType == KHeroObjectType.hotPlayer)
                    {
                        if (obj.property.isMainHero)
                            continue;
                        if (obj.property.isCanAttack)
                        {
                            AddPointer(obj, POINTER.ENEMY, t.position);
                        }
                        else
                        {
                            AddPointer(obj, POINTER.TEAMMATE, t.position);
                        }
                    }
                    else if (obj.HeroType == KHeroObjectType.hotMonster)
                    {
                        AddPointer(obj, POINTER.ENEMY, t.position);
                    }
                }
            }
        }

        protected override void Init()
        {
            //viewGo.transform.position = new Vector3(0f,0f,viewGo.transform.position.z);
            SetViewPosition(ViewPosition.TopRight);

            //uiPanel.transform.localPosition = new Vector3(0,0,0);
            uiPanel.transform.localPosition = new Vector3(-150, -95, 0);
            //uiPanel.transform.localScale = new Vector3(0.5f,0.5f,1f);
            EventDispatcher.GameWorld.Regist(ControllerCommand.LOAD_MINI_MAP, OnLoadMap);
            EventDispatcher.GameWorld.Regist(ControllerCommand.UPDATE_MINI_MAP, OnUpdateMiniMap);

            UIEventListener.Get(ImageButtonFu.gameObject).onClick += GiftHallView.GetInstance().Open;
            //UIEventListener.Get(FindGameObject("Input")).onSubmit += OnSubmitHandler;
        }
    }
}
