using System;
using UnityEngine;
public class CarBehaviour : MonoBehaviour
{
    public WheelCollider wheelColliderFl;
    public WheelCollider wheelColliderFr;
    public WheelCollider wheelColliderBl;
    public WheelCollider wheelColliderBr;
    public Rigidbody rigidBody;
    public float maxSpeedKmh = 150f;
    public float maxSpeedBackwardKmh = 30f;
    public float maxTorque = 700;
    public float maxSteerAngle = 50;
    public float sidewaysStiffness = 1.5f;
    public float forwardStiffness = 1.5f;
    private float _currentSpeedKmh = 0;
    void Start()
    {
        SetWheelFrictionStiffness(forwardStiffness, sidewaysStiffness);
    }
    void FixedUpdate ()
    {
        _currentSpeedKmh = rigidBody.velocity.magnitude * 3.6f;
        var currentTorque = maxTorque;
        if (_currentSpeedKmh >= maxSpeedKmh)
        {
            currentTorque = 0;
        }
        
        SetMotorTorque(currentTorque * Input.GetAxis("Vertical"));
        
        SetSteerAngle(maxSteerAngle * Input.GetAxis("Horizontal"));
        print(_currentSpeedKmh);
    }
    void SetSteerAngle(float angle)
    {   
        wheelColliderFl.steerAngle = angle;
        wheelColliderFr.steerAngle = angle;
    }
    void SetMotorTorque(float amount)
    {   
        wheelColliderFl.motorTorque = amount;
        wheelColliderFr.motorTorque = amount;
    }
    
    
    void SetWheelFrictionStiffness(float newForwardStiffness, float newSidewaysStiffness)
    {
        WheelFrictionCurve fwWfc = wheelColliderFl.forwardFriction;
        WheelFrictionCurve swWfc = wheelColliderFl.sidewaysFriction;
        fwWfc.stiffness = newForwardStiffness;
        swWfc.stiffness = newSidewaysStiffness;
        wheelColliderFl.forwardFriction = fwWfc; 
        wheelColliderFl.sidewaysFriction = swWfc; 
        wheelColliderFr.forwardFriction = fwWfc; 
        wheelColliderFr.sidewaysFriction = swWfc;
        wheelColliderBl.forwardFriction = fwWfc; 
        wheelColliderBl.sidewaysFriction = swWfc; 
        wheelColliderBr.forwardFriction = fwWfc; 
        wheelColliderBr.sidewaysFriction = swWfc;
    }
}