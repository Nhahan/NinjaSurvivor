using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlash : MonoBehaviour
{
        #region Datamembers

        #region Editor Settings

        [Tooltip("Material to switch to during the flash.")]
        [SerializeField] private Material flashMaterial;

        [Tooltip("Duration of the flash.")]
        [SerializeField] private float duration;

        #endregion
        #region Private Fields

        private SpriteRenderer _spriteRenderer;
        private Material _originalMaterial;
        private Coroutine _flashRoutine;

        #endregion

        #endregion


        #region Methods

        #region Unity Callbacks

        void Start()
        {
            _spriteRenderer = GameManager.Instance.GetPlayer().GetComponent<SpriteRenderer>();
            _originalMaterial = _spriteRenderer.material;
        }

        #endregion

        public void Flash()
        {
            if (_flashRoutine != null)
            {
                StopCoroutine(_flashRoutine);
            }

            _flashRoutine = StartCoroutine(FlashRoutine());
            Debug.Log(345);
        }

        private IEnumerator FlashRoutine()
        {
            _spriteRenderer.material = flashMaterial;
            yield return new WaitForSeconds(duration);

            _spriteRenderer.material = _originalMaterial;
            _flashRoutine = null;
        }

        #endregion
}
