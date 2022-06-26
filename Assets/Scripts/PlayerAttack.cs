using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Status;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerAttack : MonoBehaviour
{
    // This script is only for the AdSkills.
    [SerializeField] private List<GameObject> adSkillPrefabs = new();

    private Player _player;
    private float _createDelay;

    private IEnumerator Start()
    {
        _player = GameManager.Instance.GetPlayer();
        _createDelay = _player.AttackSpeed.CalculateFinalValue();

        while (_player.Hp.CalculateFinalValue() > 0)
        {
            yield return new WaitForSeconds(_createDelay);
            foreach (var prefab in adSkillPrefabs)
            {
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

    private IEnumerator BasicStar(GameObject prefab, Vector3 _, Quaternion rotation)
    {   // BasicStar Level + LuckySeven Level
        var level = _player.BasicStar.CalculateFinalValue();
        if (level < 1 || Vector3.Distance(transform.position, _player.transform.position) > 8.5) yield break;

        for (var i = 0; i < _player.LuckySeven.CalculateFinalValue() + 1; i++) {
            yield return new WaitForSeconds(0.04f);
            Instantiate(prefab, transform.position, rotation);
        }
    }

    private IEnumerator DiagonalStar(GameObject prefab, Vector3 _, Quaternion rotation)
    {
        var level = _player.DiagonalStar.CalculateFinalValue();
        if (level < 1) yield break;
        
        for (var i = 0; i < _player.DiagonalStar.CalculateFinalValue() * 2 + 2; i++)
        {
            Instantiate(prefab, transform.position, rotation);
            yield return new WaitForSeconds(0);
        }
    }

    private IEnumerator ThrowingStar(GameObject prefab, Vector3 _, Quaternion rotation)
    {
        var level = _player.ThrowingStar.CalculateFinalValue();
        if (level < 1) yield break;
        
        var starCounts = level + 1;
        for (var i = 0; i < starCounts; i++)
        {
            yield return new WaitForSeconds(_createDelay / 3 * 2 / starCounts);
            Instantiate(prefab, _player.transform.position, rotation);
            yield return new WaitForSeconds(_createDelay / 3 * 1 / starCounts);
        }
    }
}
