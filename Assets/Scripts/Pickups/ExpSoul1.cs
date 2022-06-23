using System;
using System.Collections;
using System.Collections.Generic;
using Status;
using UnityEngine;

public class ExpSoul1 : MonoBehaviour
{
    private Player _player;
    private bool _isTriggered;
    private float _liveTime;
    
    private void Start()
    {
        _player = GameManager.Instance.GetPlayer();
    }

    private void FixedUpdate()
    {
        if (_isTriggered)
        {
            _liveTime += Time.deltaTime * 0.3f;
            transform.position = Vector2.MoveTowards(
                transform.position, 
                _player.transform.position, 
                _liveTime * _liveTime);
        }

        if (_isTriggered && transform.position == _player.transform.position)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag.Equals("PickupRadius"))
        {
            _isTriggered = true;
        }
    }

    private void OnDestroy()
    {
        _player.EarnExp(1);
    }
}
