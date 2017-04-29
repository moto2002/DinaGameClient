using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.Manager;
using Assets.Scripts.Utils;
using Assets.Scripts.Define;
using Assets.Scripts.Data;


namespace Assets.Scripts.Logic.Scene.SceneObject.Compont
{
    class HeadePanelComponent : BaseComponent
    {
        protected GameObject headPanelGameObject;
        UILabel[] labelObjs = new UILabel[2];
		UISprite xue;
		UISprite xueCao;
		float cur_hp=0f;
		
		string tName;
		string tTitle;
        public override string GetName()
        {
            return GetType().Name;
        }

        public override void OnAttachToEntity(SceneEntity ety)
        {
            BaseInit(ety);

            // 注册事件响应函数
            Regist(ControllerCommand.CHANGE_NAME, OnChangeName);
            Regist(ControllerCommand.CHANGE_TITLE, OnChangeTitle);
            Regist(ControllerCommand.LOAD_NAME_LABEL, OnLoadNameLabel);
        }

        public override void OnDetachFromEntity(SceneEntity ety)
        {
            GameObject.Destroy(headPanelGameObject);
            base.OnDetachFromEntity(ety);
        }

        public object OnLoadNameLabel(params object[] objs)
        {
            AssetLoader.GetInstance().Load(URLUtil.GetResourceLibPath() + "UI/HeadPanel.prefab", NameLabelGameObject_LoadComplete, AssetType.BUNDLER);
            return null;
        }

		
		public object OnChangeName(params object[] objs)
        {
            string name = objs[0] as string;
            Owner.Name = name;
            refreadPanel();
            return null;
        }

        public object OnChangeTitle(params object[] objs)
        {
            string title = objs[0] as string;
            Owner.Title = title;
            refreadPanel();
            return null;
        }

        public GameObject GetHeadPanel()
        {
            return headPanelGameObject;
        }

        private void NameLabelGameObject_LoadComplete(AssetInfo info)
        {
            InstantiateHeadPanel(info);
        }
		
		private void refreadPanel()
		{
			foreach( UILabel lab in labelObjs )
			{
				if (null == lab)
					return;
			}
			int c = 1;

            if (Owner.property.sceneObjType == KSceneObjectType.sotDoodad)
            {
                if (Owner.property.doodadObjType == KDoodadType.dddCollect)
                {
                    labelObjs[0].text = "<88FF88>" + Owner.Name;
                }
				else
				{
					labelObjs[0].text = "<FFFFFF>" + Owner.Name;
				}
            }
			else if (Owner.HeroType == KHeroObjectType.hotNpc)
			{
				if (Owner.Title.Length > 0)
	            {
	                labelObjs[c].text = "<88FF88><" + Owner.Title + ">";
					c--;
	            }
				labelObjs[c].text = "<88FF88>"+Owner.Name;
			}
			else 
			{
				if (Owner.HeroType == KHeroObjectType.hotMonster)
				{
					if (Owner.Title.Length > 0)
		            {
		                labelObjs[c].text = "<FFFF88><" + Owner.Title + ">";
						c--;
		            }
					labelObjs[c].text = "<FFFF88>"+Owner.Name;
					KHeroSetting setting = KConfigFileManager.GetInstance().GetHeroSetting(Owner.TabID);
					if (null != setting)
					{
						labelObjs[c].text += ".Lv"+setting.Level.ToString();
					}
				}
				else
				{
					if (Owner.Title.Length > 0)
		            {
		                labelObjs[c].text = "<8888FF><" + Owner.Title + ">";
						c--;
		            }
					labelObjs[c].text = "<8888FF>"+Owner.Name;	
				}
			}
		}

        private void InstantiateHeadPanel(AssetInfo info)
        {
			try
			{
				headPanelGameObject = GameObject.Instantiate(info.bundle.mainAsset) as GameObject;
				headPanelGameObject.SetActive(false);
	            headPanelGameObject.transform.parent = NameLabelsManager.instance.GetRoot().transform.transform;
	            headPanelGameObject.transform.localScale = Vector3.one;
	            headPanelGameObject.transform.rotation = Quaternion.identity;
	
	            labelObjs[0] = headPanelGameObject.transform.FindChild("Name").GetComponent<UILabel>();
	            labelObjs[0].font = FontManager.GetInstance().Font;
	            labelObjs[0].transform.localScale = Vector3.one;
	            labelObjs[0].transform.localPosition = new Vector3(labelObjs[0].transform.localPosition.x, labelObjs[0].transform.localPosition.y, 50f);
	           	labelObjs[0].text = "";
	            labelObjs[1] = headPanelGameObject.transform.FindChild("Titel").GetComponent<UILabel>();
	            labelObjs[1].font = labelObjs[0].font;
	            labelObjs[1].transform.localScale = Vector3.one;
	            labelObjs[1].transform.localPosition = new Vector3(labelObjs[1].transform.localPosition.x, labelObjs[1].transform.localPosition.y, 50f);
	            labelObjs[1].text = ""; 
	
				xue = headPanelGameObject.transform.FindChild("xue").GetComponent<UISprite>();
				xueCao = headPanelGameObject.transform.FindChild("xueCao").GetComponent<UISprite>();
	            
				refreadPanel();
			}
			catch(System.Exception e)
			{
				Debug.Log(e.ToString());
				//游戏对象已经被释放.
				return;
			}
            
			
			
        }

        public override void DoUpdate()
        {
            if (null != headPanelGameObject)
            {
				if (Owner.property.isDeadTemp)
					Owner.property.fightHp = 0;
				if(cur_hp<Owner.property.fightHp)
					cur_hp = Owner.property.fightHp;
				else if(cur_hp > Owner.property.fightHp)
				{
					float _delta = cur_hp - Owner.property.fightHp;
					_delta = _delta * 1000f;
					cur_hp = Mathf.MoveTowards(cur_hp,Owner.property.fightHp,_delta);
				}

                if (Owner.property.sceneObjType == KSceneObjectType.sotDoodad)
                {
                    if (Owner.property.doodadObjType == KDoodadType.dddCollect)
                    {
                        xue.gameObject.SetActive(false);
                        xueCao.gameObject.SetActive(false);
                        labelObjs[1].gameObject.SetActive(false);
                    }
					else
					{
						xue.gameObject.SetActive(false);
                        xueCao.gameObject.SetActive(false);
                        labelObjs[1].gameObject.SetActive(false);
					}
                }
				else if (Owner.HeroType == KHeroObjectType.hotNpc)
				{
					xue.gameObject.SetActive(false);
					xueCao.gameObject.SetActive(false);
				}
				else 
				{
					float t = cur_hp  / Owner.property.maxHp;
					t = Mathf.Min(1f,t);
					xue.width = (int)(64f * t);
					if (Owner.HeroType == KHeroObjectType.hotMonster)
					{
						bool b = (Owner.property.fightHp < Owner.property.maxHp || Owner == MouseClickScene.moveCursourSceneEntity) && ! Owner.property.isDeadTemp;
						if(!b)
						{
							headPanelGameObject.SetActive(false);
							return;
						}
						xue.gameObject.SetActive(cur_hp>0);
						labelObjs[0].enabled = (Owner == MouseClickScene.moveCursourSceneEntity);
						labelObjs[1].enabled = (Owner == MouseClickScene.moveCursourSceneEntity);
					}
					else
					{
						xue.gameObject.SetActive(cur_hp>0);
					}
				}
				
                Vector3 _value = Owner.transform.position;
                BoxCollider control = Owner.GetComponent<BoxCollider>();
                if (null == control)
                {
                    _value = _value + new Vector3(0f, 0f, 0f);
                }
				else
				{
					_value = _value + new Vector3(0f, control.size.y, 0f);
				}
                Vector3 v0 = Camera.main.WorldToViewportPoint(_value);
                if (v0.z < 0)
                {
					if (headPanelGameObject.activeSelf)
                    	headPanelGameObject.SetActive(false);
                }
                else
                {
                    Vector3 p1 = Camera.main.WorldToScreenPoint(_value);
                    if (p1.x >= -50 && p1.x <= Camera.main.pixelWidth + 50
                        &&
                        p1.y >= -50 && p1.y <= Camera.main.pixelHeight + 50
                        )
                    {
                        float delta = Camera.main.pixelHeight / 2;
                        float delta0 = Camera.main.pixelWidth / 2;
                        headPanelGameObject.transform.position = new Vector3((p1.x - delta0) / delta,
                            (p1.y - delta) / delta + 0.08f,
                            0);
						if (!headPanelGameObject.activeSelf)
                        	headPanelGameObject.SetActive(true);
                    }
                    else
                    {
						if (headPanelGameObject.activeSelf)
                        	headPanelGameObject.SetActive(false);
                    }
                }
            }
        }
    }
}
