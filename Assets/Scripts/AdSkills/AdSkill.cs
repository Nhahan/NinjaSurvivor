using System.Diagnostics.CodeAnalysis;
using System.Collections;
using System.Linq;
using AdSkills;
using Monsters;
using Status;
using UnityEngine;

namespace AdSkills
{
    public class AdSkill : MonoBehaviour, IAdSkill
    {
        [SerializeField] private float bulletSpeed = 11f;
        [SerializeField] private float possibleAttackDistance = 11f;
        [SerializeField] private float damageMultiplier = 1;

        private Player _player;

        private float _liveTime = 0;
        private const float DestroyTime = 3.3f;
        private Vector3 _nearestEnemy;
        private Vector3 _bulletDirection;

        void IAdSkill.Awake()
        {
            _player = GameObject.FindWithTag("Player").GetComponent<Player>();
            if (_player.BasicStar.CalculateFinalValue() < 1)
            {
                Destroy(gameObject);
            }
        }

        void IAdSkill.Start()
        {
            IsAvailable();
        }

        void IAdSkill.Update()
        {
            _liveTime += Time.deltaTime;
            if (_liveTime > DestroyTime)
            {
                Destroy(gameObject);
            }
            transform.position = transform.position + _bulletDirection * (bulletSpeed * Time.deltaTime);
            transform.Rotate(0, 0, -230 * Time.deltaTime);
        }

        void IAdSkill.OnTriggerEnter2D(Collider2D coll)
        {
            if (!coll.CompareTag("Enemy")) return;
            Destroy(gameObject);
            var monster = coll.gameObject.GetComponent<IMonster>();

            var skillLevelBonus = (float)(1 + 0.1 * _player.BasicStar.CalculateFinalValue());

            var damage = _player.AttackDamage.CalculateFinalValue() * damageMultiplier * skillLevelBonus;
            monster.TakeDamage(damage);
        }

        void IAdSkill.IsAvailable()
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
