using System;
using System.Collections;
using System.Linq;
using Monsters;
using Status;
using UnityEngine;

namespace ApSkills
{
    public class ExplosiveShuriken : MonoBehaviour
    {
        [SerializeField] private float damageMultiplier = 1.5f;

        private Player _player;
        private Animator _animator;
        private Vector3 _target;

        private bool _isHit;
        private float _damage;
        private bool _isActivated;

        private void Start()
        {
            GetComponent<CircleCollider2D>().radius = 0.05f;
            
            AdjustDirection();

            _player = GameManager.Instance.GetPlayer();
            var targets = GameManager.Instance.GetNearestTargets(5);
            _target = targets.Aggregate(new Vector3(0,0,0), (s,v) => s + v) / targets.Count;
            _animator = GetComponent<Animator>();

            var skillLevelBonus = 1.5f + 0.2f * _player.ExplosiveShuriken.CalculateFinalValue();
            _damage = _player.AttackDamage.CalculateFinalValue() * damageMultiplier * skillLevelBonus;
        }

        private void FixedUpdate()
        {
            if (!_isHit) {
                transform.position = Vector2.MoveTowards(transform.position, Vector2.zero, 8.5f * Time.deltaTime);
            }
        }

        private void OnTriggerEnter2D(Collider2D coll)
        {
            if (!coll.CompareTag("Enemy")) return;
            if (transform.position != _target) { _isHit = true; }

            if(transform.position == _target || _isHit) {
                Invoke(nameof(Explosion), 2f);
            }

            var monster = coll.gameObject.GetComponent<IMonster>();
            monster.TakeDamage(_damage);
        }

        private void Explosion()
        {
            GetComponent<CircleCollider2D>().radius = 1.8f;
            StartCoroutine(BeforeDestroy(_animator.GetCurrentAnimatorStateInfo(0).length));
        }

        private IEnumerator BeforeDestroy(float second)
        {
            yield return new WaitForSeconds(second);
            Destroy(gameObject);
        }
        
        private void AdjustDirection()
        {
            var vDiff = (new Vector3(0, 0, 0) - transform.position);
            var atan2 = Mathf.Atan2(vDiff.y, vDiff.x);
            transform.rotation = Quaternion.Euler(0f, 0f, atan2 * Mathf.Rad2Deg - 90);
        }
    }
}