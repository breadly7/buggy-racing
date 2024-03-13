using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelBehaviour : MonoBehaviour
{
    public WheelCollider wheelCol;

    void Update()
    {
        // Get the wheel position and rotation from the wheel collider
        wheelCol.GetWorldPose(out Vector3 position, out Quaternion rotation); 
        
        var myTransform = transform;
        myTransform.position = position;
        myTransform.rotation = rotation;
    }
}
