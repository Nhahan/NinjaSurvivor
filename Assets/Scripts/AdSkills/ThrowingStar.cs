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

        private Player _player;

        private float _bulletSpeed = 0.15f;
        private float _liveTime = 0;
        private float _duration = 4.5f;
        private float _skillLevelBonus;
        private float _damageMultiplier = 1;
        
        private void Start()
        {
            _player = GameManager.Instance.GetPlayer();
            
            _skillLevelBonus = 1.6f + 0.9f * _player.ThrowingStar.CalculateFinalValue();
            _duration += _player.ThrowingStar.CalculateFinalValue() * 0.12f;
        }

        private void FixedUpdate()
        {
            _liveTime += Time.deltaTime;
            if (_liveTime > _duration) { Destroy(gameObject); }
            
            transform.Translate(_bulletSpeed * _liveTime * Vector3.up);
            transform.Rotate(0, 0, -350 * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D coll)
        {
            if (!coll.CompareTag("Enemy")) return;

            var monster = coll.gameObject.GetComponent<IMonster>();
            var damage = _player.Damage() * _damageMultiplier * _skillLevelBonus;
            
            monster.TakeDamage(damage);
            
            if (Random.Range(0,10) < 5.1f * 0.5f) Destroy(gameObject);
        }
    }
}
