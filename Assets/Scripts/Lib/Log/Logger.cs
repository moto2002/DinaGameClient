using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.IO;

namespace Assets.Scripts.Lib.Log
{
    public class Logger
	{
        private bool isStart = true;
		
		private Type logType;

        public static List<string> contentList = new List<string>();

        public static string GetUIMessage()
        {
            string retStr = "";
            foreach (string content in contentList)
            {
                retStr += content + "\n";
            }
            return retStr;
        }

        public Logger(Type _logType)
		{
            logType = _logType;
		}
		
		public bool IsStart
		{
			set
            {
                isStart = value;
            }
            get
            {
                return isStart;
            }
		}
		
		public Type  LogType
		{
            get
            {
                return logType;
            }
		}
		

		public void Error(string format, params object[] args)
		{
			if(isStart == false)
				return;
			Print("ERR", string.Format(format, args));
		}
		
		public void Warn(string format, params object[] args)
		{
			if(isStart == false)
				return;
			Print("WARN", string.Format(format, args));
		}
		
		public void Info(string format, params object[] args)
		{
			if(isStart == false)
				return;
			Print("INFO", string.Format(format, args ));
		}
		
		public void Debug(string format, params object[] args)
		{
            if (isStart == false)
				return;
			Print("DEBUG", string.Format(format, args));
		}

        public void Assert(bool value, string format)
        { 
            
        }

        public void Assert(bool value)
        {

        }

		/**
		 * 得到格式化的消息头
		 * */
		private string FormatMessageHeader()
		{
			return LogType + "|" + DateTime.Now.ToShortTimeString() + " ";
		}
		
		private void Print(string type, string message)
		{
            string printStr = "[" + logType.Name + "]" + " " + FormatMessageHeader() + message;
            if (type == "ERR")
                UnityEngine.Debug.LogError(printStr);
            else
                UnityEngine.Debug.Log(printStr);
		}

        public static void Log(string condition)
        {
            Log(condition, "", UnityEngine.LogType.Log);
        }

        public static void Log(string condition, string stackTrace, LogType type)
        {
            string printStr = condition;
            if (type != UnityEngine.LogType.Log)
            {
                printStr += stackTrace;
                UnityEngine.Debug.Log(condition + stackTrace);
            }
            else
                UnityEngine.Debug.Log(condition);

            if (Application.platform == RuntimePlatform.WindowsWebPlayer)
            {
                Application.ExternalCall("console.log", new object[] { printStr });
            }

            string[] printStrSplit = printStr.Split('\n');
            for (int i = 0; i < printStrSplit.Length; ++i)
            {
                contentList.Add(printStrSplit[i]);
            }
            while (contentList.Count > 20)
            {
                contentList.RemoveAt(0);
            }
        }
	}
}
