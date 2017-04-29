using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Scripts.Define;
using Assets.Scripts.Logic.Scene.SceneObject.Compont;

namespace Assets.Scripts.Logic.Scene.SceneObject
{
    public enum AMIN_MODEL
    {
        DEFAULT = 0,
        ONCE = 1,
        LOOP = 2,
    }

    public enum CharacterState
    {								//角色状态枚举
        IDLE1 = 0,
        IDLE2 = 1,
        IDLE3 = 2,
        MOVE1 = 3,
        MOVE2 = 4,
        JUMP = 5,
        ATTACK1 = 6,
        ATTACK2 = 7,
        DEAD1 = 8,
        HIT = 9,
        SKILL_DFC = 20,					//大风车技能
        SKILL_QDB = 21,					//敲地板技能
        SKILL_ZF = 22,					//震飞技能
        SKILL_NH = 23,					//怒吼
        SKILL_CF = 24,					//冲锋 
        SKILL_JMP = 90,					//大风车跳跃
        NONE = 100
    }

    public class EntityProperty
    {
		public KDropType dropType;
		public int dropValue ;
        public int dropIndex ;
        public bool isInteractive = true;
        public Dictionary<Renderer, Material[]> saveMaterialMap = new Dictionary<Renderer, Material[]>();
        public string SelectShaderName = "Kingsoft/Selected";
        public GameObject bodyGo;
        public Color rimColor = new Color32(255, 255, 0, 255);
        public float rimPower = 3f;
        public Color32 selectMainColor = new Color32(100, 100, 100, 255);
        public ulong ownerID;
        public uint tabID;
        public KJob job;
        public uint Id { set; get; }
        public KSceneObjectType sceneObjType { set; get; }
        public KHeroObjectType heroObjType { set; get; }
        public KDoodadType doodadObjType { set; get; }
        public float speed = 6.0f;
        public int attackSpeed = 0;
        public float boundsHeight = 0;
        public SkinnedMeshRenderer bodyGoRender;

        public AMIN_MODEL animModel = AMIN_MODEL.DEFAULT;
		public bool isDeaded = false;//角色是否死亡
        public bool isDeadTemp = false;//角色是否死亡(客户端缓存用)
        public bool isMainHero = false;

        public SceneEntity target = null;
        public bool isCanAttack = false;
        public bool AutoAttack = false;//技能释放是否自动攻击（鼠标左键点击默认自动攻击，右键攻击完毕后停止攻击）.
		public bool CmdAutoAttack = false;//自动挂机.
		public bool bNewHero = false;
		
        public CharacterState state = CharacterState.IDLE1;

        public Animation anim;
        public string l_anim_name = "";

        public Action activeAction = null;
        public Action nextAction = null;
        public Action aidAction = null;
        public string roleType;

        public Vector3 destination;
		public Vector3 finalDestination;
        public BoxCollider characterController;

        public string _name;
        public string _title = "";

        public uint[] equipIDs;
        public byte Face;
        public Vector3 ServerPos;

        public int hp;
        public int mp;
		
		public int maxHp;
        public int maxMp;
		
		public int fightHp;
		public float updateFightHpTime;
		
		public uint lastHitSkillId = 0;
		public uint lastAttackEvent = 0;
		
		public bool lockOpera = false; //是否锁定操作
		
		public GameObject []weapon = new GameObject[2];//武器1(因为绘画残影可能会频繁调用，直接把引用保留在人物身上).
    }
}
