using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour, IMonster
{
    [SerializeField] float monsterSpeed = 1.5f;

    //float monsterDamage = 20f;
    float monsterHp = 13f;

    Transform player;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, monsterSpeed * Time.deltaTime);
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
        //float playerHp = playerStatus.GetCurrentHp();
        //playerStatus.SetCurrentHp(playerHp - monsterDamage);
        //Debug.Log("슬라임의 공격!");
        //Debug.Log(playerStatus.GetCurrentHp());
    }

    void FlipSprite()
    {
        transform.localScale = transform.position.x < player.position.x ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);
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
