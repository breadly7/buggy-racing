using UnityEngine;

public class AntiRollBehaviour : MonoBehaviour
{
    public WheelCollider wheelL;
    public WheelCollider wheelR;
    public float antiRoll = 5000f;
    public Rigidbody carRigidBody;

    void FixedUpdate()
    {
        WheelHit hit;
        float travelL = 1f;
        float travelR = 1f;

        var groundedL = wheelL.GetGroundHit(out hit);
        if (groundedL)
        {
            travelL = (-wheelL.transform.InverseTransformPoint(hit.point).y - wheelL.radius) / wheelL.suspensionDistance;
        }

        var groundedR = wheelR.GetGroundHit(out hit);
        if (groundedR)
        {
            travelR = (-wheelR.transform.InverseTransformPoint(hit.point).y - wheelR.radius) / wheelR.suspensionDistance;
        }

        float antiRollForce = (travelL - travelR) * antiRoll;

        if (groundedL)
        {
            carRigidBody.AddForceAtPosition(wheelL.transform.up * -antiRollForce, wheelL.transform.position);
        }

        if (groundedR)
        {
            carRigidBody.AddForceAtPosition(wheelR.transform.up * antiRollForce, wheelR.transform.position);  
        }
    }
}


