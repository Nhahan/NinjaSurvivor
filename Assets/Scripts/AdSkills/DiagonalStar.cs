using System.Linq;
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
        private const float Duration = 2.5f;
        private float _bulletSpeed = 12f;
        private float _damage;
        private float _baseSkillDamage = 8f;
        private float _skillLevelMultiplier = 3f;
        private float _skillLevelBonus;
        
        private Vector3 _nearestEnemy;
        private Vector3 _bulletDirection;

        private void Start()
        {
            _player = GameManager.Instance.GetPlayer();
            IsAvailable();
            
            _skillLevelBonus = _baseSkillDamage + _skillLevelMultiplier * _player.BasicStar.CalculateFinalValue();
            _damage = _player.AttackDamage.CalculateFinalValue() * damageMultiplier * _skillLevelBonus;
        }

        private void FixedUpdate()
        {
            _liveTime += Time.deltaTime;
            if (_liveTime > Duration) { Destroy(gameObject); }
            
            transform.position = Vector2.MoveTowards(transform.position, _bulletDirection * 1.1f, _bulletSpeed * Time.deltaTime);
            transform.Rotate(0, 0, -450 * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D coll)
        {
            if (!coll.CompareTag("Enemy")) return;
            
            Destroy(gameObject);
            
            var monster = coll.gameObject.GetComponent<IMonster>();
            var normal = (coll.gameObject.transform.position - transform.position).normalized;
            
            monster.TakeDamage(_damage);
            monster.StartKnockback(normal);
        }
        
        public void IsAvailable()
        {
            var childNum = Random.Range(0, 16);
            
            _bulletDirection = (_player.transform.GetChild(0).GetChild(childNum).position) - 
                               new Vector3((float)(Random.Range(-5, 2)), (float)(Random.Range(-5, 2)), 0);
        }
    }
}
