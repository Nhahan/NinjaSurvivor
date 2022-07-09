using Status;
using UnityEngine;

public class GolemFire : MonoBehaviour
{
    [SerializeField] private CircleCollider2D circleCollider2D;

    private Player _player;
    
    private float _liveTime;
    private Vector3 _target = Vector3.zero;
    private float _speed;
    private float _resetTriggerCooltime;

    private void Start()
    {
        _player = GameManager.Instance.GetPlayer();
        
        
        circleCollider2D.enabled = false;
    }

    private void FixedUpdate()
    {
        _liveTime += Time.deltaTime;
        _resetTriggerCooltime += Time.deltaTime;
        if (_liveTime > 1.75f && transform.position != _target)
        {
            if (_target == Vector3.zero)
            {
                _target = _player.transform.position;
            }
            _speed += Time.deltaTime * 0.25f;
            transform.position = Vector2.MoveTowards(
                transform.position, _target, _speed * _speed);
        }
        
        if (_liveTime > 21f)
        {
            circleCollider2D.enabled = false;
            Destroy(gameObject, 1.2f);
        }
        ResetTrigger();
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (!coll.CompareTag("Player")) return;
    
        var damage = _player.MaxHp.CalculateFinalValue() * 0.08f - _player.Defense.CalculateFinalValue();
        
        _player.TakeDamage(damage);
    }

    private void ResetTrigger()
    {
        if (_liveTime < 1.5f || _liveTime > 20.9f || _resetTriggerCooltime < 0.4f) return;

        circleCollider2D.enabled = !circleCollider2D.enabled;
        _resetTriggerCooltime = 0;
    }
}
