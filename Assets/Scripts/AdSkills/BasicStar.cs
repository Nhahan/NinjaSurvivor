using System.Linq;
using Monsters;
using Status;
using UnityEngine;

namespace AdSkills
{
    public class BasicStar : MonoBehaviour
    {
        [SerializeField] private float bulletSpeed = 11f;
        [SerializeField] private float possibleAttackDistance = 11.3f;
        
        [SerializeField] private float damageMultiplier = 1;
        // [SerializeField] private float baseskillLevelMultiplier = 1;
        // [SerializeField] private float 

        private Player _player;

        private float _lifeTime = 0;
        private const float DestroyTime = 3.3f;

        private Vector3 _nearestEnemy;
        private Vector3 _bulletDirection;

        private void Awake()
        {
            _player = GameManager.Instance.GetPlayer();
            if (_player.BasicStar.CalculateFinalValue() < 1) { Destroy(gameObject); }
        }

        private void Start()
        {
            IsAvailable();
        }

        private void FixedUpdate()
        {
            _lifeTime += Time.deltaTime;
            if (_lifeTime > DestroyTime) { Destroy(gameObject); }

            transform.position += _bulletDirection * (bulletSpeed * Time.deltaTime);
            transform.Rotate(0, 0, -230 * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D coll)
        {
            if (!coll.CompareTag("Enemy")) return;

            Destroy(gameObject);
            var monster = coll.gameObject.GetComponent<IMonster>();
            var skillLevelBonus = (float)(1 + 0.1 * _player.BasicStar.CalculateFinalValue());
            var damage = _player.AttackDamage.CalculateFinalValue() * damageMultiplier * skillLevelBonus;

            monster.TakeDamage(damage);
        }

        public void IsAvailable()
        {
            try
            {
                _nearestEnemy = FindNearestObject().transform.position;
                if (_nearestEnemy == null || Vector3.Distance(_nearestEnemy, transform.position) > possibleAttackDistance)
                {
                    Destroy(gameObject);
                }

                _bulletDirection = (_nearestEnemy - transform.position -
                                    new Vector3((float)(Random.Range(-1, 2)), (float)(Random.Range(-1, 2)), 0)).normalized;
            }
            catch
            {
                Destroy(gameObject);
            }
        }
        private GameObject FindNearestObject()
        {
            return GameObject.FindGameObjectsWithTag("Enemy").ToList()
                .OrderBy(obj => Vector3.Distance(transform.position, obj.transform.position)).FirstOrDefault();
        }
    }
}
