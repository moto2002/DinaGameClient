using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Assets.Scripts.Lib.Log;
using System.Text;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.Utils;

namespace Assets.Scripts.Lib.Resource
{
    public abstract class AKTabFileObject
    {
        public abstract string getKey();

        public virtual void onComplete()
        {
        }

        public virtual void onAllComplete()
        {
        }
		
		public override string ToString()
		{
			string text = "";
			System.Type t = this.GetType();
			foreach (System.Reflection.PropertyInfo pi in t.GetProperties())
			{
				object val = pi.GetValue(this,null);
			  	text += pi.Name + "="+val+";";
				/*if (val.GetType() == typeof(int)){}*/
			}
			return text;
		}
    }

    public class KTabFile<T>
    {
        public delegate void LoadDataCompleteDelegate();

        public int m_nWidth = 0;
        public int m_nHeight = 0;

        private string m_url = null;
        private static Assembly ms_executingAssembly = null;

        private static Logger log = LoggerFactory.GetInstance().GetLogger(typeof(KTabFile<T>));
        private Dictionary<string, T> m_data = new Dictionary<string, T>();
        private event LoadDataCompleteDelegate OnDataComplete;
        private event LoadDataCompleteDelegate OnDataComplete2;
		public string tabUrl = "";

        public KTabFile(string url, LoadDataCompleteDelegate OnDataComplete, LoadDataCompleteDelegate OnDataComplete2)
        {
            this.m_url = URLUtil.GetTabFilePath(url);
            this.OnDataComplete = OnDataComplete;
            this.OnDataComplete2 = OnDataComplete2;
            AssetLoader.GetInstance().Load(m_url, LoadComplete, AssetType.BINARY);
        }

        public KTabFile(string url)
            : this(url, null, null)
        {
        }

        public KTabFile(string url, LoadDataCompleteDelegate OnDataComplete)
            : this(url, OnDataComplete, null)
        {
        }

        private void LoadComplete(AssetInfo info)
        {
			tabUrl = info.url;
            /*try
            {*/
                parse(info.binary);
            /*}
            catch (Exception ex)
            {
                Debug.Log("KTabFile error " + m_url + "\n" + ex.Message + "\n" + ex.StackTrace);
            }*/
            if (OnDataComplete != null)
            {
                OnDataComplete.Invoke();
            }
            if (OnDataComplete2 != null)
            {
                OnDataComplete2.Invoke();
            }
        }

        public string URL
        {
            get
            {
                return m_url;
            }
        }

        public int getWidth()
        {
            return m_nWidth;
        }

        public int getHeight()
        {
            return m_nHeight;
        }

        public T getData(string key)
        {
            T data;
            m_data.TryGetValue(key, out data);
            return data;
        }

        public Dictionary<string, T> getAllData()
        {
            return m_data;
        }

        ///////////////////////////////////////////////////

        private void AddData(string key, T value)
        {
			if(m_data.ContainsKey(key))
				throw new Exception(key+ "键值重复");
            m_data.Add(key, value);
        }

        private void onAllComplete()
        {
            foreach (KeyValuePair<string, T> item in m_data)
            {
                System.Type tabLineType = item.Value.GetType();
                tabLineType.InvokeMember(
                    "onAllComplete",
                    BindingFlags.Default | BindingFlags.InvokeMethod,
                    null,
                    item.Value,
                    null
                );
            }
        }

        private bool parse(byte[] abyFileDate)
        {
            String tabLineTypeName = typeof(T).FullName;
            if (abyFileDate == null || tabLineTypeName == null || abyFileDate.Length == 0)
                return false;

            string fileDate = null;
            if (abyFileDate[0] == (char)0xEF && abyFileDate[1] == (char)0xBB && abyFileDate[2] == (char)0xBF)
            {
                fileDate = Encoding.UTF8.GetString(abyFileDate, 3, abyFileDate.Length - 3);
            }
            else
                fileDate = Encoding.UTF8.GetString(abyFileDate);

            string[] fileLines = fileDate.Split('\n');
            if (fileLines.Length < 1)
                return false;
            int nContentLineCount = fileLines.Length - 1;
            if ((fileLines.Length > 0) && fileLines[fileLines.Length - 1].Equals(""))
                --nContentLineCount;

            string titleLine = fileLines[0];
            if (titleLine[titleLine.Length - 1] == '\r')
                titleLine = titleLine.Substring(0, titleLine.Length - 1);
            string[] titles = titleLine.Split('\t');
            if (titles.Length == 0)
                return false;
            m_nWidth = titles.Length;

            while (--m_nWidth > 0)
            {
                if (titles[m_nWidth] != null && !titles[m_nWidth].Equals(""))
                    break;
            }
            ++m_nWidth;

            if (ms_executingAssembly == null)
                ms_executingAssembly = Assembly.GetExecutingAssembly();

            Type tabType = typeof(KTabFile<T>);
            System.Type tabLineType = ms_executingAssembly.GetType(tabLineTypeName);

            FieldInfo[] fieldInfoTitles = new FieldInfo[m_nWidth];
            for (int j = 0; j < m_nWidth; ++j)
                fieldInfoTitles[j] = tabLineType.GetField(titles[j]);

            for (int i = 1; i < nContentLineCount + 1; ++i)
            {
                string fileLine = fileLines[i];
                if (fileLine.Length == 0)
                    continue;
                if (fileLine[0] == '#')
                    continue;
                ++this.m_nHeight;

                if (fileLine[fileLine.Length - 1] == '\r')
                    fileLine = fileLine.Substring(0, fileLine.Length - 1);

                string[] dataLineSplit = fileLine.Split('\t');
                if (dataLineSplit.Length == 0)
                    return false;
                if (dataLineSplit.Length > titles.Length)
                {
                    Debug.Log("KTableFile " + fileDate + " error, dataLineSplit.Length=" + dataLineSplit.Length + " titles.Length=" + titles.Length);
                    return false;
                }

                object tabLineObject = ms_executingAssembly.CreateInstance(tabLineTypeName);
                for (int j = 0; j < dataLineSplit.Length && j < m_nWidth; ++j)
                {
                    if (dataLineSplit[j] == null)
                        continue;
                    FieldInfo fieldInfoTitle = fieldInfoTitles[j];
                    if (fieldInfoTitle == null)
                         continue;
					try
					{
						if (fieldInfoTitle.FieldType == typeof(int))
	                        fieldInfoTitle.SetValue(tabLineObject, Convert.ToInt32(dataLineSplit[j]));
	                    else if (fieldInfoTitle.FieldType == typeof(uint))
	                        fieldInfoTitle.SetValue(tabLineObject, Convert.ToUInt32(dataLineSplit[j]));
	                    else if (fieldInfoTitle.FieldType == typeof(byte))
	                        fieldInfoTitle.SetValue(tabLineObject, Convert.ToByte(dataLineSplit[j]));
	                    else if (fieldInfoTitle.FieldType == typeof(float))
						{
							fieldInfoTitle.SetValue(tabLineObject, Convert.ToSingle(dataLineSplit[j]));
						}
	                       
	                    else if (fieldInfoTitle.FieldType == typeof(string))
	                    {
	                        string sFieldValueUtf8 = dataLineSplit[j];
	                        fieldInfoTitle.SetValue(tabLineObject, sFieldValueUtf8);
	                    }
	                    else if (fieldInfoTitle.FieldType == typeof(long))
	                        fieldInfoTitle.SetValue(tabLineObject, Convert.ToInt64(dataLineSplit[j]));
	                    else if (fieldInfoTitle.FieldType == typeof(ulong))
	                        fieldInfoTitle.SetValue(tabLineObject, Convert.ToUInt64(dataLineSplit[j]));
	                    else if (fieldInfoTitle.FieldType == typeof(bool))
	                        fieldInfoTitle.SetValue(tabLineObject, Convert.ToBoolean(dataLineSplit[j]));
	                    else if (fieldInfoTitle.FieldType.IsSubclassOf(typeof(Enum)))
	                    {
	                        if (Enum.IsDefined(fieldInfoTitle.FieldType, dataLineSplit[j]))
	                        {
	                            fieldInfoTitle.SetValue(tabLineObject, Enum.Parse(fieldInfoTitle.FieldType, dataLineSplit[j], true));
	                        }
	                    }
					}
					catch(System.FormatException e)
					{
						throw new Exception(tabUrl+ " at "+fieldInfoTitle.Name +" = "+dataLineSplit[j] + "("+fieldInfoTitle.FieldType+")\n"+e.ToString() );	
					}
                    
                }
                string sKey = (string)tabLineType.InvokeMember(
                    "getKey",
                    BindingFlags.Default | BindingFlags.InvokeMethod,
                    null,
                    tabLineObject,
                    null
                );

                object[] args = new object[] { sKey, tabLineObject };
                tabType.InvokeMember(
                    "AddData",
                    BindingFlags.Default | BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.NonPublic,
                    null,
                    this,
                    args
                );

                tabLineType.InvokeMember(
                    "onComplete",
                    BindingFlags.Default | BindingFlags.InvokeMethod,
                    null,
                    tabLineObject,
                    null
                );
            }

            tabType.InvokeMember(
                "onAllComplete",
                BindingFlags.Default | BindingFlags.InvokeMethod | BindingFlags.NonPublic,
                null,
                this,
                null
            );
            return true;
        }
    }
}
