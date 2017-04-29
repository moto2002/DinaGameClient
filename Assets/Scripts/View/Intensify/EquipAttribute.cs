using UnityEngine;
using System.Collections;

namespace Assets.Scripts.View.Intensify
{
	public class EquipAttribute : MonoBehaviour
	{
		public UILabel AddAttributeText = null;
		public UILabel CurrentAttributeText = null;
		public UILabel MaxAttributeText = null;
		public UIPanel AttributeUI = null;
		public UISprite RightArrow = null;
		public UISprite RiseArrow = null;
		
		public EquipAttribute ()
		{
			
		}
		
		public void Init ()
		{
			UIComplete ();
		}
		
		public void SetPropertyData (string curProperty )
		{
			CurrentAttributeText.text = curProperty;
		}
		public void SetNextPropertyData (string nextProperty)
		{
			AddAttributeText.text = nextProperty;
		}
		
		public void SetMaxPropertyData (string maxProperty)
		{
			MaxAttributeText.text = maxProperty;
		}
		
		public void UIComplete ()
		{
			AddAttributeText = FindUIObject<UILabel>("AddAttributeText");
			CurrentAttributeText = FindUIObject<UILabel>("CurrentAttributeText");
			MaxAttributeText = FindUIObject<UILabel>("MaxAttributeText");
			AttributeUI = FindUIObject<UIPanel>("AttributeUI");
			RightArrow = FindUIObject<UISprite>("RightArrow");
			RiseArrow = FindUIObject<UISprite>("RiseArrow");
		}
		
		public T FindUIObject<T>(string name) where T : Component
	    {
	        T[] coms = this.GetComponentsInChildren<T>(true);
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
	}
}

