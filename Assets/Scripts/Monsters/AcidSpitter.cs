using System;
using System.Collections;
using Status;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Monsters
{
    public class AcidSpitter : Monster, IMonster
    {
        [SerializeField] private GameObject bullet;

        private Player _player;
        private Animator _animator;

        private float _monsterHp = 250f;
        private const float MonsterDamage = 30f;
        private float _randomDamage;
        private const float MonsterSpeed = 0.9f;
        private const float MonsterDefense = 1.5f;
        private float _monsterSpeedMultiplier = 1;
        private float _distance;

        private float _attackCooltime;

        private void Start()
        {
            _player = GameManager.Instance.GetPlayer();
            _animator = GetComponent<Animator>();
            Indicator = GameManager.Instance.indicator;

            _randomDamage = Random.Range(0, 3);
            KnockbackDuration = 0.07f;
        }

        private void FixedUpdate()
        {
            _attackCooltime += Time.deltaTime;
            _distance = Vector3.Distance(transform.position, _player.transform.position);

            if (_distance < 8 && _attackCooltime > 2f)
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
                    FlipSprite();
                    if (_attackCooltime > 2.1f && _monsterHp > 0)
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

        private void OnTriggerEnter2D(Collider2D coll)
        {
            if (!coll.CompareTag("Player")) return;
            var finalDamage = MonsterDamage - _player.Defense.CalculateFinalValue() + _randomDamage;
            _player.TakeDamage(finalDamage);
        }

        private void AttackPlayer()
        {
            _animator.SetBool("isAttacking", true);
            var position = transform.position;
            var playerDirection = (_player.transform.position - position).normalized;
            var spitPositionAdjustment = new Vector2(-0.6f * playerDirection.x, 0.9f);
            var spitInitPosition = new Vector2(position.x - spitPositionAdjustment.x, position.y - spitPositionAdjustment.y);
            if (_distance > 1.65) 
            {
                Instantiate(bullet, spitInitPosition, transform.rotation);
            }
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
