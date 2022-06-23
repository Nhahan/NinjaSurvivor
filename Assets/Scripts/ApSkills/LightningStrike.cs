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

        private void Start()
        {
            _player = GameManager.Instance.GetPlayer();
            if (_player.LightningStrike.CalculateFinalValue() < 1)
            {
                Destroy(gameObject);
            }
            
            _animator = GetComponent<Animator>();
            StartCoroutine(BeforeDestroy(_animator.GetCurrentAnimatorStateInfo(0).length));
        }

        private void OnTriggerEnter2D(Collider2D coll)
        {
            if (!coll.CompareTag("Enemy")) return;

            var monster = coll.gameObject.GetComponent<IMonster>();
            var skillLevelBonus = (float)(1 + 0.1 * _player.BasicStar.CalculateFinalValue());
            var damage = _player.AttackDamage.CalculateFinalValue() * damageMultiplier * skillLevelBonus;

            monster.TakeDamage(damage);
        }
        
        private IEnumerator BeforeDestroy(float second)
        {
            yield return new WaitForSeconds(second);
            Destroy(gameObject);
        }
    }
}