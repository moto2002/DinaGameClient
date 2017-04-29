using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Define;
using UnityEngine;
using Assets.Scripts.Manager;
using Assets.Scripts.Lib.Log;
using Assets.Scripts.View.Scene;
using Assets.Scripts.Logic.Scene.SceneObject;

namespace Assets.Scripts.View.Scene
{
    public enum FightType
    {
        ftNormal,
        ftAttack,
        ftContinue,
        ftCrit,
        ftHurt,
        ftHp,
        ftMp,
        ftExp
    }

    //战斗数字
    public class FightNumber : MonoBehaviour
    {
        public static Logger log = LoggerFactory.GetInstance().GetLogger(typeof(FightNumber));

        private static string ResourceAtlasName = "CommonAtlas";

        private Transform panelGo;
        private SceneView scene;
        private SceneEntity targetHero;
        private FightType fightType;
        private int damage;

        private Vector3 startPosition;
        private Vector3 endPosition;

        public void Init()
        {
      //      panelGo = ParentObjectManager.GetInstance().CreateUIParent();
            //GameObject root = new GameObject();
            //root.AddComponent<UIRoot>();
            startPosition = targetHero.PositionFoot();
            if (fightType == FightType.ftCrit)
            {
                startPosition += Vector3.zero;
            }
            GenerateSpriteFont();
         //   this.transform.parent = root.transform;
            this.transform.position = startPosition;
            
        }


        public void GenerateSpriteFont()
        {
            GenerateEvent();
            string numStr = damage.ToString();
            int bit = numStr.Length;
            for (int idx = 0; idx < numStr.Length; idx ++)
            {
                GenerateNumber(numStr.Substring(idx, 1));
            }
        }

        //产生单个数字
        public void GenerateNumber(string subNum)
        {
            UISprite uiGo = MonoGameObjectPool<UISprite>.Instance.GetUIGameObject();
            uiGo.atlas = UIAtlasManager.GetInstance().GetUIAtlas(ResourceAtlasName);
            uiGo.spriteName = fightType.ToString().Substring(2).ToLower() + "_" + subNum;
            uiGo.transform.parent = this.transform;
      //      uiGo.MakePixelPerfect();
            //调整位置
        }

        //产生事件类型
        public void GenerateEvent()
        {
            UISprite uiGo = MonoGameObjectPool<UISprite>.Instance.GetUIGameObject();
            uiGo.atlas = UIAtlasManager.GetInstance().GetUIAtlas(ResourceAtlasName);
            uiGo.spriteName = fightType.ToString().Substring(2).ToLower();
            uiGo.MakePixelPerfect();
            uiGo.transform.parent = this.transform;
        }

        public void Destory()
        {
            MonoGameObjectPool<FightNumber>.Instance.Destory(this);
        }

        public static FightNumber Create(SceneView _scene, SceneEntity _targetHero, FightType _fightType, int _damage)
        {
            FightNumber fightNumberGo = MonoGameObjectPool<FightNumber>.Instance.GetUIGameObject();
            fightNumberGo.scene = _scene;
            fightNumberGo.targetHero = _targetHero;
            fightNumberGo.fightType = _fightType;
            fightNumberGo.damage = _damage;
            log.Debug("创建战斗对象");
            return fightNumberGo;
        }

        public void StartEffect()
        {
            Init();
        }
    }
}
