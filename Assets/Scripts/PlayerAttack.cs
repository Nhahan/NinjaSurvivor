using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Status;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Player player;
    
    private float _createDelay;
    private readonly List<GameObject> _weaponPrefabs = new();

    private void Start()
    {
        foreach (var prefab in player
                     .GetActivatedSkills()
                     .Select(skill => Resources.Load<GameObject>("Prefabs/AdWeapon/" + skill) as GameObject))
        {
            _weaponPrefabs.Add(prefab);
        }

        _createDelay = player.AttackSpeed.CalculateFinalValue();
        StartCoroutine(this.CreateWeapon());
    }

    private IEnumerator CreateWeapon()
    {
        while (player.Hp.CalculateFinalValue() > 0)
        {
            yield return new WaitForSeconds(_createDelay);
            foreach (var prefab in _weaponPrefabs)
            {
                var fixedTransform = transform;
                Instantiate(prefab, fixedTransform.position, fixedTransform.rotation);
                
                if (prefab.name != "BasicStar" || !(player.LuckySeven.CalculateFinalValue() >= 1)) continue;
                for (var i = 0; i < player.LuckySeven.CalculateFinalValue(); i++)
                {
                    StartCoroutine(AdditionalWeapon(prefab, fixedTransform.position, fixedTransform.rotation));
                }
            }
        }
    }

    private static IEnumerator AdditionalWeapon(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        yield return new WaitForSeconds(0.25f);
        Instantiate(prefab, position, rotation);
    }
}
