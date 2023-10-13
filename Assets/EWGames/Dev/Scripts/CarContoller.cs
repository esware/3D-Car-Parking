using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarContoller : MonoBehaviour
{
    public enum Axel
    {
        Front,
        Rear
    }
    
    [Serializable]public struct Wheel
    {
        public GameObject wheelModel;
        public WheelCollider wheelCollider;
        public Axel axel;
    }

    public float maxAcceleration = 30f;
    public float brakeAcceleration = 50f;
    public float turnSensivity = 1.0f;
    public float maxSteerAngle = 45.0f;

    public List<Wheel> wheels;
    public Vector3 centerOfMass;

    public float speed = 0f;
    public float maxSpeed = 120f;
    public float _time = 0f;

    public AnimationCurve accelerationCurve;
    public MeshRenderer brakeRenderer;

    private Rigidbody _rigidbody;
    private float _moveInput;
    private float _steerInput;
    private bool _brakeInput;
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.centerOfMass = centerOfMass;
    }

    private void Update()
    {
        GetInputs();
        AnimateWheels();
    }

    private void LateUpdate()
    {
        Move();
        Steer();
        Brake();
    }

    void GetInputs()
    {
        _moveInput = Input.GetAxis("Vertical");
        _steerInput = Input.GetAxis("Horizontal");
        _brakeInput = Input.GetKey(KeyCode.Space);
    }

    void Move()
    {
        if (_moveInput==0)
        {
            _time = 0f;
        }

        _time += Time.deltaTime;
        speed = _moveInput * _time*maxAcceleration;
        speed=Mathf.Clamp(speed, -maxSpeed, maxSpeed);
        
        foreach (var wheel in wheels)
        {
            wheel.wheelCollider.motorTorque = speed;
          
        }
    }

    void Steer()
    {
        foreach (var wheel in wheels)
        {
            if (wheel.axel == Axel.Front)
            {
                var steerAngle = _steerInput * turnSensivity * maxSteerAngle;
                wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, steerAngle,0.6f);
            }
        }
    }
    
    void AnimateWheels()
    {
        foreach (var wheel in wheels)
        {
            wheel.wheelCollider.GetWorldPose(out var pos,out var rot);
            wheel.wheelModel.transform.position = pos;
            wheel.wheelModel.transform.rotation = rot;
        }
    }

    void Brake()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (var m in brakeRenderer.materials)
            {
                m.EnableKeyword("_EMISSION");
            }
        }
        
        if (Input.GetKeyUp(KeyCode.Space))
        {
            foreach (var m in brakeRenderer.materials)
            {
                m.DisableKeyword("_EMISSION");
            }
            
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque =0f;
            }
        }
        
        if (Input.GetKey(KeyCode.Space))
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = 300 * (_brakeInput?1:0) * brakeAcceleration * Time.deltaTime;
            }
        }

    }
}
