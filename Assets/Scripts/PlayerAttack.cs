using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Status;
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
                Instantiate(prefab, transform.position, transform.rotation);
            }
        }
    }
}
