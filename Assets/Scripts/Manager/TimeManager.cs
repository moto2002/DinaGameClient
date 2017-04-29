using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Lib.Log;

namespace Assets.Scripts.Manager
{
    class TimeManager :MonoBehaviour
    {
        private static Logger log = LoggerFactory.GetInstance().GetLogger(typeof(TimeManager));

        public delegate void TimerHandler();

        private Dictionary<TimerHandler, TimerInfo> timerDict;

        class TimerInfo
        {
            public TimerHandler OnEventHander;
            public int delay;
            public int count;
            public float lastTimer;
        }

        public void Awake()
        {
            timerDict = new Dictionary<TimerHandler, TimerInfo>();
        }
		
		public void Add(TimerHandler timeHandler, int delay)
        {
			Add(timeHandler, delay, -1);
		}
		
        public void Add(TimerHandler timeHandler, int delay, int count)
        {
            TimerInfo info = null;
            if (timerDict.TryGetValue(timeHandler, out info) == false)
            {
                info = new TimerInfo();
                info.count = count;
                info.OnEventHander = timeHandler;
                info.delay = delay;
                info.lastTimer = Time.realtimeSinceStartup * 1000;
                timerDict.Add(timeHandler, info);
            //    log.Debug("Add TimerHandler : " + timeHandler + " delay : " + delay + " count : " + count);
            }
            else
            {
                info.delay = delay;
                info.count = count;
            //    log.Debug("Update TimerHandler : " + info.OnEventHander + " delay : " + delay + " count : " + count);
            }
        }

        public void AddOnce(TimerHandler timeHandler, int delay)
        {
            Add(timeHandler, delay, 1);
        }

        public void Remove(TimerHandler timeHandler)
        {
            TimerInfo info = null;
            if (timerDict.TryGetValue(timeHandler, out info) == false)
            {
                log.Warn("Remove Null TimerHandler : " + timeHandler);
            }
            else
            {
                info.count = 0;
            }
        }

        public void FixedUpdate()
        {
            List<TimerHandler> removeList = new List<TimerHandler>();
            if (timerDict == null)
            {
                timerDict = new Dictionary<TimerHandler, TimerInfo>();
            }
            foreach (KeyValuePair<TimerHandler, TimerInfo> timerPair in timerDict)
            {
                if (timerPair.Value.count == 0)
                {
                    removeList.Add(timerPair.Key);
                }
            }
            foreach (TimerHandler timerHandler in removeList)
            {
                timerDict.Remove(timerHandler);
                log.Debug("Remove TimerHandler : " + timerHandler);
            }

            TimerInfo timerInfo = null;
            foreach (KeyValuePair<TimerHandler, TimerInfo> timerPair in timerDict)
            {
                timerInfo = timerPair.Value;
                if ((timerInfo.lastTimer + timerInfo.delay) <= Time.realtimeSinceStartup*1000)
                {
                    if (timerInfo.count != -1)
                    {
                        timerInfo.count--;
                    }
                    timerInfo.OnEventHander();
                    timerInfo.lastTimer += timerInfo.delay;
                }
            }
        }

        private static TimeManager mInstance;
        public static TimeManager GetInstance()
        {
            if (mInstance == null)
            {
                mInstance = Object.FindObjectOfType(typeof(TimeManager)) as TimeManager;

                if (mInstance == null)
                {
                    GameObject go = new GameObject("_TimeManager");
					go.hideFlags = HideFlags.HideAndDontSave;
                    DontDestroyOnLoad(go);
                    mInstance = go.AddComponent<TimeManager>();
                }
            }
            return mInstance;
        }
    }
}
