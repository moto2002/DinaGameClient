using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Utils;
using Assets.Scripts.Lib.Log;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.Manager;

namespace Assets.Scripts.Lib.View
{
    public class ViewScript 
    {
        private const float RANG_OF_LAYER = -200f;
        protected Logger log = null;
        protected int layerIndex = 0;
        protected string viewName;
        protected GameObject viewGo;
        protected AssetInfo info;
        protected int Width = 0;
        protected int Height = 0;
        private ViewPosition position = ViewPosition.Center;
        protected UIAnchor uiAnchor = null;
        protected UIPanel uiPanel = null;
        protected UIAtlas uiSelfAtlas = null;
        public ViewType viewType = ViewType.Window;

		public bool IsNotLoad()
		{
			return null == viewGo;
		}

        public ViewScript(string _viewName, int Width, int Height)
        {
            this.Width = Width;
            this.Height = Height;
            log = LoggerFactory.GetInstance().GetLogger(this.GetType());
            this.viewName = _viewName;
            log.Debug("加载UI地址" + URLUtil.GetUIPath(viewName));
            AssetLoader.GetInstance().Load(URLUtil.GetUIPath(viewName), info_OnLoadComplete,AssetType.UI);
        }

        public ViewScript(string _viewName)
            : this(_viewName, 0, 0)
        {
        }
		
		public virtual void Update()
        {
			
        }

        public virtual void FixedUpdate()
        {

        }

        private void info_OnLoadComplete(AssetInfo _info)
        {
            info = _info;
            viewGo = Object.Instantiate(info.bundle.mainAsset) as GameObject;
            PreInit();
            Init();
            InitEvent();

        }

        //主要用于组件引用的初始化
        protected virtual void PreInit()
        {
            if (uiAnchor == null)
            {
                uiAnchor = viewGo.GetComponentInChildren<UIAnchor>();
                uiAnchor.gameObject.transform.parent = LayerManager.GetInstance().UILayer.transform;
                uiAnchor.gameObject.name = viewGo.name;
                Object.DestroyObject(viewGo);
                viewGo = uiAnchor.gameObject;
                viewGo.transform.localScale = new Vector3(1, 1, 1);//初始化缩放为1
            }

            if (uiPanel == null)
            {
                uiPanel = viewGo.GetComponentInChildren<UIPanel>();
            }

            UIPanel[] panelArray = viewGo.GetComponentsInChildren<UIPanel>(true);
            foreach (UIPanel panel in panelArray)
            {
                panel.viewScript = this;
            }
            
            UIManager.GetInstance().AddChild(this);
        }

        virtual protected void Init()
        { 
            
        }

        virtual public void UpdateUIOnDataChanged()
        {

        }

        virtual protected void InitEvent()
        { 
            
        }

        virtual protected void DestoryEvent()
        { 
            
        }

        public virtual void Show()
        {
            Show(false);
        }
        public virtual void Show(bool isForce)
        {
            if (viewGo != null)
            {
                viewGo.SetActive(true);
                Front();
            }  
        }

        public bool isOpen()
        {
            if(viewGo != null)
            {
                return viewGo.activeSelf;
            }
            return false;
        }

        protected void OnClickClose(GameObject go)
        {
            Hide();
        }

        public void Open(GameObject go) 
        {
            if (viewGo != null)
            {
                Show(true);
            }
        }

        public virtual void Hide()
        {
			if (viewGo != null)
            	viewGo.SetActive(false);
            //DestoryEvent();
        }

        public virtual void DestroyObject()
        {
            Object.DestroyObject(viewGo);
        }

        public void Front()
        {
            UIManager.GetInstance().Front(this);
        }

        public void SetViewPosition(ViewPosition position)
        {
            UIAnchor.Side side = UIAnchor.Side.Center;
            int widthAdd = 0;
            int heightAdd = 0;
            if (this.position != position)
            {
                switch (position)
                {
                    case ViewPosition.TopLeft:
                        side = UIAnchor.Side.TopLeft;
                        widthAdd = Width / 2;
                        heightAdd = -Height / 2;
                        break;
                    case ViewPosition.Top:
                        side = UIAnchor.Side.Top;
                        heightAdd = -Height / 2;
                        break;
                    case ViewPosition.TopRight:
                        side = UIAnchor.Side.TopRight;
                        widthAdd = -Width / 2;
                        heightAdd = -Height / 2;
                        break;
                    case ViewPosition.Left:
                        side = UIAnchor.Side.Left;
                        widthAdd = Width / 2;
                        break;
                    case ViewPosition.Center:
                        break;
                    case ViewPosition.Right:
                        side = UIAnchor.Side.Right;
                        widthAdd = -Width / 2;
                        break;
                    case ViewPosition.BottomLeft:
                        side = UIAnchor.Side.BottomLeft;
                        widthAdd = Width / 2;
                        heightAdd = Height / 2;
                        break;
                    case ViewPosition.Bottom:
                        side = UIAnchor.Side.Bottom;
                        heightAdd = Height / 2;
                        break;
                    case ViewPosition.BottomRight:
                        side = UIAnchor.Side.BottomRight;
                        widthAdd = -Width / 2;
                        heightAdd = Height / 2;
                        break;
                }

                if (uiAnchor != null)
                {
                    uiAnchor.side = side;
                }

                if (uiPanel != null)
                {
                    uiPanel.transform.localPosition += new Vector3(widthAdd, heightAdd, 0);
                }
            }
        }

        protected T FindUIObject<T>(string name) where T : Component
        {
            return NGUIUtils.FindUIObject<T>(viewGo, name);
        }

        protected T FindUIObject<T>(GameObject parent, string name) where T : Component
        {
            return NGUIUtils.FindUIObject<T>(parent, name);
        }

        protected GameObject FindGameObject(string name)
        {
            return NGUIUtils.FindGameObject(viewGo, name);
        }

        protected void GetAlats()
        {
            UISprite[] uisp = viewGo.GetComponentsInChildren<UISprite>(true);
            int count = uisp.Length;
            for (int i = 0; i < count; i++ )
            {
                UIAtlas a = uisp[i].atlas;
            }
        }

        protected UIAtlas GetSelfAlats()
        {
            if (uiSelfAtlas != null)
                return uiSelfAtlas;

            UISprite[] uisp = viewGo.GetComponentsInChildren<UISprite>(true);
            int count = uisp.Length;
            //string selfAtlasName = viewName.Replace("UI", "Atlas");

            for (int i = 0; i < count; i++)
            {
                UIAtlas atlas = uisp[i].atlas;
                //if (atlas.name == selfAtlasName)
                //{
                    uiSelfAtlas = atlas;
                    return atlas;
                //}
            }

            return null;
        }

        public void ReplacementLayer()
        {
            Vector3 pos = viewGo.transform.position;
            pos.z = RANG_OF_LAYER * layerIndex + LayerManager.GetLayer(LayerManager.GetInstance().UILayer).Rect.y;
            viewGo.transform.localPosition = pos;

            UIWidget[] widgets = viewGo.GetComponentsInChildren<UIWidget>(true);
            for (int i = 0; i < widgets.Length; ++i)
            {
                UIWidget widget = widgets[i];
                widget.layer = layerIndex;
                //widget.depth = (widget.depth%1000) + layerIndex * 1000;
            }
        }

        public void SetLayerIndex(int layerIndex)
        {
            this.layerIndex = layerIndex;
            ReplacementLayer();
        }

        public int GetLayerIndex()
        {
            return this.layerIndex;
        }
    }
}
