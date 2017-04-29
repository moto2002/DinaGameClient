using UnityEngine;
using System.Collections;

public class FellowMainCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Camera camera = GetComponent<Camera>();
		camera.nearClipPlane = Camera.main.nearClipPlane;
		camera.farClipPlane = Camera.main.farClipPlane;
		camera.fieldOfView = Camera.main.fieldOfView;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Camera.main.transform.position;
		transform.forward = Camera.main.transform.forward;
	}
}
