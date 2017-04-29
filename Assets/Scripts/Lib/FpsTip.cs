using UnityEngine;
using System.Collections;

public class FpsTip : MonoBehaviour {
	
	int count = 0;
	float time = 0f;
	int fps = 60;
	
	public GUIText text = null;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (null == text)
		{
			text = gameObject.GetComponent<GUIText>();
			if (text == null)
				return;
		}
		time+=Time.deltaTime;
		if (time > 1f)
		{
			time = time - 1f;
			fps = count;
			count = 0;
			text.text = "FPS:"+fps;
		}
		count++;
	
	}
}
