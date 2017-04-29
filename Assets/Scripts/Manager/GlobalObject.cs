using UnityEngine;
using System.Collections;

public class GlobalObject : MonoBehaviour {
	void Start () {
		DontDestroyOnLoad(this.gameObject);
	}
}
