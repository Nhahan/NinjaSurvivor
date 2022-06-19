using System.Diagnostics.CodeAnalysis;
using System.Collections;
using System.Linq;
using Monsters;
using Status;
using UnityEngine;

namespace Weapons
{
    [SuppressMessage("ReSharper", "Unity.InefficientPropertyAccess")]
    public class BasicStar : MonoBehaviour
    {
        [SerializeField] private float bulletSpeed = 11f;
        [SerializeField] private float possibleAttackDistance = 11f;
        [SerializeField] private float damageMultiplier = 1;

        private Player _player;

        private float _liveTime = 0;
        private Vector3 _nearestEnemy;
        private Vector3 _bulletDirection;

        private void Awake()
        {
            _player = GameObject.FindWithTag("Player").GetComponent<Player>();
            if (_player.BasicStar.CalculateFinalValue() < 1)
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            IsAvailable();
            StartCoroutine(this.LuckySeven());
        }
        
        private IEnumerator LuckySeven()
        {
            var luckySeven = (int)_player.LuckySeven.CalculateFinalValue();
            if (luckySeven < 1) yield break;
            
            yield return new WaitForSeconds(0.5f);
                
            for (var i = 0; i <= luckySeven; i++)
            {
                Instantiate(this, _player.transform.position, this.transform.rotation);
            }
        }

        private void Update()
        {
            _liveTime += Time.deltaTime;
            if (_liveTime > 3.3)
            {
                Destroy(gameObject);
            }
            transform.position = transform.position + _bulletDirection * (bulletSpeed * Time.deltaTime);
            transform.Rotate(0, 0, -230 * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D coll)
        {
            if (!coll.CompareTag("Enemy")) return;
            Destroy(gameObject);
            var monster = coll.gameObject.GetComponent<IMonster>();
            
            var damage = _player.AttackDamage.CalculateFinalValue() * damageMultiplier;
            monster.TakeDamage(damage);
        }

        private void IsAvailable()
        {
            try
            {
                _nearestEnemy = FindNearestObject().transform.position;
                if (_nearestEnemy == null || Vector3.Distance(_nearestEnemy, transform.position) > possibleAttackDistance)
                {
                    Destroy(gameObject);
                }

                _bulletDirection = (
                        _nearestEnemy -
                        transform.position -
                        new Vector3((float)(Random.Range(-1, 2)), (float)(Random.Range(-1, 2)), 0))
                    .normalized;
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
