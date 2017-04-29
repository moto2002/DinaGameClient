using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Lib.View;
using Assets.Scripts.Model.Scene;
using Assets.Scripts.View.Scene;
using Assets.Scripts.Utils;
using Assets.Scripts.Model.Player;
using Assets.Scripts.View.Drag;
using Assets.Scripts.Logic.Item;
using Assets.Scripts.Logic.Bag;
using Assets.Scripts.Manager;
using Assets.Scripts.Controller;
using Assets.Scripts.Logic.Scene;
using Assets.Scripts.View.UIDetail;
using Assets.Scripts.Define;


namespace Assets.Scripts.View.Role
{
    public class RoleView : RoleUIDetail
    {
        public static Vector3 TURNerlurAngle = new Vector3(0, 45, 0);
        private RoleDetailView detailView = null;
        private GameObject role;
        private bool hasLight = false;
        private PointBtn pointBtn1 = null;
        private PointBtn pointBtn2 = null;
        private PointBtn pointBtn3 = null;
        private PointBtn pointBtn4 = null;
        private UIToggleButtom autoBtn = null;

        private const int LEFT_X = 37;
        private const int RIGHT_X = 87;
        private const int PADDING_TOP = 80;
        private const int V_PADDING = 13;
        private EquipItem[] equipItems = new EquipItem[16];

        public RoleView()
            : base(598, 483)
        {
            //generator = CharacterGenerator.Create();
        }

        protected override void PreInit()
        {
            base.PreInit();

            pointBtn1 = FindGameObject("pointBtn1").AddComponent<PointBtn>();
            pointBtn2 = FindGameObject("pointBtn2").AddComponent<PointBtn>();
            pointBtn3 = FindGameObject("pointBtn3").AddComponent<PointBtn>();
            pointBtn4 = FindGameObject("pointBtn4").AddComponent<PointBtn>();

            UpdateAvatar(null);
            UpdateAttribute(null);

            CreateEquipItems();
            SetAllEquips(BagLogic.GetInstance().equipItems);
        }

        private void CreateEquipItems()
        {
            for (int i = 0; i < 6; i++)
            {
                CreateItem(i, LEFT_X, PADDING_TOP - (i - 1) * (40 + V_PADDING));
            }
            for (int i = 6; i < 12; i++)
            {
                CreateItem(i, RIGHT_X, PADDING_TOP - (i - 6 - 1) * (40 + V_PADDING));
            }
            CreateItem(12, -206, -181);
            CreateItem(13, -150, -181);
            CreateItem(14, -96, -181);
            CreateItem(15, -43, -181);
        }

        private void CreateItem(int putWhere, float x, float y)
        {
            EquipItem item = NGUITools.AddChild<EquipItem>(uiPanel.gameObject);
            item.PutWhere = putWhere;
            NGUITools.AddWidgetCollider(item.gameObject);
            item.transform.localPosition = new Vector3(x, y, 0);
            UIEventListener.Get(item.gameObject).onDoubleClick += OnDoubleClickItem;
            equipItems[putWhere] = item;
            ObjectUtil.SetLayerWithAllChildren(item.gameObject, "2D");
        }

        protected override void Init()
        {
            base.Init();

            UIEventListener.Get(CloseBtn.gameObject).onClick += OnCloseHandler;
            UIEventListener.Get(detailBtn.gameObject).onClick += OnDetailHandler;
            UIEventListener.Get(turnLeftBtn.gameObject).onClick += OnTurnLeftHandler;
            UIEventListener.Get(turnRightBtn.gameObject).onClick += OnTurnRightHandler;

            EventDispatcher.GameWorld.Regist(ControllerCommand.UPDATE_ROLE_EQUIP, OnUpdateEquipHandler);
        }

        public void UpdateEquip(int putWhere, ItemInfo item)
        {
            equipItems[putWhere].UpdateContent(item);
        }

        public void SetAllEquips(ItemInfo[] items)
        {
            int len = equipItems.Length;
            for (int i = 0; i < len; i++)
            {
                equipItems[i].UpdateContent(items[i]);
            }
        }

        protected override void InitEvent()
        {
            base.InitEvent();
            EventDispatcher.GameWorld.Regist(ControllerCommand.UPDATE_ROLE_AVATAR, UpdateAvatar);
            EventDispatcher.GameWorld.Regist(ControllerCommand.UPDATE_ROLE_ATTRIBUTE, UpdateAttribute);
            EventDispatcher.GameWorld.Regist(ControllerCommand.PLAYER_LEVEL_UP, UpdateAttribute);
        }

        public override void Show(bool isForce = false)
        {
            base.Show(isForce);
            if (isOpen() && role != null)
            {
                CrossFadeAnimation();
                UpdateAttribute(null);
            }
        }

        public bool AutoPoint
        {
            get { return autoBtn.Selected; }
        }

        private void OnCloseHandler(GameObject go)
        {
            Hide();

        }

        private void OnDoubleClickItem(GameObject go)
        {
            DragItem item = go.GetComponent<DragItem>();
            EquipInfo equipVO = item.DragItemVO as EquipInfo;
            if (equipVO != null)
            {
                int target = BagLogic.GetInstance().GetNullPosition();
                if (target == -1)
                {
                    Debug.Log("背包已满");
                }
                else
                {
                    BagLogic.GetInstance().UnLoadEquip(equipVO.Position, target);
                }
            }
        }

        public override void Hide()
        {
            base.Hide();
            if (detailView != null && detailView.isOpen())
            {
                detailView.Hide();
            }
        }


        private void OnDetailHandler(GameObject go)
        {
            if (detailView == null)
            {
                detailView = new RoleDetailView();
                detailView.Panel = uiPanel;
            }
            else
            {
                detailView.Show();
            }
        }

        private void OnTurnLeftHandler(GameObject go)
        {
            if (role != null)
            {
                role.transform.localEulerAngles += TURNerlurAngle;
            }
        }

        private void OnTurnRightHandler(GameObject go)
        {
            if (role != null)
            {
                role.transform.localEulerAngles -= TURNerlurAngle;
                //SceneLogic.GetInstance().MainHero.BodyGo.transform.localScale += new Vector3(0.2f, 0.0f, 0.0f);
            }
        }

        private object OnUpdateEquipHandler(params object[] objs)
        {
            int putWhere = Convert.ToInt32(objs[0]);
            ItemInfo itemVO = BagLogic.GetInstance().GetEquipByPos(putWhere);
            equipItems[putWhere].UpdateContent(itemVO);
            return null;
        }

        private object UpdateAttribute(params object[] objs)
        {
            MajorPlayer player = PlayerManager.GetInstance().MajorPlayer;
            PlayerHeroData hero = player.HeroData as PlayerHeroData;
            if (hero == null)
            {
                return null;
            }
            nicknameTxt.text = player.PlayerName;
            careerTxt.text = GetJobName(player.uJob);
            spouseTxt.text = player.spouseName; ;
            levelTxt.text = player.level.ToString();
            factionTxt.text = player.factionName;
            hp1Txt.text = SceneLogic.GetInstance().MainHero.Hp + "/" + hero[KAttributeType.atMaxHP];
            mp1Txt.text = SceneLogic.GetInstance().MainHero.Mp + "/" + hero[KAttributeType.atMaxMP];

            attack1Txt.text = hero[KAttributeType.atAttack].ToString();
            defend1Txt.text = hero[KAttributeType.atDefence].ToString();
            crite1Txt.text = hero[KAttributeType.atCritHurt].ToString();
            attackSpeed1Txt.text = hero[KAttributeType.atAttackSpeed].ToString();

            hpSp.fillAmount = SceneLogic.GetInstance().MainHero.Hp / Math.Max(hero[KAttributeType.atMaxHP], 1);
            mpSp.fillAmount = SceneLogic.GetInstance().MainHero.Mp / Math.Max(hero[KAttributeType.atMaxMP], 1);

            if (detailView != null)
                detailView.UpdateAttribute();
            return null;
        }

        private object UpdateAvatar(params object[] objs)
        {
            if (role != null)
            {
                GameObject.DestroyImmediate(role);
            }
            if (SceneLogic.GetInstance().MainHero.BodyGo == null)
            {
                return null;
            }
            role = GameObject.Instantiate(SceneLogic.GetInstance().MainHero.BodyGo) as GameObject;
            role.name = "avatar";
            role.transform.parent = uiPanel.transform;
            role.transform.localPosition = new Vector3(-120, -120, -80);
            role.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            role.transform.localEulerAngles = new Vector3(0, 180, 0);
            ObjectUtil.SetLayerWithAllChildren(role, "2D");
            role.animation.Play("idle1");

            //AddLightForAvatar(role);
            return null;
        }

        private void CrossFadeAnimation()
        {
            Animation ani = role.GetComponent<Animation>();
            if (ani != null)
            {
                ani.Play("idle1");
            }
        }



        private void AddLightForAvatar(GameObject role)
        {
            if (hasLight)
            {
                return;
            }
            GameObject lightObj = new GameObject("TheLight");
            lightObj.transform.parent = role.transform;
            lightObj.layer = LayerMask.NameToLayer("2D");
            Light light = lightObj.AddComponent<Light>();
            light.type = LightType.Spot;
            light.range = 9.7f;
            light.spotAngle = 105;
            light.color = Color.white;
            light.intensity = 0.7f;
            light.shadows = LightShadows.None;
            light.renderMode = LightRenderMode.Auto;
            light.cullingMask = CameraLayerManager.GetInstance().Get2DTag();
            light.transform.localPosition = new Vector3(-83.46f, 232.51f, 45.33f);
            light.transform.localEulerAngles = new Vector3(41.43f, 125f, 331.77f);
            light.transform.localScale = Vector3.one;
            //light.transform.parent = role.transform;

            lightObj = new GameObject("TheLight");
            lightObj.transform.parent = role.transform;
            lightObj.layer = LayerMask.NameToLayer("2D");
            light = lightObj.AddComponent<Light>();
            light.type = LightType.Spot;
            light.range = 24.37f;
            light.spotAngle = 41;
            light.color = new Color(253f / 255, 255f / 255, 255f / 255, 255f / 255);
            light.intensity = 0.7f;
            light.shadows = LightShadows.None;
            light.renderMode = LightRenderMode.Auto;
            light.cullingMask = CameraLayerManager.GetInstance().Get2DTag();
            light.transform.localPosition = new Vector3(119f, 15.141f, 102.04f);
            light.transform.localEulerAngles = new Vector3(340.69f, 228.50f, 22.41f);
            light.transform.localScale = Vector3.one;
            //light.transform.parent = role.transform;

            lightObj = new GameObject("TheLight");
            lightObj.transform.parent = role.transform;
            lightObj.layer = LayerMask.NameToLayer("2D");
            light = lightObj.AddComponent<Light>();
            light.type = LightType.Point;
            light.range = 7.7f;
            light.color = new Color(14f / 255, 195f / 255, 231f / 255, 255f / 255);
            light.intensity = 0.7f;
            light.shadows = LightShadows.None;
            light.renderMode = LightRenderMode.Auto;
            light.cullingMask = CameraLayerManager.GetInstance().Get2DTag();
            light.transform.localPosition = new Vector3(10f, 68.45f, -91.37f);
            light.transform.localEulerAngles = new Vector3(9.61f, 186.16f, 330.63f);
            light.transform.localScale = Vector3.one;
            //light.transform.parent = role.transform;
            hasLight = true;
        }

        public string GetJobName(ushort job)
        {
            if (job == 1)
            {
                return "霸刀";
            }
            else
            {
                return "法师";
            }
        }
    }

    public enum ChangePointType { AddAll, Add, Del, DelAll }
    public class PointBtn : MonoBehaviour
    {
        public delegate void ChangePoint(PointBtn go, ChangePointType type);
        public event ChangePoint OnChange;

        public int value;


        void Awake()
        {
            UIEventListener.Get(NGUIUtils.FindGameObject(gameObject, "addAllBtn")).onClick += OnClickAddAll;
            UIEventListener.Get(NGUIUtils.FindGameObject(gameObject, "addBtn")).onClick += OnClickAdd;
            UIEventListener.Get(NGUIUtils.FindGameObject(gameObject, "delBtn")).onClick += OnClickDel;
            UIEventListener.Get(NGUIUtils.FindGameObject(gameObject, "delAllBtn")).onClick += OnClickDelAll;
        }

        private void OnClickAddAll(GameObject go)
        {
            if (OnChange != null)
            {
                OnChange(this, ChangePointType.AddAll);
            }
        }

        private void OnClickAdd(GameObject go)
        {
            if (OnChange != null)
            {
                OnChange(this, ChangePointType.Add);
            }
        }

        private void OnClickDel(GameObject go)
        {
            if (OnChange != null)
            {
                OnChange(this, ChangePointType.Del);
            }
        }

        private void OnClickDelAll(GameObject go)
        {
            if (OnChange != null)
            {
                OnChange(this, ChangePointType.DelAll);
            }
        }
    }
}
