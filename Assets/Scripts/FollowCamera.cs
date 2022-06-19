using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private Transform _thingToFollow;

    private void Start()
    {
        _thingToFollow = GameObject.FindWithTag("Player").transform;
    }

    private void LateUpdate()
    {
        transform.position = _thingToFollow.position + new Vector3(0, 0, -10);
    }
}
