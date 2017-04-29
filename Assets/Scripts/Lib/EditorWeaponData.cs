using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Text;

public class EditorWeaponData  {
	public Vector3		localScale;
	public Vector3		localPosition;
	public Quaternion	localRot;
	
	
	public Vector3		localScaleB;
	public Vector3		localPositionB;
	public Quaternion	localRotB;
	
	public void LoadXml(string XmlText)
	{
		XmlDocument xmlDoc = new XmlDocument();
		xmlDoc.LoadXml(XmlText);
        XmlNode root = xmlDoc.SelectSingleNode("weapons");
		
		{
			XmlNodeList accsList = root.SelectNodes("rh");
	        foreach (XmlNode axn in accsList)
			{
				
				XmlElement axe = (XmlElement)axn;
				localPosition = new Vector3(
					System.Convert.ToSingle(axe.GetAttribute("pos_x")),
					System.Convert.ToSingle(axe.GetAttribute("pos_y")),
					System.Convert.ToSingle(axe.GetAttribute("pos_z"))
					);
				localScale = new Vector3(
					System.Convert.ToSingle(axe.GetAttribute("localRot_x")),
					System.Convert.ToSingle(axe.GetAttribute("localRot_y")),
					System.Convert.ToSingle(axe.GetAttribute("localRot_z"))
					);
				localRot = new Quaternion(
					System.Convert.ToSingle(axe.GetAttribute("localRot_x")),
					System.Convert.ToSingle(axe.GetAttribute("localRot_y")),
					System.Convert.ToSingle(axe.GetAttribute("localRot_z")),
					System.Convert.ToSingle(axe.GetAttribute("localRot_w"))
					);
			}
		}
		
		
		{
			XmlNodeList accsList = root.SelectNodes("b");
	        foreach (XmlNode axn in accsList)
			{
				
				XmlElement axe = (XmlElement)axn;
				localPositionB = new Vector3(
					System.Convert.ToSingle(axe.GetAttribute("pos_x")),
					System.Convert.ToSingle(axe.GetAttribute("pos_y")),
					System.Convert.ToSingle(axe.GetAttribute("pos_z"))
					);
				localScaleB = new Vector3(
					System.Convert.ToSingle(axe.GetAttribute("localRot_x")),
					System.Convert.ToSingle(axe.GetAttribute("localRot_y")),
					System.Convert.ToSingle(axe.GetAttribute("localRot_z"))
					);
				localRotB = new Quaternion(
					System.Convert.ToSingle(axe.GetAttribute("localRot_x")),
					System.Convert.ToSingle(axe.GetAttribute("localRot_y")),
					System.Convert.ToSingle(axe.GetAttribute("localRot_z")),
					System.Convert.ToSingle(axe.GetAttribute("localRot_w"))
					);
			}
		}
		
	}
	public string Save()
	{
		XmlDocument doc = new XmlDocument(); 
		XmlElement root = doc.CreateElement("weapons"); 
		doc.AppendChild(root); 
		
		{
			XmlElement _map = doc.CreateElement("rh");
			root.AppendChild(_map);
			_map.SetAttribute("pos_x",""+localPosition.x);
			_map.SetAttribute("pos_y",""+localPosition.y);
			_map.SetAttribute("pos_z",""+localPosition.z);
			_map.SetAttribute("scale_x",""+localScale.x);
			_map.SetAttribute("scale_y",""+localScale.y);
			_map.SetAttribute("scale_z",""+localScale.z);
			_map.SetAttribute("localRot_x",""+localRot.x);
			_map.SetAttribute("localRot_y",""+localRot.y);
			_map.SetAttribute("localRot_z",""+localRot.z);
			_map.SetAttribute("localRot_w",""+localRot.w);
		}
		
		{
			XmlElement _map = doc.CreateElement("b");
			root.AppendChild(_map);
			_map.SetAttribute("pox_x",""+localPositionB.x);
			_map.SetAttribute("pos_y",""+localPositionB.y);
			_map.SetAttribute("pos_z",""+localPositionB.z);
			_map.SetAttribute("scale_x",""+localScaleB.x);
			_map.SetAttribute("scale_y",""+localScaleB.y);
			_map.SetAttribute("scale_z",""+localScaleB.z);
			_map.SetAttribute("localRot_x",""+localRotB.x);
			_map.SetAttribute("localRot_y",""+localRotB.y);
			_map.SetAttribute("localRot_z",""+localRotB.z);
			_map.SetAttribute("localRot_w",""+localRotB.w);
		}
		return doc.OuterXml;
	}	
		
}
