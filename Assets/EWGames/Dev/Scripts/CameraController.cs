using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSmoothness;
    public float rotationSmoothess;
    
    public Vector3  moveOffset;
    public Vector3 rotationOffset;
    
    public Transform target;
    
    void LateUpdate()
    {
        FollowTarget();
    }

    private void FollowTarget()
    {
        HandleMovement();
        HandleRotation();
    }

    private void HandleRotation()
    {
        var direction = target.position - transform.position;
        var rotation = new Quaternion();
        
        rotation = Quaternion.LookRotation(direction+rotationOffset,Vector3.up);
        
        transform.rotation=Quaternion.Slerp(transform.rotation,rotation,rotationSmoothess*Time.deltaTime);
    }

    private void HandleMovement()
    {
        Vector3 targetPosition = new Vector3();
        targetPosition = target.TransformPoint(moveOffset);

        transform.position = Vector3.Slerp(transform.position, targetPosition, moveSmoothness * Time.deltaTime);
    }
}
