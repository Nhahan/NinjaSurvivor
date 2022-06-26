using System.Collections;
using Status;
using UnityEngine;

namespace Monsters
{
    public class Suicider : Monster, IMonster
    {
        private Player _player;
        private Animator _animator;

        private float _monsterHp = 22f;
        private const float MonsterDamage = 20f;
        private float _randomDamage;
        private const float MonsterSpeed = 1.65f;

        private void Start()
        {
            _player = GameManager.Instance.GetPlayer();
            _animator = GetComponent<Animator>();

            _randomDamage = Random.Range(0, 3);
        }

        private void FixedUpdate()
        {
            if (gameObject.tag.Equals("Dead")) return;
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
            gameObject.tag = "Dead";
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
            _monsterHp -= damage;
            Flash();

            if (_monsterHp > 0) return;
            GetComponent<SpriteRenderer>().color = new Color(255, 83, 83, 255);
            _animator.SetBool("isDead", true);
            MonsterSpeedMultiplier = 0;
            gameObject.tag = "Dead";
            StartCoroutine(BeforeDestroy(_animator.GetCurrentAnimatorStateInfo(0).length));
        }
    }
}
