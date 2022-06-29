using System.Collections;
using System.Security.Cryptography;
using Monsters;
using Status;
using UnityEngine;

namespace ApSkills
{
    public class Flamer : MonoBehaviour
    {
        [SerializeField] private float damageMultiplier = 1;

        private Player _player;
        private Transform _flamer;
        private Animator _animator;
        private float _damage;
        private bool _isOpposite;
        
        private float _skillLevelMultiplier = 0.4f;

        private void Start()
        {
            _player = GameManager.Instance.GetPlayer();
            _flamer = _player.transform.Find("SkillPoints").Find("Flamer");
            TransformUpdate();
            
            _animator = GetComponent<Animator>();
            
            var skillLevelBonus = _skillLevelMultiplier * _player.Flamer.CalculateFinalValue();
            _damage = _player.Damage() * damageMultiplier * skillLevelBonus + _player.Damage();
            Destroy(gameObject, 0.9f);
        }

        private void FixedUpdate()
        {
            TransformUpdate();
        }

        private void OnTriggerEnter2D(Collider2D coll)
        {
            if (!coll.CompareTag("Enemy")) return;

            var monster = coll.gameObject.GetComponent<IMonster>();
            
            monster.TakeDamage(_damage);
        }
    
        private void TransformUpdate()
        {
            var fireDirection = Mathf.Sign((_flamer.transform.position - _player.transform.position).x);
            if (_isOpposite)
            {
                fireDirection *= -1;
            }
            transform.localScale = new Vector2(Mathf.Sign(fireDirection), 1f);
            transform.position = _player.transform.position + new Vector3(fireDirection * 2.35f, 0.175f, 0);
        }

        public void SetOpposite(bool value)
        {
            _isOpposite = true;
        }
    }
}
