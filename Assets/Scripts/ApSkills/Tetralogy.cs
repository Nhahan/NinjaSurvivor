using System;
using Status;
using UnityEngine;

namespace ApSkills
{
    public class Tetralogy : MonoBehaviour
    {
        private Player _player;
        private Rigidbody2D _rb;
        private float _damage;
        private bool _isOpposite;
        private float _liveTime;
        private float _skillLevel;

        private void Start()
        {
            _player = GameManager.Instance.GetPlayer();
            _rb = GetComponent<Rigidbody2D>();

            _skillLevel = _player.Tetralogy.CalculateFinalValue();
            
            Destroy(gameObject, 9.8f + _skillLevel * 0.25f);
        }
        

        private void FixedUpdate()
        {
            _liveTime += Time.fixedDeltaTime;
            _rb.MovePosition(_player.transform.position);
            transform.Rotate(0, 0, -6f - _skillLevel / 3.5f);

            if (transform.localScale.x < 1f)
            {
                transform.localScale += new Vector3(0.007f, 0.007f, 0.007f);
            }

            if (_liveTime > 8.4f + _skillLevel * 0.25f)
            {
                transform.localScale += new Vector3(0.075f, 0.075f, 0.075f);
            }
        }
    }
}