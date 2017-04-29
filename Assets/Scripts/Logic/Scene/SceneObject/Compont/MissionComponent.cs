using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.Manager;
using Assets.Scripts.Utils;
using Assets.Scripts.Define;
using Assets.Scripts.Logic.Mission;
using Assets.Scripts;

namespace Assets.Scripts.Logic.Scene.SceneObject.Compont
{
    class MissionComponent : BaseComponent
    {
        protected GameObject missionSign = null;
        private MissionInfo.MisssionStatus missionStatus = MissionInfo.MisssionStatus.None;

        public override string GetName()
        {
            return GetType().Name;
        }

        public override void OnAttachToEntity(SceneEntity ety)
        {
            BaseInit(ety);

            // 注册事件响应函数
            Regist(ControllerCommand.UPDATE_MISSION_SIGN, OnUpdateMissionSign);
        }

        public override void OnDetachFromEntity(SceneEntity ety)
        {
            UnRegist(ControllerCommand.UPDATE_MISSION_SIGN, OnUpdateMissionSign);

            base.OnDetachFromEntity(ety);
        }

        public object OnUpdateMissionSign(params object[] objs)
        {
            Dictionary<int, MissionInfo> npcMissionList = MissionLogic.GetInstance().GetNpcMissionList((int)Owner.TabID);
            if (npcMissionList != null && npcMissionList.Values.Count != 0)
            {
                bool hasFinish = false;
                bool hasBeenAccept = false;
                foreach (MissionInfo vo in npcMissionList.Values)
                {
                    if (vo.curStatus == MissionInfo.MisssionStatus.Finish)
                    {
                        hasFinish = true;
                        break;
                    }
                    if (vo.curStatus == MissionInfo.MisssionStatus.BeenAccepted)
                        hasBeenAccept = true;
                }
                if (hasFinish)
                {
                    UpdateMissionSign(MissionInfo.MisssionStatus.Finish);
                }
                else if (hasBeenAccept)
                {
                    UpdateMissionSign(MissionInfo.MisssionStatus.BeenAccepted);
                }
                else
                {
                    UpdateMissionSign(MissionInfo.MisssionStatus.Accept);
                }
            }
            else
            {
                RemoveSign();
            }
            return null;
        }

        private void UpdateMissionSign(MissionInfo.MisssionStatus status)
        {
            if (missionStatus == status)
                return;
            missionStatus = status;
            if (Owner.BodyGo == null)
                return;

            RemoveSign();
            switch (missionStatus)
            {
                case MissionInfo.MisssionStatus.Accept:
                    AssetLoader.GetInstance().Load(URLUtil.GetEffectPath("effect_tanhao"), Sign_LoadComplete, AssetType.BUNDLER);
                    break;
                case MissionInfo.MisssionStatus.BeenAccepted:
                    AssetLoader.GetInstance().Load(URLUtil.GetEffectPath("effect_wenhao_hui"), Sign_LoadComplete, AssetType.BUNDLER);
                    break;
                case MissionInfo.MisssionStatus.Finish:
                    AssetLoader.GetInstance().Load(URLUtil.GetEffectPath("effect_wenhao"), Sign_LoadComplete, AssetType.BUNDLER);
                    break;
                default:
                    break;
            }
        }

        private void RemoveSign()
        {
            if (missionSign != null)
            {
                GameObject.DestroyImmediate(missionSign);
                missionSign = null;
            }
        }

        private void Sign_LoadComplete(AssetInfo info)
        {
            if (missionSign == null)
            {
                missionSign = GameObject.Instantiate(info.bundle.mainAsset) as GameObject;
                missionSign.transform.parent = Owner.BodyGo.transform;
                missionSign.transform.localPosition = new Vector3(0, 3, 0);
                missionSign.transform.localScale = Vector3.one;
                ObjectUtil.SetTagWithAllChildren(missionSign, CameraLayerManager.GetInstance().GetMissionSignName());
            }
        }
    }
}
