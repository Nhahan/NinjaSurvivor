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

        private float _liveTime = 0;
        private const float Duration = 2.7f;
        private float _damage;
        private const float BulletSpeed = 12f;
        private float _baseSkillDamage = 0f;
        private float _skillLevelMultiplier = 10f;
        private Vector3 _bulletDirection;
        private float _skillLevelBonus;

        private void Start()
        {
            _player = GameManager.Instance.GetPlayer();
            
            _skillLevelBonus = _baseSkillDamage + 10f * _player.ExplosiveShuriken.CalculateFinalValue();
            
            _damage = _player.AttackDamage.CalculateFinalValue() * damageMultiplier * _skillLevelBonus;
            try
            {
                _bulletDirection = (GameManager.Instance.GetNearestTarget() - transform.position -
                                    new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0))
                    .normalized;
            }
            catch // if there is no enemy to attack
            {
                Destroy(gameObject);
            }
        }

        private void FixedUpdate()
        {
            _liveTime += Time.deltaTime;
            if (_liveTime > Duration) { Destroy(gameObject); }

            transform.position += _bulletDirection * (BulletSpeed * Time.deltaTime);
            transform.Rotate(0, 0, -350 * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D coll)
        {
            if (!coll.CompareTag("Enemy")) return;
            
            var monster = coll.gameObject.GetComponent<IMonster>();
            _damage = _player.AttackDamage.CalculateFinalValue() * damageMultiplier * _skillLevelBonus;

            var normal = (coll.gameObject.transform.position - transform.position).normalized;
            Debug.Log($"normal: {normal}");
            monster.TakeDamage(_damage);
            monster.StartKnockback(normal);
            
            if (Random.Range(0,10) < 4.1) Destroy(gameObject);
        }
    }
}
