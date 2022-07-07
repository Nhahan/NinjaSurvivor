using System;
using Monsters;
using Status;
using UnityEngine;

namespace AdSkills
{
    public class Slash : MonoBehaviour
    {
        [SerializeField] private float damageMultiplier = 1;

        private Player _player;
        private Transform _flamer;
        private SpriteRenderer _slash;
        private float _damage;
        private float _liveTime;
        
        private float _skillLevelMultiplier = 0.215f;

        private void Start()
        {
            _player = GameManager.Instance.GetPlayer();
            _flamer = _player.transform.Find("SkillPoints").Find("Flamer");
            _slash = transform.GetChild(0).GetComponent<SpriteRenderer>();
            
            var attackDirection = Mathf.Sign((_flamer.transform.position - _player.transform.position).x);
            if (attackDirection < 0)
            {
                transform.localScale *= -1;
            }

            if (_player.Slash.CalculateFinalValue() > 9)
            {
                transform.localScale *= 1.15f;
                _slash.color = Color.red;
            }

            var skillLevelBonus = _skillLevelMultiplier * _player.Slash.CalculateFinalValue();
            _damage = _player.Damage() * damageMultiplier * skillLevelBonus + _player.Damage() * 0.4f;
            Destroy(gameObject, 0.52f);
        }

        private void FixedUpdate()
        {
            _slash.color = new Color(_slash.color.r / 2, _slash.color.g / 2,
                _slash.color.b / 2, _slash.color.a);
        }

        private void OnTriggerEnter2D(Collider2D coll)
        {
            if (!coll.CompareTag("Enemy")) return;
                        
            var monster = coll.gameObject.GetComponent<IMonster>();

            var normal = (coll.gameObject.transform.position - transform.position).normalized;
            monster.TakeDamage(_damage);
            monster.StartKnockback(normal);
        }
    }
}
