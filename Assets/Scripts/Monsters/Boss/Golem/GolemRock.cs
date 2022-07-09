using Status;
using UnityEngine;

public class GolemRock : MonoBehaviour
{
    [SerializeField] private CircleCollider2D circleCollider2D;

    private Player _player;

    private void Start()
    {
        _player = GameManager.Instance.GetPlayer();
        
        transform.position = new Vector3(
            transform.position.x + Random.Range(-2.5f, 2.5f), transform.position.y + Random.Range(-2.5f, 2.5f), 0);
        
        circleCollider2D.enabled = false;
        Invoke(nameof(ColliderEnable), 1.5f);
        Destroy(gameObject, 2.75f);
    }
    
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (!coll.CompareTag("Player")) return;
    
        var damage = _player.MaxHp.CalculateFinalValue() * 0.1f - _player.Defense.CalculateFinalValue();
        
        _player.TakeDamage(damage);
    }

    private void ColliderEnable()
    {
        circleCollider2D.enabled = false;
    }
}
