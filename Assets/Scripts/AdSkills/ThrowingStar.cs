using System;
using System.Collections;
using Monsters;
using Status;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AdSkills
{
    public class ThrowingStar : MonoBehaviour
    {
        [SerializeField] private float bulletSpeed = 11f;
        [SerializeField] private float damageMultiplier = 1;

        private Player _player;

        private float _liveTime = 0;
        private const float Duration = 6f;
        private float _bulletDirection = 1;
        private float _skillLevelBonus;
        
        private void Start()
        {
            _player = GameManager.Instance.GetPlayer();
            _bulletDirection = GetRandomSign();
            _skillLevelBonus = 1.6f + 0.9f * _player.ThrowingStar.CalculateFinalValue();
        }

        private void FixedUpdate()
        {
            _liveTime += Time.deltaTime;
            if (_liveTime > Duration) { Destroy(gameObject); }
            
            transform.Translate(_bulletDirection * bulletSpeed * Time.deltaTime * Vector3.up);
            transform.Rotate(0, 0, -350 * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D coll)
        {
            if (!coll.CompareTag("Enemy")) return;

            var monster = coll.gameObject.GetComponent<IMonster>();
            var damage = _player.AttackDamage.CalculateFinalValue() + damageMultiplier * _skillLevelBonus;
            
            monster.TakeDamage(damage);
            
            if (Random.Range(0,10) < 5.1f * 0.5f) Destroy(gameObject);
        }

        private int GetRandomSign()
        {
            int[] plusMinus = { 1, -1 };
            var idx = Random.Range(0, 2);
            return plusMinus[idx];
        }
    }
}
