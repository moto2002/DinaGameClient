using UnityEngine;
using System.Collections;

public class ShaderManager : MonoBehaviour {
	
	static ShaderManager instance = null;
	public static ShaderManager GetInstance()
	{
		return instance;
	}
	
	public Material  KingSoftDiffuse = null;
	public Material  KingSoftDiffuseCO = null;
	public Material  KingSoftDiffuseACO = null;
	public Material  KingSoftNumTip = null;
	public Material  KingSoftSelected = null;
	public Material  KingSoftGhost = null;
	
	void Start () {
		instance = this;
	}
}
