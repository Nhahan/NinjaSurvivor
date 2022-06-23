using System.Collections;
using Monsters;
using Status;
using UnityEngine;

namespace AdSkills
{
    public class ThrowingStar : MonoBehaviour
    {
        [SerializeField] private float bulletSpeed = 11f;
        [SerializeField] private float damageMultiplier = 1;

        private Player _player;

        private float _liveTime = 0;
        private const float DestroyTime = 6f;
        private float _bulletDirection = 1;

        private void Awake()
        {
            _player = GameManager.Instance.GetPlayer();
            if (_player.BasicStar.CalculateFinalValue() < 1) { Destroy(gameObject); }
        }

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(2);
            _bulletDirection = GetRandomSign();
        }

        private void FixedUpdate()
        {
            _liveTime += Time.deltaTime;
            if (_liveTime > DestroyTime) { Destroy(gameObject); }
            
            transform.Translate(_bulletDirection * bulletSpeed * Time.deltaTime * Vector3.up);
            transform.Rotate(0, 0, -350 * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D coll)
        {
            if (!coll.CompareTag("Enemy")) return;
            
            Destroy(gameObject);
            
            var monster = coll.gameObject.GetComponent<IMonster>();
            var skillLevelBonus = (float)(0.6 + 0.05 * _player.BasicStar.CalculateFinalValue());
            var damage = _player.AttackDamage.CalculateFinalValue() * damageMultiplier * skillLevelBonus;
            
            monster.TakeDamage(damage);
        }

        private int GetRandomSign()
        {
            int[] plusMinus = { 1, -1 };
            var idx = Random.Range(0, 2);
            return plusMinus[idx];
        }
    }
}
