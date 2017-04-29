using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Scripts.Lib.Log;

namespace Assets.Scripts.Manager
{
    public class Layer
    {
        public string Name;
        public Vector2 rect = new Vector2();
        public GameObject LayerGo;
        public float NeedDepth;

        public Layer(string _name, float _needDepth)
        {
            Name = _name;
            NeedDepth = _needDepth;
            LayerManager.GetInstance().SetNextRect(this);

            LayerGo = LayerManager.GetInstance().CreateUILayer();
            LayerGo.name = Name;
            if (LayerGo.name.Equals("UILayer"))
            {
                GameObject.Destroy(LayerGo.GetComponent<UIPanel>());
            }
            LayerGo.transform.position = new UnityEngine.Vector3(LayerGo.transform.position.x, LayerGo.transform.position.y, rect.x);
        }

        //深度基数
        public int GetBaseDepth()
        {
            return 1000000 - (int)rect.x * 100;
        }

        public Vector2 Rect
        {
            set
            {
                rect = value;
                if (LayerGo != null)
                {
                    LayerGo.transform.position = new UnityEngine.Vector3(LayerGo.transform.position.x, LayerGo.transform.position.y, rect.x);
                }
            }
            get
            {
                return rect;
            }
        }
    }

    public class LayerManager
    {
        private static Logger log = LoggerFactory.GetInstance().GetLogger(typeof(LayerManager));

        public GameObject CursorLayer;         //鼠标层
        public GameObject AlertLayer;          //警告层
        public GameObject EffectLayer;         //特效层
        public GameObject UILayer;             //界面层
        public GameObject SceneNameLayer;      //名字层
        private GameObject anchorGo;

        private static Dictionary<string, Layer> LayerDict = new Dictionary<string, Layer>();

        private float nearZ = 0;
        private float farZ = 1000;
        private float currZ = 0;

        public void Init()
        {
            InitLayer(out CursorLayer, "CursorLayer", 200);
            InitLayer(out AlertLayer, "AlertLayer", 200);
            InitLayer(out EffectLayer, "EffectLayer", 200);
            InitLayer(out UILayer, "UILayer", 50000);
            InitLayer(out SceneNameLayer, "SceneNameLayer", 200); 
        }

        private void InitLayer(out GameObject go, string name, float depth)
        {
            Layer layer = new Layer(name, depth);
            LayerDict.Add(name, layer);
            go = layer.LayerGo;
        }

        public void SetNextRect(Layer layer)
        {
            float nextCurrZ = currZ + layer.NeedDepth;
            layer.Rect = new Vector2(currZ, nextCurrZ);
            currZ = nextCurrZ;
#if Debug
            if (currZ > farZ)
            {
                log.Warn("UI overflow rect , currZ: " + currZ + " farZ: " + farZ);
            }
#endif
        }

        public static Layer GetLayer(GameObject layerGo)
        {
            Layer layer;
            if (LayerDict.TryGetValue(layerGo.name, out layer))
            {
                return layer;
            }
            return null;
        }

        //得到基础深度
        public static int GetBaseDepth(GameObject layerGo)
        {
            Layer layer;
            if (LayerDict.TryGetValue(layerGo.name, out layer))
            {
                return layer.GetBaseDepth();
            }
            return 0;
        }

        //重置面板深度
        public static void ResetDepth(GameObject layerGo)
        {
            Layer layer;
            if (LayerDict.TryGetValue(layerGo.name, out layer))
            {
                UIWidget[] widgets = layerGo.GetComponentsInChildren<UIWidget>(true);
                for (int i = 0; i < widgets.Length; ++i)
                {
                    UIWidget widget = widgets[i];
                    widget.depth += layer.GetBaseDepth();
                }
            }
        }

        public GameObject CreateUILayer()
        {
            if (anchorGo == null)
            {
                GameObject uiParentGo = new GameObject("UILayer");
                Object.DontDestroyOnLoad(uiParentGo);
                uiParentGo.AddComponent<UIRoot>();
                UIRoot uiRoot = uiParentGo.GetComponent<UIRoot>();
                uiRoot.scalingStyle = UIRoot.Scaling.PixelPerfect;
                uiRoot.manualHeight = 900;
                uiRoot.minimumHeight = 900;
                uiParentGo.layer = CameraLayerManager.GetInstance().Get2DLayTag();

                anchorGo = new GameObject("anchor");
                anchorGo.AddComponent<UIAnchor>();
                anchorGo.transform.parent = uiParentGo.transform;
                anchorGo.layer = CameraLayerManager.GetInstance().Get2DLayTag();
            }

            GameObject panelGo = new GameObject("layer");
            panelGo.AddComponent<UIPanel>();
            panelGo.transform.parent = anchorGo.transform;
            panelGo.layer = CameraLayerManager.GetInstance().Get2DLayTag();

            return panelGo;
        }

        private static LayerManager mInstance;
        public static LayerManager GetInstance()
        {
            if (mInstance == null)
            {
                mInstance = new LayerManager();
            }
            return mInstance;
        }
    }
}
