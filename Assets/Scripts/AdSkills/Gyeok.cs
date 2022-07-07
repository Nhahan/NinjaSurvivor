using System.Collections;
using Monsters;
using Status;
using UnityEngine;

namespace AdSkills
{
    public class Gyeok : MonoBehaviour
    {
        [SerializeField] private float damageMultiplier = 1;

        private Player _player;
        private Transform _flamer;
        private float _damage;
        private bool _isOpposite;
        private float _liveTime;
        
        private float _skillLevelMultiplier = 0.2f;

        private float _skillLevel;

        private void Start()
        {
            _player = GameManager.Instance.GetPlayer();
            _skillLevel = _player.Gyeok.CalculateFinalValue();

            var skillLevelBonus = _skillLevelMultiplier * _skillLevel;
            if (_skillLevel > 4)
            {
                skillLevelBonus *= 1.5f;
            }
            else if (_skillLevel > 9)
            {
                skillLevelBonus *= 1.75f;
            }
            else if (_skillLevel > 14)
            {
                skillLevelBonus *= 2f;
            }
            else if (_skillLevel > 14)
            {
                skillLevelBonus *= 2.25f;
            }
            else
            {
                skillLevelBonus *= 1.25f;
            }
            
            _damage = _player.Damage() * damageMultiplier * skillLevelBonus;
            Destroy(gameObject, 0.1f * (_skillLevel  < 5 ? 5 : _skillLevel) + 0.1f);
        }

        private void FixedUpdate()
        {
            _liveTime += Time.deltaTime;
        }

        private void OnTriggerEnter2D(Collider2D coll)
        {
            if (!coll.CompareTag("Enemy") || _liveTime > 0.25f) return;

            var monster = coll.gameObject.GetComponent<IMonster>();
            var normal = (coll.gameObject.transform.position - transform.position).normalized;

            StartCoroutine(AttackTimes(monster, normal));
        }

        public IEnumerator AttackTimes(IMonster monster, Vector3 normal)
        {
            var count = _skillLevel  < 5 ? 5 : _skillLevel;
            
            for (var i = 0; i < count; i++)
            {
                try
                {
                    monster.TakeDamage(_damage + Random.Range(-2, 3));

                    if (count % 3 == 0)
                    {
                        monster.StartKnockback(normal);
                    }
                }
                catch
                {
                    break;
                }
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
