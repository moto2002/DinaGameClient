using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KingSoftMath
{

    public const int HUNDRED_NUM = 100;
    public static bool MoveTowards(ref Vector3 current, Vector3 target, float moveDelta)
    {
        float distance = Vector3.Distance(current, target);
        if (distance < moveDelta)
        {
            current = target;
            return true;
        }
        Vector3 dir = target - current;
        current = current + moveDelta * dir.normalized;
        return false;
    }
	
	
    public static float GetValueBetweenToPoint(Vector3 begin, Vector3 target, Vector3 current)
    {
        if (begin.x != target.x && current.x != target.x)
        {
            return Mathf.Abs(begin.x - current.x) / Mathf.Abs(begin.x - target.x);
        }
        if (begin.z != target.z && current.z != target.z)
        {
            return Mathf.Abs(begin.z - current.z) / Mathf.Abs(begin.z - target.z);
        }
        return 1f;
    }

    public static void MakeInRange(int nValue, int nMinValue, int nMaxValue)
    {
        if (nValue > nMaxValue)
            nValue = nMaxValue;
        if (nValue < nMinValue)
            nValue = nMinValue;
    }

    public static float CheckDistance(Vector3 pos1, Vector3 pos2)
    {
        return Vector3.Distance(pos1, pos2);
    }
	public static float CheckDistanceXZ(Vector3 pos1, Vector3 pos2)
    {
        return Vector2.Distance(new Vector2(pos1.x,pos1.z), new Vector2(pos2.x,pos2.z));
    }
	
	public static Vector3 lerp(Vector3 v1, Vector3 v2,float t)
	{
		return new Vector3(Mathf.Lerp(v1.x,v2.x,t),Mathf.Lerp(v1.y,v2.y,t),Mathf.Lerp(v1.z,v2.z,t));
	}

}
