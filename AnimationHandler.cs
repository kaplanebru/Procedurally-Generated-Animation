using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using _Scripts;
using UnityEngine;
using DG.Tweening;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class AnimationHandler : MonoBehaviour
{
    [SerializeField] private AnimationData data;
    [SerializeField] private Transform target;
    
    [Header("Body Parts")]
    [SerializeField] private Transform body;
    [SerializeField] private Transform[] shoulders;
    [SerializeField] private Transform[] legTargets;
    public BodyState bodyState = BodyState.Immobile;
    
    private Vector3[] fixedLegPos = new Vector3[2];
    private Quaternion[] shoulderStartRot = new Quaternion[2];
    private Vector3 latestTargetPos;
    private Vector3 followDirection;
    float ContinuousDistance() =>Vector3.Distance(target.position, transform.position);
    float DiscreteDistance() => Vector3.Distance(transform.position, latestTargetPos);
    

    private void Start()
    {
        AssignStartValues();
        StartCoroutine(nameof(AnimationRoutine));
        StartCoroutine(nameof(BodyRoutine));
    }
    
    void AssignStartValues()
    {
        for (int i = 0; i < shoulderStartRot.Length; i++)
        {
            shoulderStartRot[i] = shoulders[i].transform.localRotation;
        }
    }

    IEnumerator AnimationRoutine()
    {
        while (true)
        {
            yield return null;
            if (ContinuousDistance() < data.maxDistance) continue;
            
            latestTargetPos = target.position;
            followDirection = (latestTargetPos - transform.position).normalized;
            SetFixedPos(0);
            
            ChangeState(BodyState.Mobile);
            yield return new WaitUntil(() => DiscreteDistance() < 0.01f);
            ChangeState(BodyState.Immobile);
        }
    }

    void ChangeState(BodyState currentState)
    {
        bodyState = currentState;
        
        switch (bodyState)
        {
            case BodyState.Mobile:
                ReleaseLeg(1);
                RotateShoulders(1, 0);
                JumpBody();
                break;
            case BodyState.Immobile:
                ShiftLegs(0, 1);
                RotateShoulders(0,1);
                break;
        }
    }

    IEnumerator BodyRoutine()
    {
        while (true)
        {
            yield return null;
            if (bodyState == BodyState.Immobile) continue;
            
            transform.position += followDirection * data.bodySpeed;
            RotateTowardsTarget(GetLookRotation());
            FixLeg(0);
            Utilities.CheckGround(transform,false);
        }
    }

    void SetFixedPos(int i)
    {
        fixedLegPos[i] = legTargets[i].transform.position;
    }

    void FixLeg(int i)
    {
        legTargets[i].transform.position = fixedLegPos[i]; 
    }

    void ReleaseLeg(int i)
    {
        legTargets[i].DOLocalMove(Vector3.zero, data.legDuration/2).SetEase(data.releaseEase);
    }
    void ShiftLegs(int first, int second)
    {
        Vector3 lastLeftTarget = legTargets[first].transform.localPosition;
        ReleaseLeg(first);
        legTargets[second].DOLocalMove(lastLeftTarget, data.legDuration).SetEase(data.releaseEase);
    }

    void RotateShoulders(int first, int second)
    {
        shoulders[first].DOLocalRotateQuaternion(shoulderStartRot[first] * Quaternion.Euler(-data.rotAngle, 0, 0), data.legDuration);
        shoulders[second].DOLocalRotateQuaternion(shoulderStartRot[second], data.legDuration);
    }
    
    void JumpBody()
    {
        body.transform.DOLocalMoveY(body.transform.localPosition.y + 0.05f, data.jumpDuration).OnComplete(() =>
        {
            body.transform.DOLocalMoveY(0, data.jumpDuration);
        });
    }


    Quaternion GetLookRotation()
    {
        Vector3 dir = (latestTargetPos- body.transform.position).normalized;
        return Quaternion.LookRotation(dir, Vector3.up);  //Quaternion.Euler(-90, 180, 0) * Vector3.up
    }
    void RotateTowardsTarget(Quaternion lookRotation)
    {
        var rot = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0); //Quaternion.Lerp(transform.rotation, lookRotation, data.bodySpeed);
        body.transform.rotation = Quaternion.Lerp(body.transform.rotation, rot, data.bodySpeed);
    }
 
    
    
}
