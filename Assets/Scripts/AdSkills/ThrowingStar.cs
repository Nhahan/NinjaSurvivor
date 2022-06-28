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

        private float _bulletSpeed = 1f;
        private float _liveTime = 0;
        private float _duration = 3.5f;
        private float _damageMultiplier = 1f;
        private float _baseSkillDamage = 10f;
        private float _skillLevelMultiplier = 0.2f;
        private float _damage;
        
        private void Start()
        {
            _player = GameManager.Instance.GetPlayer();
            
            var skillLevelBonus = _skillLevelMultiplier * _player.ThrowingStar.CalculateFinalValue();
            _damage = _player.Damage() * _damageMultiplier * skillLevelBonus + _baseSkillDamage;
            _duration += _player.ThrowingStar.CalculateFinalValue() * 0.12f;
        }

        private void FixedUpdate()
        {
            _liveTime += Time.deltaTime;
            if (_liveTime > _duration) { Destroy(gameObject); }
            
            transform.Translate(_bulletSpeed * (-_liveTime) * Vector3.up);
            transform.Rotate(0, 0, -350 * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D coll)
        {
            if (!coll.CompareTag("Enemy")) return;

            var monster = coll.gameObject.GetComponent<IMonster>();
            
            monster.TakeDamage(_damage);
            
            if (Random.Range(0,10) > 3.8f + _player.ThrowingStar.CalculateFinalValue() * 0.5f) Destroy(gameObject);
        }
    }
}
