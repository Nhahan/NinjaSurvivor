using System.Collections;
using Status;
using UnityEngine;

namespace Monsters
{
    public class Suicider : Monster, IMonster
    {
        private Player _player;
        private Animator _animator;

        private float _monsterHp = 30f;
        private const float MonsterDamage = 30f;
        private float _randomDamage;
        private const float MonsterSpeed = 1.75f;
        private const float MonsterDefense = 2f;

        private void Start()
        {
            _player = GameManager.Instance.GetPlayer();
            _animator = GetComponent<Animator>();
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
            transform.position = Vector2.MoveTowards(
                transform.position, 
                _player.transform.position, 
                MonsterSpeed * MonsterSpeedMultiplier * Time.deltaTime);
        
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
            _monsterHp = _monsterHp - damage + MonsterDefense;
            ShowDamage(damage);
            Flash();

            if (_monsterHp > 0) return;
            _animator.SetBool("isDead", true);
            MonsterSpeedMultiplier = 0;
            StartCoroutine(BeforeDestroy(_animator.GetCurrentAnimatorStateInfo(0).length));
        }
    }
}
