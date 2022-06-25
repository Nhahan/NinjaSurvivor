using System.Collections;
using Status;
using Unity.VisualScripting;
using UnityEngine;

namespace Monsters
{
    public class Suicider : Monster, IMonster
    {
        [SerializeField] private GameObject expSoul1;
        
        private Animator _animator;

        private float _monsterHp = 200f;
        private const float MonsterDamage = 20f;
        private float _randomDamage;
        private const float MonsterSpeed = 1.65f;

        private void Start()
        {
            _animator = GetComponent<Animator>();

            _randomDamage = Random.Range(0, 3);
        }

        private void FixedUpdate()
        {
            if (gameObject.tag.Equals("Dead")) return;
            if (KnockbackTimer > 0)
            {
                PlayKnockback();
            }
            else 
            {
                transform.position = Vector2.MoveTowards(
                    transform.position, 
                    player.transform.position, 
                    MonsterSpeed * MonsterSpeedMultiplier * Time.deltaTime);
            }
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
            var finalDamage = MonsterDamage - player.Defense.CalculateFinalValue() + _randomDamage;
            player.TakeDamage(finalDamage);
        }

        private void FlipSprite()
        {
            transform.localScale = transform.position.x < player.transform.position.x ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);
        }

        public void SetMonsterHp(float hp)
        {
            _monsterHp = hp;
        }
        
        public void TakeDamage(float damage)
        {
            SetMonsterHp(_monsterHp - damage);
            Flash();

            if (_monsterHp > 0) return;
            GetComponent<SpriteRenderer>().color = new Color(255, 83, 83, 255);
            _animator.SetBool("isDead", true);
            MonsterSpeedMultiplier = 0;
            gameObject.tag = "Dead";
            StartCoroutine(BeforeDestroy(_animator.GetCurrentAnimatorStateInfo(0).length));
        }

        private IEnumerator BeforeDestroy(float second)
        {
            yield return new WaitForSeconds(second);
            Instantiate(expSoul1, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
