using System.Collections;
using System.Linq;
using Monsters;
using Status;
using UnityEngine;

namespace ApSkills
{
    public class ExplosiveShurikenSecondImpact : MonoBehaviour
    {
        [SerializeField] private float damageMultiplier = 1.5f;

        private Player _player;
        private Animator _animator;

        private float _damage;
        private bool _isAvailable = true;

        private void Start()
        {
            _player = GameManager.Instance.GetPlayer();
            _animator = GetComponent<Animator>();

            var skillLevelBonus = 1.0f + 0.1f * _player.ExplosiveShuriken.CalculateFinalValue();

            _damage = _player.AttackDamage.CalculateFinalValue() * damageMultiplier * skillLevelBonus;

            Explosion();
        }

        private void OnTriggerEnter2D(Collider2D coll)
        {
            if (!_isAvailable) return;
            if (!coll.CompareTag("Enemy")) return;

            var monster = coll.gameObject.GetComponent<IMonster>();
            monster.TakeDamage(_damage);
        }

        private void Explosion()
        {
            var animationLength = _animator.GetCurrentAnimatorStateInfo(0).length;
            
            Invoke(nameof(ToNotAvailable), animationLength / 0.85f);
            StartCoroutine(BeforeDestroy(_animator.GetCurrentAnimatorStateInfo(0).length));
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
