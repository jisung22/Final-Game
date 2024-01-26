using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform followTarget;
    public Vector3 offset = new Vector3(0, 10f, -10f);
    public float followSpeed = 10f;

    void LateUpdate()
    {
        var targetPosition = followTarget.position + offset;

        //transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);
        transform.position = targetPosition;
        // transform.LookAt(followTarget);
    }
}
