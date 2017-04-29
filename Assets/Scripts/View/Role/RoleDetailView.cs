using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Lib.View;
using UnityEngine;
using Assets.Scripts.Model.Scene;
using Assets.Scripts.View.Scene;
using Assets.Scripts.Utils;
using Assets.Scripts.Model.Player;
using Assets.Scripts.Define;
using Assets.Scripts.Logic.Scene;
using Assets.Scripts.View.UIDetail;

namespace Assets.Scripts.View.Role
{
    public class RoleDetailView : RoleDetailUIDetail
    {
        public RoleDetailView()
            : base(171, 422)
        {

        }

        protected override void PreInit()
        {
            base.PreInit();
            UpdateAttribute();
        }

        protected override void Init()
        {
            base.Init();
            UIPanel uiPanel = viewGo.GetComponentInChildren<UIPanel>();
            uiPanel.transform.localScale = new Vector3(1.68f, 1.68f, 1f);

            uiPanel.transform.localPosition += new Vector3(380f, 0f, 0f);
            uiPanel.transform.parent = Panel.transform;
        }

        public override void Show(bool isForce)
        {
            if (uiPanel.gameObject != null)
            {
                if (isForce || !uiPanel.gameObject.activeSelf)
                {
                    uiPanel.gameObject.SetActive(true);
                    Front();
                    InitEvent();
                }
                else
                {
                    Hide();
                }
            }
        }



        public override void Hide()
        {
            uiPanel.gameObject.SetActive(false);
            DestoryEvent();
        }

        public void UpdateAttribute()
        {
            MajorPlayer player = PlayerManager.GetInstance().MajorPlayer;
            PlayerHeroData hero = player.HeroData as PlayerHeroData;

            hp2Txt.text = SceneLogic.GetInstance().MainHero.Hp + "/" + hero[KAttributeType.atMaxHP];
            mp2Txt.text = SceneLogic.GetInstance().MainHero.Mp + "/" + hero[KAttributeType.atMaxMP];
            attack2Txt.text = hero[KAttributeType.atAttack].ToString();
            defend2Txt.text = hero[KAttributeType.atDefence].ToString();
            dodge2Txt.text = hero[KAttributeType.atMiss].ToString();
            crite2Txt.text = hero[KAttributeType.atCrit].ToString();
            criteHurtTxt.text = hero[KAttributeType.atCritHurt].ToString();
            curiteResistanceTxt.text = hero[KAttributeType.atReduceCrit].ToString();
            speedTxt.text = hero[KAttributeType.atMoveSpeed].ToString();
            attackSpeedTxt.text = hero[KAttributeType.atAttackSpeed].ToString();
            hpSp.fillAmount = SceneLogic.GetInstance().MainHero.Hp / hero[KAttributeType.atMaxHP];
            mpSp.fillAmount = SceneLogic.GetInstance().MainHero.Mp / hero[KAttributeType.atMaxMP];
        }
    }
}
