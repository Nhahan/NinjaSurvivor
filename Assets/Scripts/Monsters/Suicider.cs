using System;
using System.Collections;
using Status;
using UnityEngine;
using UnityEngine.UI;

namespace Monsters
{
    public class Suicider : MonoBehaviour, IMonster
    {
        [SerializeField] private float monsterSpeed = 1.5f;
        [SerializeField] private float monsterDamage = 20f;
        [SerializeField] private float monsterHp = 10f;
        [SerializeField] private float monsterExp = 1f;

        private float _monsterSpeedMultiplier = 1;
        private Player _player;
        private Animator _animator;

        private void Start()
        {
            _player = GameManager.Instance.GetPlayer();
            _animator = GetComponent<Animator>();
        }

        private void FixedUpdate()
        {
            if (gameObject.tag.Equals("Dead")) return;
            transform.position = Vector2.MoveTowards(
                transform.position, 
                _player.transform.position, 
                monsterSpeed * _monsterSpeedMultiplier * Time.deltaTime);
            FlipSprite();
        }

        private void OnTriggerEnter2D(Collider2D coll)
        {
            if (!coll.CompareTag("Player")) return;
            AttackPlayer();
            _monsterSpeedMultiplier = 0;
            _animator.SetBool("isAttacking", true);
            gameObject.tag = "Dead";
            StartCoroutine(DestroyAfter(_animator.GetCurrentAnimatorStateInfo(0).length));
        }

        private void AttackPlayer()
        {
            _player.TakeDamage(monsterDamage);
        }

        private void FlipSprite()
        {
            transform.localScale = transform.position.x < _player.transform.position.x ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);
        }

        public void SetMonsterHp(float hp)
        {
            monsterHp = hp;
        }
        
        public void TakeDamage(float damage)
        {
            SetMonsterHp(monsterHp - damage);

            if (monsterHp > 0) return;
            _animator.SetBool("isDead", true);
            _monsterSpeedMultiplier = 0;
            GetComponent<SpriteRenderer>().color = new Color(255, 83, 83, 255);
            gameObject.tag = "Dead";
            StartCoroutine(DestroyAfter(_animator.GetCurrentAnimatorStateInfo(0).length));
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

        private IEnumerator DestroyAfter(float second)
        {
            yield return new WaitForSeconds(second);
            _player.EarnExp(monsterExp);
            Destroy(gameObject);
        }
    }
}
