using System;
using UnityEngine;

public class GhostTrailor : MonoBehaviour
{
    private float _liveTime;

    private void Start()
    {
        try
        {
            var normal = (GameManager.Instance.GetPlayer().transform.position - transform.position).normalized;
            transform.localScale = new Vector2(Mathf.Sign(normal.x), 1f);
        }
        catch
        {
            throw new ArgumentNullException();
        }
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
