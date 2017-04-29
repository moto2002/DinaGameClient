using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Lib.Log;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.Utils;
using System.Text;

namespace Assets.Scripts.Lib.Resource
{
    public class KIniFile
    {
        public delegate void LoadDataCompleteDelegate();

        private Dictionary<string, Dictionary<string, string>> m_iniContent = null;
        private string m_url = null;
        private static Logger log = LoggerFactory.GetInstance().GetLogger(typeof(KIniFile));
        private event LoadDataCompleteDelegate OnDataComplete;
        private event LoadDataCompleteDelegate OnDataComplete2;

        public KIniFile(string url, LoadDataCompleteDelegate OnDataComplete, LoadDataCompleteDelegate OnDataComplete2)
        {
            this.m_url = URLUtil.GetIniFilePath(url);
            this.OnDataComplete = OnDataComplete;
            this.OnDataComplete2 = OnDataComplete2;
            AssetLoader.GetInstance().Load(m_url, LoadComplete, AssetType.BINARY);
        }

        public KIniFile(string url)
            : this(url, null, null)
        {
        }

        public KIniFile(string url, LoadDataCompleteDelegate OnDataComplete)
            : this(url, OnDataComplete, null)
        {
        }

        private void LoadComplete(AssetInfo info)
        {
            parse(info.binary);
            if (OnDataComplete != null)
            {
                OnDataComplete.Invoke();
            }
            if (OnDataComplete2 != null)
            {
                OnDataComplete2.Invoke();
            }
        }

        private bool parse(byte[] abyFileDate)
        {
            string fileDate = null;
            if (abyFileDate[0] == (char)0xEF && abyFileDate[1] == (char)0xBB && abyFileDate[2] == (char)0xBF)
            {
                fileDate = Encoding.UTF8.GetString(abyFileDate, 3, abyFileDate.Length - 3);
            }
            else
                fileDate = Encoding.UTF8.GetString(abyFileDate);

            string[] fileLines = fileDate.Split('\n');
            if (fileLines.Length < 1)
            {
                Debug.Log("KIniFile error, fileLines.Length=" + fileLines.Length);
                return false;
            }

            string sectionName = null;
            Dictionary<string, string> sectionContent = null;
            for (int i = 0; i < fileLines.Length; ++i)
            {
                string fileLine = fileLines[i];
                fileLine = fileLine.Trim();
                if (fileLine.Length == 0)
                    continue;
                if (fileLine[0] == '#')
                    continue;

                if (fileLine[fileLine.Length - 1] == '\r')
                    fileLine = fileLine.Substring(0, fileLine.Length - 1);
                fileLine.Trim();
                if (fileLine.Equals(""))
                    continue;

                if (fileLine[0] == '[' && fileLine[fileLine.Length - 1] == ']')
                {
                    if (m_iniContent == null)
                        m_iniContent = new Dictionary<string, Dictionary<string, string>>();
                    sectionName = fileLine.Substring(1, fileLine.Length - 2);
                    sectionContent = new Dictionary<string, string>();
                    m_iniContent.Add(sectionName, sectionContent);
                    continue;
                }
                int fileLineEqualIndex = fileLine.IndexOf('=');
                if (fileLineEqualIndex <= 0)
                {
                    Debug.Log("KIniFile error, fileLine is not contain =:" + fileLine);
                    return false;
                }
                string key = fileLine.Substring(0, fileLineEqualIndex);
                string value = fileLine.Substring(fileLineEqualIndex + 1);
                sectionContent.Add(key, value);
            }

            return true;
        }

        public string GetString(string sectionName, string key)
        {
            if (m_iniContent == null)
                return null;

            Dictionary<string, string> sectionContent = m_iniContent[sectionName];
            if (sectionContent == null)
                return null;

            return sectionContent[key];
        }

        public int GetInt(string sectionName, string key)
        {
            return System.Convert.ToInt32(GetString(sectionName, key));
        }

        public float GetFloat(string sectionName, string key)
        {
            return System.Convert.ToSingle(GetString(sectionName, key));
        }
    }
}
