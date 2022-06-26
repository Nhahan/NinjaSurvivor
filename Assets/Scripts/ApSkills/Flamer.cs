using System.Collections;
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
    
        private bool _isAvailable = true;
        private float _damage;

        private void Start()
        {
            _player = GameManager.Instance.GetPlayer();
            _animator = GetComponent<Animator>();
            _flamer = _player.transform.Find("SkillPoints").Find("Flamer");
            
            var skillLevelBonus = 2f + 2f * _player.Flamer.CalculateFinalValue();
            _damage = _player.AttackDamage.CalculateFinalValue() * damageMultiplier * skillLevelBonus + Random.Range(-2, 0);
            
            var animationLength = _animator.GetCurrentAnimatorStateInfo(0).length;
            
            Invoke(nameof(ToNotAvailable), animationLength / 0.7f);
            StartCoroutine(BeforeDestroy(_animator.GetCurrentAnimatorStateInfo(0).length));
        }

        private IEnumerator BeforeDestroy(float second)
        {
            yield return new WaitForSeconds(second);
            Destroy(gameObject);
        }

        private void FixedUpdate()
        {
            if (!_isAvailable) return;
            FlipSprite();
            transform.position = _flamer.position;
        }

        private void ToNotAvailable()
        {
            _isAvailable = false;
        }

        private void OnTriggerEnter2D(Collider2D coll)
        {
            if (!_isAvailable) return;
            if (!coll.CompareTag("Enemy")) return;

            Debug.Log("Flamer" + _damage);

            var monster = coll.gameObject.GetComponent<IMonster>();
            monster.TakeDamage(_damage);
        }
    
        private void FlipSprite()
        {
            transform.localScale = new Vector2(Mathf.Sign((_flamer.transform.position - transform.position).normalized.x), 1f);
        }
    }
}
