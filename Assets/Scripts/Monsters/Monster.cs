using System;
using System.Collections;
using Pickups;
using Status;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Monsters
{
    public abstract class Monster : MonoBehaviour
    {
        protected enum State
        {
            Moving,
            Attacking
        }

        // ReSharper disable once InconsistentNaming
        protected State _state = State.Moving;
        
        protected float MonsterSpeedMultiplier = 1;
        protected float KnockbackDuration = 0.1f;
        protected float KnockbackTimer; // KnockbackTimer = KnockbackDuration;
        protected float KnockbackScale = 5f;
        protected Vector3 KnockbackDirection;
        protected const float Duration = 0.12f;

        protected SpriteRenderer SpriteRenderer;
        protected GameObject _indicator;
        private Coroutine _flashRoutine;

        protected IEnumerator BeforeDestroy(float second)
        {
            yield return new WaitForSeconds(second);
            Instantiate(GameManager.Instance.expSoul1, transform.position, transform.rotation);
            GameManager.Instance.RemoveTarget(gameObject);
            Destroy(gameObject);
        }

        public void StopMonster()
        {
            MonsterSpeedMultiplier = 0;
            GetComponent<Animator>().enabled = false;
        }
        
        public void ResumeMonster()
        {
            MonsterSpeedMultiplier = 1;
            GetComponent<Animator>().enabled = true;
        }
    
        public void StartKnockback(Vector3 knockbackVector)
        {
            KnockbackDirection = knockbackVector;
            KnockbackTimer = KnockbackDuration;
        }
        
        public void PlayKnockback()
        {
            transform.position += KnockbackDirection * KnockbackScale * Time.deltaTime;
            KnockbackTimer -= Time.deltaTime;
        }

        public virtual void Flash()
        {
            if (_flashRoutine != null)
            {
                StopCoroutine(_flashRoutine);
            }

            _flashRoutine = StartCoroutine(FlashRoutine());
        }

        // ReSharper disable Unity.PerformanceAnalysis
        protected virtual IEnumerator FlashRoutine()
        {
            SpriteRenderer = GetComponent<SpriteRenderer>();
            SpriteRenderer.material = GameManager.Instance.GetFlashMaterial();
            yield return new WaitForSeconds(Duration);

            SpriteRenderer.material = GameManager.Instance.GetDefaultMaterial();
            _flashRoutine = null;
        }
        
        protected void ShowDamage(float damage)
        {
            var indicator = Instantiate(_indicator, transform.position, transform.rotation);
            indicator.GetComponent<TextMeshPro>().text = damage < 1 ? "1" : Mathf.RoundToInt(damage).ToString();
        }
    }
}
