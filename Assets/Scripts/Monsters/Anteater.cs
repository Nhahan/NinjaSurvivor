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
    
        private float _monsterHp = 35f;
        private const float MonsterDamage = 10f;
        private float _randomDamage;
        private const float MonsterSpeed = 1.6f;
        private float _monsterSpeedMultiplier = 1;
        private float _distance;
        private const float MonsterDefense = 2f;

        private float _attackCooltime;
    
        private void Start()
        {
            _player = GameManager.Instance.GetPlayer();
            _animator = GetComponent<Animator>();
            _indicator = GameManager.Instance.indicator;

            _randomDamage = Random.Range(8, 12);
            KnockbackDuration = 0.11f;
        }

        private void FixedUpdate()
        {
            _attackCooltime += Time.deltaTime;
            _distance = Vector3.Distance(transform.position, _player.transform.position);

            if (_monsterHp < 0)
            {
                _monsterSpeedMultiplier = 0;
            }

            if (_distance < 1.1 && _attackCooltime > 1.125f)
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
                    _monsterSpeedMultiplier = 1;
                    transform.position = Vector2.MoveTowards(
                        transform.position,
                        _player.transform.position,
                        MonsterSpeed * _monsterSpeedMultiplier * Time.deltaTime);
                    FlipSprite();
                    break;
                case State.Attacking:
                    _monsterSpeedMultiplier = 0;
                    AttackPlayer();
                    _attackCooltime = 0;
                    break;
                case State.Dead:
                    _monsterSpeedMultiplier = 0;
                    _animator.SetBool("isDead", true);
                    StartCoroutine(BeforeDestroy(_animator.GetCurrentAnimatorStateInfo(0).length));
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
            _monsterSpeedMultiplier = 1;
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
            StartCoroutine(BeforeDestroy(_animator.GetCurrentAnimatorStateInfo(0).length));
        }
    }
}
