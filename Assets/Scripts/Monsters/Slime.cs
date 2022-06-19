using Status;
using UnityEngine;

namespace Monsters
{
    public class Slime : MonoBehaviour, IMonster
    {
        [SerializeField] private float monsterSpeed = 1.5f;

        [SerializeField] private float monsterDamage = 20f;
        [SerializeField] private float monsterHp = 10f;

        private Player _player;

        private void Start()
        {
            _player = GameObject.FindWithTag("Player").GetComponent<Player>();
        }

        private void FixedUpdate()
        {
            transform.position = Vector2.MoveTowards(transform.position, _player.transform.position, monsterSpeed * Time.deltaTime);
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

        public float GetMonsterHp()
        {
            return monsterHp;
        }

        public void SetMonsterHp(float monsterHp)
        {
            this.monsterHp = monsterHp;
        }

        public void TakeDamage(float damage)
        {
            var currentHp = GetMonsterHp();
            SetMonsterHp(currentHp - damage);
            if (GetMonsterHp() <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
