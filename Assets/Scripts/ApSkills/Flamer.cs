using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Monsters;
using Status;
using UnityEngine;

public class Flamer : MonoBehaviour
{
    [SerializeField] private float damageMultiplier = 1;

    private Player _player;
    private Transform _flamer;
    private Animator _animator;
    private Rigidbody2D _playerRb;
    private Vector2 _fireDirection;
    
    private bool _isAvailable = true;

    private void Awake()
    {
        _player = GameManager.Instance.GetPlayer();
        if (_player.Flamer.CalculateFinalValue() < 1) { Destroy(gameObject); }
    }

    private void Start()
    {
        _playerRb = _player.GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _flamer = _player.transform.Find("SkillPoints").Find("Flamer");
        var animationLength = _animator.GetCurrentAnimatorStateInfo(0).length;
        Invoke(nameof(ToNotAvailable), animationLength / 0.8f);
        StartCoroutine(BeforeDestroy(_animator.GetCurrentAnimatorStateInfo(0).length));
    }

    private IEnumerator BeforeDestroy(float second)
    {
        yield return new WaitForSeconds(second);
        Destroy(gameObject);
    }

    private void Update()
    {
        _fireDirection = _playerRb.velocity;
        FlipSprite();
        if (_isAvailable)
        {
            transform.position = _flamer.position;
        }
    }

    private void ToNotAvailable()
    {
        _isAvailable = false;
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (!_isAvailable) return;
        if (!coll.CompareTag("Enemy")) return;

        var monster = coll.gameObject.GetComponent<IMonster>();
        var skillLevelBonus = (float)(1 + 1.5 * _player.Flamer.CalculateFinalValue());
        var damage = _player.AttackDamage.CalculateFinalValue() * damageMultiplier * skillLevelBonus;

        monster.Flamer(damage);
    }
    
    private void FlipSprite()
    {
        transform.localScale = new Vector2(Mathf.Sign(_fireDirection.x), 1f);
    }
}
