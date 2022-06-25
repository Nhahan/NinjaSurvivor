using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlash : MonoBehaviour
{
        #region Datamembers

        #region Editor Settings

        private const float Duration = 0.1f;

        #endregion
        #region Private Fields

        private SpriteRenderer _spriteRenderer;
        private Material _originalMaterial;
        private Material _flashMaterial;
        private Coroutine _flashRoutine;

        #endregion

        #endregion


        #region Methods

        #region Unity Callbacks

        void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _originalMaterial = _spriteRenderer.material;
            _flashMaterial = GameManager.Instance.GetComponent<SpriteRenderer>().material;
        }

        #endregion

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
            _spriteRenderer.material = _flashMaterial;
            yield return new WaitForSeconds(Duration);

            _spriteRenderer.material = _originalMaterial;
            _flashRoutine = null;
        }

        #endregion
}
