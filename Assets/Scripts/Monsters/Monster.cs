using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float monsterSpeed = 2f;

    void FixedUpdate()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, monsterSpeed * Time.deltaTime);
        FlipSprite();
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("weapon"))
        {
            Destroy(gameObject);
        }
    }

    void FlipSprite()
    {
        transform.localScale = transform.position.x < player.position.x ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);
    }
}
