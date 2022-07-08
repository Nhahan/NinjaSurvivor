using System;
using System.Collections;
using System.Net;
using Status;
using UnityEngine;
using Random = UnityEngine.Random;

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

        private float _monsterHp = 499;
        private const float MonsterDamage = 10f;
        private float _randomDamage;
        private const float MonsterSpeed = 4.5f;
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

        [SerializeField] private GameObject handUpPoint;
        [SerializeField] private GameObject laser;
        [SerializeField] private GameObject fire;

        private void Start()
        {
            _player = GameManager.Instance.GetPlayer();
            _animator = GetComponent<Animator>();
            Indicator = GameManager.Instance.indicator;

            _randomDamage = Random.Range(10, 25);
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
            if (_handUpCooltime > 6.5f)
            {
                _animator.SetBool("isHandUp", true);
                _bossState = BossState.Attacking;
                _handUpCooltime = 0;
                _monsterSpeedMultiplier = 0f;
                StartCoroutine(InstantiateHandUpFire());
                StartCoroutine(BackToWantedState("isHandUp", false, 3.85f));
            }
            else if (_laserCooltime > 14f)
            {
                _animator.SetBool("isFloorLaser", true);
                Debug.Log("isFloorLaser");
                _bossState = BossState.Attacking;
                _laserCooltime = 0;
                _monsterSpeedMultiplier = 0f;
                StartCoroutine(BackToWantedState("isFloorLaser", false, 2.5f));
            }
            else if (_lighteningCooltime > 24f)
            {
                _animator.SetBool("isLightening", true);
                Debug.Log("isLightening");
                _bossState = BossState.Attacking;
                _lighteningCooltime = 0;
                _monsterSpeedMultiplier = 0f;
                StartCoroutine(BackToWantedState("isLightening", false, 3.5f));
            }
            else if (_earthQuakeCooltime > 30f)
            {
                _animator.SetBool("isEarthQuake", true);
                Debug.Log("isEarthQuake");
                _bossState = BossState.Attacking;
                _earthQuakeCooltime = 0;
                _monsterSpeedMultiplier = 0f;
                StartCoroutine(BackToWantedState("isEarthQuake", false, 3.5f));
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
                FlipSprite();
                transform.position = Vector2.MoveTowards(
                transform.position,
                _player.transform.position,
                MonsterSpeed * _monsterSpeedMultiplier * Time.deltaTime);
            }
        }

        private IEnumerator BackToWantedState(string n, bool b, float s)
        {
            yield return new WaitForSeconds(s);
            _animator.SetBool(n, b);
        }

        private void AttackPlayer()
        {
            _animator.SetBool("isAttacking", true);
            var finalDamage = _randomDamage;
            _player.TakeDamage(finalDamage);
            StartCoroutine(IsAttackingToFalse(_animator.GetCurrentAnimatorStateInfo(0).length));
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
            _monsterHp = _monsterHp - damage + MonsterDefense;
            ShowDamage(damage);
            Flash();

            if (_monsterHp > 0) return;
            _animator.SetBool("isDead", true);
            _monsterSpeedMultiplier = 0;
            StartCoroutine(BeforeDestroy(_animator.GetCurrentAnimatorStateInfo(0).length));
        }

        private IEnumerator InstantiateHandUpFire()
        {
            var count = 1;
            if (_monsterHp < 500) count = 2;
            for (var i = 0; i < count; i++ ) 
            {
                yield return new WaitForSeconds(1.5f);
                Instantiate(fire, handUpPoint.transform.position, transform.rotation);
            }
        }
    }
}