using Status;
using UnityEngine;
using Random = UnityEngine.Random;

public class GolemLaser : MonoBehaviour
{
    [SerializeField] private BoxCollider2D boxCollider2D;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Player _player;
    private float _liveTime;

    private void Start()
    {
        _player = GameManager.Instance.GetPlayer();
        boxCollider2D.enabled = false;

        var normal = (_player.transform.position - transform.position).normalized;
        Quaternion goal =  Quaternion.FromToRotation(transform.up, normal) 
                           * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, goal, 1);

        Invoke(nameof(EnableCollider), 1.75f);
    }

    private void FixedUpdate()
    {
        _liveTime += Time.deltaTime;
        if (_liveTime < 1.75)
        {
            transform.localScale = new Vector3(transform.localScale.x * 1.05f, transform.localScale.y, transform.localScale.z);
            spriteRenderer.color = new Color(255, 255, 255, spriteRenderer.color.a * 1.05f);
        }
        else
        {
            transform.localScale = new Vector3(0.9f, 500, transform.localScale.z);
            spriteRenderer.color = new Color(255, 255, 255, 150);
        }

        if (_liveTime > 2.75)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (!coll.CompareTag("Player")) return;
    
        var damage = _player.MaxHp.CalculateFinalValue() * 0.125f + Random.Range(0, 10);
        _player.TakeDamage(damage);
    }

    private void EnableCollider()
    {
        boxCollider2D.enabled = true;
    }
}
