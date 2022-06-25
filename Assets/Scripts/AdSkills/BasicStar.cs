using System.Linq;
using Monsters;
using Status;
using UnityEngine;

namespace AdSkills
{
    public class BasicStar : MonoBehaviour
    {
        [SerializeField] private float damageMultiplier = 1;

        private Player _player;

        private float _lifeTime = 0;
        private const float DestroyTime = 2.8f;
        private float _damage;
        private const float BulletSpeed = 12f;
        private Vector3 _bulletDirection;

        private void Start()
        {
            _player = GameManager.Instance.GetPlayer();
            
            var skillLevelBonus = 1.0f + 0.1f * _player.ExplosiveShuriken.CalculateFinalValue();
            
            _damage = _player.AttackDamage.CalculateFinalValue() * damageMultiplier * skillLevelBonus;
            try
            {
                _bulletDirection = (GameManager.Instance.GetNearestTarget() - transform.position -
                                    new Vector3((Random.Range(-1, 2) * 0.5f), (Random.Range(-1, 2) * 0.5f), 0))
                    .normalized;
            }
            catch // if there is no enemy to attack
            {
                Destroy(gameObject);
            }
        }

        private void FixedUpdate()
        {
            _lifeTime += Time.deltaTime;
            if (_lifeTime > DestroyTime) { Destroy(gameObject); }

            transform.position += _bulletDirection * (BulletSpeed * Time.deltaTime);
            transform.Rotate(0, 0, -350 * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D coll)
        {
            if (!coll.CompareTag("Enemy")) return;

            Destroy(gameObject);
            var monster = coll.gameObject.GetComponent<IMonster>();
            var skillLevelBonus = 1f + 0.1f * _player.BasicStar.CalculateFinalValue();
            _damage = _player.AttackDamage.CalculateFinalValue() * damageMultiplier * skillLevelBonus;

            var normal = (coll.gameObject.transform.position - transform.position).normalized;
            Debug.Log($"normal: {normal}");
            monster.TakeDamage(_damage);
            monster.StartKnockback(normal);
        }
    }
}
