using UnityEngine;

public class CarBehaviour : MonoBehaviour
{
    public WheelCollider wheelColliderFl;
    public WheelCollider wheelColliderFr;
    public WheelCollider wheelColliderBl;
    public WheelCollider wheelColliderBr;
    public Rigidbody rigidBody;
    public float maxSpeedKmh = 150f;
    public int nGears = 5;
    public float maxSpeedBackwardKmh = 30f;
    public float maxTorque = 1000;
    public float maxSteerAngle = 50;
    public float sidewaysStiffness = 1.5f;
    public float forwardStiffness = 1.5f;
    private float _currentSpeedKmh = 0;
    public Transform centerOfMass;
    private Rigidbody _rigidbody;
    private bool _movingForward;
    
    void Start()
    {
        SetCenterofMass();
        SetWheelFrictionStiffness(forwardStiffness, sidewaysStiffness);
        SetMotorTorque(0);
    }
    void FixedUpdate ()
    {
        // Determine if the car is driving forwards or backwards
        _movingForward = Vector3.Angle(transform.forward, _rigidbody.velocity) < 50f;
        _currentSpeedKmh = rigidBody.velocity.magnitude * 3.6f;
        
        HandlePower();
        SetSteerAngle(maxSteerAngle * Input.GetAxis("Horizontal") * ( 1 - _currentSpeedKmh / maxSpeedKmh * 0.95f));
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

    void SetCenterofMass()
    {
        _rigidbody = GetComponent<Rigidbody>();
        var localPosition = centerOfMass.localPosition;
        _rigidbody.centerOfMass = new Vector3(localPosition.x, localPosition.y, localPosition.z);
    }

    float GetTorqueBySpeed()
    {
        float maxSpeedReference = maxSpeedKmh;
        if (!_movingForward)
        {
            maxSpeedReference = maxSpeedBackwardKmh;
        }
        
        // WIP for changing gears
        var actualTorque = 0f;
        
        for (float i = 0; i < nGears; i++)
        {
            if (_currentSpeedKmh > (maxSpeedReference / nGears * i))
            {
                float gearReduction = i / nGears + 0.15f;
                actualTorque = maxTorque *(1f - gearReduction*(_currentSpeedKmh - maxSpeedReference / nGears * i) / (maxSpeedReference / nGears));
            }
        }
        
        return actualTorque;
    }

    void HandlePower()
    {
        bool doBraking = _currentSpeedKmh > 0.5f && (Input.GetAxis("Vertical") < 0 && _movingForward ||
                                                     Input.GetAxis("Vertical") > 0 && !_movingForward);
        if (doBraking)
        {   wheelColliderFl.brakeTorque = 5000;
            wheelColliderFr.brakeTorque = 5000;
            wheelColliderBl.brakeTorque = 5000;
            wheelColliderBr.brakeTorque = 5000;
            wheelColliderFl.motorTorque = 0;
            wheelColliderFr.motorTorque = 0;
        } else
        {   wheelColliderFl.brakeTorque = 0;
            wheelColliderFr.brakeTorque = 0;
            wheelColliderBl.brakeTorque = 0;
            wheelColliderBr.brakeTorque = 0;

            var torque = GetTorqueBySpeed() * Input.GetAxis("Vertical");
            wheelColliderFl.motorTorque = torque; 
            wheelColliderFr.motorTorque = torque;
            wheelColliderBr.motorTorque = torque;
            wheelColliderBl.motorTorque = torque;
        }
    }
}