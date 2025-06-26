using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

[Serializable]
public class WheelColliders
{
    public WheelCollider FLWheel;
    public WheelCollider FRWheel;
    public WheelCollider RLWheel;
    public WheelCollider RRWheel;
}

[Serializable]
public class WheelMeshes
{
    public MeshRenderer FLWheel;
    public MeshRenderer FRWheel;
    public MeshRenderer RLWheel;
    public MeshRenderer RRWheel;
}
[Serializable]
public class WheelSmoke
{
    public ParticleSystem FLWheel;
    public ParticleSystem FRWheel;
    public ParticleSystem RLWheel;
    public ParticleSystem RRWheel;
}

public class CarController : MonoBehaviour
{
    public WheelColliders wheelColls;
    public WheelMeshes wheelMeshes;
    private float throttleInput;
    private float brakeInput;
    private float steerInput;
    public AnimationCurve steeringCurve;
    public WheelSmoke wheelSmoke;
    public GameObject smokeParticle;

    public float enginePower;
    public float brakeForce;
    float slipAngle;
    public float speed;
    public bool isEngineOn;

    public float RPM;
    public float redLine;
    public float idleRPM;

    public Rigidbody carRb;
    private bool debugged = false;
    public bool playerControl = true;
    public bool forcedHandbrake = false;

    void Start()
    {
        carRb = gameObject.GetComponent<Rigidbody>();
        speed = carRb.velocity.magnitude;
        CreateSmoke();
    }

    void Update()
    {
        GetInput();
        UpdateWheels();
    }

    void LateUpdate()
    {
        ApplyTorque();
        ApplyBrake();
        Handbrake();
        CheckSlip();
        ApplySteering();
    }

    void GetInput()
    {
        if (playerControl)
        {
            throttleInput = Input.GetAxis("Vertical");
            steerInput = Input.GetAxis("Horizontal");
        }
        else
        {
            throttleInput = 0;
            steerInput = 0;
        }
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

    void ApplyTorque()
    {
        wheelColls.RRWheel.motorTorque = enginePower * throttleInput;
        wheelColls.RLWheel.motorTorque = enginePower * throttleInput;
    }

    void ApplyBrake()
    {
        wheelColls.FRWheel.brakeTorque = brakeInput * brakeForce * 0.6f;
        wheelColls.FLWheel.brakeTorque = brakeInput * brakeForce * 0.6f;
        wheelColls.RRWheel.brakeTorque = brakeInput * brakeForce * 0.4f;
        wheelColls.RLWheel.brakeTorque = brakeInput * brakeForce * 0.4f;
    }

    void ApplySteering()
    {
        float steeringAngle = steerInput * steeringCurve.Evaluate(speed);
        steeringAngle += Vector3.SignedAngle(transform.forward, carRb.velocity + transform.forward, Vector3.up) * (3f/4f);
        steeringAngle = Mathf.Clamp(steeringAngle, -20, 20);
        wheelColls.FRWheel.steerAngle = steeringAngle;
        wheelColls.FLWheel.steerAngle = steeringAngle;
    }

    void Handbrake()
    {
        wheelColls.RRWheel.brakeTorque =
            Convert.ToInt32(Input.GetKey(KeyCode.Space) || forcedHandbrake) * 1200 * brakeForce * Time.deltaTime;
        wheelColls.RLWheel.brakeTorque =
            Convert.ToInt32(Input.GetKey(KeyCode.Space) || forcedHandbrake) * 1200 * brakeForce * Time.deltaTime;
    }

    void CreateSmoke()
    {
        wheelSmoke.FRWheel
            = Instantiate(smokeParticle, wheelColls.FRWheel.transform.position-Vector3.up*wheelColls.FRWheel.radius,
            Quaternion.identity, wheelColls.FRWheel.transform)
            .GetComponent<ParticleSystem>();
        wheelSmoke.FLWheel
            = Instantiate(smokeParticle, wheelColls.FLWheel.transform.position-Vector3.up*wheelColls.FLWheel.radius,
            Quaternion.identity, wheelColls.FLWheel.transform)
            .GetComponent<ParticleSystem>();
        wheelSmoke.RLWheel
            = Instantiate(smokeParticle, wheelColls.RLWheel.transform.position-Vector3.up*wheelColls.RLWheel.radius,
            Quaternion.identity, wheelColls.RLWheel.transform)
            .GetComponent<ParticleSystem>();
        wheelSmoke.RRWheel
            = Instantiate(smokeParticle, wheelColls.RRWheel.transform.position-Vector3.up*wheelColls.RRWheel.radius,
            Quaternion.identity, wheelColls.RRWheel.transform)
            .GetComponent<ParticleSystem>();
    }

    void CheckSlip()
    {
        WheelHit[] wheelHits = new WheelHit[4];
        wheelColls.FRWheel.GetGroundHit(out wheelHits[0]);
        wheelColls.FLWheel.GetGroundHit(out wheelHits[1]);
        wheelColls.RRWheel.GetGroundHit(out wheelHits[2]);
        wheelColls.RLWheel.GetGroundHit(out wheelHits[3]);

        float slipAllowance = 0.7f;

        if (Mathf.Abs(wheelHits[0].sidewaysSlip) + Mathf.Abs(wheelHits[0].forwardSlip) > slipAllowance)
        {
            wheelSmoke.FRWheel.Play();
        }
        else
        {
            wheelSmoke.FRWheel.Stop();
        }

        if (Mathf.Abs(wheelHits[1].sidewaysSlip) + Mathf.Abs(wheelHits[1].forwardSlip) > slipAllowance)
        {
            wheelSmoke.FLWheel.Play();
        }
        else
        {
            wheelSmoke.FLWheel.Stop();
        }

        if (Mathf.Abs(wheelHits[2].sidewaysSlip) + Mathf.Abs(wheelHits[2].forwardSlip) > slipAllowance)
        {
            wheelSmoke.RRWheel.Play();
        }
        else
        {
            wheelSmoke.RRWheel.Stop();
        }

        if (Mathf.Abs(wheelHits[3].sidewaysSlip) + Mathf.Abs(wheelHits[3].forwardSlip) > slipAllowance)
        {
            wheelSmoke.RLWheel.Play();
        }
        else
        {
            wheelSmoke.RLWheel.Stop();
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
        if (!enable && !debugged)
        {
            Debug.Log("Player control disabled!");
            debugged = true;
        }
        playerControl = enable;
        forcedHandbrake = !enable;
    }
    /*
        void LateUpdate()
        {
            Move();
            Steer();
            Handbrake();
        }

        void GetInputs()
        {
            moveInput = Convert.ToInt32(playerControl) * Input.GetAxis("Vertical");
            steerInput = Convert.ToInt32(playerControl) * Input.GetAxis("Horizontal");
        }

        void Move()
        {
            foreach(var wheel in wheels)
            {
                if(wheel.axel == Axel.Rear)
                {
                    wheel.WheelCollider.motorTorque = moveInput * 580 * maxAccel * Time.deltaTime;
                }
            }
        }

        void Steer()
        {
            foreach(var wheel in wheels)
            {
                if (wheel.axel == Axel.Front)
                {
                    var _steerAngle = steerInput * turnSens * maxSteerAngle;
                    wheel.WheelCollider.steerAngle = Mathf.Lerp(wheel.WheelCollider.steerAngle, _steerAngle, 0.6f);
                }
            }
        }

        void Handbrake()
        {
            foreach(var wheel in wheels)
            {
                if(wheel.axel == Axel.Rear)
                {
                    wheel.WheelCollider.brakeTorque =
                        Convert.ToInt32(Input.GetKey(KeyCode.Space) || forcedHandbrake) * 600 * brakeAccel * Time.deltaTime;
                }
            }
        }

        void AnimatedWheels()
        {
            foreach(var wheel in wheels)
            {
                Quaternion rot;
                Vector3 pos;
                wheel.WheelCollider.GetWorldPose(out pos, out rot);
                wheel.wheelModel.transform.position = pos;
                wheel.wheelModel.transform.rotation = rot;

            }
        }
    */
}
