using Status;
using UnityEngine;

namespace Pickups
{
    public class ExpSoul1 : MonoBehaviour
    {
        private Player _player;
        private bool _isTriggered;
        private float _liveTime;
        private float _speed = 0f;

        private const float TimeToDisappear = 9f;

        private void Start()
        {
            _player = GameManager.Instance.GetPlayer();
        }

        private void FixedUpdate()
        {
            if (_speed == 0f) { _liveTime += Time.deltaTime; }
            if (_isTriggered)
            {
                _speed += Time.deltaTime * 0.3f;
                transform.position = Vector2.MoveTowards(
                    transform.position, 
                    _player.transform.position, 
                    _speed * _speed);
            }

            if (_isTriggered && transform.position == _player.transform.position)
            {
                _player.EarnExp(1);
                Destroy(gameObject);
            }
        }

        private void LateUpdate()
        {
            if (_liveTime > TimeToDisappear)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.tag.Equals("PickupRadius"))
            {
                _isTriggered = true;
            }
        }
    }
}