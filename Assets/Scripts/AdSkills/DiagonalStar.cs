using System.Linq;
using Monsters;
using Status;
using UnityEngine;

namespace AdSkills
{
    public class DiagonalStar : MonoBehaviour
    {
        [SerializeField] private float bulletSpeed = 12.5f;
        [SerializeField] private float possibleAttackDistance = 10f;
        [SerializeField] private float damageMultiplier = 1;

        private Player _player;

        private float _liveTime = 0;
        private const float DestroyTime = 2.5f;
        
        private Vector3 _nearestEnemy;
        private Vector3 _bulletDirection;

        private void Start()
        {
            _player = GameManager.Instance.GetPlayer();
            IsAvailable();
        }

        private void FixedUpdate()
        {
            _liveTime += Time.deltaTime;
            if (_liveTime > DestroyTime) { Destroy(gameObject); }
            
            transform.position = Vector2.MoveTowards(transform.position, _bulletDirection * 1.5f, bulletSpeed * Time.deltaTime);
            transform.Rotate(0, 0, -450 * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D coll)
        {
            if (!coll.CompareTag("Enemy")) return;
            
            Destroy(gameObject);
            
            var monster = coll.gameObject.GetComponent<IMonster>();
            var skillLevelBonus = 1.1f * _player.DiagonalStar.CalculateFinalValue();
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

                var childNum = Random.Range(0, 16);
                
                _bulletDirection = (_player.transform.GetChild(0).GetChild(childNum).position) - 
                                   new Vector3((float)(Random.Range(-10, 2)), (float)(Random.Range(-1, 2)), 0);
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
