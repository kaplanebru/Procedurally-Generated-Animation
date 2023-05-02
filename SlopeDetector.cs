using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SlopeDetector : MonoBehaviour
{
    [SerializeField] private Transform head;
    [SerializeField] private float headAngle = 30;

    private float angledPoint;
    private float straightPoint;
    private void Start()
    {
        StartCoroutine(nameof(TurnHead));
    }

    private void Update()
    {
        Detect();
    }

    void Detect()
    {
       if(!Physics.Raycast(transform.position,
            Quaternion.Euler(-5, 0, 0) * -Vector3.up,
            out RaycastHit hit,
            Mathf.Infinity,
            LayerMask.GetMask("Ground"))) return;

       angledPoint = hit.point.y;

       Physics.Raycast(transform.position,
           -Vector3.up,
           out RaycastHit hitStraight,
           Mathf.Infinity, 
           LayerMask.GetMask("Ground"));
       
       straightPoint = hitStraight.point.y;
       
       Debug.DrawRay(transform.position,
           Quaternion.Euler(-5, 0, 0) * -Vector3.up, Color.green, 0.5f);
       
    }

    private float offset = 0.0001f;
    IEnumerator TurnHead()
    {
        while (true)
        {
            if (Math.Abs(angledPoint- straightPoint) > offset)
            {
                head.transform.DOLocalRotateQuaternion(Quaternion.Euler(headAngle, 0, 0), 0.3f);
                yield return new WaitUntil(() => Math.Abs(angledPoint-straightPoint) < offset);
                head.transform.DOLocalRotateQuaternion(Quaternion.Euler(0, 0, 0), 0.3f);
            }
            yield return null;
        }
      

    }
}
