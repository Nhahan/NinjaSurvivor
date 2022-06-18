using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] float createDelay = 1.5f;
    [SerializeField] GameObject weaponPrefab;

    void Start()
    {
        StartCoroutine(this.CreateWeapon());
    }

    void Update()
    {

    }

    IEnumerator CreateWeapon()
    {
        //while (playerStatus.GetCurrentHp() > 0)
        {
            yield return new WaitForSeconds(createDelay);
            Instantiate(weaponPrefab, transform.position, transform.rotation);
        }
    }
}
