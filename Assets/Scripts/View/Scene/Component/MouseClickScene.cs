using UnityEngine;
using System.Collections;
using System;
using Assets.Scripts.Model.Scene;
using Assets.Scripts.Controller;
using Assets.Scripts.Manager;
using Assets.Scripts.Model.Player;
using Assets.Scripts.View.Scene;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.Utils;
using Assets.Scripts.Define;
using Assets.Scripts.Lib.Log;
using Assets.Scripts.View.Drag;
using Assets.Scripts.Model.Skill;
using Assets.Scripts.Data;
using Assets.Scripts.Logic.Scene.SceneObject;
using Assets.Scripts.Logic.Scene.SceneObject.Compont;
using Assets.Scripts.Logic.Scene;
using Assets.Scripts.View.Chat;
using Assets.Scripts.Logic.Skill;


public class MouseClickScene : MonoBehaviour
{
	private static Logger log = LoggerFactory.GetInstance ().GetLogger (typeof(MouseClickScene));

    private SceneView scene;

	private GameObject mouseClickEffect;
	private Vector3 clickPosition = Vector3.zero;

    public static SceneEntity moveCursourSceneEntity;

    private SceneEntity clickTarget;

    public SceneEntity skillTarget;

	private float clickTimer;
	private float clickTimerInterval = 0.1f;

	void Awake ()
	{
		clickTimer = Time.realtimeSinceStartup;
	}
	
	void Update ()
	{
		//"Effect/effect_skill_daobin_01_pugong_gmale.res";
		CheckClickSceneObject();
        if (
            SceneLogic.GetInstance() != null &&
            CMDView.GetInstance() != null &&
            !CMDView.GetInstance().IsEditing()
        )
        {
			
			if(Input.GetKeyDown(KeyCode.U))
			{
				if(SceneLogic.GetInstance().MainHero.GetCurActionName().CompareTo("ActionDie") == 0)
				{
                    SceneLogic.GetInstance().MainHero.Net.SendReliveRequest(true);
				}
			}
			
			else if(Input.GetKeyDown(KeyCode.Alpha0)){
				SceneLogic.GetInstance().MainHero.property.CmdAutoAttack = true;
			}
        }
		
		//鼠标点击或者鼠标帧移动时间间隔到..
		if (UICamera.hoveredObject == null) 
        {
			
			
			if (CursorManager.GetInstance().IsDragging())
			{
				if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
				{
					CursorManager.GetInstance().ClearDragCursor();
				}
				return;
			}
				
			MouseSceneHandler ();
		}
		else
		{
			if (DragItem.isDragging)
				return;
			CursorManager.GetInstance().SetCursor(CursorType.ctNormal);
		}
	}
	public void CopyMeshCollageMesh()
	{
		Vector3 [] vs;
		int [] ts;
		NavMesh.Triangulate(out vs,out ts);
		
		Mesh mesh = new Mesh();
		mesh.vertices = vs;
		mesh.triangles = ts;
		GameObject go = new GameObject();
		MeshCollider mc =  go.AddComponent<MeshCollider>();
		mc.sharedMesh = mesh;
		go.name = "NavMesh";
	}

	void FixedUpdate ()
	{
		
	}

	private void PlayMouseClickEffect (Vector3 position)
	{
		if (mouseClickEffect == null) 
        {
            AssetLoader.GetInstance().Load(URLUtil.GetEffectPath("effect_shubiaodianjitexiao"), MouseClickEffect_LoadComplete, AssetType.BUNDLER);
		} 
        else 
        {
			TimeManager.GetInstance ().AddOnce (MouseClickEffect_TimeOver, 1000);
			mouseClickEffect.transform.position = clickPosition;
            mouseClickEffect.transform.up = Vector3.up;
			mouseClickEffect.SetActive(true);
		}
	}

    private void StopMouseClickEffect()
    {
        MouseClickEffect_TimeOver();
    }

	private void MouseClickEffect_LoadComplete (AssetInfo info)
	{
        mouseClickEffect = Instantiate(info.bundle.mainAsset) as GameObject;
        mouseClickEffect.transform.localPosition = clickPosition;
        mouseClickEffect.transform.localScale = Vector3.one;
        mouseClickEffect.hideFlags = HideFlags.HideAndDontSave;
	}

	private void MouseClickEffect_TimeOver ()
	{
        if (mouseClickEffect == null)
            return;
		mouseClickEffect.SetActive (false);
	}

    //设置鼠标在场景上移动的效果..
    private void SetMoveSceneObject(SceneEntity so)
    {
		if(so==moveCursourSceneEntity)
			return;
		if(moveCursourSceneEntity != null)
            moveCursourSceneEntity.DispatchEvent(ControllerCommand.SET_GLOW,false);
		moveCursourSceneEntity = null;
        
		if (so == null)
        {
            CursorManager.GetInstance().SetCursor(CursorType.ctNormal);
            SetCursorBySkill();
        }
        else
        {
			SceneEntity _hero = so as SceneEntity;
            if (_hero.property.isDeaded)
			{
				CursorManager.GetInstance().SetCursor(CursorType.ctNormal);
            	SetCursorBySkill();
				return;
			}
			moveCursourSceneEntity = so;
            moveCursourSceneEntity.DispatchEvent(ControllerCommand.SET_GLOW, true);
            if (moveCursourSceneEntity.HeroType == KHeroObjectType.hotNpc)
            {
                CursorManager.GetInstance().SetCursor(CursorType.ctChat);
            }
            else if (moveCursourSceneEntity.HeroType == KHeroObjectType.hotMonster)
            {
                CursorManager.GetInstance().SetCursor(CursorType.ctAttack);
            }
            else if (moveCursourSceneEntity.HeroType == KHeroObjectType.hotPlayer)
            {
                CursorManager.GetInstance().SetCursor(CursorType.ctFriend);
            }
        }
    }

    public void SetCursorBySkill()
    {
        
    }


    //取消鼠标点击位置
    private void CancelPosition()
    {
        clickPosition = Vector3.zero;
        StopMouseClickEffect();
    }
	
	
	private void OnCatchNPC(SceneEntity skillTarget)
    {
        EventDispatcher.GameWorld.Dispath(ControllerCommand.OPEN_NPC_PANEL, skillTarget.property.Id);
	}
    //设置点击目标
    private void SetClickSceneObject(SceneEntity so)
	{
		if (so != null && clickTarget != so)
		{
            CancelClickSceneObject(true);
			if(so is SceneEntity && SceneLogic.GetInstance().MainHero != so)
			{
                SceneLogic.GetInstance().MainHero.property.target = so as SceneEntity;
                EventDispatcher.GameWorld.DispatchEvent(ControllerCommand.CHANGE_TARGET);
			}
			clickTarget = so;
			if (clickTarget is SceneEntity)
			{
				SceneEntity _hero = clickTarget as SceneEntity;
                if (_hero.property.isDeaded)
					return;
				skillTarget = _hero;
				if(skillTarget.HeroType == KHeroObjectType.hotPlayer)
				{
                    EventDispatcher.GameWorld.Dispath(ControllerCommand.SHOW_PLAYER_PANEL, skillTarget);
				}
                else if (skillTarget.HeroType == KHeroObjectType.hotMonster)
				{
                    EventDispatcher.GameWorld.Dispath(ControllerCommand.SHOW_MONSTER_PANEL, skillTarget);
				}
                else if (skillTarget.HeroType == KHeroObjectType.hotNpc)
                {
                    //EventDispatcher.GameWorld.Dispath(ControllerCommand.OPEN_NPC_PANEL, skillTarget.Id);
                }
			}
		}
	}
	 public void CheckClickSceneObject()
    {
		if(null != clickTarget )
		{
			SceneEntity _hero = clickTarget as SceneEntity;
            if (_hero.property.isDeaded)
				CancelClickSceneObject(true);
		}
    }

    //取消鼠标点击目标
    public void CancelClickSceneObject(bool isSkillTarget)
    {
        if (isSkillTarget == true)
            CancelClickSkillObject();
		clickTarget = null;
        SceneLogic.GetInstance().MainHero.property.target = null;
        EventDispatcher.GameWorld.DispatchEvent(ControllerCommand.CHANGE_TARGET);
    }

    //取消鼠标点击技能对象
    public void CancelClickSkillObject()
    {
        if (skillTarget != null)
        {
            skillTarget = null;
        }
    }

    //移动到场景对象
    private void MoveToSceneObject(SceneEntity so)
	{
		CancelPosition();
        if (Vector3.Distance(SceneLogic.GetInstance().MainHero.PositionFoot(), so.transform.position) > 0.5f)
        {
            SceneLogic.GetInstance().ClearAutoMoveAcrossMap();
            SceneLogic.GetInstance().MainHero.DispatchEvent(ControllerCommand.MOVE_TO_DES, so.transform.position);
        }
	}

    //鼠标在屏幕中移动
	private void MouseSceneHandler ()
	{
        if (null== scene || scene.IsLock)
            return;
		if( SceneLogic.GetInstance().MainHero.property.lockOpera)
			return;
        
		//这里是为做点击做的。
        if (Input.GetMouseButtonDown(0))
        {
		    Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		    RaycastHit[] hitList = Physics.RaycastAll (ray, 200);
		    foreach (RaycastHit hit in hitList) 
            {
			    if (hit.collider.gameObject.tag == TagManager.GetInstance ().NavMeshTag) 
                {
                    MouseClickHandler(hit.point, true);
			    } 
		    }
        }
		if (Input.GetMouseButtonDown(0))
        {
			SceneLogic.GetInstance().MainHero.property.CmdAutoAttack = false;
		}
		GameObject go = KingSoftCommonFunction.ScreenMouseGetObject();
		if(null != go)
		{
            SceneEntity so = go.GetComponent<SceneEntity>() as SceneEntity;
            if (so.property.isInteractive == false)
            {
                return;
            }
            SetMoveSceneObject(so);
			if (Input.GetMouseButtonDown(0))
            {
				SceneLogic.GetInstance().MainHero.property.AutoAttack = false;
                SetClickSceneObject(so);
				if (so.property.sceneObjType == KSceneObjectType.sotDoodad )
				{
					Debug.LogWarning("PickUpDrop");
					SceneLogic.GetInstance().PickUpDrop(so);
					return;
					//so.property.dropValue;	
				}
				else if (so.property.sceneObjType == KSceneObjectType.sotHero)
                {
					AnimationComponent.OperaWalking = false;
			
                    if (so.HeroType == KHeroObjectType.hotNpc)
                    {
                        SceneLogic.GetInstance().MainHero.Action.MoveToNPC(so, OnCatchNPC);
                    }
                    else if (so.HeroType == KHeroObjectType.hotPlayer)
                    {
                        bool canPk = true;
                        if (canPk == true)
                        {
							SceneEntity player = so;
                            player.property.isCanAttack = true;
                            SceneLogic.GetInstance().MainHero.property.AutoAttack = true;

                            SceneLogic.GetInstance().MainHero.Action.CommonAttack(so);
                        }
                    }
                    else if (so.HeroType == KHeroObjectType.hotMonster)
                    {
                        SceneLogic.GetInstance().MainHero.property.AutoAttack = true;
                        SceneLogic.GetInstance().MainHero.Action.CommonAttack(so);
                    }
					
                }
            }
			else if (Input.GetMouseButtonDown(1))
			{
				SceneLogic.GetInstance().MainHero.property.AutoAttack = false;
				if (so.property.sceneObjType == KSceneObjectType.sotHero)
                {
					if (so.HeroType == KHeroObjectType.hotPlayer)
                    {
                        bool canPk = true;
                        if (canPk == true)
                        {
							SceneEntity h = so as SceneEntity;
                            h.property.isCanAttack = true;
							SetClickSceneObject(so);
                            SceneLogic.GetInstance().MainHero.property.AutoAttack = false;
							SceneLogic.GetInstance().MainHero.DispatchEvent(ControllerCommand.Idle, true);
							SceneLogic.GetInstance().MainHero.DispatchEvent(ControllerCommand.LookAtPos, h.Position);
						}
					}
                    else if (so.HeroType == KHeroObjectType.hotMonster)
                    {
						SceneEntity h = so as SceneEntity;
                        h.property.isCanAttack = true;
						SetClickSceneObject(so);
                        SceneLogic.GetInstance().MainHero.property.AutoAttack = false;
					    SceneLogic.GetInstance().MainHero.DispatchEvent(ControllerCommand.Idle, true);
						SceneLogic.GetInstance().MainHero.DispatchEvent(ControllerCommand.LookAtPos,h.Position);
                    }
                }
			}
		}
		else 
		{
			SetMoveSceneObject(null);
            if (Input.GetMouseButtonDown(0))
            {	
				//寻路.
				SceneLogic.GetInstance().MainHero.property.AutoAttack = false;
                Vector3 pos = KingSoftCommonFunction.ScreenMouseToGround(SceneLogic.GetInstance().MainHero.Position);
                SceneLogic.GetInstance().ClearAutoMoveAcrossMap();
                SceneLogic.GetInstance().MainHero.DispatchEvent(ControllerCommand.MOVE_TO_DES, pos, true);
            }	
		}
	}

    //点击了场景.
    private void MouseClickHandler(Vector3 _clickPosition, bool isShowClick)
    {
        if (Time.realtimeSinceStartup - clickTimer < clickTimerInterval)
        {
            if (clickPosition != null && Vector3.Distance(_clickPosition, clickPosition) < 100){
                EventDispatcher.GameWorld.Dispath(ControllerCommand.SCENE_DOUBLE_CLICK, clickPosition);
                return;
            }
        }
        else
        {
            clickTimer = Time.realtimeSinceStartup;
        }
        clickPosition = _clickPosition;
        if (isShowClick){
            //CancelClickSkillObject();
            PlayMouseClickEffect(clickPosition);
        }
        else{
            StopMouseClickEffect();
        }
        EventDispatcher.GameWorld.Dispath(ControllerCommand.SCENE_CLICK, clickPosition);
    }

    public static MouseClickScene Create(SceneView scene)
    {
        GameObject go = new GameObject("MouseClickScene");
        go.hideFlags = HideFlags.HideAndDontSave;
        MouseClickScene mcs = go.AddComponent<MouseClickScene>();
        mcs.scene = scene;
        return mcs;
    }
}
