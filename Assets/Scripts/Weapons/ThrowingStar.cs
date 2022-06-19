using UnityEngine;

public class ThrowingStar : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 11f;
    [SerializeField] float coefficient = 1;

    Player player;

    float liveTime = 0;
    float bulletDirection = 1;

    void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        if (player.BasicStar.CalculateFinalValue() < 1)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        bulletDirection = GetRandomSign();
        Vector2 bulletVelocity = PlayerController.LatestDirection;
        transform.position = Vector2.MoveTowards(transform.position, bulletVelocity, bulletSpeed * Time.deltaTime);
    }

    void Update()
    {
        liveTime += Time.deltaTime;
        if (liveTime > 8)
        {
            Destroy(gameObject);
        }
        transform.Translate(bulletDirection * bulletSpeed * Time.deltaTime * Vector3.up);
        transform.Rotate(0, 0, -300 * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            coll.gameObject.GetComponent<IMonster>().TakeDamage(player.AttackDamage.CalculateFinalValue() * coefficient);
        }
    }

    int GetRandomSign()
    {
        int[] plusMinus = { 1, -1 };
        int idx = Random.Range(0, 2);
        Debug.Log(plusMinus[idx]);
        return plusMinus[idx];
    }
}
