using System;
using System.Collections;
using Status;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Monsters
{
    public class Disist : Monster, IMonster
    {
        private Player _player;
        private Animator _animator;
    
        private float _monsterHp = 50f;
        private const float MonsterDamage = 10f;
        private float _randomDamage;
        private const float MonsterSpeed = 0.6f;
        private float _monsterSpeedMultiplier = 1;
        private float _distance;
        private const float MonsterDefense = 0f;

        private float _attackCooltime;
    
        private void Start()
        {
            _player = GameManager.Instance.GetPlayer();
            _animator = GetComponent<Animator>();
            Indicator = GameManager.Instance.indicator;

            _monsterHp += _player.GetLevel();

            _randomDamage = Random.Range(10, 20);
            KnockbackDuration = 0.105f;
        }

        private void FixedUpdate()
        {
            _attackCooltime += Time.deltaTime;
            _distance = Vector3.Distance(transform.position, _player.transform.position);

            if (_distance < 1.2f && _attackCooltime > 1.125f)
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
                    transform.position = Vector2.MoveTowards(
                        transform.position,
                        _player.transform.position,
                        MonsterSpeed * _monsterSpeedMultiplier * Time.deltaTime);
                    if (_attackCooltime > 1.25f && _monsterHp > 0)
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
            StartCoroutine(BeforeDestroy(0.35f));
        }
    }
}
