using Status;
using UnityEngine;

namespace ApSkills
{
    public class Tetralogy : MonoBehaviour
    {

        private Player _player;
        private Transform _flamer;
        private float _damage;
        private bool _isOpposite;
        private float _liveTime;

        private void FixedUpdate()
        {
            _liveTime += Time.deltaTime;

            if (_liveTime > 0.5f) return;
        }

        private void TransformUpdate()
        {
            transform.position = _player.transform.position;
        }
    }
}