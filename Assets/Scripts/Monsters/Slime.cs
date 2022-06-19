using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour, IMonster
{
    [SerializeField] float monsterSpeed = 1.5f;

    float monsterDamage = 20f;
    float monsterHp = 10f;

    Player player;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    void FixedUpdate()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, monsterSpeed * Time.deltaTime);
        FlipSprite();
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            AttackPlayer();
            Destroy(gameObject);
        }
    }

    void AttackPlayer()
    {
        player.TakeDamage(monsterDamage);
    }

    void FlipSprite()
    {
        transform.localScale = transform.position.x < player.transform.position.x ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);
    }

    public float GetMonsterHp()
    {
        return monsterHp;
    }

    public void SetMonsterHp(float monsterHp)
    {
        this.monsterHp = monsterHp;
    }

    public void TakeDamage(float damage)
    {
        float currentHp = GetMonsterHp();
        SetMonsterHp(currentHp - damage);
        if (GetMonsterHp() <= 0)
        {
            Destroy(gameObject);
        }
    }
}
