using System.Collections;
using Monsters;
using Status;
using UnityEngine;

namespace ApSkills
{
    public class Flamer : MonoBehaviour
    {
        [SerializeField] private float damageMultiplier = 1;

        private Player _player;
        private Transform _flamer;
        private Animator _animator;
        private Rigidbody2D _playerRb;
    
        private bool _isAvailable = true;
        private float _damage;

        private void Start()
        {
            _player = GameManager.Instance.GetPlayer();
            _playerRb = _player.GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _flamer = _player.transform.Find("SkillPoints").Find("Flamer");
            
            var skillLevelBonus = 2f + 2f * _player.Flamer.CalculateFinalValue();
            _damage = _player.AttackDamage.CalculateFinalValue() * damageMultiplier * skillLevelBonus;
            
            var animationLength = _animator.GetCurrentAnimatorStateInfo(0).length;
            
            Invoke(nameof(ToNotAvailable), animationLength / 0.7f);
            StartCoroutine(BeforeDestroy(_animator.GetCurrentAnimatorStateInfo(0).length));
        }

        private IEnumerator BeforeDestroy(float second)
        {
            yield return new WaitForSeconds(second);
            Destroy(gameObject);
        }

        private void FixedUpdate()
        {
            FlipSprite();
            if (_isAvailable)
            {
                transform.position = _flamer.position;
            }
        }

        private void ToNotAvailable()
        {
            _isAvailable = false;
        }

        private void OnTriggerEnter2D(Collider2D coll)
        {
            if (!_isAvailable) return;
            if (!coll.CompareTag("Enemy")) return;

            var monster = coll.gameObject.GetComponent<IMonster>();

            var normal = (coll.gameObject.transform.position - transform.position).normalized;
            monster.TakeDamage(_damage);
            monster.StartKnockback(normal);
        }
    
        private void FlipSprite()
        {
            transform.localScale = new Vector2(Mathf.Sign((_flamer.transform.position - transform.position).normalized.x), 1f);
        }
    }
}
