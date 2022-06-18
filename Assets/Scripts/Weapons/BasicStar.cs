using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BasicStar : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 11f;
    [SerializeField] float possibleAttackDistance = 11f;

    float liveTime = 0;
    Vector3 nearestEnemy;
    Vector3 bulletDirection;

    void Start()
    {
        try
        {
            nearestEnemy = FindNearestObject().transform.position;
            if (nearestEnemy == null || Vector3.Distance(nearestEnemy, transform.position) > possibleAttackDistance)
            {
                Destroy(gameObject);
            }

            bulletDirection = (
                nearestEnemy -
                transform.position -
                new Vector3((float)(Random.Range(-2, 3)), (float)(Random.Range(-2, 3)), 0))
                    .normalized;
        }
        catch
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        liveTime += Time.deltaTime;
        if (liveTime > 3.5)
        {
            Destroy(gameObject);
        }
        transform.position = transform.position + (bulletDirection * bulletSpeed) * Time.deltaTime;
        transform.Rotate(0, 0, -230 * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            //coll.gameObject.GetComponent<IMonster>().SetMonsterHp(playerStatus.GetCurrentAttackDamage());
        }
    }

    GameObject FindNearestObject()
    {
        return GameObject.FindGameObjectsWithTag("Enemy").ToList()
            .OrderBy(obj =>
            {
                return Vector3.Distance(transform.position, obj.transform.position);
            }).FirstOrDefault();
    }
}
