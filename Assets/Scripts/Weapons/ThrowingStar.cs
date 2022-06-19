using Monsters;
using Status;
using UnityEngine;

namespace Weapons
{
    public class ThrowingStar : MonoBehaviour
    {
        [SerializeField] private float bulletSpeed = 11f;
        [SerializeField] private float coefficient = 1;

        private Player _player;

        private float _liveTime = 0;
        private float _bulletDirection = 1;

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
            _bulletDirection = GetRandomSign();
            var bulletVelocity = PlayerController.LatestDirection;
            transform.position = Vector2.MoveTowards(transform.position, bulletVelocity, bulletSpeed * Time.deltaTime);
        }

        private void Update()
        {
            _liveTime += Time.deltaTime;
            if (_liveTime > 8)
            {
                Destroy(gameObject);
            }
            transform.Translate(_bulletDirection * bulletSpeed * Time.deltaTime * Vector3.up);
            transform.Rotate(0, 0, -300 * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D coll)
        {
            if (!coll.CompareTag("Enemy")) return;
            Destroy(gameObject);
            coll.gameObject.GetComponent<IMonster>().TakeDamage(_player.AttackDamage.CalculateFinalValue() * coefficient);
        }

        private int GetRandomSign()
        {
            int[] plusMinus = { 1, -1 };
            var idx = Random.Range(0, 2);
            Debug.Log(plusMinus[idx]);
            return plusMinus[idx];
        }
    }
}
