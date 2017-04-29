using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Assets.Scripts.Manager
{
	public class LabelObjects : MonoBehaviour
	{
		public readonly float ENABLE_TIME = 5f;
		public string text;
		public GameObject label = null;
		public UILabel uilabel = null;
		public Vector3 Position
		{
			get{return position;}
			set
			{
				Vector3 v0 = Camera.main.WorldToViewportPoint(value);
				if(v0.z<0)
				{
					position = new Vector3(-1000,-1000,-1000);
				}
				else
				{
					position = Camera.main.WorldToScreenPoint(value);
				}
			}
			
		}
		private Vector3 position;
		public float lastUpdateTime = Time.realtimeSinceStartup;
		public string parent_name;
		void Start() {
	    }
		
		void Update () {
			
			if(position.x >= -50 && position.x <= Camera.main.pixelWidth + 50 
				&&
				position.y >= -50 && position.y <= Camera.main.pixelHeight + 50 
				)
			{
				if(label == null)
				{
					NameLabelsManager.instance.SetLabel(this);	
					uilabel = label.GetComponent<UILabel>();
					label.name = "_"+parent_name;
				}
				//Debug.Log("show "+parent_name);
				if(uilabel.text.CompareTo(text)!=0)
					uilabel.text = text;
				float delta = Camera.main.pixelHeight/2;
				float delta0 = Camera.main.pixelWidth/2;
				Vector3 p1 = new Vector3((position.x-delta0)/ delta ,
					(position.y  - delta)/ delta ,
					-20f);
				
				label.transform.position = p1;
			}
			else
			{
				if(null != label)
				{
					NameLabelsManager.instance.ReleaseLabel(label);
					label = null;
					uilabel = null;
					return;
				}
			}	
			
		}
		void OnDestroy() {
	        if(null != label)
			{
				NameLabelsManager.instance.ReleaseLabel(label);
			}
	    }
	}
	public class NameLabelsManager  {
		public static readonly NameLabelsManager instance = new NameLabelsManager();
		private GameObject root ;
		private GameObject panel;
		private List<GameObject> layoutPools = new List<GameObject>();
		private int len = 0;
		
		
		public GameObject GetRoot()
		{
			if (null==root)
				root = GameObject.Find("SceneNameLayer");
			return root;
		}
		private NameLabelsManager(){
		}
		public void SetLabel(LabelObjects o)
		{
			if(len>0)
			{
				o.label = layoutPools[0];
				layoutPools.RemoveAt(0);
				len = layoutPools.Count;
			}
			else
			{
				GameObject nameObject = new GameObject();
				nameObject.transform.parent = LayerManager.GetInstance().SceneNameLayer.transform;
				//nameObject.transform.parent = panel.transform;
	            nameObject.name = "DynNameLabel";
	            UILabel nameLabel = nameObject.AddComponent<UILabel>();
	            nameLabel.font = FontManager.GetInstance().Font;
				o.label = nameObject;
			}
			UILabel uilabel = o.label.GetComponent<UILabel>();
			o.label.transform.localScale = new Vector3(uilabel.font.size,uilabel.font.size,uilabel.font.size);
			o.label.SetActive(true);
		}
		
		public LabelObjects RequitLabel()
		{
			GameObject o = new GameObject();
			LabelObjects obj = o.AddComponent<LabelObjects>();
			o.layer = LayerMask.NameToLayer("2D");
			return obj;
		}
		public void ReleaseLabel(GameObject label)
		{
			label.SetActive(false);
			layoutPools.Add(label);
			len = layoutPools.Count;
		}
	}
}

