using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Scripts.Manager;

namespace Assets.Scripts.Utils
{
    class PhysicUtils
    {
        /************************************************************************/
        /* 得到地表层上的坐标                                                                     */
        /************************************************************************/
        public static Vector3 GetMapPoint(Vector2 vec)
        {
			NavMeshHit _hit;
			/*
			if(NavMesh.Raycast(new Vector3(vec.x, 200f, vec.y),new Vector3(vec.x, -200f, vec.y),out _hit,-1))
			{
				return _hit.position;
			}*/
			
			
			{
				RaycastHit[] hits = Physics.RaycastAll(new Vector3(vec.x, 200, vec.y), Vector3.down,400);
				foreach(RaycastHit hit in hits)
				{
					if (hit.collider.gameObject.tag == TagManager.GetInstance().NavMeshTag)
	                {
	                    return hit.point;
	                }
				}
			}
			
			if(NavMesh.SamplePosition(new Vector3(vec.x,0,vec.y),out  _hit,100f,-1))
			{
				return _hit.position;
			}
            return new Vector3(vec.x,0,vec.y);
        }
		
    }
}
