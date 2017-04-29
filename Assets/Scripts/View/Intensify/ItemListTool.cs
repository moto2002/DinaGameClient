using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.View.Intensify
{
	/************************************************************************/
    /* UIGrid列表工具
     * author@linfeng*/
    /************************************************************************/
	public class ItemListTool 
	{
		/* usage: 
        List<GameObject> canUseList = ItemListTool.GetCanUseItemList(gridRoot); 
        for (int i=0; i<totalData.Length; ++i) 
        { 
            GameObject go = ItemListTool.GetNewItemObj(canUseList, parent, prefab);  
        } 
        ItemListTool.ActiveUnuseItem(canUseList); 
     	*/ 
		public static GameObject GetNewItemObj(List<GameObject> canUseList, GameObject parent, GameObject prefab) 
		{
			GameObject go = null;  
        	if (canUseList.Count > 0) 
			{  
            	go = canUseList [0];  
            	canUseList.RemoveAt (0);  
        	} 
			else 
			{  
            	go = NGUITools.AddChild (parent, prefab);
        	}  
        	NGUITools.SetActive(go, true);  
        	return go;  
		}
		
		public static List<GameObject> GetCanUseItemList (GameObject parent , string nameStr)  
	    {  
	        List<GameObject> itemList = new List<GameObject> ();  
	        Transform parentTran = parent.transform;  
	        for (int i=0; i<parentTran.childCount; ++i)
			{  
	            GameObject go = parentTran.GetChild (i).gameObject;  
	            if (IsNotTemplateGameObject(go , nameStr))  
	            {  
	                itemList.Add (go);  
	            }  
	        }  
	        return itemList;  
	    }
		
	    public static void ActiveUnuseItem (List<GameObject> canUseList)  
	    {  
	        foreach (var item in canUseList)
			{  
	            NGUITools.SetActive(item, false);  
	        }  
	    }  
      
	    private static bool IsNotTemplateGameObject(GameObject go , string nameStr)  
	    {  
	        bool result = !go.name.ToLower().Contains(nameStr);  
	        if (!result && go.activeSelf)  
	        {  
	            NGUITools.SetActive(go, false);  
	        }  
	        return result;  
	    }  
	}
}

