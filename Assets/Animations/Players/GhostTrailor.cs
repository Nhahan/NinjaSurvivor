using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostTrailor : MonoBehaviour
{
    private float _liveTime;

    private void Start()
    {
        var normal = (GameManager.Instance.GetPlayer().transform.position - transform.position).normalized;
        transform.localScale = new Vector2(Mathf.Sign(normal.x), 1f);
    }

    private void Update()
    {
        _liveTime += Time.deltaTime;
        if (_liveTime > 1)
        {
            Destroy(gameObject);
        }
    }
}
