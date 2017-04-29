using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Logic.Scene.SceneObject;
using Assets.Scripts.Manager;
using Assets.Scripts.Logic.Scene.SceneObject.Compont;
using Assets.Scripts.Data;
using Assets.Scripts.Lib.Loader;

public class ActionPlysiceDie : Action {
	
	
	string  rootName = "Bip01";
	string  bodyName = "Bip01 Pelvis";
	string  RightHips ="Bip01 R Thigh";
	string  LeftHips = "Bip01 L Thigh";
	string LeftCalf = "Bip01 L Calf";
	string RightCalf = "Bip01 R Calf";
	string LeftFoot = "Bip01 L Foot";
	string RightFoot = "Bip01 R Foot";
	string  RightArm = "Bip01 R UpperArm";
	string LeftArm = "Bip01 L UpperArm";
	string RightHand = "Bip01 R Hand";
	string LeftHand  = "Bip01 L Hand";
	string Head  = "Bip01 Head";
	
	public SceneEntity attacker;
	
	Rigidbody _rootRB;
	Rigidbody _bodyRB;
	Rigidbody _rightHandRB;
	
	public static T FindObject<T>(GameObject parent, string name) where T : Component
    {
        T[] coms = parent.GetComponentsInChildren<T>();
        int count = coms.Length;
        for (int i = 0; i < count; i++)
        {
            if (coms[i].gameObject.name.Equals(name))
            {
                return coms[i];
            }
        }
        return null;
    }
	
	List<Object> componentList = new List<Object>();
	
	void UnBind()
	{
		foreach ( Object o in componentList )
		{
			Object.Destroy(o);
		}
		componentList.Clear();
	}
	bool Bind()
	{
		float _meshScale = 3;
		float _scale = hero.heroSetting.Scale;
		Transform _root = FindObject<Transform> (hero.gameObject,rootName);
		if (_root == null )
		{
			UnBind();
			return false;
		}
		BoxCollider _rootBC = _root.gameObject.AddComponent<BoxCollider>();
		_rootBC.size = new Vector3(0.2f,0.2f,0.2f)*_scale;
		_rootBC.center = new Vector3(0,0.8f,1f)*_scale;
		_rootRB =  _root.gameObject.AddComponent<Rigidbody>();
		_rootRB.mass = 200f*_meshScale;
		_rootRB.angularDrag = 0.05f;
		
		componentList.Add(_bodyRB);
		componentList.Add(_rootBC);
		
		Transform _body = FindObject<Transform> (hero.gameObject,bodyName); 
		if (_body == null )
		{
			UnBind();
			return false;
		}
		BoxCollider _bodyBC = _body.gameObject.AddComponent<BoxCollider>();
		_bodyBC.size = new Vector3(0.4f,0.2f,0.2f)*_scale;
		_bodyBC.center = new Vector3(-0.4f,0f,0f)*_scale;
		_bodyRB =  _body.gameObject.AddComponent<Rigidbody>();
		_bodyRB.mass = 10f*_meshScale;
		_bodyRB.angularDrag = 0.05f;
		FixedJoint _bodyCJ = _body.gameObject.AddComponent<FixedJoint>();
		_bodyCJ.connectedBody = _rootRB;
		
		componentList.Add(_bodyCJ);
		componentList.Add(_bodyBC);
		componentList.Add(_bodyRB);
		
		
		Transform _head = FindObject<Transform> (_body.gameObject,Head); 
		if (_head == null )
		{
			UnBind();
			return false;
		}
		BoxCollider _headBC = _head.gameObject.AddComponent<BoxCollider>();
		_headBC.size = new Vector3(0.2f,0.2f,0.2f)*_scale;
		Rigidbody _headRB =  _head.gameObject.AddComponent<Rigidbody>();
		_headRB.mass = 10f*_meshScale;
		_headRB.angularDrag = 0.05f;
		FixedJoint _headCJ = _head.gameObject.AddComponent<FixedJoint>();
		_headCJ.connectedBody = _bodyRB;
		
		componentList.Add(_headCJ);
		componentList.Add(_headBC);
		componentList.Add(_headRB);
		
		
		Transform _LeftHips = FindObject<Transform> (_body.gameObject,LeftHips);
		if (_LeftHips == null )
		{
			UnBind();
			return false;
		}
		BoxCollider _LHBC = _LeftHips.gameObject.AddComponent<BoxCollider>();
		_LHBC.size = new Vector3(0.2f,0.2f,0.2f)*_scale;
		Rigidbody _LHRB =  _LeftHips.gameObject.AddComponent<Rigidbody>();
		_LHRB.mass = 10f*_meshScale;
		_LHRB.angularDrag = 0.5f;
		CharacterJoint _LHCJ = _LeftHips.gameObject.AddComponent<CharacterJoint>();
		_LHCJ.connectedBody = _rootRB;
		_LHCJ.swingAxis = new Vector3(0,0,-1);
		_LHCJ.axis = new Vector3(0,1,0);
		
		componentList.Add(_LHCJ);
		componentList.Add(_LHBC);
		componentList.Add(_LHRB);
		
		
			
		Transform _LeftCalf = FindObject<Transform> (_body.gameObject,LeftCalf);
		if (_LeftCalf == null )
		{
			UnBind();
			return false;
		}
		BoxCollider _LCBC = _LeftCalf.gameObject.AddComponent<BoxCollider>();
		_LCBC.size = new Vector3(0.2f,0.2f,0.2f)*_scale;
		Rigidbody _LCRB =  _LeftCalf.gameObject.AddComponent<Rigidbody>();
		_LCRB.mass = 10f*_meshScale;
		_LCRB.angularDrag = 0.5f;
		CharacterJoint _LCCJ = _LeftCalf.gameObject.AddComponent<CharacterJoint>();
		_LCCJ.connectedBody = _LHRB;
		_LCCJ.swingAxis = new Vector3(0,0,-1);
		
		componentList.Add(_LCBC);
		componentList.Add(_LCRB);
		componentList.Add(_LHRB);

		
		/*Transform _LeftFoot = FindObject<Transform> (_body.gameObject,LeftFoot);
		if (_LeftFoot == null )
		{
			UnBind();
			return false;
		}
		BoxCollider _LFBC = _LeftFoot.gameObject.AddComponent<BoxCollider>();
		_LFBC.size = new Vector3(0.2f,0.2f,0.2f)*_scale;
		Rigidbody _LFRB =  _LeftFoot.gameObject.AddComponent<Rigidbody>();
		_LFRB.mass = 1f;
		_LFRB.angularDrag = 0.5f;
		CharacterJoint _LFCJ = _LeftFoot.gameObject.AddComponent<CharacterJoint>();
		_LFCJ.connectedBody = _LCRB;
		_LFCJ.swingAxis = new Vector3(0,0,-1);
		
		
		componentList.Add(_LFBC);
		componentList.Add(_LFRB);
		componentList.Add(_LCRB);*/
		
		Transform _RightHips =  FindObject<Transform> (_body.gameObject,RightHips);
		if (_RightHips == null )
		{
			UnBind();
			return false;
		}
		BoxCollider _RHBC = _RightHips.gameObject.AddComponent<BoxCollider>();
		_RHBC.size = new Vector3(0.2f,0.2f,0.2f)*_scale;
		Rigidbody _RHRB =  _RightHips.gameObject.AddComponent<Rigidbody>();
		_RHRB.mass = 10f*_meshScale;
		_RHRB.angularDrag = 0.5f;
		CharacterJoint _RHCJ = _RightHips.gameObject.AddComponent<CharacterJoint>();
		_RHCJ.connectedBody = _rootRB;
		_RHCJ.swingAxis = new Vector3(0,0,-1);
		_RHCJ.axis = new Vector3(0,1,0);
		
		componentList.Add(_RHBC);
		componentList.Add(_RHRB);
		componentList.Add(_RHCJ);
		
		Transform _RightCalf = FindObject<Transform> (_body.gameObject,RightCalf);
		if (_RightCalf == null )
		{
			UnBind();
			return false;
		}
		BoxCollider _RCBC = _RightCalf.gameObject.AddComponent<BoxCollider>();
		_RCBC.size = new Vector3(0.2f,0.2f,0.2f)*_scale;
		Rigidbody _RCRB =  _RightCalf.gameObject.AddComponent<Rigidbody>();
		_RCRB.mass = 10f*_meshScale;
		_RCRB.angularDrag = 0.5f;
		CharacterJoint _RCCJ = _RightCalf.gameObject.AddComponent<CharacterJoint>();
		_RCCJ.connectedBody = _RHRB;
		_RCCJ.swingAxis = new Vector3(0,0,-1);
		
		componentList.Add(_RCBC);
		componentList.Add(_RCRB);
		componentList.Add(_RCCJ);
		
		
		/*Transform _RightFoot = FindObject<Transform> (_body.gameObject,RightFoot);
		if (_RightFoot == null )
		{
			UnBind();
			return false;
		}
		BoxCollider _RFBC = _RightFoot.gameObject.AddComponent<BoxCollider>();
		_RFBC.size = new Vector3(0.2f,0.2f,0.2f)*_scale;
		Rigidbody _RFRB =  _RightFoot.gameObject.AddComponent<Rigidbody>();
		_RFRB.mass = 1f;
		_RFRB.angularDrag = 0.5f;
		CharacterJoint _RFCJ = _RightFoot.gameObject.AddComponent<CharacterJoint>();
		_RFCJ.connectedBody = _RCRB;
		_RFCJ.swingAxis = new Vector3(0,0,-1);
		
		componentList.Add(_RFBC);
		componentList.Add(_RFRB);
		componentList.Add(_RFCJ);*/

		Transform _RightArm = FindObject<Transform> (_body.gameObject,RightArm);
		if (_RightArm == null )
		{
			UnBind();
			return false;
		}
		BoxCollider _RABC = _RightArm.gameObject.AddComponent<BoxCollider>();
		_RABC.size = new Vector3(0.1f,0.1f,0.1f)*_scale;
		_rightHandRB =  _RightArm.gameObject.AddComponent<Rigidbody>();
		_rightHandRB.mass = 10f*_meshScale;
		_rightHandRB.angularDrag = 0.5f;
		CharacterJoint _RACJ = _RightArm.gameObject.AddComponent<CharacterJoint>();
		_RACJ.connectedBody = _bodyRB;
		
		componentList.Add(_RABC);
		componentList.Add(_rightHandRB);
		componentList.Add(_RACJ);
		
		/*Transform _RightHand = FindObject<Transform> (_body.gameObject,RightHand);
		if (_RightHand == null )
		{
			UnBind();
			return false;
		}
		BoxCollider _RHDBC = _RightHand.gameObject.AddComponent<BoxCollider>();
		_RHDBC.size = new Vector3(0.1f,0.1f,0.1f)*_scale;
		Rigidbody _RHDRB =  _RightHand.gameObject.AddComponent<Rigidbody>();
		_RHDRB.mass = 1;
		_RHDRB.angularDrag = 0.5f;
		CharacterJoint _RHDCJ = _RightHand.gameObject.AddComponent<CharacterJoint>();
		_RHDCJ.connectedBody = _rightHandRB;
		
		componentList.Add(_RHDBC);
		componentList.Add(_RHDRB);
		componentList.Add(_RHDRB);*/
		
		Transform _LeftArm = FindObject<Transform> (_body.gameObject,LeftArm);
		if (_LeftArm == null )
		{
			UnBind();
			return false;
		}
		BoxCollider _LABC = _LeftArm.gameObject.AddComponent<BoxCollider>();
		_LABC.size = new Vector3(0.1f,0.1f,0.1f)*_scale;
		Rigidbody _LARB =  _LeftArm.gameObject.AddComponent<Rigidbody>();
		_LARB.mass = 10f*_meshScale;
		_LARB.angularDrag = 0.5f;
		CharacterJoint _LACJ = _LeftArm.gameObject.AddComponent<CharacterJoint>();
		_LACJ.connectedBody = _bodyRB;
		
		componentList.Add(_LABC);
		componentList.Add(_LARB);
		componentList.Add(_LACJ);
		
		
		Transform _LeftHand = FindObject<Transform> (_body.gameObject,LeftHand);
		if (_LeftHand == null )
		{
			UnBind();
			return false;
		}
		BoxCollider _LHDBC = _LeftHand.gameObject.AddComponent<BoxCollider>();
		_LHDBC.size = new Vector3(0.1f,0.1f,0.1f)*_scale;
		Rigidbody _LHDRB =  _LeftHand.gameObject.AddComponent<Rigidbody>();
		_LHDRB.mass = 10f*_meshScale;
		_LHDRB.angularDrag = 0.5f;
		CharacterJoint _LHDCJ = _LeftHand.gameObject.AddComponent<CharacterJoint>();
		_LHDCJ.connectedBody = _LARB;
		_LHDCJ.axis = new Vector3(0,0,1);
		
		componentList.Add(_LHDBC);
		componentList.Add(_LHDRB);
		componentList.Add(_LHDCJ);
	
		
	
		KingSoftCommonFunction.SetLayer(hero.gameObject, 10);
		Physics.IgnoreLayerCollision(10,10,true);
		Physics.gravity = new Vector3(0f,-20f,0f);
		return true;
	}
	
	
	
	public ActionPlysiceDie(SceneEntity hero):base("ActionDie",hero)
	{
		
		isDead = true;
	} 

	
	public override void Active()
	{
		
		base.Active();
		if (null == hero.BodyGo)
			return;
		KSkillDisplay skillDisplay = KConfigFileManager.GetInstance().GetSkillDisplay(hero.property.lastHitSkillId,hero.property.tabID);
		Vector3 forward = hero.Position - attacker.Position;
		forward.Normalize();
		hero.transform.position = hero.transform.position + (Vector3.up*0.2f);
		if (!Bind())
		{
			ActionMonsterDie action = new ActionMonsterDie(hero);
			action.attacker = attacker;
            action.IsPushStack = false;
            hero.DispatchEvent(ControllerCommand.SetActiveAction, action);
			hero.transform.position = hero.transform.position - (Vector3.up*0.2f);
			return;
		}
		
		hero.BodyGo.animation.Stop();
		float _f = 10f;
		_rootRB.velocity = forward* skillDisplay.PhysicsVelocity*Random.Range(0.8f,1.2f)  ;
		_rootRB.velocity += Vector3.up*skillDisplay.PhysicsVelocityUp*Random.Range(0.8f,1.2f);
		_rootRB.angularVelocity = new Vector3(0,Random.Range(30f,60f),0);
	}
	public override void Update()
	{
		
	}
	
	public override bool IsCanActive()
	{	
		return isFinish;
	}
	public override bool TryFinish()
	{
		return false;
	}
	public override bool IsCanFinish()
	{
		return false;
	}
	public virtual bool IsFinish()
	{
		return false;
	}
	
}