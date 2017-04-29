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

namespace Assets.Scripts.Logic.Scene.SceneObject
{
    public class EntityPropertyInterface : MonoBehaviour
    {
        public EntityProperty property = new EntityProperty();

        public Animation Anim
        {
            set
            {
                property.anim = value;
            }
            get
            {
                return property.anim;
            }
        }
        public AMIN_MODEL AnimModel
        {
            set
            {
                property.animModel = value;
            }
            get
            {
                return property.animModel;
            }
        }
        public string L_Anim_Name
        {
            set
            {
                property.l_anim_name = value;
            }
            get
            {
                return property.l_anim_name;
            }
        }
        public float Speed
        {
            set
            {
                property.speed = value;
            }
            get
            {
                return property.speed;
            }
        }
        public int AttackSpeed
        {
            set
            {
                property.attackSpeed = value;
            }
            get
            {
                return property.attackSpeed;
            }
        }
        public GameObject BodyGo
        {
            set
            {
                property.bodyGo = value;
            }
            get
            {
                return property.bodyGo;
            }
        }

        public Vector3 Position
        {
            get
            {
                try
                {
                    Vector3 p = transform.position;
                }
                catch (Exception e)
                {
                }
                if (this == null)
                    return Vector3.zero;
                return transform.position;
            }
            set
            {
                transform.position = value;
            }
        }

        public Vector3 Forward
        {
            get
            {
                return transform.forward;
            }
            set
            {
                transform.forward = value;
            }
        }

        public Quaternion Rotation
        {
            get
            {
                return transform.rotation;
            }
            set
            {
                transform.rotation = value;
            }
        }


        public Action ActiveAction
        {
            get
            {
                if (null == property.activeAction)
                    property.activeAction = new ActionNull(this as SceneEntity);
                return property.activeAction;
            }
            set
            {
                if (null != property.activeAction)
				{
					property.activeAction.PopMsg();
                    property.activeAction.Release();
				}
                property.activeAction = value;
                property.activeAction.Active();				
            }
        }

        public Action AidAction
        {
            get
            {
                return property.aidAction;
            }
            set
            {
                if (null != property.aidAction)
                    property.aidAction.Release();
                property.aidAction = value;
                property.aidAction.Active();
            }
        }

        public string GetCurActionName()
        {
            if (null == ActiveAction)
            {
                return "";
            }
            return ActiveAction.NAME;
        }


        public string Name
        {
            get
            {
                return property._name;
            }
            set
            {
                property._name = value;

            }
        }
        public string Title
        {
            get
            {
                return property._title;
            }
            set
            {
                property._title = value;
            }
        }

        public Vector3 PositionFoot()
        {
            return this.transform.position;
        }

        public Vector3 PositinCenter()
        {
            return this.transform.position;
        }

        public CharacterState State
        {
            get { return property.state; }
        }


        public string CharacterStateName(CharacterState state)
        {
            if (state == CharacterState.ATTACK1)
            {
                return "attack1";
            }
            if (state == CharacterState.ATTACK2)
            {
                return "shunpi";
            }
            if (state == CharacterState.MOVE1)
            {
                return "run";
            }
            if (state == CharacterState.MOVE2)
            {
                return "run";
            }
            if (state == CharacterState.JUMP)
            {
                return "run";
            }
            else if (state == CharacterState.IDLE1)
            {
                return "idle1";
            }
            else if (state == CharacterState.SKILL_DFC)
            {
                return "dafengche";
            }
            else if (state == CharacterState.SKILL_JMP)
            {
                return "run";
            }
            else if (state == CharacterState.SKILL_QDB)
            {
                return "qiaodiban";
            }
            else if (state == CharacterState.SKILL_ZF)
            {
                return "zhenfei";
            }
            else if (state == CharacterState.SKILL_NH)
            {
                return "nuhou";
            }
            else if (state == CharacterState.SKILL_CF)
            {
                return "chongfeng";
            }
            else if (state == CharacterState.DEAD1)
            {
                return "dead";
            }
            return state.ToString().ToLower();
        }

        public string CharacterStateName()
        {
            return CharacterStateName(State);
        }

        public KHeroObjectType HeroType
        {
            get
            {
                return property.heroObjType;
            }
            set
            {
                property.heroObjType = value;
            }
        }

        public uint[] EquipIDs
        {
            get
            {
                if (property.equipIDs == null)
                {
                    property.equipIDs = new uint[16];
                }
                return property.equipIDs;
            }
            set
            {
                property.equipIDs = value;
            }
        }

        public ulong OwnerID
        {
            get { return property.ownerID; }
            set { property.ownerID = value; }
        }

        public uint TabID
        {
            get { return property.tabID; }
            set { property.tabID = value; }
        }
        public KJob Job
        {
            get { return property.job; }
            set { property.job = value; }
        }

        public int Hp
        {
            get { return property.hp; }
            set { property.hp = value; }
        }
        public int Mp
        {
            get { return property.mp; }
            set { property.mp = value; }
        }
		
		
		 public int MaxHp
        {
            get { return property.maxHp; }
            set { property.maxHp = value; }
        }
        public int MaxMp
        {
            get { return property.maxMp; }
            set { property.maxMp = value; }
        }
		
		
    }
}
