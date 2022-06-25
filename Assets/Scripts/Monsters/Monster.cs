using System;
using System.Collections;
using System.Collections.Generic;
using Status;
using UnityEngine;

namespace Monsters
{
    public class Monster : MonoBehaviour
    {
        protected Player player;
        
        protected float MonsterSpeedMultiplier = 1;
        protected float KnockbackDuration = 0.1f;
        protected float KnockbackTimer; // KnockbackTimer = KnockbackDuration;
        protected float KnockbackScale = 5f;
        protected Vector3 KnockbackDirection;
        protected const float Duration = 0.1f;

        protected SpriteRenderer SpriteRenderer;
        protected Material OriginalMaterial;
        protected Material FlashMaterial;
        private Coroutine _flashRoutine;
        
        private void Awake()
        {
            player = GameManager.Instance.GetPlayer();
            SpriteRenderer = GetComponent<SpriteRenderer>();
            OriginalMaterial = player.GetComponent<SpriteRenderer>().material;
            FlashMaterial = GameManager.Instance.GetComponent<SpriteRenderer>().material;
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
        
        #region FlashWhenHit
        
        public void PlayKnockback()
        {
            transform.position += KnockbackDirection * KnockbackScale * Time.deltaTime;
            KnockbackTimer -= Time.deltaTime;
        }

        public void Flash()
        {
            if (_flashRoutine != null)
            {
                StopCoroutine(_flashRoutine);
            }

            _flashRoutine = StartCoroutine(FlashRoutine());
        }

        private IEnumerator FlashRoutine()
        {
            SpriteRenderer.material = FlashMaterial;
            yield return new WaitForSeconds(Duration);

            SpriteRenderer.material = OriginalMaterial;
            _flashRoutine = null;
        }
        
        #endregion
    }
}
