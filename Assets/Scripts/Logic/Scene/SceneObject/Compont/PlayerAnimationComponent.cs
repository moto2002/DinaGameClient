using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Define;
using UnityEngine;
using Assets.Scripts.Manager;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.Utils;

namespace Assets.Scripts.Logic.Scene.SceneObject.Compont
{
    public class PlayerAnimationComponent : AnimationComponent
    {
        public override string GetName()
        {
            return GetType().Name;
        }

        public override void OnAttachToEntity(SceneEntity ety)
        {
            BaseInit(ety);
			fightTicker.Stop();
            // 注册事件响应函数
            Regist(ControllerCommand.CrossFadeAnimation, CrossFadeAnimation);
            Regist(ControllerCommand.SetActiveAction, SetActiveAction);
            Regist(ControllerCommand.FinishImmediate, FinishImmediate);
            Regist(ControllerCommand.TryFinishAction, TryFinishAction);
            Regist(ControllerCommand.ActionMoveToDistance, ActionMoveToDistance);
            Regist(ControllerCommand.PlayAnimation, PlayAnimation);
            Regist(ControllerCommand.LookAtPos, LookAtPos);
            Regist(ControllerCommand.IsPlayingActionFinish, IsPlayingActionFinish);
            Regist(ControllerCommand.Back, Back);
			Regist(ControllerCommand.FuKong, FuKong);
            Regist(ControllerCommand.Drag, Drag);
            Regist(ControllerCommand.Jump, Jump);
            Regist(ControllerCommand.Idle, Idle);
        }

        public override void OnDetachFromEntity(SceneEntity ety)
        {
            UnRegist(ControllerCommand.CrossFadeAnimation, CrossFadeAnimation);
            UnRegist(ControllerCommand.SetActiveAction, SetActiveAction);
            UnRegist(ControllerCommand.FinishImmediate, FinishImmediate);
            UnRegist(ControllerCommand.TryFinishAction, TryFinishAction);
            UnRegist(ControllerCommand.ActionMoveToDistance, ActionMoveToDistance);
            UnRegist(ControllerCommand.PlayAnimation, PlayAnimation);
            UnRegist(ControllerCommand.LookAtPos, LookAtPos);
            UnRegist(ControllerCommand.IsPlayingActionFinish, IsPlayingActionFinish);
            UnRegist(ControllerCommand.Back, Back);
			UnRegist(ControllerCommand.FuKong, FuKong);
            UnRegist(ControllerCommand.Drag, Drag);
            UnRegist(ControllerCommand.Jump, Jump);
            UnRegist(ControllerCommand.Idle, Idle);

            base.OnDetachFromEntity(ety);
        }
        public override object CrossFadeAnimation(params object[] objs)
        {
            string animName = Convert.ToString(objs[0]);
            AMIN_MODEL model = AMIN_MODEL.DEFAULT;
            bool noReset = true;
			bool isFade = false;
			float fadeTime = 0;
            if (objs.Length == 3)
            {
				model = (AMIN_MODEL)objs[1];
                noReset = Convert.ToBoolean(objs[2]);
            }
			else if(objs.Length == 2)
			{
				isFade = true;
				fadeTime = (float)objs[1];
			}
			else if(objs.Length == 4)
			{
				isFade = true;
				fadeTime = (float)objs[3];
			}
			//Debug.LogWarning("L_Anim_Name = " + Owner.L_Anim_Name);
            Owner.L_Anim_Name = animName;
            if (Owner.Anim && Owner.Anim.GetClip(animName) != null)
            {
				if (isFade)
					return base.CrossFadeAnimation(animName, model, noReset,fadeTime);
				else
	                return base.CrossFadeAnimation(animName, model, noReset);
            }
            LoadRoleAnimAndPlay(Owner.property.roleType, animName);
            return false;
        }

        public void LoadRoleAnimAndPlay(string character, string animName)
        {
            if (Owner.Anim && Owner.Anim.GetClip(animName) != null )
            {
				//Debug.LogWarning("CrossFade "+animName);
                Owner.Anim.CrossFade(animName);
            }
            else
            {
                string resFile = URLUtil.url("/ResourceLib/Actor/" + character + "/" + animName + ".actorAnim");
                AssetLoader.GetInstance().Load(resFile, AnimLoadComplete, AssetType.BUNDLER);
            }
        }

        private void AnimLoadComplete(AssetInfo info)
        {
            if (null == Owner || null == Owner.Anim)
            {
                return;
            }
			string animName ;
           	AnimationClip clip ;
			try
			{
				animName = info.url.Substring(info.url.LastIndexOf("/") + 1).Replace(".actorAnim", "");
           		clip = Owner.Anim.GetClip(animName);
			}
			catch(System.Exception e)
			{
				//游戏对象已经被释放.
				return;
			}
            if (clip == null)
            {
                clip = (AnimationClip)GameObject.Instantiate(info.bundle.mainAsset);
                if (clip == null)
                    return;
                Owner.Anim.AddClip(clip, animName);
                Owner.Anim.CrossFade(animName);
				//Debug.LogWarning("CrossFade "+animName);
            }
        }
    }
}
