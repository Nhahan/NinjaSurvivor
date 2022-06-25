using System.Collections;
using Status;
using UnityEngine;

namespace Monsters
{
    public class Suicider : MonoBehaviour, IMonster
    {
        [SerializeField] private GameObject expSoul1;

        private float _monsterSpeedMultiplier = 1;
        private Player _player;
        private Animator _animator;

        private float _monsterHp = 20f;
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
            transform.position = Vector2.MoveTowards(
                transform.position, 
                _player.transform.position, 
                MonsterSpeed * _monsterSpeedMultiplier * Time.deltaTime);
            FlipSprite();
        }

        private void OnTriggerEnter2D(Collider2D coll)
        {
            if (!coll.CompareTag("Player")) return;
            AttackPlayer();
            _monsterSpeedMultiplier = 0;
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

        public void SetMonsterHp(float hp)
        {
            _monsterHp = hp;
        }
        
        public void TakeDamage(float damage)
        {
            SetMonsterHp(_monsterHp - damage);

            if (_monsterHp > 0) return;
            GetComponent<SpriteRenderer>().color = new Color(255, 83, 83, 255);
            _animator.SetBool("isDead", true);
            _monsterSpeedMultiplier = 0;
            gameObject.tag = "Dead";
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
