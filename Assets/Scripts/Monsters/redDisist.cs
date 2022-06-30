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
    
        private float _monsterHp = 18f;
        private const float MonsterDamage = 10f;
        private float _randomDamage;
        private float _monsterSpeed = 1.65f;
        private float _monsterSpeedMultiplier = 1;
        private float _distance;
        private const float MonsterDefense = 5f;

        private float _attackCooltime;
    
        private void Start()
        {
            _player = GameManager.Instance.GetPlayer();
            _animator = GetComponent<Animator>();
            _indicator = GameManager.Instance.indicator;

            _monsterHp += _player.GetLevel() * 1.5f;
            _monsterSpeed += Random.Range(1, 1) / 0.5f;

            _randomDamage = Random.Range(10, 20) * 1.5f;
            KnockbackDuration = 0.075f;
        }

        private void FixedUpdate()
        {
            _attackCooltime += Time.deltaTime;
            _distance = Vector3.Distance(transform.position, _player.transform.position);

            if (_monsterHp < 0)
            {
                _monsterSpeedMultiplier = 0;
            }

            if (_distance < 1.25f && _attackCooltime > 1.1f)
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
                        _monsterSpeed * _monsterSpeedMultiplier * Time.deltaTime);
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
            var finalDamage = _randomDamage + MonsterDamage;
            _player.TakeDamage(finalDamage);
            StartCoroutine(IsAttackingToFalse(_animator.GetCurrentAnimatorStateInfo(0).length));
        }

        private IEnumerator IsAttackingToFalse(float second)
        {
            yield return new WaitForSeconds(second);
            _monsterSpeedMultiplier = 1;
            _animator.SetBool("isAttacking", false);
        }

        public void TakeDamage(float damage)
        { 
            _monsterHp = _monsterHp - damage + MonsterDefense;
            ShowDamage(damage);
            Flash();

            if (_monsterHp > 0) return;
            _monsterSpeedMultiplier = 0;
            StartCoroutine(BeforeDestroy(_animator.GetCurrentAnimatorStateInfo(0).length));
        }
    }
}