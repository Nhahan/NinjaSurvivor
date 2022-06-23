using System.Collections;
using Monsters;
using Status;
using UnityEngine;

namespace ApSkills
{
    public class LightningStrike : MonoBehaviour
    {
        [SerializeField] private float possibleAttackDistance = 12f;
        [SerializeField] private float damageMultiplier = 1.5f;

        private Player _player;
        private Animator _animator;

        private bool _isAvailable = true;

        private void Awake()
        {
            _player = GameManager.Instance.GetPlayer();
            if (_player.BasicStar.CalculateFinalValue() < 1)
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            _animator = GetComponent<Animator>();
            IsAvailable();
            var animationLength = _animator.GetCurrentAnimatorStateInfo(0).length;
            Invoke(nameof(ToNotAvailable), animationLength / 0.8f);
            StartCoroutine(BeforeDestroy(_animator.GetCurrentAnimatorStateInfo(0).length));
        }

        private void OnTriggerEnter2D(Collider2D coll)
        {
            if (!_isAvailable) return;
            if (!coll.CompareTag("Enemy")) return;

            var monster = coll.gameObject.GetComponent<IMonster>();
            var skillLevelBonus = (float)(1 + 0.1 * _player.BasicStar.CalculateFinalValue());
            var damage = _player.AttackDamage.CalculateFinalValue() * damageMultiplier * skillLevelBonus;

            monster.TakeDamage(damage);
        }

        public void IsAvailable()
        {
            try
            {
                var nearestEnemy = GameManager.Instance.GetNearestTarget();
                if (Vector3.Distance(nearestEnemy, transform.position) > possibleAttackDistance)
                {
                    Destroy(gameObject);
                }
            }
            catch
            {
                Destroy(gameObject);
            }
        }
        
        private void ToNotAvailable()
        {
            _isAvailable = false;
        }
        
        private IEnumerator BeforeDestroy(float second)
        {
            yield return new WaitForSeconds(second);
            Destroy(gameObject);
        }
    }
}