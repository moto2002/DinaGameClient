using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.View.Scene;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.Lib.Resource;
using Assets.Scripts.Data;
using Assets.Scripts.Logic.Scene.SceneObject;
using Assets.Scripts.Utils;
using Assets.Scripts.Manager;

public class SceneCamera : MonoBehaviour
{
	
	//跟随相机...
    public Camera _camere = null;
	//跟随目标...
    public SceneEntity target = null;
	//跟随距离...
    public float distance = 7.5f;
	//离人物偏移...
    public Vector3 MaXOffset = new Vector3(0, 0, 0);
	
	public Vector3 MinOffset = new Vector3(0, 1.15f, 0);
   
	//最大Y旋转角度...
    public float MaxRotY = 45f;
	//最小Y旋转角度...
    public float MinRotY = -45f;
	//最大X旋转角度...
    public float MaxRotX = 50.0f;
	//最小X旋转角度...
    public float MinRotX = 12.5f;
	//最大距离...
    public float MaxDistance = 9.5f;
	//最小距离...
    public float MinDistance = 3f;
	//Y左旋转键值...
	public KeyCode LeftRotKeyCode = KeyCode.PageUp;
	//Y右旋转键值...
	public KeyCode RightRotKeyCode = KeyCode.PageDown;
	
	public static float RotX = 60f;
    public static float RotY = 45f;
    
	//鼠标移动灵敏度...
	public float MoveScale = 1.0f / 4f *60;
	//鼠标旋转转灵敏度...
	public float RotScale = 5*60;
	//相机移动灵敏度...
    public float fellowDistanceScale = 0.01f*60;
	//相机旋转灵敏度...
	public float fellowRotScale = 1f*60 ;
	
	public static float curRotY = 0f;
	private float fellowRotY = 0f;
	private float t = 0.0f;
    private float fellow_t = 0.0f;

    public bool m_bAllowMouseScroll = true;
    private KPlotInfo.PlotType m_bPlotType;
    private bool m_bPlayPlot = false;
    private bool m_bPlayPlotSlerp = false;
    private Vector3 m_vecPlotPos; 

	public bool isDebug = false;
	
	public KTabFile<KCameraConfig> cameraConfig;
	bool notInit = true;
	static ShakeObj shakeObj = new ShakeObj();
	
    void Awake()
    {
        if (null == _camere)
        {
            _camere = Camera.main;
        }
		
        cameraConfig = new KTabFile<KCameraConfig>("Settings/camera_config");
    }
	
	void Start () {
		
	}
	
	static Dictionary<string,AssetInfo> assetList = new Dictionary<string, AssetInfo>();
	static List<GameObject> shakeObjects = new List<GameObject>();
	public static void Shake(string name,float _time,float scale,float speed)
	{
		AssetInfo infor = null;
		if (!assetList.TryGetValue(name,out infor))
		{	
			//"effect_zhenping_zuoyou"
			//"effect_zhenping_shangxia"
			//"effect_zhenping_yuanquan"
			assetList[name] =  AssetLoader.GetInstance().PreLoad(URLUtil.GetEffectPath(name));
			return;
		}
		if (infor.isDone())
		{
			GameObject g = GameObject.Instantiate(infor.bundle.mainAsset) as GameObject;
			Animation anim = g.GetComponent<Animation>();
			if (null==anim || anim.clip == null)
			{
				GameObject.Destroy(g);
				return;
			}
			anim[anim.clip.name].normalizedSpeed = speed;
			
			g.transform.localScale = new Vector3(scale,scale,scale);
			GameObject c = new GameObject("aim");
			c.transform.parent = g.transform;
			DestoryObject dos = g.AddComponent<DestoryObject>();
			dos.delta = _time;
			shakeObjects.Add(c);
		}
		
	}

	public static void Shake(float _time,float _oscillation,SHAKE_TYPE type)
	{
		shakeObj.Add(_time,_oscillation,type);
	}

    public void SetTarget(SceneEntity majorHero)
    {
        target = majorHero;
    }

    void LateUpdate()
    {
		/*if (Input.GetKeyDown(KeyCode.F10))
		{
			Shake("effect_zhenping_zuoyou",1,1f,1f);
		}
		if (Input.GetKeyDown(KeyCode.F9))
		{
			Shake("effect_zhenping_zuoyou",1,1f,2f);
		}*/
		if( notInit && cameraConfig.m_nHeight>0 && !isDebug)
		{
			KCameraConfig cf = cameraConfig.getData("1");
			MaxDistance = cf.CameraMaxDistance;
			MinDistance = cf.CameraMinDistance;
			MaxRotX = cf.CameraMaxRotX;
			MinRotX = cf.CameraMinRotX;
			RotY = curRotY = cf.RotY;
			notInit = true;
		}
        if (target == null)
            return;
        MoveCamera();
		KeyPress();
        MouseScrollWheel();
    }

    private float tTemp;
    private Vector3 vecCameraFollow;
    private Vector3 plotStartVec;
    private float fellowtStart;
    private float fTimeUse = 0;
    public float fPlayTime = 1f;

    public void PlayPlot(KPlotInfo.PlotType type, Vector3 vecPostion, float fDis)
    {
        m_vecPlotPos = vecPostion;
        m_bPlotType = type;
        plotStartVec = vecCameraFollow;

        if (!m_bPlayPlot)
        {
            m_bAllowMouseScroll = false;
            m_bPlayPlot = true;
            tTemp = t;
        }

        if (type == KPlotInfo.PlotType.NormalCamera)
        {
            fellow_t = fDis;
            t = fDis;
        }
        else if (type == KPlotInfo.PlotType.TransCamera)
        {
            float dis = Vector3.Distance(vecCameraFollow, vecPostion);
            fPlayTime = dis / 10.0f + 1;

            m_bPlayPlotSlerp = true;
            fellowtStart = t;
            t = fDis;
        }
    }

    public void StopPlot()
    {
        m_bAllowMouseScroll = true;
        m_bPlayPlot = false;
        m_vecPlotPos = Vector3.zero;

        plotStartVec = vecCameraFollow;

        if (m_bPlotType == KPlotInfo.PlotType.NormalCamera)
        {
            fellow_t = tTemp;
            t = tTemp;
        }
        else if (m_bPlotType == KPlotInfo.PlotType.TransCamera)
        {
            float dis = Vector3.Distance(vecCameraFollow, target.transform.position);
            fPlayTime = dis / 10.0f + 1;

            m_bPlayPlotSlerp = true;
            fellowtStart = t;
            t = tTemp;
        }
    }

    private void MoveCamera()
    {
        Vector3 targetPos;
        if (m_bPlayPlot)
        {
            targetPos = m_vecPlotPos;
        }
        else
        {
            targetPos = target.transform.position;
        }

        if (m_bPlayPlotSlerp)
        {
            fTimeUse += Time.deltaTime;
            fellow_t = Mathf.Lerp(fellowtStart, t, fTimeUse / fPlayTime);

            fellowRotY = MoveTo(curRotY, fellowRotY, fellowRotScale * Time.deltaTime);
            RotX = Mathf.Lerp(MaxRotX, MinRotX, fellow_t);
            //RotY = fellowRotY;
            RotY = curRotY;
            Vector3 offset = new Vector3(Mathf.Lerp(MaXOffset.x, MinOffset.x, fellow_t), Mathf.Lerp(MaXOffset.y, MinOffset.y, fellow_t), Mathf.Lerp(MaXOffset.z, MinOffset.z, fellow_t));

            vecCameraFollow = Vector3.Lerp(plotStartVec, targetPos + offset, fTimeUse / fPlayTime);

            if (fTimeUse > fPlayTime)
            {
                vecCameraFollow = targetPos + offset;
                fellow_t = t;
                m_bPlayPlotSlerp = false;
                fTimeUse = 0;
                fPlayTime = 1;
            }
        }
        else
        {
            fellow_t = MoveTo(t, fellow_t, fellowDistanceScale * Time.deltaTime);

            fellowRotY = MoveTo(curRotY, fellowRotY, fellowRotScale * Time.deltaTime);
            RotX = Mathf.Lerp(MaxRotX, MinRotX, fellow_t);
            //RotY = fellowRotY;
            RotY = curRotY;
            Vector3 offset = new Vector3(Mathf.Lerp(MaXOffset.x, MinOffset.x, fellow_t), Mathf.Lerp(MaXOffset.y, MinOffset.y, fellow_t), Mathf.Lerp(MaXOffset.z, MinOffset.z, fellow_t));

            vecCameraFollow = targetPos + offset;
        }
        
        distance = Mathf.Lerp(MaxDistance, MinDistance, fellow_t);
        _camere.transform.position = vecCameraFollow;
        _camere.transform.forward = new Vector3(0, 0, 1);
        _camere.transform.Rotate(RotX, RotY, 0, Space.Self);
        _camere.transform.Translate(new Vector3(0, 0, -distance), Space.Self);

		_camere.transform.localPosition = _camere.transform.localPosition + shakeObj.GetDelta();

		int _len = shakeObjects.Count;
		for (int i = _len-1 ; i >=0 ; i--)
		{
			GameObject o = shakeObjects[i];
			if (null == o)
			{
				shakeObjects.RemoveAt(i);
			}
			else
			{
				_camere.transform.position = _camere.transform.position + o.transform.position;
			}
		}
    }

	float MoveTo(float des,float src,float delta)
	{
		if (src - des > delta)
        {
            return src - delta;
        }
        else if (des - src > delta)
        {
            return src + delta;
        }
        else
        {
            return  des;
        }
	}

	void KeyPress()
	{
		
		if ( isDebug)
		{
			
		}
		if(Input.GetKey(LeftRotKeyCode))
		{
			curRotY = MoveTo(MaxRotY,curRotY,RotScale*Time.deltaTime);
		}
		else if(Input.GetKey(RightRotKeyCode))
		{
			curRotY = MoveTo(MinRotY,curRotY,RotScale*Time.deltaTime);
		}
		
	}

    private void MouseScrollWheel()
    {
        if (!m_bAllowMouseScroll)
        {
            return;
        }

        float ScrollWheel = Input.GetAxis("Mouse ScrollWheel");
        if (ScrollWheel == 0 )
        {
            return;
        }

		if (ScrollWheel > 0)
        {
			t = MoveTo(1f,t,MoveScale*Time.deltaTime);
        }
        else if (ScrollWheel < 0)
        {
			t = MoveTo(0f,t,MoveScale*Time.deltaTime);
        }
    }
}