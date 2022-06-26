using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostTrailor : MonoBehaviour
{
    private float _liveTime;

    private void Update()
    {
        _liveTime += Time.deltaTime;
        if (_liveTime > 1)
        {
            Destroy(gameObject);
        }
    }
}
