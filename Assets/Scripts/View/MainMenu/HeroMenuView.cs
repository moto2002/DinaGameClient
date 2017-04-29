using UnityEngine;
using System.Collections;
using Assets.Scripts.View.UIDetail;
using Assets.Scripts.Model.Player;
using Assets.Scripts.Manager;
using Assets.Scripts.Utils;
using Assets.Scripts.Define;
using Assets.Scripts.Logic.Scene.SceneObject;
using Assets.Scripts.Logic.Scene;
using System.Collections.Generic;
using Assets.Scripts.Logic.Item;
using Assets.Scripts.Lib;
using Assets.Scripts.Lib.Log;

namespace Assets.Scripts.View.MainMenu
{  
    /************************************************************************/
    /* 左上角菜单界面（包含玩家菜单、宠物、队友面板和怪物血条）
     * author@wuheyang*/
    /************************************************************************/
    public class HeroMenuView : HeroMenuUIDetail
    {
        private GameObject Enemy = null;
        private GameObject Pet = null;
        private GameObject Player = null;
        private GameObject Teammate = null;
        private GameObject TeammateItem = null;
        private GameObject TeammateList = null;
        private EnemyBlood enemyBlood = null;
        private SceneEntity curEnemySceneEntity = null;

        protected override void Init()
        {
            base.Init();
            base.SetViewPosition(Lib.View.ViewPosition.TopLeft);

            Enemy = FindGameObject("Enemy");
            Pet = FindGameObject("Pet");
            Player = FindGameObject("Player");
            Teammate = FindGameObject("Teammate");
            TeammateItem = FindGameObject("TeammateItem");
            TeammateList = FindGameObject("TeammateList");

            enemyBlood = Enemy.AddComponent<EnemyBlood>();

            Pet.SetActive(false);
            Teammate.SetActive(false);
            Enemy.SetActive(false);

            MajorPlayer player = PlayerManager.GetInstance().MajorPlayer;
            base.CombatLabel.text = player.combat.ToString();

            OnUpdatePlayerMoney(null);
            OnChangeNickname(null);
            OnChangeHead(null);
            OnPlayerLevelUp(null);
            OnChangeCombat(null);
        }

        protected override void InitEvent()
        {
            base.InitEvent();
            EventDispatcher.GameWorld.Regist(ControllerCommand.SYNC_MONEY, OnUpdatePlayerMoney);
            EventDispatcher.GameWorld.Regist(ControllerCommand.CHANGE_NICKNAME, OnChangeNickname);
            EventDispatcher.GameWorld.Regist(ControllerCommand.CHANGE_HEAD, OnChangeHead);
            EventDispatcher.GameWorld.Regist(ControllerCommand.PLAYER_LEVEL_UP, OnPlayerLevelUp);
            EventDispatcher.GameWorld.Regist(ControllerCommand.UPDATE_COMBAT, OnChangeCombat);
            EventDispatcher.GameWorld.Regist(ControllerCommand.ATTACK_BOSS, OnAttackBoss);
            EventDispatcher.GameWorld.Regist(ControllerCommand.CHANGE_TARGET, OnAttackBoss);
            EventDispatcher.GameWorld.Regist(ControllerCommand.ADD_ITEM, OnAddItem);
            EventDispatcher.GameWorld.Regist(ControllerCommand.CHANGE_MAP, OnChangeMap);
        }

        public List<string> PlayerMessageList = new List<string>();

        private object OnUpdatePlayerMoney(params object[] objs)
        {
            MajorPlayer player = PlayerManager.GetInstance().MajorPlayer;

            if (player.addMoney != 0)
            {
                PlayerMessageList.Add(player.addMoney > 0 ? Util.ADD_MONEY + player.addMoney : Util.DESCEND_MONEY + player.addMoney);
                player.addMoney = 0;
            }

            if (player.addCoin != 0)
            {
                PlayerMessageList.Add(player.addCoin > 0 ? Util.ADD_COIN + player.addCoin : Util.DESCEND_COIN + player.addCoin);
                player.addCoin = 0;
            }

            if (player.addMenterPoint != 0)
            {
                PlayerMessageList.Add(player.addMenterPoint > 0 ? Util.ADD_MENTER_POINT + player.addMenterPoint : Util.DESCEND_MENTER_POINT + player.menterPoint);
                player.addMenterPoint = 0;
            }

            base.MoneyLabel.text = FormatMoney(player.money);
            base.CoinLabel.text = FormatMoney(player.coin);
            base.MenterpointLabel.text = FormatMoney(player.menterPoint);
            return null;
        }

        public static string FormatMoney(int money)
        {
            if (money > 10000)
            {
                string text = (money /= 10000).ToString() + "万";
                return text;
            }
            return money.ToString();
        }

        private object OnChangeNickname(params object[] objs)
        {
            MajorPlayer player = PlayerManager.GetInstance().MajorPlayer;
            base.PlayerNameLabel.text = player.PlayerName;
            return null;
        }

        private object OnChangeHead(params object[] objs)
        {
            KJob job = (KJob)PlayerManager.GetInstance().MajorPlayer.Job;
            if (KJob.JobLance == job)
            {
                PlayerAvatarSprite.spriteName = "女主";
            }
            else
            {
                PlayerAvatarSprite.spriteName = "男主";
            }
            return null;
        }

        private int curCombat = 0;
        private int targetCombat = 0;
        private const int TOTAL_COUNT = 50;
        private int addCount = 0;

        private object OnChangeCombat(params object[] objs)
        {
            MajorPlayer player = PlayerManager.GetInstance().MajorPlayer;

            targetCombat = player.combat;
            curCombat = int.Parse(base.CombatLabel.text);

            addCount = (int)Mathf.Ceil((targetCombat - curCombat) / TOTAL_COUNT);

            return null;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (Mathf.Abs(targetCombat - curCombat) > Mathf.Abs(addCount))
            {
                curCombat += addCount;
                base.CombatLabel.text = curCombat.ToString();
            }
            else if (targetCombat != curCombat)
            {
                curCombat = targetCombat;
                base.CombatLabel.text = curCombat.ToString();
            }
            LoggerView.GetInstance().PlayerMessage = GetPlayerMessage();
        }

        private object OnPlayerLevelUp(params object[] objs)
        {
            MajorPlayer player = PlayerManager.GetInstance().MajorPlayer;
            base.PlayerLevelLabel.text = player.levelCurrent.ToString();

            return null;
        }

        private object OnAttackBoss(params object[] objs)
        {
            SceneEntity targetSceneEntity = SceneLogic.GetInstance().MainHero.property.target;

            bool isBoss = targetSceneEntity != null && targetSceneEntity.heroSetting != null && targetSceneEntity.heroSetting.HeroType == KHeroObjectType.hotMonster && targetSceneEntity.heroSetting.MonsterGrade == KMonsterGrade.mgQuestBoss;
            if (isBoss)
            {
                Enemy.SetActive(true);
            }
            else
            {
                Enemy.SetActive(false);
                return null;
            }

            float cellBoold = (float)targetSceneEntity.MaxHp / 7;
            float toRate = ((float)targetSceneEntity.Hp / cellBoold) + 1f;

            if (curEnemySceneEntity != targetSceneEntity)
            {
                enemyBlood.ClearRate(toRate, base.EnemyBloodCountLabel);
            }
            curEnemySceneEntity = targetSceneEntity;

            enemyBlood.Set(toRate, base.EnemyBloodCountLabel);

            base.EnemyBloodValueLabel.text = targetSceneEntity.Hp.ToString() + "/" + targetSceneEntity.MaxHp.ToString();
            base.EnemyNameLabel.text = targetSceneEntity.heroSetting.Name;
            base.EnemyLevelLabel.text = targetSceneEntity.heroSetting.Level.ToString();
            return null;
        }

        private object OnChangeMap(params object[] objs)
        {
            SceneLogic.GetInstance().MainHero.property.target = null;
            OnAttackBoss(null);
            return null;
        }

        public object OnAddItem(params object[] objs)
        {
            //EquipInfo itemInfo = (EquipInfo)objs[0];
            PlayerMessageList.Add(Util.ADD_EQUIP);
            return null;
        }

        private string msg = "";
        private int lastCount = 0;
        public string GetPlayerMessage()
        {
            if (lastCount == PlayerMessageList.Count)
            {
                return msg;
            }
            msg = "";
            lastCount = PlayerMessageList.Count;
            int begin = PlayerMessageList.Count > 4 ? PlayerMessageList.Count - 4 : 0;
            for (; begin != PlayerMessageList.Count; ++begin)
            {
                msg += (PlayerMessageList[begin] + "\n");
            }
            return msg;
        }

        public HeroMenuView()
            : base(0, 0)
        {

        }
        private static HeroMenuView instance = null;
        public static HeroMenuView GetInstance()
        {
            if (instance == null)
            {
                instance = new HeroMenuView();
            }
            return instance;
        }
    }



}
