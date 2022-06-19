using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] Player player;
    float createDelay;
    readonly List<GameObject> weaponPrefabs = new();

    void Start()
    {
        foreach (string skill in player.GetActivatedSkills())
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/AdWeapon/" + skill) as GameObject;
            weaponPrefabs.Add(prefab);
        }

        createDelay = player.AttackSpeed.CalculateFinalValue();
        StartCoroutine(this.CreateWeapon());
    }

    void Update()
    {
    }

    IEnumerator CreateWeapon()
    {
        while (player.Hp.CalculateFinalValue() > 0)
        {
            yield return new WaitForSeconds(createDelay);
            foreach (GameObject prefab in weaponPrefabs)
            {
                Instantiate(prefab, transform.position, transform.rotation);
            }
        }
    }
}
