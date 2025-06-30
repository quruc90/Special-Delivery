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
    public float maxAngleTolerance;

    [SerializeField] float withinTargetDistance;
    float distanceToTarget;
    [SerializeField] float reverseDistance;
    [SerializeField] float retreatDistance;

    Vector3 moveDirToTarget;
    float angleToTarget;
    bool isStuck;
    bool checkIfStuck;
    bool isReached;

    private bool sensorHit;

    [Header("Sensors")]
    public float sensorLength;
    public float sensorHeightOffset;
    public float frontSensorOffset;
    public float sideSensorOffset;
    public float sideSensorAngle;
    public LayerMask obtsructLos;

    private void Start()
    {
        isStuck = false;
        checkIfStuck = true;
        carController = GetComponent<CarController>();
        SetTarget(target);
    }

    private void Update()
    {
        distanceToTarget = Vector3.Distance(transform.position, targetPos);

        AutoThrottle();
        AutoSteer();
        //Retreat();
        LineOfSightCheck();

        if (!isStuck && checkIfStuck)
        {
            StartCoroutine(IsCarStuck());
        }

        Mathf.Clamp(steer, -1, 1);

        Debug.Log("steerInput" + steer);
        carController.SetInput(throttle, steer);
    }

    private void LineOfSightCheck()
    {
        Vector3 directionToTarget = target.transform.position - transform.position;
        Vector3 sensorStartPos = transform.position + transform.forward * frontSensorOffset + transform.up * sensorHeightOffset;
        if (!Physics.Raycast(sensorStartPos, directionToTarget, out RaycastHit los, distanceToTarget, obtsructLos))
        {
            Debug.DrawLine(sensorStartPos, target.transform.position, Color.green);
        }
        else
        {
            Debug.DrawLine(sensorStartPos, target.transform.position, Color.red);
            Sensors();
        }
    }

    private void Sensors()
    {
        RaycastHit hit;
        Vector3 sensorStartPos = transform.position + transform.forward * frontSensorOffset + transform.up * sensorHeightOffset;
        float sensorHitDirection = 0;
        sensorHit = false;

        //right sensor
        sensorStartPos += transform.right * sideSensorOffset;
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength) && hit.collider.CompareTag("AiBarrier"))
        {
            Debug.DrawLine(sensorStartPos, hit.point);
            sensorHitDirection -= 1f;
            sensorHit = true;
        }

        //right angled sensor
        if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(sideSensorAngle, transform.up) * carController.carRb.velocity.normalized, out hit, sensorLength)
        && hit.collider.CompareTag("AiBarrier"))
        {
            Debug.DrawLine(sensorStartPos, hit.point);
            sensorHitDirection -= 0.5f;
            sensorHit = true;
        }

        //right side sensor
        if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(sideSensorAngle * 2.5f, transform.up) * transform.forward, out hit, sensorLength * 0.5f)
        && hit.collider.CompareTag("AiBarrier") && distanceToTarget > 20)
        {
            Debug.DrawLine(sensorStartPos, hit.point);
            sensorHitDirection -= 0.3f;
            sensorHit = true;
        }

        //left sensor
        sensorStartPos -= 2 * sideSensorOffset * transform.right;
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength) && hit.collider.CompareTag("AiBarrier"))
        {
            Debug.DrawLine(sensorStartPos, hit.point);
            sensorHitDirection += 1f;
            sensorHit = true;
        }

        //left angled sensor
        if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(-sideSensorAngle, transform.up) * carController.carRb.velocity.normalized, out hit, sensorLength)
        && hit.collider.CompareTag("AiBarrier"))
        {
            Debug.DrawLine(sensorStartPos, hit.point);
            sensorHitDirection += 0.5f;
            sensorHit = true;
        }

        //left side sensor
        if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(-sideSensorAngle * 2.5f, transform.up) * transform.forward, out hit, sensorLength * 0.5f)
        && hit.collider.CompareTag("AiBarrier") && distanceToTarget > 20)
        {
            Debug.DrawLine(sensorStartPos, hit.point);
            sensorHitDirection += 0.3f;
            sensorHit = true;
        }

        //frontal sensor
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength) && hit.collider.CompareTag("AiBarrier")
        && Mathf.Abs(sensorHitDirection) < 0.4 && distanceToTarget > 10)
        {
            Debug.DrawLine(sensorStartPos, hit.point);
            sensorHit = true;
            if (hit.normal.x < 0)
            {
                sensorHitDirection = 1;
            }
            else
            {
                sensorHitDirection = -1;
            }
        }

        if (sensorHit)
        {
            steer = sensorHitDirection;
        }
        if (isStuck)
            steer *= -1;
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
        float angleTolerance = Mathf.Min(distanceToTarget/4f, maxAngleTolerance);

        if (angleToTarget > angleTolerance)
            steer = 1f;
        else if (angleToTarget < -angleTolerance)
            steer = -1f;
        else
            steer = 0;

        if (isStuck)
            steer *= -1;
    }

    private void Retreat()
    {
        if (withinTargetDistance > distanceToTarget)
            isReached = true;

        if (isReached)
        {
            if (distanceToTarget < retreatDistance)
            {
                throttle *= -1;
                steer *= -1;
                checkIfStuck = false;
            }
            else
            {
                isReached = false;
                checkIfStuck = true;
            }
        }
    }

    IEnumerator IsCarStuck()
    {
        float oldSpeed = GetComponent<Rigidbody>().velocity.magnitude;
        yield return new WaitForSeconds(2);
        float newSpeed = GetComponent<Rigidbody>().velocity.magnitude;
        if (Mathf.Abs(newSpeed - oldSpeed) < 1f && Mathf.Abs(newSpeed) < 4f && !isStuck)
        {
            checkIfStuck = false;
            isStuck = true;
            yield return new WaitForSeconds(2);
            isStuck = false;
            yield return new WaitForSeconds(2);
            checkIfStuck = true;
        }
    }
}
