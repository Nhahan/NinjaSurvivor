using System.Collections;
using System.Collections.Generic;
using Status;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // This script is only for the AdSkills.
    [SerializeField] private List<GameObject> weaponPrefabs = new();
    
    private Player _player;
    
    private float _createDelay;
    private List<string> _activatedSkills = new();

    private IEnumerator Start()
    {
        _player = GameManager.Instance.GetPlayer();
        _activatedSkills = GameManager.Instance.GetActivatedSkillsString();
        _createDelay = _player.AttackSpeed.CalculateFinalValue();
        while (_player.Hp.CalculateFinalValue() > 0)
        {
            yield return new WaitForSeconds(_createDelay);
            foreach (var prefab in weaponPrefabs)
            {
                if (!_activatedSkills.Contains(prefab.name))
                {
                    yield break;
                }

                var fixedTransform = transform;
                switch (prefab.name)
                {
                    case "BasicStar": StartCoroutine(BasicStar(prefab, fixedTransform.position, fixedTransform.rotation)); break;
                    case "DiagonalStar": DiagonalStar(prefab, fixedTransform.position, fixedTransform.rotation); break;
                    case "ThrowingStar": StartCoroutine(ThrowingStar(prefab, fixedTransform.position, fixedTransform.rotation)); break;
                }
            }
        }
    }

    private IEnumerator BasicStar(GameObject prefab, Vector3 position, Quaternion rotation)
    {      // BasicStar Level + LuckySeven Level
            for (var i = 0; i < _player.LuckySeven.CalculateFinalValue() + 1; i++) {
                yield return new WaitForSeconds(0.04f);
                Instantiate(prefab, position, rotation);
            }
    }
    
    private void DiagonalStar(GameObject prefab, Vector3 position, Quaternion rotation)
    { 
            for (var i = 0; i < _player.DiagonalStar.CalculateFinalValue() * 2; i++)
            {
                Instantiate(prefab, position, rotation);
            }
    }

    private IEnumerator ThrowingStar(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        var starCounts = _player.ThrowingStar.CalculateFinalValue() + 1;
        for (var i = 0; i < starCounts; i++)
        {
            yield return new WaitForSeconds(_createDelay / 3 * 2 / starCounts);
            Instantiate(prefab, position, rotation);
            yield return new WaitForSeconds(_createDelay / 3 * 1 / starCounts);
        }
    }
}
