using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

public class CarAIController : MonoBehaviour
{
    [SerializeField] private GameObject target;
    private Vector3 targetPos;
    private CarController carController;

    private float throttle;
    private float steer;

    float withinTargetDistance;
    float distanceToTarget;
    float reverseDistance = 6f;

    Vector3 moveDirToTarget;
    float angleToTarget;
    bool isStuck;
    bool isReached;

    private void Start()
    {
        carController = GetComponent<CarController>();
    }

    private void Update()
    {
        SetTarget(target);
        Debug.Log(distanceToTarget);

        withinTargetDistance = 2.5f;
        distanceToTarget = Vector3.Distance(transform.position, targetPos);

        if (withinTargetDistance < distanceToTarget)
        {
            AutoThrottle();
            AutoSteer();
            if(!isStuck)
                StartCoroutine(IsCarStuck());
        }
        else
            isReached = true;
        if ((distanceToTarget < reverseDistance) && isReached)
        {
            throttle *= -1;
            steer *= -1;
        }
        else
            isReached = false;
        

        carController.SetInput(throttle, steer);
    }

    public void SetTarget(GameObject target)
    {
        if (target.GetComponent<Rigidbody>())
        {
            SetTargetOffset();
        }
        SetTargetPos(target.transform.position);
    }

    private void SetTargetOffset()
    {
        targetPos += target.GetComponent<Rigidbody>().velocity;
    }

    public void SetTargetPos(Vector3 targetPos)
    {
        this.targetPos = targetPos;
    }

    private void AutoThrottle()
    {
        moveDirToTarget = (targetPos - transform.position).normalized;
        float dot = Vector3.Dot(transform.forward, moveDirToTarget);

        if (dot > 0)
            throttle = 1f;
        else
        {
            if (distanceToTarget > reverseDistance)
                throttle = 1f;
            else
                throttle = -1f;
        }

        if (isStuck)
            throttle *= -1;
    }

    private void AutoSteer()
    {
        angleToTarget = Vector3.SignedAngle(transform.forward, moveDirToTarget, Vector3.up);

        if (angleToTarget > 4)
            steer = 1f;
        else if (angleToTarget < -4)
            steer = -1f;
        else
            steer = 0;

        if (isStuck)
            steer *= -1;
    }

    IEnumerator IsCarStuck()
    {
        float oldSpeed = GetComponent<Rigidbody>().velocity.magnitude;
        yield return new WaitForSeconds(2);
        float newSpeed = GetComponent<Rigidbody>().velocity.magnitude;
        if (Mathf.Abs(newSpeed - oldSpeed) < 1f && Mathf.Abs(newSpeed) < 4f)
        {
            isStuck = true;
            yield return new WaitForSeconds(2);
            isStuck = false;
            yield return new WaitForSeconds(4);
        }
    }
}
