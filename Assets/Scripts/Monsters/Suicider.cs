using System.Collections;
using Status;
using UnityEngine;

namespace Monsters
{
    public class Suicider : Monster, IMonster
    {
        private Player _player;
        private Animator _animator;
        private Rigidbody2D _rb;

        private float _monsterHp = 30f;
        private const float MonsterDamage = 30f;
        private float _randomDamage;
        private float _monsterSpeed = 1.9f;
        private float _monsterSpeedMultiplier = 1;

        private void Start()
        {
            _player = GameManager.Instance.GetPlayer();
            _animator = GetComponent<Animator>();
            _rb = GetComponent<Rigidbody2D>();
            
            Indicator = GameManager.Instance.indicator;

            _randomDamage = Random.Range(0, 3);
        }

        private void FixedUpdate()
        {
            if (KnockbackTimer > 0)
            {
                PlayKnockback();
                return;
            }

            if (_monsterHp < 0) return;
            Vector2 direction = (_player.transform.position - transform.position).normalized;
            _rb.MovePosition(_rb.position + direction * (_monsterSpeed * Time.fixedDeltaTime * _monsterSpeedMultiplier));
            
            FlipSprite();
        }

        private void OnTriggerEnter2D(Collider2D coll)
        {
            if (!coll.CompareTag("Player")) return;
            AttackPlayer();
            MonsterSpeedMultiplier = 0;
            _animator.SetBool("isAttacking", true);
            StartCoroutine(BeforeDestroy(_animator.GetCurrentAnimatorStateInfo(0).length));
        }

        private void AttackPlayer()
        {
            var finalDamage = MonsterDamage - _player.Defense.CalculateFinalValue() + _randomDamage;
            _player.TakeDamage(finalDamage);
        }

        private void FlipSprite()
        {
            transform.localScale = transform.position.x < _player.transform.position.x ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);
        }

        public void TakeDamage(float damage)
        { 
            _monsterHp = _monsterHp - damage;
            ShowDamage(damage);
            Flash();

            if (_monsterHp > 0) return;
            MonsterSpeedMultiplier = 0;
            _animator.SetBool("isDead", true);
            StartCoroutine(BeforeDestroy(_animator.GetCurrentAnimatorStateInfo(0).length));
        }
    }
}
