using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Manager
{
    public delegate void KeyDelegate(KeyCode code);

    public class InputManager : MonoBehaviour
    {
        public const KeyCode KEY_Q = KeyCode.Q;
        public const KeyCode KEY_W = KeyCode.W;
        public const KeyCode KEY_E = KeyCode.E;
        public const KeyCode KEY_R = KeyCode.R;

        public const KeyCode KEY_1 = KeyCode.Alpha1;
        public const KeyCode KEY_2 = KeyCode.Alpha2;
        public const KeyCode KEY_3 = KeyCode.Alpha3;
        public const KeyCode KEY_4 = KeyCode.Alpha4;

        public static readonly KeyCode[] MainIputKeyList = { KEY_Q, KEY_W, KEY_E, KEY_R, KEY_1, KEY_2, KEY_3, KEY_4 };

        public const int FRAME_COUNT = 100;

        private List<KeyCode[]> keySampleList = null;

        private KeyCode currentKeyCode = KeyCode.None;
        private bool startFrame = false;
        private int currentFrame = 0;
        List<KeyCode> inputKeyList = null;
        private bool isSuccess = false;

        private int minimumSampleCount = 0;
        private int mainIputKeyCount = 0;

        private Dictionary<KeyCode, Observer> observerDict = null;

        void Awake()
        {
            keySampleList = new List<KeyCode[]>();
            inputKeyList = new List<KeyCode>();
            observerDict = new Dictionary<KeyCode, Observer>();
            mainIputKeyCount = MainIputKeyList.Length;
        }

        void Update()
        {
            if (Input.anyKeyDown && !UICamera.inputHasFocus)
            {
                UpdateKey();
                Analyse();
            }
            
        }

        public void RegisterListener(KeyCode key, KeyDelegate handler)
        {
            Observer observer;
            if (!observerDict.TryGetValue(key, out observer))
            {
                observer = new Observer(key);
                observerDict.Add(key, observer);
            }
            observer.OnKeyDown += handler;
        }

        public void RemoveListener(KeyCode key, KeyDelegate handler)
        {
            Observer observer;
            if (observerDict.TryGetValue(key, out observer))
            {
                observer.OnKeyDown -= handler;
                if (observer.Size() == 0)
                {
                    observerDict.Remove(key);
                }
            }
        }

        

        private void Analyse()
        {
            if (currentKeyCode == KeyCode.None)
            {
                Reset();
            }
            else if(minimumSampleCount > 0)
            {
                if (isSuccess)
                {
                    isSuccess = false;
                    Reset();
                }

                if (!startFrame)
                {
                    startFrame = true;
                }

                
                inputKeyList.Add(currentKeyCode);
            }
        }

        private void Reset()
        {
            currentFrame = 0;
            startFrame = false;
            inputKeyList.Clear();
        }

        private void UpdateKey()
        {
            currentKeyCode = KeyCode.None;
            for (int i = 0; i < mainIputKeyCount; i++)
            {
                if (Input.GetKeyDown(MainIputKeyList[i]))
                {
                    currentKeyCode = MainIputKeyList[i];
                    break;
                }
            }
            if (currentKeyCode != KeyCode.None)
            {
                Notify(currentKeyCode);
            }
        }

        private void Notify(KeyCode key)
        {
            Observer observer;
            if (observerDict.TryGetValue(key, out observer))
            {
                observer.Notify();
            }
        }

        private static InputManager instance = null;

        public static InputManager GetInstance()
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType(typeof(InputManager)) as InputManager;

                if (instance == null)
                {
                    GameObject go = new GameObject("_InputManager");
                    DontDestroyOnLoad(go);
                    instance = go.AddComponent<InputManager>();
                }
            }
            return instance;
        }
    }

    class Observer
    {
        public KeyCode key;
        public event KeyDelegate OnKeyDown;

        public Observer(KeyCode key)
        {
            this.key = key;
        }

        public int Size()
        {
            if (OnKeyDown != null)
            {
                return OnKeyDown.GetInvocationList().Length;
            }
            else
            {
                return 0;
            }
        }


        public void Notify()
        {
            if (OnKeyDown != null)
            {
                OnKeyDown(key);
            }
        }
    }
}
