using UnityEngine;
using System.Collections;

public class BooldController : MonoBehaviour {
    MeshRenderer[] meshRendererArray = null;
	void Start () {
        meshRendererArray = this.GetComponentsInChildren<MeshRenderer>(true);
	}

    public void SetRate(float rate)
    {
        if (meshRendererArray != null)
        {
            foreach (MeshRenderer meshRenderer in meshRendererArray)
            {
                Material[] materialArray = meshRenderer.materials;
                if (materialArray != null && materialArray.Length != 0)
                {
                    Material material = materialArray[0];
                    material.SetTextureOffset("_MainTex", new Vector2(1, rate));
                }
            }
        }
        
	}
}
