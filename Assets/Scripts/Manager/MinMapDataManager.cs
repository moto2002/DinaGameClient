using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Text;

namespace Assets.Scripts.Manager
{
	public class MinMapInfor
	{
		public string name = "";
		public int minX = 0;
		public int maxX = 0;
		public int minZ = 0;
		public int maxZ = 0;
	}
	public class MinMapDataManager  {
		public static readonly MinMapDataManager instance = new MinMapDataManager(); 
		public Dictionary<string,MinMapInfor> minMaps = new Dictionary<string, MinMapInfor>();
		public void Load(string xmlPath)
		{
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load(xmlPath);
	        XmlNode root = xmlDoc.SelectSingleNode("min_map");
			XmlNodeList accsList = root.SelectNodes("map");
			minMaps.Clear();
	        foreach (XmlNode axn in accsList)
			{
				MinMapInfor infor = new MinMapInfor();
				XmlElement axe = (XmlElement)axn;
				infor.name = axe.GetAttribute("name");
				infor.minX = System.Convert.ToInt32(axe.GetAttribute("min_x"));
				infor.maxX = System.Convert.ToInt32(axe.GetAttribute("max_x"));
				infor.minZ = System.Convert.ToInt32(axe.GetAttribute("min_z"));
				infor.maxZ = System.Convert.ToInt32(axe.GetAttribute("max_z"));
				minMaps[infor.name] = infor;
			}
		}
		public void LoadXml(string XmlText)
		{
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.LoadXml(XmlText);
	        XmlNode root = xmlDoc.SelectSingleNode("min_map");
			XmlNodeList accsList = root.SelectNodes("map");
			minMaps.Clear();
	        foreach (XmlNode axn in accsList)
			{
				MinMapInfor infor = new MinMapInfor();
				XmlElement axe = (XmlElement)axn;
				infor.name = axe.GetAttribute("name");
				infor.minX = System.Convert.ToInt32(axe.GetAttribute("min_x"));
				infor.maxX = System.Convert.ToInt32(axe.GetAttribute("max_x"));
				infor.minZ = System.Convert.ToInt32(axe.GetAttribute("min_z"));
				infor.maxZ = System.Convert.ToInt32(axe.GetAttribute("max_z"));
				minMaps[infor.name] = infor;
			}
		}
		public void Save(string xmlPath)
		{
			XmlDocument doc = new XmlDocument(); 
			XmlElement root = doc.CreateElement("min_map"); 
			doc.AppendChild(root); 
			foreach ( string _name in minMaps.Keys )
			{
				MinMapInfor infor = minMaps[_name];
				XmlElement _map = doc.CreateElement("map");
				root.AppendChild(_map);
				_map.SetAttribute("name",infor.name);
				_map.SetAttribute("min_x",""+infor.minX);
				_map.SetAttribute("max_x",""+infor.maxX);
				_map.SetAttribute("min_z",""+infor.minZ);
				_map.SetAttribute("max_z",""+infor.maxZ);
			}
			doc.Save(xmlPath);
		}
		public MinMapInfor GetMinMap(uint mapId)
		{
			string key = ""+mapId;
			return minMaps[key];
		}
	}
}
