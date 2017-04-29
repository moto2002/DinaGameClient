using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Utils;
using Assets.Scripts.Lib.Log;
using Assets.Scripts.View.Drag;

namespace Assets.Scripts.Manager
{
    enum CursorType
    {
        ctNormal,
        ctNormalClick,
        ctChat,
        ctAttack,
        ctForbid,
        ctFriend
    }
	public enum DrawDataType
	{
		NONE,
		ITEM,
		SKILL,
	}

    class CursorManager : MonoBehaviour
    {
		public object mDraggingData = null;
		
        private static Logger log = LoggerFactory.GetInstance().GetLogger(typeof(CursorManager));

        private Dictionary<CursorType, string> cursorNameDict = new Dictionary<CursorType, string>();

        public string NormalCursorName = "CursorNormal";

        public string ResourceAtlasName = "CommonAtlas";

        private UISprite cursor;

        private Transform mTrans;

        private Camera uiCamera;

        private CursorType currentCursor = CursorType.ctForbid;
		
		DrawDataType drawType = DrawDataType.NONE;
		
		GameObject obj;
        void Awake()
        {

            GameObject cursorLayer = LayerManager.GetInstance().CursorLayer;
            GameObject go = new GameObject("cursor");
			GameObject.DontDestroyOnLoad(go);
            go.hideFlags = HideFlags.DontSave;
            go.transform.parent = cursorLayer.transform;
			obj = go;
            cursor = go.AddComponent<UISprite>();
            cursor.layer = int.MaxValue;
            cursor.atlas = UIAtlasManager.GetInstance().GetUIAtlas(ResourceAtlasName);
            cursor.pivot = UIWidget.Pivot.TopLeft;

            ObjectUtil.SetLayerWithAllChildren(cursor.gameObject, "2D");
            mTrans = go.transform;
            RegisterCursor();
            SetCursor(CursorType.ctNormal);

            LayerManager.ResetDepth(cursorLayer);

            uiCamera = ViewCameraManager.GetInstance().UICamera;
        }

        private void RegisterCursor()
        {
            cursorNameDict.Add(CursorType.ctNormal, NormalCursorName);
            cursorNameDict.Add(CursorType.ctNormalClick, "CursorNormalClick");
            cursorNameDict.Add(CursorType.ctChat, "CursorChat");
            cursorNameDict.Add(CursorType.ctAttack, "CursorAttack");
            cursorNameDict.Add(CursorType.ctForbid, "CursorForbid");
            cursorNameDict.Add(CursorType.ctFriend, "CursorFriend");
        }

        public CursorType CurrentCursor
        {
            get
            {
                return currentCursor;
            }
        }

        public void SetCursor(CursorType cursorType)
        {
            if (DragItem.isDragging)
                return;
            if (currentCursor == cursorType)
                return;

            string cursorName = null;
            
            if (cursorNameDict.TryGetValue(cursorType, out cursorName))
            {
                if (UIAtlasManager.GetInstance().HasSprite(ResourceAtlasName, cursorName))
                {
                    currentCursor = cursorType;
                    cursor.spriteName = cursorNameDict[cursorType];
                }
            }
            else
            {
                cursorType = CursorType.ctNormal;
                cursor.spriteName = cursorNameDict[CursorType.ctNormal];
            }
			cursor.layer = 1000000;
            cursor.MakePixelPerfect();
			
			
        }
		public object getDraggingData()
		{
			return mDraggingData;
		}
		public void SetDragingCur(UIAtlas atlas, string spriteName ,DrawDataType type,object draggingData = null)
        {
			mDraggingData = draggingData;
            cursor.atlas = atlas;
            cursor.spriteName = spriteName;
			drawType = type;
        }
        public void SetCursor(UIAtlas atlas, string spriteName)
        {
			mDraggingData = null;
            cursor.atlas = atlas;
            cursor.spriteName = spriteName;
        }
		public DrawDataType GetDraggingDataType()
		{
			return drawType;
		}
		public bool IsDragging()
		{
			return drawType != DrawDataType.NONE;
		}
        public void ClearDragCursor()
        {
			drawType = DrawDataType.NONE;
            cursor.atlas = UIAtlasManager.GetInstance().GetUIAtlas(ResourceAtlasName);
            cursor.spriteName = cursorNameDict[currentCursor];
        }

        private void Update()
        {
            if (mTrans == null)
            {
                return;
            }
            Screen.showCursor = false;

            Vector3 pos = Input.mousePosition;
            pos = pos + new Vector3(mTrans.localScale.x/2, -mTrans.localScale.y/2, 0);

            if (uiCamera != null)
            {
                pos.x = pos.x / Screen.width;
                pos.y = (pos.y + 4) / Screen.height;//绘画的鼠标会比原广播底几像素.
                mTrans.position = uiCamera.ViewportToWorldPoint(pos);

                if (uiCamera.isOrthoGraphic)
                {
                    mTrans.localPosition = NGUIMath.ApplyHalfPixelOffset(mTrans.localPosition, mTrans.localScale);
                }
            }
            else
            {
                pos.x -= Screen.width * 0.5f;
                pos.y -= Screen.height * 0.5f;
                mTrans.localPosition = NGUIMath.ApplyHalfPixelOffset(pos, mTrans.localScale);
            }
        }

        private static CursorManager mInstance;
        public static CursorManager GetInstance()
        {
            if (mInstance == null)
            {
                mInstance = Object.FindObjectOfType(typeof(CursorManager)) as CursorManager;

                if (mInstance == null)
                {
                    GameObject go = new GameObject("_CursorManager");
                    go.hideFlags = HideFlags.HideAndDontSave;
                    DontDestroyOnLoad(go);
                    mInstance = go.AddComponent<CursorManager>();
                }
            }
            return mInstance;
        }
    }
}
