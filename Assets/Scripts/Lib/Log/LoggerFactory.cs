using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Assets.Scripts.Controller;

namespace Assets.Scripts.Lib.Log
{
	class LoggerFactory
	{
        //日志系统是否开启
		public bool IsDefaultStart = true;
		//日志字典
		private Dictionary<Type, Logger> logDict;
		//当前已经打开的日志
		private List<Logger> currStartList;
        //默认开启的日志
        private List<Type> InitType = null;
		
		public LoggerFactory()
		{
			logDict = new Dictionary<Type, Logger>();
			currStartList = new List<Logger>();
            InitType = new List<Type>();
		}
		
		/**
		 * 得到日志的工厂方法
		 * @param 	类名字
		 * @return 	日志
		 * */
		public Logger GetLogger(Type type)
		{
			Logger log = null;
			if(!logDict.ContainsKey(type))
			{
				//throw new Exception("重复定义的日志类型");
				log = new Logger(type);
				logDict[type] = log;
			}
			else
			{
				log = logDict[type];
			}
            log.IsStart = IsDefaultStart;
			return log;
		}
		
        public void StartLog()
        {
        }

        public void StartLog(Type type)
        {
            Logger log = GetLogger(type);
            log.IsStart = true;
            currStartList.Add(log);
        }

		/**
		 * 开启日志
		 * */
		public int StartLog(string typeName)
		{
			int count = 0;			//开启的日志数
			Logger log;
			foreach(KeyValuePair<Type, Logger> kvp in logDict)
			{
				if(kvp.Key.Name.IndexOf(typeName) > 0)
				{
					log = kvp.Value;
					log.IsStart = true; //设置此类日志开启
					currStartList.Add(log);
					count ++;
				}
			}
			return count;
		}
		
		/**
		 * 关闭给出名的日志
		 * */
		public int CloseLog(string typeName)
		{
			int count = 0;
			foreach(Logger log in currStartList)
			{
				if(log.LogType.Name.IndexOf(typeName) > -1)
				{
					log.IsStart = false;
					currStartList.Remove(log);
				}
				count ++;
			}
			return count;
		}
		
		/**
		 * 关闭所有日志
		 * */
		public int CloseAllLog()
		{
			int count = 0;
            foreach(Logger log in currStartList)
            {
				log.IsStart = false;
                count ++;
            }
            currStartList = new List<Logger>();
			return count;
		}
		

		
		/**
		 * 得到当前打开的日志数
		 * */
		public int Count
		{
            get
            {
                return currStartList.Count();
            }
		}
		
		private static LoggerFactory instance;
        public static LoggerFactory GetInstance()
		{
			if(instance == null)
			{
				instance = new LoggerFactory();
			}
			return instance;
		}
	}
}
