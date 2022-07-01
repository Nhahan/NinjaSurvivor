using Monsters;
using Status;
using UnityEngine;

namespace ApSkills
{
    public class FireCross : MonoBehaviour
    {
        [SerializeField] private float damageMultiplier = 1;

        private Player _player;
        private BoxCollider2D _collider2D;
        
        private float _damage;
        private bool _isOpposite;
        private float _liveTime;
    
        private float _skillLevelMultiplier = 0.175f;

        private void Start()
        {
            _player = GameManager.Instance.GetPlayer();
            _collider2D = GetComponent<BoxCollider2D>();

            var skillLevelBonus = _skillLevelMultiplier * _player.FireCross.CalculateFinalValue();
            Debug.Log("FireCross init");
            Debug.Log(_player.FireCross.CalculateFinalValue());
            _damage = _player.Damage() * damageMultiplier * skillLevelBonus + _player.Damage() * 0.5f;
            Destroy(gameObject, 0.6f);
        }

        private void FixedUpdate()
        {
            _collider2D.size += new Vector2(0, Time.deltaTime / 5.5f * 13);
            
            if (_liveTime > 0.1f) return;
            _liveTime += Time.deltaTime;

            transform.position = _player.transform.position;
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
