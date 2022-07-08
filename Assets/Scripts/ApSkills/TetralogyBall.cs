using Monsters;
using Status;
using UnityEngine;

namespace ApSkills
{
    public class TetralogyBall : MonoBehaviour
    {
        private Player _player;
        private Transform _flamer;
        private float _damage;
        private bool _isOpposite;
        private float _liveTime;
        private float _damageMultiplier = 1f;

        private float _skillLevelMultiplier = 0.1f;

        private void Start()
        {
            _player = GameManager.Instance.GetPlayer();

            var skillLevelBonus = _skillLevelMultiplier * _player.Tetralogy.CalculateFinalValue();
            _damage = _player.Damage() * _damageMultiplier * skillLevelBonus + _player.Damage() * 0.75f;
        }

        private void OnTriggerEnter2D(Collider2D coll)
        {
            if (!(coll.CompareTag("Enemy") || coll.CompareTag("Boss"))) return;

            var monster = coll.gameObject.GetComponent<IMonster>();
            var normal = (coll.gameObject.transform.position - transform.position).normalized;

            monster.TakeDamage(_damage);

            if (Random.Range(0, 10) < 3f)
            {
                monster.StartKnockback(normal);
            }
        }
    }
}