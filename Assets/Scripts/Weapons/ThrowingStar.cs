using UnityEngine;

public class ThrowingStar : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 11f;

    float liveTime = 0;
    float bulletDirection = 1;

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
            Destroy(coll.gameObject);
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
