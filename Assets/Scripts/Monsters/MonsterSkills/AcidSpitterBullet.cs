using Status;
using UnityEngine;

public class AcidSpitterBullet : MonoBehaviour
{
    private Player _player;
    private Rigidbody2D _rb;

    private Vector2 _target;
    private float _speed;

    private void Start()
    {
        _player = GameManager.Instance.GetPlayer();
        _rb = GetComponent<Rigidbody2D>();
        
        _target = (_player.transform.position - transform.position).normalized;

        AdjustDirection();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        _speed += Time.fixedDeltaTime * 0.35f;
        if (_speed > 1.1f) {Destroy(gameObject);}
        _rb.MovePosition(_rb.position + _target * _speed);
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (!coll.CompareTag("Player")) return;
        var damage = 11f - _player.Defense.CalculateFinalValue() + Random.Range(1, 20);
        _player.TakeDamage(damage > 0 ? damage : 1);
        Destroy(gameObject);
    }
    
    private void AdjustDirection()
    {
        var vDiff = (new Vector3(0, 0, 0) - transform.position);
        var atan2 = Mathf.Atan2(vDiff.y, vDiff.x);
        transform.rotation = Quaternion.Euler(0f, 0f, atan2 * Mathf.Rad2Deg - 260);
    }
}
