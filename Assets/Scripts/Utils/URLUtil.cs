using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Utils
{
	class URLUtil
	{
        public static string url(String url)
		{
            if (Application.isWebPlayer)
            {
                return Application.dataPath + url;//TODO 
            }
            else
            {
               return "file://" + Application.dataPath + url;
            }
		}

        public static string GetRootPath()
        {
            if (Application.isWebPlayer)
            {
                return Application.dataPath;//TODO 
            }
            else
            {
                return "file://" + Application.dataPath;
            }
        }

        public static string GetResourceLibPath()
        {
            if (Application.isWebPlayer)
            {
                return Application.dataPath + "/ResourceLib/";
            }
            else
            {
                return "file://" + Application.dataPath + "/ResourceLib/";
            }
        }

        public static string GetHeroPath(string name)
        {
            return GetRootPath() + "/ResourceLib/Hero/h_" + name + ".hero";
        }
		
		public static string GetEquipModelPath(string name)
		{
			return GetRootPath() + "/ResourceLib/Hero/" + name + ".equip";
		}

        public static string GetUIPath(string name)
        {
            return GetRootPath() + "/ResourceLib/UI/" + name + ".ui";
        }

        public static string GetScenePath(uint mapId)
        {
			return GetRootPath() + "/ResourceLib/Scene/" + mapId + "/" + mapId + ".sceneall";
        }

        public static string GetIniFilePath(string name)
        {
            return GetRootPath() + "/ResourceLib/Tab/" + name + ".ini";
        }

        public static string GetTabFilePath(string name)
        {
            return GetRootPath() + "/ResourceLib/Tab/" + name + ".tab";
        }

        public static string GetIconPath(string name)
        {
            return GetRootPath() + "/ResourceLib/Icon/" + name + ".png";
        }

        public static string GetPrefabPath(string name)
        {
            return GetRootPath() + "/ResourceLib/Prefab/" + name + ".prefab";
        }

        public static string GetAtlasPath(string name)
        {
            return GetRootPath() + "/ResourceLib/Atlas/" + name + ".atlas";
        }

        public static string GetEffectPath(string name)
        {
            return GetRootPath() + "/ResourceLib/Effect/" + name + ".res";
        }
	}
}
