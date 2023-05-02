using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities 
{
    public static void CheckGround(Transform transform, bool alterPos)
    {
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, Mathf.Infinity,
                LayerMask.GetMask("Ground")))
        {
            if(alterPos)
                transform.position = hit.point + hit.normal * 0.01f;
            
            transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
        }
    }
}
