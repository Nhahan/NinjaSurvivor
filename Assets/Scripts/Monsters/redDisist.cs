using System;
using System.Collections;
using Status;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Monsters
{
    public class RedDisist : Monster, IMonster
    {
        private Player _player;
        private Animator _animator;
        private Rigidbody2D _rb;
    
        private float _monsterHp = 105f;
        private const float MonsterDamage = 10f;
        private float _randomDamage;
        private float _monsterSpeed = 0.8f;
        private float _monsterSpeedMultiplier = 1;
        private float _distance;
        private const float MonsterDefense = 1f;

        private float _attackCooltime;
    
        private void Start()
        {
            _player = GameManager.Instance.GetPlayer();
            _animator = GetComponent<Animator>();
            _rb = GetComponent<Rigidbody2D>();
            
            Indicator = GameManager.Instance.indicator;

            _monsterHp += _player.GetLevel() * 2f;

            _randomDamage = Random.Range(10, 20) * 1.5f;
            KnockbackDuration = 0.11f;
        }

        private void FixedUpdate()
        {
            try
            {
                _attackCooltime += Time.deltaTime;
                _distance = Vector3.Distance(transform.position, _player.transform.position);

                if (_distance < 1.5f && _attackCooltime > 1.1f)
                {
                    _state = State.Attacking;
                }
                else
                {
                    _state = State.Moving;
                }

                if (KnockbackTimer > 0)
                {
                    PlayKnockback();
                    return;
                }

                switch (_state)
                {
                    case State.Moving:
                        Vector2 direction = (_player.transform.position - transform.position).normalized;
                        _rb.MovePosition(_rb.position + direction * (_monsterSpeed * Time.fixedDeltaTime * _monsterSpeedMultiplier));
                        if (_attackCooltime > 1.1f && _monsterHp > 0)
                        {
                            _monsterSpeedMultiplier = 1;
                        }

                        break;
                    case State.Attacking:
                        _monsterSpeedMultiplier = 0;
                        AttackPlayer();
                        _attackCooltime = 0;
                        break;
                }
            }
            catch
            {
                // ignore
            }
        }
    
        private void AttackPlayer()
        {
            var finalDamage = _randomDamage + MonsterDamage;
            _player.TakeDamage(finalDamage);
        }

        public void TakeDamage(float damage)
        { 
            _monsterHp = _monsterHp - damage + MonsterDefense;
            ShowDamage(damage);
            Flash();

            if (_monsterHp > 0) return;
            _animator.SetBool("isDead", true);
            _monsterSpeedMultiplier = 0;
            StartCoroutine(BeforeDestroy(_animator.GetCurrentAnimatorStateInfo(0).length));
        }
    }
}
