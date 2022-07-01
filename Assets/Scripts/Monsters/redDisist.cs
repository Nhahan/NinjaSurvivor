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
    
        private float _monsterHp = 20f;
        private const float MonsterDamage = 10f;
        private float _randomDamage;
        private float _monsterSpeed = 1.025f;
        private float _monsterSpeedMultiplier = 1;
        private float _distance;
        private const float MonsterDefense = 5f;

        private float _attackCooltime;
    
        private void Start()
        {
            _player = GameManager.Instance.GetPlayer();
            _animator = GetComponent<Animator>();
            _indicator = GameManager.Instance.indicator;

            _monsterHp += _player.GetLevel() * 2f;
            _monsterSpeed += Random.Range(1, 2) / 0.65f;

            _randomDamage = Random.Range(10, 20) * 1.5f;
            KnockbackDuration = 0.1f;
        }

        private void FixedUpdate()
        {
            _attackCooltime += Time.deltaTime;
            _distance = Vector3.Distance(transform.position, _player.transform.position);

            if (_distance < 1.275f && _attackCooltime > 1.1f)
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
                        _monsterSpeed * _monsterSpeedMultiplier * Time.deltaTime);
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
                default:
                    throw new ArgumentOutOfRangeException();
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
