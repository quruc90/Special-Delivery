using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class WheelColliders : IEnumerable<WheelCollider>
{
    public WheelCollider FLWheel;
    public WheelCollider FRWheel;
    public WheelCollider RLWheel;
    public WheelCollider RRWheel;

    public IEnumerator<WheelCollider> GetEnumerator()
    {
        yield return FLWheel;
        yield return FRWheel;
        yield return RLWheel;
        yield return RRWheel;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

[Serializable]
public class WheelMeshes
{
    public MeshRenderer FLWheel;
    public MeshRenderer FRWheel;
    public MeshRenderer RLWheel;
    public MeshRenderer RRWheel;
}



public class CarController : MonoBehaviour
{
    public WheelColliders wheelColls;
    public WheelMeshes wheelMeshes;
    private float throttleInput;
    private float brakeInput;
    private float steerInput;
    public AnimationCurve steeringCurve;
    private float targetSteerAngle = 0;
    private float steerSpeed = 5f;

    public float enginePower;
    public float brakeFrontBias;
    public float brakeForce;
    float slipAngle;
    public float speed;

    /*
    public bool isEngineOn;
    public float RPM;
    public float redLine;
    public float idleRPM;
    */

    public Rigidbody carRb;
    public bool playerControl = true;
    public bool forcedHandbrake = false;

    void Start()
    {
        carRb = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        GetInput();
        UpdateWheels();
        speed = carRb.velocity.magnitude;
    }

    void LateUpdate()
    {
        ApplyTorque();
        ApplyBrake();
        ApplySteering();
        Handbrake();
    }

    void GetInput()
    {
        if (playerControl)
        {
            throttleInput = Input.GetAxis("Vertical");
            steerInput = Input.GetAxis("Horizontal");
            slipAngle = Vector3.Angle(transform.forward, carRb.velocity - transform.forward);
            if (slipAngle < 120f)
            {
                if (throttleInput < 0)
                {
                    brakeInput = Mathf.Abs(throttleInput);
                    throttleInput = 0;
                }
                else
                {
                    brakeInput = 0;
                }
            }
            else
            {
                brakeInput = 0;
            }
        }
    }

    public void SetInput(float throttle, float steer)
    {
        throttleInput = throttle;
        steerInput = steer;
    }

    void ApplyTorque()
    {
        wheelColls.RRWheel.motorTorque = enginePower * throttleInput;
        wheelColls.RLWheel.motorTorque = enginePower * throttleInput;
    }

    void ApplyBrake()
    {
        wheelColls.FRWheel.brakeTorque = brakeInput * brakeForce * brakeFrontBias;
        wheelColls.FLWheel.brakeTorque = brakeInput * brakeForce * brakeFrontBias;
        wheelColls.RRWheel.brakeTorque = brakeInput * brakeForce * (1-brakeFrontBias);
        wheelColls.RLWheel.brakeTorque = brakeInput * brakeForce * (1-brakeFrontBias);
    }

    void ApplySteering()
    {
        float steeringAngle = steerInput * steeringCurve.Evaluate(speed);
        if (Vector3.Dot(carRb.velocity, transform.forward) > 0.1f)
            steeringAngle += Vector3.SignedAngle(transform.forward, carRb.velocity + transform.forward, Vector3.up) * (1f / 5f);
        steeringAngle = Mathf.Clamp(steeringAngle, -60, 60);
        wheelColls.FRWheel.steerAngle = Mathf.Lerp(wheelColls.FLWheel.steerAngle, steeringAngle, Time.deltaTime * steerSpeed);
        wheelColls.FLWheel.steerAngle = Mathf.Lerp(wheelColls.FLWheel.steerAngle, steeringAngle, Time.deltaTime * steerSpeed);
    }

    void Handbrake()
    {
        List<WheelCollider> colls = new()
        {
            wheelColls.RRWheel,
            wheelColls.RLWheel
        };
        foreach (WheelCollider coll in colls)
        {
            coll.brakeTorque = Convert.ToInt32((Input.GetKey(KeyCode.Space) && playerControl) || forcedHandbrake) * 1200 * brakeForce * Time.deltaTime;
        }
    }
    
    void UpdateWheels()
    {
        UpdateWheel(wheelColls.FRWheel, wheelMeshes.FRWheel);
        UpdateWheel(wheelColls.FLWheel, wheelMeshes.FLWheel);
        UpdateWheel(wheelColls.RRWheel, wheelMeshes.RRWheel);
        UpdateWheel(wheelColls.RLWheel, wheelMeshes.RLWheel);
    }
    void UpdateWheel(WheelCollider coll, MeshRenderer wheelMesh)
    {
        Quaternion rot;
        Vector3 pos;
        coll.GetWorldPose(out pos, out rot);
        wheelMesh.transform.position = pos;
        wheelMesh.transform.rotation = rot;
    }

    public void EnablePlayerControl(bool enable)
    {
        playerControl = enable;
        forcedHandbrake = !enable;
        if (!enable)
        {
            throttleInput = 0;
            steerInput = 0;
        }
    }
}
