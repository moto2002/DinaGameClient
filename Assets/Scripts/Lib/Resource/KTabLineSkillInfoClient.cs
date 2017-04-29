using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Lib.Resource
{
    public class KTabLineSkillInfoClient : AKTabFileObject
    {
        public int SkillID = 0;
        public string Text = null;
        public int LaunchRange = 0;
        public int BulletId = 0;
        public int AttachVFX = 0;
        public int UsePRI = 0;
        public float EffectTime = 0;
        public int EffectVFXLayer = 0;
        public int EffectTarget = 0;
        public int EffectVFX = 0;
        public int Icon = 0;
        public int UniteSkill = 0;
        public string SkillActionList = null;
        public int CastTag = 0;
        public string BigFace = null;
        public string BigFaceBG = null;

        // 该方法必须实现
        public override string getKey()
        {
            return SkillID.ToString();
        }

        //// 该方法可以不用实现
        //public new void onComplete()
        //{
        //    Debug.Log("tab line onComplete " + Text);
        //}

        //// 该方法可以不用实现
        //public new void onAllComplete()
        //{
        //    Debug.Log("tab line onAllComplete " + Text);
        //}
    }
}
