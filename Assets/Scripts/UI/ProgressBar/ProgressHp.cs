using System;
using System.Runtime.CompilerServices;
using Status;
using UnityEngine;

public class ProgressHp : MonoBehaviour
{
    private Player _player;
    private float _maxHp;
    private float _currentHp;
    private RectTransform _x; 
    private float _y;
    private float _z;

    private void Start()
    {
        _player = GameManager.Instance.GetPlayer();
        _maxHp = _player.MaxHp.CalculateFinalValue();
        _x = GetComponent<RectTransform>();
        _y = GetComponent<RectTransform>().localScale.y;
        _z = GetComponent<RectTransform>().localScale.z;
    }

    private void FixedUpdate()
    {
        _currentHp = _player.Hp.CalculateFinalValue();
        _x.localScale = new Vector3(_currentHp / _maxHp, _y, _z);
    }
}
