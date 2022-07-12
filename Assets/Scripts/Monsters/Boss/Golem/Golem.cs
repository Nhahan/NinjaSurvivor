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

        private float _monsterHp = 10000f;
        private float _monsterSpeed = 6.75f;
        private float _monsterSpeedMultiplier = 1;
        private float _distance;
        private const float MonsterDefense = 4f;

        private BossState _bossState;
        private float _idleCooltime;
        private float _movingCooltime;
        private float _laserCooltime;
        private float _earthQuakeCooltime;
        private float _handUpCooltime;
        private float _lighteningCooltime;

        private bool _isShieldOn;

        [SerializeField] private GameObject handUpPoint;
        [SerializeField] private GameObject laser;
        [SerializeField] private GameObject fire;
        [SerializeField] private GameObject energyShield;
        [SerializeField] private GameObject rock;

        private void Start()
        {
            GameManager.Instance.SetBoss(gameObject);
            
            _player = GameManager.Instance.GetPlayer();
            _animator = GetComponent<Animator>();
            Indicator = GameManager.Instance.indicator;
            _rb = GetComponent<Rigidbody2D>();
            
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
            StartCoroutine(Pattern());
        }

        private IEnumerator MovementSpeed()
        {
            while (_monsterHp > 0) {
                if (_distance > 4.55f && _bossState == BossState.Moving)
                {
                    _monsterSpeedMultiplier = _monsterSpeedMultiplier == 1 ? 0 : 1;
                }
                yield return new WaitForSeconds(0.55f);
            }
        }

        private IEnumerator Pattern()
        {
            if (_monsterHp > 4000)
            {
                yield return new WaitForSeconds(4f);
                WhenNotMoving();
            }
            else
            {
                yield return new WaitForSeconds(3.25f);
                WhenNotMovingAngry();
            }
        }

        private void WhenNotMoving()
        {
            if (_handUpCooltime > 8f)
            {
                _animator.SetBool("isHandUp", true);
                _bossState = BossState.Attacking;
                _handUpCooltime = 0;
                _monsterSpeedMultiplier = 0f;
                StartCoroutine(BackToWantedState("isHandUp", false, 3f));
                StartCoroutine(InstantiateHandUpFire());
            }
            else if (_laserCooltime > 14f)
            {
                _animator.SetBool("isFloorLaser", true);
                _bossState = BossState.Attacking;
                _laserCooltime = 0;
                _monsterSpeedMultiplier = 0f;
                StartCoroutine(BackToWantedState("isFloorLaser", false, 1.25f));
                StartCoroutine(InstantiateEnergyShield());
            }
            else if (_lighteningCooltime > 20f)
            {
                _animator.SetBool("isLightening", true);
                _bossState = BossState.Attacking;
                _lighteningCooltime = 0;
                _monsterSpeedMultiplier = 0f;
                StartCoroutine(BackToWantedState("isLightening", false, 3.25f));
                StartCoroutine(InstantiateLaser());
            }
            else if (_earthQuakeCooltime > 28f)
            {
                _animator.SetBool("isEarthQuake", true);
                _bossState = BossState.Attacking;
                _earthQuakeCooltime = 0;
                _monsterSpeedMultiplier = 0f;
                StartCoroutine(BackToWantedState("isEarthQuake", false, 2.5f));
                StartCoroutine(InstantiateRock());
            }
            else
            {
                _bossState = BossState.Moving;
            }
        }
        
        private void WhenNotMovingAngry()
        {
            if (_handUpCooltime > 7f)
            {
                _animator.SetBool("isHandUp", true);
                _bossState = BossState.Attacking;
                _handUpCooltime = 0;
                _monsterSpeedMultiplier = 0f;
                StartCoroutine(BackToWantedState("isHandUp", false, 2.7f));
                StartCoroutine(InstantiateHandUpFire());
            }
            else if (_laserCooltime > 13f)
            {
                _animator.SetBool("isFloorLaser", true);
                _bossState = BossState.Attacking;
                _laserCooltime = 0;
                _monsterSpeedMultiplier = 0f;
                StartCoroutine(BackToWantedState("isFloorLaser", false, 0.75f));
                StartCoroutine(InstantiateEnergyShield());
            }
            else if (_lighteningCooltime > 18f)
            {
                _animator.SetBool("isLightening", true);
                _bossState = BossState.Attacking;
                _lighteningCooltime = 0;
                _monsterSpeedMultiplier = 0f;
                StartCoroutine(BackToWantedState("isLightening", false, 3f));
                StartCoroutine(InstantiateLaser());
            }
            else if (_earthQuakeCooltime > 23f)
            {
                _animator.SetBool("isEarthQuake", true);
                _bossState = BossState.Attacking;
                _earthQuakeCooltime = 0;
                _monsterSpeedMultiplier = 0f;
                StartCoroutine(BackToWantedState("isEarthQuake", false, 2f));
                StartCoroutine(InstantiateRock());
            }
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
            _monsterSpeedMultiplier = 0;
            GameManager.Instance.SetBoss(null);
            StartCoroutine(BeforeDestroy(_animator.GetCurrentAnimatorStateInfo(0).length));
        }

        private IEnumerator InstantiateHandUpFire()
        {
            var count = 1;
            if (_monsterHp < 4000f) count = 3;
            if (_monsterHp < 1250f) count = 5;
            yield return new WaitForSeconds(0.35f);
            for (var i = 0; i < count; i++ ) 
            {
                Instantiate(fire, handUpPoint.transform.position, transform.rotation);
                yield return new WaitForSeconds(1f);
            }
        }
        
        private IEnumerator InstantiateEnergyShield()
        {
            _isShieldOn = true;
            var isAngry = _monsterHp < 4000f;
            Instantiate(energyShield, transform.position, transform.rotation).GetComponent<EnergyShield>().SetGolem(gameObject, isAngry);
            yield return new WaitForSeconds(8.4f);
            _isShieldOn = false;
        }
        
        private IEnumerator InstantiateLaser()
        {
            var count = 9;
            if (_monsterHp < 4000f) count += 18;
            if (_monsterHp < 1250f) count += 18;
            for (var i = 0; i < count; i++ ) 
            {
                yield return new WaitForSeconds(3.7f / count);
                Instantiate(laser, transform.position, transform.rotation);
            }
        }
        
        private IEnumerator InstantiateRock()
        {
            var count = 24;
            var delay = 3.5f / count;
            if (_monsterHp < 4000f)
            {
                count *= 2;
                delay = (3.5f + 1.8f) / count;
            }
            yield return new WaitForSeconds(1f);
            for (var i = 0; i < count; i++ ) 
            {
                
                Instantiate(rock, _player.transform.position, transform.rotation);
                Instantiate(rock, _player.transform.position, transform.rotation);
                yield return new WaitForSeconds(delay / 2);
                if (_monsterHp < 4000f)
                {
                    Instantiate(rock, _player.transform.position, transform.rotation);
                    Instantiate(rock, _player.transform.position, transform.rotation);
                }
                yield return new WaitForSeconds(delay / 2);
            }
        }

        public void SetIsShieldOnToFalse()
        {
            _isShieldOn = false;
        }
    }
}