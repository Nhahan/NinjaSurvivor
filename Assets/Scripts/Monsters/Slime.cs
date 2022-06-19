using System;
using Status;
using UnityEngine;

namespace Monsters
{
    public class Slime : MonoBehaviour, IMonster
    {
        [SerializeField] private float monsterSpeed = 1.5f;
        [SerializeField] private float monsterDamage = 20f;
        [SerializeField] private float monsterHp = 10f;
        [SerializeField] private float monsterExp = 1f;

        private float _monsterSpeedMultiplier = 1;
        private Player _player;

        private void Start()
        {
            _player = GameObject.FindWithTag("Player").GetComponent<Player>();
        }

        private void FixedUpdate()
        {
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
            Destroy(gameObject);
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
            if (monsterHp <= 0)
            {
                Destroy(gameObject);
            }
            SetMonsterHp(monsterHp - damage);

            if (!(monsterHp <= 0)) return;
            _player.EarnExp(monsterExp);
            Destroy(gameObject);
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
    }
}
