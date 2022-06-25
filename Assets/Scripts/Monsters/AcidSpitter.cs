using System;
using System.Collections;
using Status;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Monsters
{
    public class AcidSpitter : MonoBehaviour, IMonster
    {
        private enum State
        {
            Moving,
            Attacking
        }

        private State state = State.Moving;
        
        [SerializeField] private GameObject expSoul1;
        [SerializeField] private GameObject bullet;

        private float _monsterSpeedMultiplier = 1;
        private Player _player;
        private Animator _animator;

        private float _monsterHp = 300f;
        private const float MonsterDamage = 30f;
        private float _randomDamage;
        private const float MonsterSpeed = 1.3f;

        private bool _isAttacking;
        private float _distance;

        private float _attackCooltime;

        private void Start()
        {
            _player = GameManager.Instance.GetPlayer();
            _animator = GetComponent<Animator>();

            _randomDamage = Random.Range(0, 3);
        }

        private void FixedUpdate()
        {
            _attackCooltime += Time.deltaTime;
            _distance = Vector3.Distance(transform.position, _player.transform.position);

            if (_distance < 8 && _attackCooltime > 1)
            {
                state = State.Attacking;
            }
            else
            {
                state = State.Moving;
            }

            switch (state)
            {
                case State.Moving:
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
            var spitInitPosition = new Vector2(position.x - spitPositionAdjustment.x, position.y - spitPositionAdjustment.y) * 1.1f;
            Instantiate(bullet, spitInitPosition, transform.rotation);
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

        public void SetMonsterHp(float hp)
        {
            _monsterHp = hp;
        }
        
        public void TakeDamage(float damage)
        {
            SetMonsterHp(_monsterHp - damage);

            if (_monsterHp > 0) return;
            _animator.SetBool("isDead", true);
            _monsterSpeedMultiplier = 0;
            StartCoroutine(BeforeDestroy(_animator.GetCurrentAnimatorStateInfo(0).length));
        }

        public void StopMonster()
        {
            _monsterSpeedMultiplier = 0;
            GetComponent<Animator>().enabled = false;
        }
        
        public void ResumeMonster()
        {
            _monsterSpeedMultiplier = 1;
            GetComponent<Animator>().enabled = true;
        }

        private IEnumerator BeforeDestroy(float second)
        {
            yield return new WaitForSeconds(second);
            Instantiate(expSoul1, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
