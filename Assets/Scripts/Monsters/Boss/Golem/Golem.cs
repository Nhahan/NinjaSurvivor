using System.Collections;
using Status;
using UnityEngine;

namespace Monsters.Boss
{
    internal enum BossState 
    {
        Moving,
        Attacking,
    }

    public class Golem : Monster, IMonster
    {
        private Player _player;
        private Animator _animator;
        private Rigidbody2D _rb;

        private float _monsterHp = 10000;
        private float _monsterSpeed = 4.5f;
        private float _monsterSpeedMultiplier = 1;
        private float _distance;
        private const float MonsterDefense = 10f;

        private BossState _bossState;
        private float _idleCooltime; // 7
        private float _movingCooltime; // 7
        private float _laserCooltime; // 7
        private float _earthQuakeCooltime; // 20
        private float _handUpCooltime; // 30
        private float _lighteningCooltime; // 40 

        private bool _isShieldOn;

        [SerializeField] private GameObject handUpPoint;
        [SerializeField] private GameObject laser;
        [SerializeField] private GameObject fire;
        [SerializeField] private GameObject energyShield;

        private void Start()
        {
            GameManager.Instance.boss = gameObject;
            
            _player = GameManager.Instance.GetPlayer();
            _animator = GetComponent<Animator>();
            Indicator = GameManager.Instance.indicator;
            _rb = GetComponent<Rigidbody2D>();
            
            KnockbackDuration = 0.015f;

            _bossState = BossState.Moving;
            
            StartCoroutine(MovementSpeed());
        }

        private void FixedUpdate()
        {
            _laserCooltime += Time.deltaTime;
            _earthQuakeCooltime += Time.deltaTime;
            _lighteningCooltime += Time.deltaTime;
            _handUpCooltime += Time.deltaTime;
            _distance = Vector3.Distance(transform.position, _player.transform.position);
            Moving();
            Pattern();
        }

        private IEnumerator MovementSpeed()
        {
            while (_monsterHp > 0) {
                if (_distance > 4.75f && _bossState == BossState.Moving)
                {
                    _monsterSpeedMultiplier = _monsterSpeedMultiplier == 1 ? 0 : 1;
                }
                yield return new WaitForSeconds(0.6f);
            }
        }

        private void Pattern()
        {
            Invoke(nameof(WhenNotMoving), 4f);
        }

        private void WhenNotMoving()
        {
            if (_handUpCooltime > 7.5f)
            {
                _animator.SetBool("isHandUp", true);
                _bossState = BossState.Attacking;
                _handUpCooltime = 0;
                _monsterSpeedMultiplier = 0f;
                StartCoroutine(BackToWantedState("isHandUp", false, 3f));
                StartCoroutine(InstantiateHandUpFire());
            }
            else if (_laserCooltime > 11.5f)
            {
                _animator.SetBool("isFloorLaser", true);
                _bossState = BossState.Attacking;
                _laserCooltime = 0;
                _monsterSpeedMultiplier = 0f;
                StartCoroutine(BackToWantedState("isFloorLaser", false, .9f));
                StartCoroutine(InstantiateEnergyShield());
            }
            else if (_lighteningCooltime > 20f)
            {
                _animator.SetBool("isLightening", true);
                _bossState = BossState.Attacking;
                _lighteningCooltime = 0;
                _monsterSpeedMultiplier = 0f;
                StartCoroutine(BackToWantedState("isLightening", false, 3.85f));
                StartCoroutine(InstantiateLaser());
            }
            // else if (_earthQuakeCooltime > 30f)
            // {
            //     _animator.SetBool("isEarthQuake", true);
            //     Debug.Log("isEarthQuake");
            //     _bossState = BossState.Attacking;
            //     _earthQuakeCooltime = 0;
            //     _monsterSpeedMultiplier = 0f;
            //     StartCoroutine(BackToWantedState("isEarthQuake", false, 3.75f));
            // }
            else
            {
                _bossState = BossState.Moving;
            }
        }

        private void Moving()
        {
            if (_distance > 4.75f && _bossState == BossState.Moving && _animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) 
            {
                Vector2 direction = (_player.transform.position - transform.position).normalized;
                _rb.MovePosition(_rb.position + direction * (_monsterSpeed * Time.fixedDeltaTime * _monsterSpeedMultiplier));
                FlipSprite();
            }
        }

        private IEnumerator BackToWantedState(string n, bool b, float s)
        {
            yield return new WaitForSeconds(s);
            _animator.SetBool(n, b);
        }

        private IEnumerator IsAttackingToFalse(float second)
        {
            yield return new WaitForSeconds(second);
            _animator.SetBool("isAttacking", false);
        }

        private void FlipSprite()
        {
            transform.localScale = transform.position.x < _player.transform.position.x ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);
        }

        public void TakeDamage(float damage)
        {
            
            
            if (_isShieldOn) return;
            _monsterHp = _monsterHp - damage + MonsterDefense;
            ShowDamage(damage);
            Flash();

            if (_monsterHp > 0) return;
            _animator.SetBool("isDead", true);
            _animator.SetBool("isDead", true);
            _monsterSpeedMultiplier = 0;
            GameManager.Instance.boss = null;
            StartCoroutine(BeforeDestroy(_animator.GetCurrentAnimatorStateInfo(0).length));
        }

        private IEnumerator InstantiateHandUpFire()
        {
            var count = 1;
            if (_monsterHp < 2500) count = 3;
            yield return new WaitForSeconds(0.5f);
            for (var i = 0; i < count; i++ ) 
            {
                Instantiate(fire, handUpPoint.transform.position, transform.rotation);
                yield return new WaitForSeconds(1f);
            }
        }
        
        private IEnumerator InstantiateEnergyShield()
        {
            _isShieldOn = true;
            var isAngry = _monsterHp < 2500;
            Instantiate(energyShield, transform.position, transform.rotation).GetComponent<EnergyShield>().SetGolem(gameObject, isAngry);
            yield return new WaitForSeconds(4.2f);
            _isShieldOn = false;
        }
        
        private IEnumerator InstantiateLaser()
        {
            var count = 6;
            if (_monsterHp < 2500) count += 18;
            for (var i = 0; i < count; i++ ) 
            {
                yield return new WaitForSeconds(3.7f / count);
                Instantiate(laser, transform.position, transform.rotation);
            }
        }

        public void SetIsShieldOnToFalse()
        {
            Debug.Log("please...");
            _isShieldOn = false;
        }
    }
}