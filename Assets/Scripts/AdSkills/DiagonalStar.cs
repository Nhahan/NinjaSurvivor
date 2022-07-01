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
        private const float Duration = 1.8f;
        private float _bulletSpeed = 12f;
        private float _damage;
        private float _baseSkillDamage = 10f;
        private float _skillLevelMultiplier = 0.225f;
        
        private Vector3 _nearestEnemy;
        private Vector3 _bulletDirection;

        private void Start()
        {
            _player = GameManager.Instance.GetPlayer();
            IsAvailable();
            
            var skillLevelBonus = _skillLevelMultiplier * _player.BasicStar.CalculateFinalValue();
            _damage = _player.Damage() * damageMultiplier * skillLevelBonus + _baseSkillDamage;
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
            
            if (Random.Range(0,10) > 3.3f + _player.DiagonalStar.CalculateFinalValue() * 0.5f) Destroy(gameObject);
        }
        
        public void IsAvailable()
        {
            var childNum = Random.Range(0, 16);
            
            _bulletDirection = (_player.transform.GetChild(0).GetChild(childNum).position) - 
                               new Vector3((float)(Random.Range(-3, 2)), (float)(Random.Range(-3, 2)), 0);
        }
    }
}
