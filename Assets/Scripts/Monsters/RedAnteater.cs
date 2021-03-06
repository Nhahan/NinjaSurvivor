using System;
using System.Collections;
using Status;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Monsters
{
    public class RedAnteater : Monster, IMonster
    {
        private Player _player;
        private Animator _animator;
        private Rigidbody2D _rb;
    
        private float _monsterHp = 140;
        private const float MonsterDamage = 10f;
        private float _randomDamage;
        private float _monsterSpeed = 1.85f;
        private float _monsterSpeedMultiplier = 1;
        private float _distance;
        private const float MonsterDefense = 4.5f;

        private float _attackCooltime;
    
        private void Start()
        {
            _player = GameManager.Instance.GetPlayer();
            _animator = GetComponent<Animator>();
            _rb = GetComponent<Rigidbody2D>();
            
            Indicator = GameManager.Instance.indicator;

            _randomDamage = Random.Range(5, 25);

            _monsterHp += _player.Level * 1.5f;
            KnockbackDuration = 0.11f;
        }

        private void FixedUpdate()
        {
            _attackCooltime += Time.deltaTime;
            _distance = Vector3.Distance(transform.position, _player.transform.position);

            if (_distance < 1.2f && _attackCooltime > 0.92f)
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
                    FlipSprite();
                    if (_attackCooltime > 1.15f && _monsterHp > 0)
                    {
                        _monsterSpeedMultiplier = 1;
                    }
                    break;
                case State.Attacking:
                    _monsterSpeedMultiplier = 0;
                    AttackPlayer();
                    _attackCooltime = 0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    
        private void AttackPlayer()
        {
            _animator.SetBool("isAttacking", true);
            var finalDamage = _randomDamage;
            _player.TakeDamage(finalDamage);
            StartCoroutine(IsAttackingToFalse(_animator.GetCurrentAnimatorStateInfo(0).length));
        }

        private IEnumerator IsAttackingToFalse(float second)
        {
            yield return new WaitForSeconds(second);
            _animator.SetBool("isAttacking", false);
        }

        private void FlipSprite()
        {
            transform.localScale = transform.position.x < _player.transform.position.x ? new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);
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
