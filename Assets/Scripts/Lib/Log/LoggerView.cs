using UnityEngine;
using System.Collections;
using Assets.Scripts.Model.Application;
using Assets.Scripts.Model.Player;
using Assets.Scripts.Model.Scene;
using System;
using Assets.Scripts.View.Scene;
using Assets.Scripts.Logic.Scene;
using Assets.Scripts.Define;
using Assets.Scripts.View.MainMenu;

namespace Assets.Scripts.Lib.Log
{
    public class LoggerView : MonoBehaviour
    {
        private bool isShowThis = false;
        private float homeInterval = 0.5f;
        private float homeButtonTime = 0;
        private FPS fps = null;

        void Start()
        {
            isShowThis = true;
            fps = new FPS();
        }

        void Update()
        {
            fps.Update();

            if (Time.realtimeSinceStartup - homeButtonTime < homeInterval)
                return;
            if (Input.GetKey(KeyCode.Home))
            {
                homeButtonTime = Time.realtimeSinceStartup;
                isShowThis = !isShowThis;
            }
        }

        void OnGUI()
        {
            MajorPlayer majorPlayer = PlayerManager.GetInstance().MajorPlayer;
            string outputString = "";
            
            if (SceneLogic.GetInstance().MainHero)
            {
                outputString += "坐标:" + SceneLogic.GetInstance().MainHero.Position;
            }

            if (majorPlayer.level > 0)
            {
                outputString += "等级:" + majorPlayer.level;
                try
                {
                    PlayerHeroData hero = majorPlayer.HeroData;
                    outputString += " 气血:" + SceneLogic.GetInstance().MainHero.Hp + "/" + hero[KAttributeType.atMaxHP];
                }
                catch (NullReferenceException e)
                {

                }
            }
            GUI.Label(new Rect(Screen.width - 400f, 0, 200, 20), outputString + " fps:" + fps.Fps);

            if (isShowThis)
            {
                GUI.Label(new Rect(10, 10, Screen.width - 20f, Screen.height - 20), Logger.GetUIMessage() + "\n按Home键关闭日志!");
            }

            if (PlayerMessage != "")
            {
                GUI.color = Color.yellow;
                GUI.Label(new Rect(Screen.width - 212, Screen.height - 158, 210, 79), PlayerMessage);
                //GUI.Box(new Rect(Screen.width - 212, Screen.height - 158, 170, 64), "");
                GUI.color = Color.white;
            }
           
        }

        public string PlayerMessage = "";

        private static LoggerView mInstance;
        public static LoggerView GetInstance()
        {
            if (mInstance == null)
            {
                mInstance = GameObject.FindObjectOfType(typeof(LoggerView)) as LoggerView;

                if (mInstance == null)
                {
                    GameObject go = new GameObject("_LoggerView");
					go.hideFlags = HideFlags.HideAndDontSave;
                    DontDestroyOnLoad(go);
                    mInstance = go.AddComponent<LoggerView>();
                }
            }
            return mInstance;
        }
    }
  
}
