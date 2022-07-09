using Monsters;
using Status;
using UnityEngine;

namespace AdSkills
{
    public class DiagonalStar : MonoBehaviour
    {
        [SerializeField] private float damageMultiplier = 1;

        private Player _player;

        private float _liveTime = 0;
        private const float Duration = 2.1f;
        private float _bulletSpeed = 0.6f;
        private float _damage;
        private float _baseSkillDamage = 10f;
        private float _skillLevelMultiplier = 0.25f;
        
        private Vector3 _nearestEnemy;
        private Vector3 _bulletDirection;

        private void Start()
        {
            _player = GameManager.Instance.GetPlayer();
            _bulletDirection = new Vector3(Random.Range(-360, 360), Random.Range(-360, 360), 0).normalized;
            
            var skillLevelBonus = _skillLevelMultiplier * _player.DiagonalStar.CalculateFinalValue();
            _damage = _player.Damage() * damageMultiplier * skillLevelBonus + _baseSkillDamage;
        }

        private void FixedUpdate()
        {
            _liveTime += Time.deltaTime;
            if (_liveTime > Duration) { Destroy(gameObject); }

            transform.position += _bulletDirection * _bulletSpeed;
            transform.Rotate(0, 0, -450 * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D coll)
        {
            if (!(coll.CompareTag("Enemy") || coll.CompareTag("Boss"))) return;
            
            Destroy(gameObject);
            
            var monster = coll.gameObject.GetComponent<IMonster>();
            var normal = (coll.gameObject.transform.position - transform.position).normalized;
            
            monster.TakeDamage(_damage);
            monster.StartKnockback(normal);
            
            if (Random.Range(0,10) > 4.4f + _player.DiagonalStar.CalculateFinalValue() * 0.5f) Destroy(gameObject);
        }
    }
}
