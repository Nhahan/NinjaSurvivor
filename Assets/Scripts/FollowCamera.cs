using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    Transform thingToFollow;

    void Start()
    {
        thingToFollow = GameObject.FindWithTag("Player").transform;
    }

    void LateUpdate()
    {
        transform.position = thingToFollow.position + new Vector3(0, 0, -10);
    }
}
