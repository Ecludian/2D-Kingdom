using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraActive : MonoBehaviour
{
    public Transform Target;
    public Vector2 minPos;
    public Vector2 maxPos;
    public float Speed;

    private void LateUpdate()
    {
        if (transform.position != Target.position)
        {
            var targetPos = new Vector3(Target.position.x, Target.position.y, transform.position.z);
            targetPos.x = Mathf.Clamp(targetPos.x, minPos.x, maxPos.x);
            targetPos.y = Mathf.Clamp(targetPos.y, minPos.y, maxPos.y);
            transform.position = Vector3.Lerp(transform.position, targetPos, Speed);
        }
    }
}
