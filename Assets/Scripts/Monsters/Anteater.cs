using System;
using System.Collections;
using Status;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Monsters
{
    public class Anteater : Monster, IMonster
    {
        private Player _player;
        private Animator _animator;
        private Rigidbody2D _rb;
    
        private float _monsterHp = 80f;
        private float _randomDamage;
        private float _monsterSpeed = 1.4f;
        private float _monsterSpeedMultiplier = 1;
        private float _distance;
        private const float MonsterDefense = 4f;

        private float _attackCooltime;
    
        private void Start()
        {
            _player = GameManager.Instance.GetPlayer();
            _animator = GetComponent<Animator>();
            _rb = GetComponent<Rigidbody2D>();
            
            Indicator = GameManager.Instance.indicator;

            _randomDamage = Random.Range(8, 12);
            KnockbackDuration = 0.11f;
        }

        private void FixedUpdate()
        {
            _attackCooltime += Time.deltaTime;
            _distance = Vector3.Distance(transform.position, _player.transform.position);

            if (_distance < 1.1 && _attackCooltime > 1.3f)
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
                    if (_attackCooltime > 1.3f)
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
            var finalDamage = damage + MonsterDefense;
            _monsterHp = _monsterHp - finalDamage;
            ShowDamage(finalDamage);
            Flash();

            if (_monsterHp > 0) return;
            _animator.SetBool("isDead", true);
            _monsterSpeedMultiplier = 0;
            StartCoroutine(BeforeDestroy(0.05f));
        }
    }
}
