using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CarController : MonoBehaviour
{
    public enum Axel
    {
        Front,
        Rear
    }

    [Serializable]
    public struct Wheel
    {
        public GameObject wheelModel;
        public WheelCollider WheelCollider;
        public Axel axel;
    }

    public float maxAccel = 60.0f;
    public float brakeAccel = 70.0f;
    
    public float turnSens = 1.0f;
    public float maxSteerAngle = 24.0f;

    public Vector3 _centerOfMass;

    public List<Wheel> wheels;

    float moveInput;
    float steerInput;

    private Rigidbody carRb;

    void Start()
    {
            carRb = GetComponent<Rigidbody>();
            carRb.centerOfMass = _centerOfMass;
    }

    void Update()
    {
        GetInputs();
        AnimatedWheels();
    }

    void LateUpdate()
    {
        Move();
        Steer();
        Handbrake();
    }

    void GetInputs()
    {
        moveInput = Input.GetAxis("Vertical");
        steerInput = Input.GetAxis("Horizontal");
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
        if (Input.GetKey(KeyCode.Space))
        {
            foreach(var wheel in wheels)
            {
                if(wheel.axel == Axel.Rear)
                {
                    wheel.WheelCollider.brakeTorque = 600 * brakeAccel * Time.deltaTime;
                }
            }
        }
        else
        {
            foreach(var wheel in wheels)
            {
                if(wheel.axel == Axel.Rear)
                {
                    wheel.WheelCollider.brakeTorque = 0;
                }
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
}
