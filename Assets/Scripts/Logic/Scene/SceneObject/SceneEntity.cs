using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Manager;
using Assets.Scripts.Define;
using UnityEngine;
using Assets.Scripts.Model.Scene;
using Assets.Scripts.Utils;
using Assets.Scripts.Lib.Log;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.Logic.Scene.SceneObject;
using Assets.Scripts.Logic.Scene.SceneObject.Compont;
using Assets.Scripts.Model.Player;
using Assets.Scripts.Data;

namespace Assets.Scripts.Logic.Scene.SceneObject
{
    public class SceneEntity : EntityPropertyInterface
    {
		public KHeroSetting heroSetting = null;
		public BindedUI ui = new BindedUI();
        public KCollectMissionInfo collectInfo = null;
        public EventDispatcher eventDispatcher = new EventDispatcher();
        public static Logger log = LoggerFactory.GetInstance().GetLogger(typeof(SceneEntity));
        Dictionary<string, BaseComponent> ComponentArray = new Dictionary<string, BaseComponent>();
        static GameObject sceneParentObject;
		public LoadAssetComponent Loader = null;
		public Transform GetChildTransform(string name)
		{
			if (null == BodyGo)
				return null;
			Transform[] transforms = BodyGo.GetComponentsInChildren<Transform>();
			foreach (Transform t in transforms)
			{
				if (t.name.CompareTo(name)==0)
					return t;
			}
			return null;
		}
		public Animation GetAnimation()
		{
			if (null != BodyGo)
				return BodyGo.animation;
			return null;
		}
		public  void CreateTraceComponent()
		{
			if (null == GetEntityComponent<TraceComponent>())
			{
				AddEntityComponent<TraceComponent>();
			}
		}
        public virtual void Init()
        {
            if (property.sceneObjType == KSceneObjectType.sotHero)
            {
                this.transform.localScale = Vector3.one;
                this.transform.eulerAngles += MapUtils.GetEulerAngles(property.Face);
                if (property.sceneObjType == KSceneObjectType.sotHero)
                {
					AddEntityComponent<LoadAssetComponent>();
                    AddEntityComponent<GlowComponent>();
                    AddEntityComponent<MoveComponent>();
                    AddEntityComponent<BuffComponent>();
					AddEntityComponent<GhostShadowComponent>();
                    AddEntityComponent<HeadePanelComponent>();
                    AddEntityComponent<ActionComponent>();
					AddEntityComponent<SlowComponent>();
					AddEntityComponent<ShakeComponent>();
                    if (ConfigManager.GetInstance().DebugMode)
                    {
                        AddEntityComponent<TraceComponent>();
                    }
                    if (property.heroObjType == KHeroObjectType.hotPlayer)
                    {
                        AddEntityComponent<PlayerAnimationComponent>();
                        AddEntityComponent<EquipChangeComponent>();
						AddEntityComponent<WeaponComponent>();
                        KGender sex = KGender.gNone;
                        Player newPlayerInfo;
                        newPlayerInfo = PlayerManager.GetInstance().GetPlayer(OwnerID);
                        if (newPlayerInfo != null)
                            sex = newPlayerInfo.Gender;
                        else
                            sex = PlayerManager.GetInstance().MajorPlayer.Gender;
                        property.roleType = heroSetting.FbxName;
                        DispatchEvent(ControllerCommand.EQUIP_CHANGE, EquipIDs);

                        if (property.Id == PlayerManager.GetInstance().MajorPlayer.hero)
                        {
                            AddEntityComponent<NetComponent>();
                        }
                    }
                    else
                    {
                        if (property.heroObjType == KHeroObjectType.hotNpc)
                        {
                            AddEntityComponent<MissionComponent>();
                            property.rimColor = new Color32(0, 255, 0, 255);
                            property.rimPower = 5f;
                            property.selectMainColor = new Color32(170, 170, 170, 255);

                        }
                        else if (property.heroObjType == KHeroObjectType.hotMonster)
                        {
                            property.rimColor = new Color32(255, 0, 0, 255);
                            property.isCanAttack = true;
                        }

                        AddEntityComponent<AnimationComponent>();
                        AddEntityComponent<LoadNpcResComponent>();
						AddEntityComponent<ColorComponent>();
                    }
                }
            }
            else if (property.sceneObjType == KSceneObjectType.sotDoodad)
            {
                if (property.doodadObjType == KDoodadType.dddCollect)
                {
                    AddEntityComponent<CollectObjComponent>();
                    AddEntityComponent<HeadePanelComponent>();
					AddEntityComponent<ItemGlowComponent>();
					property.rimColor = new Color32(255, 255, 255, 255);
                    property.rimPower = 5f;
                }
                else if (property.doodadObjType == KDoodadType.dddDrop)
                {
                    AddEntityComponent<ItemComponent>();
                    AddEntityComponent<HeadePanelComponent>();
					AddEntityComponent<ItemGlowComponent>();
					property.rimColor = new Color32(255, 255, 255, 255);
                    property.rimPower = 5f;
                }   
            }
        }

        public virtual EventRet DispatchEvent(string evt, params object[] objs)
        {
            EventRet b = eventDispatcher.DispatchEvent(evt, objs);
			return b;
        }

        public bool AddEntityComponent<T>() where T : BaseComponent, new()
        {
            T compObject = new T();

            if (compObject == null)
            {
                Console.Write("AddComponent Failed, Component.Name=" + compObject.GetName());
                return false;
            }

            ComponentArray.Add(compObject.GetName(), compObject);
            compObject.OnAttachToEntity(this);
            OnComponentAttached(compObject);

            return true;
        }

        public bool AddEntityComponent(BaseComponent compObject)
        {
            if (compObject == null)
            {
                Console.Write("AddComponent Failed, Component.Name=" + compObject.GetName());
                return false;
            }

            ComponentArray.Add(compObject.GetName(), compObject);
            compObject.OnAttachToEntity(this);
            OnComponentAttached(compObject);

            return true;
        }

        public bool RemoveEntityComponent<T>() where T : BaseComponent
        {
            foreach (string compName in ComponentArray.Keys)
            {
                if (typeof(T).Name == compName)
                {
                    ComponentArray[compName].OnDetachFromEntity(this);
                    OnComponentDetached(ComponentArray[compName]);
                    ComponentArray.Remove(compName);

                    return true;
                }
            }
            return false;
        }

        public bool RemoveAllEntityComponent()
        {
            foreach (BaseComponent comp in ComponentArray.Values)
            {
                comp.OnDetachFromEntity(this);
                OnComponentDetached(comp);
            }
            ComponentArray.Clear();
            return true;
        }

        public BaseComponent GetEntityComponent(string compName)
        {
            BaseComponent component = null;
            ComponentArray.TryGetValue(compName, out component);
            return component;
        }

        public T GetEntityComponent<T>() where T : BaseComponent
        {
            BaseComponent component = GetEntityComponent(typeof(T).Name);
            if (component != null)
            {
                return component as T;
            }
            foreach (BaseComponent comp in ComponentArray.Values)
            {
                Type type = comp.GetType();
                while (type != null)
                {
                    if (type == typeof(T))
                    {
                        return comp as T;
                    }
                    type = type.BaseType;
                }
            }

            return null;
        }


        public virtual void OnComponentAttached(BaseComponent compObject)
        {
        }

        public virtual void OnComponentDetached(BaseComponent compObject)
        {
        }

        void OnDestroy()
        {
            RemoveAllEntityComponent();
        }

        private void Update()
        {
            DoUpdate();
            if (null != BodyGo)
            {
                BodyGo.transform.localRotation = Quaternion.identity;
            }
        }

        protected virtual void DoUpdate()
        {
            foreach (BaseComponent comp in ComponentArray.Values)
            {
                comp.DoUpdate();
            }
        }

        public void Dispose() { }

        public static SceneEntity Create(string name)
        {
            if (null == SceneEntity.sceneParentObject)
            {
				GameObject _go = GameObject.Find("SceneObject");
				if (null != _go)
					GameObject.DestroyObject(_go);
                SceneEntity.sceneParentObject = new GameObject("SceneObject");
				sceneParentObject.hideFlags = HideFlags.DontSave;
            }

            GameObject sceneObject = new GameObject(name);
            sceneObject.transform.parent = SceneEntity.sceneParentObject.transform;
            sceneObject.tag = TagManager.GetInstance().SceneObjectTag;
            SceneEntity t = sceneObject.AddComponent<SceneEntity>();
            t.enabled = false;
            return t;
        }

        public GameObject GetHeadPanel()
        {
            HeadePanelComponent comp = GetEntityComponent<HeadePanelComponent>();
            if (comp != null)
            {
                return comp.GetHeadPanel();
            }
            return null;
        }

        public ActionComponent Action
        {
            get
            {
                return GetEntityComponent<ActionComponent>();
            }
        }
		
        public NetComponent Net
        {
            get
            {
                return GetEntityComponent<NetComponent>();
            }
        }
		
		AnimationComponent _anim = null;
		public AnimationComponent AnimCmp
		{
			get
            {
				if( null == _anim)
					_anim = GetEntityComponent<AnimationComponent>();
                return _anim;
            }
		}
		NumTipComponent _tips = null;
		public NumTipComponent TipsCmp
		{
			get
            {
				if( null == _tips)
				{
					AddEntityComponent<NumTipComponent>();
					_tips = GetEntityComponent<NumTipComponent>();
				}
                return _tips;
            }
		}
		WeaponComponent _weapon = null;
		public WeaponComponent Weapon
		{
			get
            {
				if( null == _weapon)
				{
					_weapon = GetEntityComponent<WeaponComponent>();
				}
                return _weapon;
            }
		}
    }
}
