using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Status;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerAttack : MonoBehaviour
{
    // This script is only for the AdSkills.
    [SerializeField] private Player player;
    [SerializeField] private List<GameObject> weaponPrefabs = new();
    
    private float _createDelay;
    
    public float diagonalAngle = 0; 

    private IEnumerator Start()
    {  
        _createDelay = player.AttackSpeed.CalculateFinalValue();
        while (player.Hp.CalculateFinalValue() > 0)
        {
            yield return new WaitForSeconds(_createDelay);
            foreach (var prefab in weaponPrefabs)
            {
                Debug.Log(prefab);
                var fixedTransform = transform;
                switch (prefab.name)
                {
                    case "BasicStar": StartCoroutine(BasicStar(prefab, fixedTransform.position, fixedTransform.rotation)); break;
                    case "DiagonalStar": StartCoroutine(DiagonalStar(prefab, fixedTransform.position, fixedTransform.rotation)); break;
                    case "ThrowingStar": StartCoroutine(ThrowingStar(prefab, fixedTransform.position, fixedTransform.rotation)); break;
                }
            }
        }
    }

    private IEnumerator BasicStar(GameObject prefab, Vector3 position, Quaternion rotation)
    {      // BasicStar Level + LuckySeven Level
            for (var i = 0; i < player.LuckySeven.CalculateFinalValue() + 1; i++) {
                yield return new WaitForSeconds(0.04f);
                Instantiate(prefab, position, rotation);
            }
    }
    
    private IEnumerator DiagonalStar(GameObject prefab, Vector3 position, Quaternion rotation)
    { 
            for (var i = 0; i < player.DiagonalStar.CalculateFinalValue(); i++)
            {
                yield return new WaitForSeconds(0f);
                Instantiate(prefab, position, rotation);
                Debug.Log("Hello");
            }
    }

    private IEnumerator ThrowingStar(GameObject prefab, Vector3 position, Quaternion rotation)
    { 
        for (var i = 0; i < player.ThrowingStar.CalculateFinalValue() * 2; i++) {
            yield return new WaitForSeconds(_createDelay / 3 * 2);
            Instantiate(prefab, position, rotation);
            yield return new WaitForSeconds(_createDelay / 3 * 1);
        }
        
    }
}
