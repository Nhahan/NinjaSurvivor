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
        private int _playerLevel;

        private Vector3 _closestTarget;

        private float _liveTime = 0;
        private const float Duration = 2f;
        private float _damage;
        private const float BulletSpeed = 12f;
        private float _baseSkillDamage = 10f;
        private float _skillLevelMultiplier = 0.25f;
        private Vector3 _bulletDirection;

        private void Start()
        {
            _player = GameManager.Instance.GetPlayer();
            _playerLevel = _player.GetLevel();
            
            var skillLevelBonus = _skillLevelMultiplier * _player.BasicStar.CalculateFinalValue();
            _damage = _player.Damage() * damageMultiplier * skillLevelBonus + _baseSkillDamage;
            _bulletDirection = (_closestTarget - transform.position -
                                new Vector3(Random.Range(-2, 2) / _player.BasicStar.CalculateFinalValue(),
                                    Random.Range(-2, 2) / _player.BasicStar.CalculateFinalValue(), 0)).normalized;
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

            var normal = (coll.gameObject.transform.position - transform.position).normalized;
            monster.TakeDamage(_damage);
            monster.StartKnockback(normal);
            
            if (Random.Range(0,10) > 5.4 + _player.BasicStar.CalculateFinalValue() * 0.5f) Destroy(gameObject);
        }

        public void SetClosestTarget(Vector3 target)
        {
            _closestTarget = target;
        }
    }
}
