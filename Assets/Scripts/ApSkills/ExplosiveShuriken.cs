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

        private void Awake()
        {
            GetComponent<CircleCollider2D>().enabled = false;
        }

        private void Start()
        {
            _player = GameManager.Instance.GetPlayer();
            var targets = GameManager.Instance.GetNearestTargets(5);
            _target = targets.Aggregate(new Vector3(0,0,0), (s,v) => s + v) / targets.Count;
            _animator = GetComponent<Animator>();
            
            var skillLevelBonus = (float)(1.5 + 0.2 * _player.ExplosiveShuriken.CalculateFinalValue());
            _damage = _player.AttackDamage.CalculateFinalValue() * damageMultiplier * skillLevelBonus;
            
            StartCoroutine(BeforeDestroy(_animator.GetCurrentAnimatorStateInfo(0).length));
        }

        private void FixedUpdate()
        {
            if (!_isHit) {
                transform.position = Vector2.MoveTowards(transform.position, _target, 8.5f * Time.deltaTime);
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
            GetComponent<CircleCollider2D>().enabled = true;
            GetComponent<CircleCollider2D>().radius = 2.5f;
        }
        
        private IEnumerator BeforeDestroy(float second)
        {
            yield return new WaitForSeconds(second);
            Destroy(gameObject);
        }
    }
}