using UnityEngine;
using System.Collections;
using System.IO;
using System;

namespace Assets.Scripts.Lib.Resource
{
    public class KTextFile
    {
        public static string LoadFile(string filename)
        {
            StreamReader reader = null;
            try
            {
                reader = File.OpenText(filename);
            }
            catch (Exception e)
            {
                Debug.Log("加载Tab文件出错=" + e.ToString());
                return null;
            }

            string fileDate = reader.ReadToEnd();

            reader.Close();
            reader.Dispose();

            return fileDate;
        }
    }
}
