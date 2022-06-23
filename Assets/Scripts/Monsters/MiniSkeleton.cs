using System.Collections;
using System.Collections.Generic;
using Monsters;
using Status;
using UnityEngine;

public class MiniSkeleton : MonoBehaviour, IMonster
{

        [SerializeField] private float monsterSpeed = 1.5f;
        [SerializeField] private float monsterDamage = 20f;
        [SerializeField] private float monsterHp = 10f;
        [SerializeField] private float monsterExp = 1f;
        
        private Animator _animator;
        private Rigidbody2D _rb;

        private float _monsterSpeedMultiplier = 1;
        private Player _player;
        private float _actualDistance;

        private void Start()
        {
            
            _animator = GetComponent<Animator>();
            _rb = GetComponent<Rigidbody2D>();
            _player = GameManager.Instance.GetPlayer();
            
            _actualDistance = Vector2.Distance(transform.position, _player.transform.position);
        }

        private void FixedUpdate()
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                _player.transform.position,
                monsterSpeed * _monsterSpeedMultiplier * Time.deltaTime);

            if (_actualDistance < 1f)
            {
                _animator.SetBool("isAttacking", true);
                _monsterSpeedMultiplier = 0;
                StartCoroutine(AttackPlayer());
                _monsterSpeedMultiplier = 1;
            }

            if (IsDeadAnimationFinished())
            {
                _player.EarnExp(monsterExp);
                Destroy(gameObject);
            }
            FlipSprite();
            Debug.Log($"_actualDistance: {_actualDistance}");
        }

        private IEnumerator AttackPlayer()
        {
            _monsterSpeedMultiplier = 0;
            _player.TakeDamage(monsterDamage);
            var animationLength = _animator.GetCurrentAnimatorStateInfo(0).length;
            yield return new WaitForSecondsRealtime(animationLength);
        }

        private void FlipSprite()
        {
            transform.localScale = transform.position.x < _player.transform.position.x ? new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);
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

        private bool IsDeadAnimationFinished()
        {
            return _animator.GetCurrentAnimatorStateInfo(0).IsName("Dead") && 
                   _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f;
        }
}
