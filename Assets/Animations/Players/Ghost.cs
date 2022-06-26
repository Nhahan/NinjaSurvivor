using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    private float _ghostDelaySeconds = 0.2f;

    [SerializeField] private GameObject ghost;

    private void Start()
    {
        var normal = (GameManager.Instance.GetPlayer().transform.position - transform.position).normalized;
        transform.localScale = new Vector2(Mathf.Sign(normal.x), 1f);
    }

    private void Update()
    {
            if (_ghostDelaySeconds > 0)
            {
                _ghostDelaySeconds -= Time.deltaTime;
            }
            else
            {
                Instantiate(ghost, transform.position, transform.rotation);
                _ghostDelaySeconds = 0.2f;
            }
    }
}
