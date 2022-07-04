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
        private float _duration = 2f;
        private float _damageMultiplier = 1f;
        private float _baseSkillDamage = 9.5f;
        private float _skillLevelMultiplier = 0.15f;
        private float _damage;
        
        private void Start()
        {
            _player = GameManager.Instance.GetPlayer();
            
            var skillLevelBonus = _skillLevelMultiplier * _player.ThrowingStar.CalculateFinalValue();
            _damage = _player.Damage() * _damageMultiplier * skillLevelBonus + _baseSkillDamage;
            _duration += _player.ThrowingStar.CalculateFinalValue() * 0.09f;
        }

        private void FixedUpdate()
        {
            _liveTime += Time.deltaTime;
            if (_liveTime > _duration) { Destroy(gameObject); }
            
            transform.Translate(-_liveTime *_bulletSpeed * Vector3.up);
            transform.Rotate(0, 0, -450 * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D coll)
        {
            if (!coll.CompareTag("Enemy")) return;

            var monster = coll.gameObject.GetComponent<IMonster>();
            
            monster.TakeDamage(_damage);
            
            if (Random.Range(0,10) > 7f + _player.ThrowingStar.CalculateFinalValue() * 0.1f) Destroy(gameObject);
        }
    }
}
