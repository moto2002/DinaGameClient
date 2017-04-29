using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.Manager;
using Assets.Scripts.Utils;
using Assets.Scripts.Define;
using Assets.Scripts.Proto;
using Assets.Scripts.Model.Player;
using Assets.Scripts.Lib.Log;

namespace Assets.Scripts.Logic.Scene.SceneObject.Compont
{
    public class NetComponent : BaseComponent
    {
        public override string GetName()
        {
            return GetType().Name;
        }

        public override void OnAttachToEntity(SceneEntity ety)
        {
            BaseInit(ety);
            log = LoggerFactory.GetInstance().GetLogger(typeof(NetComponent));

            // 注册事件响应函数
        }

        public override void OnDetachFromEntity(SceneEntity ety)
        {
            base.OnDetachFromEntity(ety);
        }

        public void SendHeroMove(Vector3 position)
        {
            C2S_HERO_MOVE msg = new C2S_HERO_MOVE();
            ushort x, y;
            short z;
            MapUtils.GetIntFromMetre(position, out x, out y, out z);
            msg.protocolID = (byte)KC2S_Protocol.c2s_hero_move;
            msg.heroID = PlayerManager.GetInstance().MajorPlayer.hero;
            msg.posX = x;
            msg.posY = y;
            msg.posZ = z;
            GameWorld.GetInstance().SendMessage(msg);
        }

        public void SendReliveRequest(bool origin)
        {
            C2S_RELIVE_REQUEST msg = new C2S_RELIVE_REQUEST();
            msg.protocolID = (ushort)KC2S_Protocol.c2s_relive_request;
            if (origin)
                msg.reliveSelect = 1;
            else
                msg.reliveSelect = 2;
            GameWorld.GetInstance().SendMessage(msg);
        }

        public void SendCastSkill(ushort skillId, uint targetId, Vector3 position)
        {	
            C2S_CAST_SKILL msg = new C2S_CAST_SKILL();
            msg.protocolID = (ushort)KC2S_Protocol.c2s_cast_skill;
            msg.casterID = PlayerManager.GetInstance().MajorPlayer.hero;
            msg.skillID = skillId;
            msg.targetID = targetId;
            msg.protocolID = (ushort)KC2S_Protocol.c2s_cast_skill;
            ushort x, y;
            short z;
            MapUtils.GetIntFromMetre(position, out x, out y, out z);
            msg.x = x;
            msg.y = y;
            msg.z = z;
            GameWorld.GetInstance().SendMessage(msg);
        }
    }
}
