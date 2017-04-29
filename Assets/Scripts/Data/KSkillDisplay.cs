using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Lib.Resource;
using Assets.Scripts.Manager;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.Logic.Scene.SceneObject.Compont;

namespace Assets.Scripts.Data
{
    public class KSkillDisplay : AKTabFileObject
    {
		public enum DEAD_TYPE
		{
			NONE,
			BOMB,
			PHYSICS,
		}
		public enum ACTION_AUIDO_TYPE
		{
			Begin,
			Hit
		}
		public KSkillDisplay()
		{
			
		}
        public uint SkillID;
		public uint HeroId;
		public string SkillType;
		public string Anim;
		public bool IsCrossFade;
		public string BeginEffect;
		public string EndEffect;
		public string HitEffect;
		public string BulletEffect;
		public float BulletSpeed;
		public string CameraEffect;
		public string CameraShakeFile;
		public float CameraShakeTime;
		public float CameraShakeSpeed;		
		public float CameraShakeScale;
		public float Param0;
		public float Param1;
		public float Param2;
		public float DieJump;
		public float DieDistance;
		public float DieSpeed;
		public float DieSpeed2;
		public string BeginEffectBindPoint;
		public string EndEffectBindPoint;
		public string HitBindPoint;
		public bool SingleHitFx;
		public float DieAttackSlowTime;
		public float DieAttackSpeed;
		public string Opera;
		public bool IsHeroSkill;
		public float hitDelay;
		public bool BeginEffectBind;
		public bool EndEffectBind;
		public bool WeaponTrail;
		public bool GhostShadow;
		public string Sound;
		public ACTION_AUIDO_TYPE SoundType;
		
		public float HitShakeTime;
		public float HitShakeDelta;
		
		public float PhysicsVelocity;
		public float PhysicsVelocityUp;

		public DEAD_TYPE DeadType;
		
		public WeaponComponent.BIND_POINT WeaponPosition = WeaponComponent.BIND_POINT.DEFAULT;
		
		public float HitDelayTimeScale;
		public string OnHitAction;
		public string OnHitAnim;
		public float OnHitEffecTime;
		public float OnHitHeight;

			
        public override string getKey()
        {
            return SkillID + "_" + HeroId;
        }
		
    }
}

