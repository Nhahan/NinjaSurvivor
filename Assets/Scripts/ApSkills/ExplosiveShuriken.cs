using System;
using System.Collections;
using System.Linq;
using Monsters;
using Status;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ApSkills
{
    public class ExplosiveShuriken : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;

        private Player _player;
        private Animator _animator;
        private Vector3 _target;

        private float _bulletSpeed;
        private const float DamageMultiplier = 1f;

        private float _damage;

        private void Start()
        {
            _player = GameManager.Instance.GetPlayer();

            var targets = GameManager.Instance.GetNearestTargets(4);
            var skillLevelBonus = 2f + 2f * _player.ExplosiveShuriken.CalculateFinalValue();

            _target = targets.Aggregate(new Vector3(0,0,0), (s,v) => s + v) / targets.Count;
            _damage = _player.AttackDamage.CalculateFinalValue() * DamageMultiplier * skillLevelBonus;
            
            AdjustDirection();
            StartCoroutine(Explosion(3.5f));
        }

        private void FixedUpdate()
        {
            _bulletSpeed += Time.deltaTime * 0.5f;
            try
            {
                transform.position = Vector2.MoveTowards(transform.position, _target, _bulletSpeed);
            }
            catch
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(Random.Range(-2,3), Random.Range(-2,3)) * 5, _bulletSpeed);
            }
        }

        private void OnTriggerEnter2D(Collider2D coll)
        {
            if (!coll.CompareTag("Enemy")) return;

            var monster = coll.gameObject.GetComponent<IMonster>();
            var normal = (coll.gameObject.transform.position - transform.position).normalized;
            monster.TakeDamage(_damage);
            monster.StartKnockback(normal);
        }

        private IEnumerator Explosion(float second)
        {
            yield return new WaitForSeconds(second);
            Instantiate(prefab, transform.position, transform.rotation);
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