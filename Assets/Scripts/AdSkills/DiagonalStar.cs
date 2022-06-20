using System.Linq;
using Monsters;
using Status;
using UnityEngine;

namespace AdSkills
{
    public class DiagonalStar : MonoBehaviour
    {
        [SerializeField] private float bulletSpeed = 11f;
        [SerializeField] private float possibleAttackDistance = 10f;
        [SerializeField] private float damageMultiplier = 1;

        private Player _player;

        private float _liveTime = 0;
        private const float DestroyTime = 3.3f;
        
        private Vector3 _nearestEnemy;
        private Vector3 _bulletDirection;
        
        private void Awake()
        {
            _player = GameObject.FindWithTag("Player").GetComponent<Player>();
            if (_player.DiagonalStar.CalculateFinalValue() < 1) { Destroy(gameObject); }
        }

        private void Start()
        {
            IsAvailable();
            Debug.Log("Start");
        }

        private void Update()
        {
            _liveTime += Time.deltaTime;
            if (_liveTime > DestroyTime) { Destroy(gameObject); }
            
            transform.position += _bulletDirection * (bulletSpeed * Time.deltaTime);
            transform.Rotate(0, 0, -230 * Time.deltaTime);
            Debug.Log("Update");
        }

        private void OnTriggerEnter2D(Collider2D coll)
        {
            if (!coll.CompareTag("Enemy")) return;
            
            Destroy(gameObject);
            
            var monster = coll.gameObject.GetComponent<IMonster>();
            var skillLevelBonus = (float)(0.8 + 0.9 * _player.BasicStar.CalculateFinalValue());
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

                _player.GetComponent<PlayerAttack>().diagonalAngle += 1;
                var toDiagonal = _player.GetComponent<PlayerAttack>().diagonalAngle * 30;
                
                _bulletDirection = new Vector3(Random.Range(0, 90) * toDiagonal, 0, 0);
                Debug.Log(toDiagonal);
                Debug.Log(_bulletDirection);
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
