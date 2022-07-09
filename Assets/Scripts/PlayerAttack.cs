using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AdSkills;
using fbg;
using Status;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;

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
                    case "Slash":
                        StartCoroutine(Slash(prefab, _v, transform.rotation));
                        break;
                    case "Gyeok":
                        StartCoroutine(Gyeok(prefab, _v, transform.rotation));
                        break;
                }
            }
        }
    }

    private IEnumerator BasicStar(GameObject prefab, Vector3 _, Quaternion rotation)
    {
        // BasicStar Level + LuckySeven Level
        var level = _player.BasicStar.CalculateFinalValue();
        if (level < 1) yield break;

        for (var i = 0; i < _player.LuckySeven.CalculateFinalValue() + 1; i++)
        {
                yield return new WaitForSeconds(0.02f);
            try
            {
                var target = GameManager.Instance.GetClosestTarget(8.75f);
                Instantiate(prefab, transform.position, rotation).GetComponent<BasicStar>().SetClosestTarget(target);
            }
            catch
            {
                break;
            }
        }
    }

    private IEnumerator DiagonalStar(GameObject prefab, Vector3 _, Quaternion rotation)
    {
        var level = _player.DiagonalStar.CalculateFinalValue();
        if (level < 1) yield break;
        
        for (var i = 0; i < _player.DiagonalStar.CalculateFinalValue() * 3 + 4; i++)
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
    
    private IEnumerator Slash(GameObject prefab, Vector3 _, Quaternion rotation)
    {
        var level = _player.Slash.CalculateFinalValue();
        if (level < 1) yield break;
        
        yield return new WaitForSeconds(_attackSpeed * 1 / 7);

        var count = level > 4 ? 3 : 2;
        var slashSpeed = (7 - 1) / count;
        for (var i = 0; i < count; i++)
        {
            Instantiate(prefab, _player.transform.position, rotation);
            yield return new WaitForSeconds(_attackSpeed * slashSpeed / 7);
        }
    }
    
    private IEnumerator Gyeok(GameObject prefab, Vector3 _, Quaternion rotation)
    {
        var level = _player.Gyeok.CalculateFinalValue();
        if (level < 1) yield break;
        
        yield return new WaitForSeconds(_attackSpeed * 1.5f);

        var boss = GameManager.Instance.boss;
        if (boss != null)
        {
            try{
                var bossPosition = boss.transform.position;
                if (Vector3.Distance(_player.transform.position, bossPosition) < 10f)
                {
                    Instantiate(prefab, bossPosition, rotation);
                }
                else
                {
                    GyeokNotToBoss(prefab, rotation);
                }
            }
            catch
            {
                GyeokNotToBoss(prefab, rotation);
            }
            yield break;
        }
        GyeokNotToBoss(prefab, rotation);
        
        void GyeokNotToBoss(GameObject o, Quaternion quaternion)
        {
            try
            {
                Instantiate(o, GameManager.Instance.GetClosestTargets(6.75f, 1)[0], quaternion);
            }
            catch
            {
                // ignored
            }
        }
    }
}
