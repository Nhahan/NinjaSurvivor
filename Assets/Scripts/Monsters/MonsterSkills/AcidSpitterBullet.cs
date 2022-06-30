using Status;
using UnityEngine;

public class AcidSpitterBullet : MonoBehaviour
{
    private Player _player;

    private Vector3 _target;
    private float _speed;

    private void Start()
    {
        _player = GameManager.Instance.GetPlayer();
        _target = (_player.transform.position - transform.position).normalized;

        AdjustDirection();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        _speed += Time.deltaTime;
        if (_speed > 2f) {Destroy(gameObject);}
        
        transform.position += _target * (_speed * 1.125f);
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (!coll.CompareTag("Player")) return;
        var damage = 50f - _player.Defense.CalculateFinalValue() + Random.Range(1, 10);
        _player.TakeDamage(damage);
        Destroy(gameObject);
    }
    
    private void AdjustDirection()
    {
        var vDiff = (new Vector3(0, 0, 0) - transform.position);
        var atan2 = Mathf.Atan2(vDiff.y, vDiff.x);
        transform.rotation = Quaternion.Euler(0f, 0f, atan2 * Mathf.Rad2Deg - 260);
    }
}
