using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Status;
using UnityEngine;

public class PlayerApAttack : MonoBehaviour
{
    [SerializeField] private List<GameObject> apSkillPrefabs = new();
    [SerializeField] private GameObject flamer;

    private Player _player;
    private float _createDelay;
    
    private readonly Vector3 _v = new(0, 0, 0);

    private IEnumerator Start()
    {
        _player = GameManager.Instance.GetPlayer();
        _createDelay = _player.Cooltime.CalculateFinalValue();

        while (_player.Hp.CalculateFinalValue() > 0)
        {
            yield return new WaitForSeconds(_createDelay);
            foreach (var prefab in apSkillPrefabs.TakeWhile(_ => GameManager.Instance.GetTarget()))
            {
                switch (prefab.name)
                {
                    case "Flamer": StartCoroutine(Flamer(prefab, _v, transform.rotation)); break;
                    case "LightningStrike": StartCoroutine(LightningStrike(prefab, _v, transform.rotation)); break;
                    case "ExplosiveShuriken": StartCoroutine(ExplosiveShuriken(prefab, _v, transform.rotation)); break;
                }
            }
        }
    }

    private IEnumerator Flamer(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        var level = _player.Flamer.CalculateFinalValue();
        if (level < 1) yield break;
        
        yield return new WaitForSeconds(1);
        
        var xPosition = (flamer.transform.position - transform.position).normalized.x * 1.55f;
        Instantiate(prefab, new Vector3(xPosition, transform.position.y, transform.position.y), rotation);
        if (level >= 5)
        {
            Instantiate(prefab,  new Vector3(xPosition * -1, transform.position.y, transform.position.y), rotation);
        }
    }
    
    private IEnumerator LightningStrike(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        var level = (int)_player.LightningStrike.CalculateFinalValue();
        if (level < 1) yield break;
        
        var count = level * 2;
        var targets = GameManager.Instance.GetTargets(count);
        
        for (var i = 0; i < count; i++)
        {
            try
            {
                Instantiate(prefab, targets[i], rotation);
                
            }
            catch
            {
                continue;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
    
    private IEnumerator ExplosiveShuriken(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (_player.ExplosiveShuriken.CalculateFinalValue() < 1) yield break;
        Instantiate(prefab, _player.transform.position, rotation);
    }
}
