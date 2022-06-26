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
    [SerializeField] private List<GameObject> adSkillPrefabs = new();

    private Player _player;
    private float _attackSpeed;

    private readonly Vector3 _v = new(0, 0, 0);
    
    private IEnumerator Start()
    {
        _player = GameManager.Instance.GetPlayer();
        _attackSpeed = _player.AttackSpeed.CalculateFinalValue();

        while (!GameManager.Instance.GetIsGameOver())
        {
            yield return new WaitForSeconds(_attackSpeed);
            foreach (var prefab in adSkillPrefabs)
            {
                if (!GameManager.Instance.isTargetOn()) break;
                switch (prefab.name)
                {
                    case "BasicStar":
                        StartCoroutine(BasicStar(prefab, _v, transform.rotation));
                        break;
                    case "DiagonalStar":
                        StartCoroutine(DiagonalStar(prefab, _v, transform.rotation));
                        break;
                    case "ThrowingStar":
                        StartCoroutine(ThrowingStar(prefab, _v, transform.rotation));
                        break;
                }
            }
        }
    }

    private IEnumerator BasicStar(GameObject prefab, Vector3 _, Quaternion rotation)
    {   // BasicStar Level + LuckySeven Level
        var level = _player.BasicStar.CalculateFinalValue();
        if (level < 1 || Vector3.Distance(transform.position, _player.transform.position) > 8.5) yield break;

        for (var i = 0; i < _player.LuckySeven.CalculateFinalValue() + 1; i++) {
            yield return new WaitForSeconds(0.02f);
            Instantiate(prefab, transform.position, rotation);
        }
    }

    private IEnumerator DiagonalStar(GameObject prefab, Vector3 _, Quaternion rotation)
    {
        var level = _player.DiagonalStar.CalculateFinalValue();
        if (level < 1) yield break;
        
        for (var i = 0; i < _player.DiagonalStar.CalculateFinalValue() * 2 + 1; i++)
        {
            Instantiate(prefab, transform.position, rotation);
            yield return new WaitForSeconds(0);
        }
    }

    private IEnumerator ThrowingStar(GameObject prefab, Vector3 _, Quaternion rotation)
    {
        var level = _player.ThrowingStar.CalculateFinalValue();
        if (level < 1) yield break;

        for (var i = 0; i < level; i++)
        {
            yield return new WaitForSeconds(_attackSpeed / 3 * 2 / level);
            Instantiate(prefab, _player.transform.position, rotation);
            yield return new WaitForSeconds(_attackSpeed / 3 * 1 / level);
        }
    }
}
