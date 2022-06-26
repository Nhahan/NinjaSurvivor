using System.Collections;
using Monsters;
using Status;
using UnityEngine;

namespace ApSkills
{
    public class LightningStrike : MonoBehaviour
    {
        [SerializeField] private float damageMultiplier = 1.5f;

        private Player _player;
        private Animator _animator;

        private float _damage;

        private void Start()
        {
            _player = GameManager.Instance.GetPlayer();
            _animator = GetComponent<Animator>();
            
            var skillLevelBonus = 5f + 2f * _player.BasicStar.CalculateFinalValue();
            _damage = _player.AttackDamage.CalculateFinalValue() * damageMultiplier * skillLevelBonus;
            
            StartCoroutine(BeforeDestroy(_animator.GetCurrentAnimatorStateInfo(0).length));
        }

        private void OnTriggerEnter2D(Collider2D coll)
        {
            if (!coll.CompareTag("Enemy")) return;

            var monster = coll.gameObject.GetComponent<IMonster>();

            monster.TakeDamage(_damage);
        }
        
        private IEnumerator BeforeDestroy(float second)
        {
            yield return new WaitForSeconds(second);
            Destroy(gameObject);
        }
    }
}