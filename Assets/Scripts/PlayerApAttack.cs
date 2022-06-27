using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ApSkills;
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
            yield return new WaitForSeconds(1f);
            foreach (var prefab in apSkillPrefabs)
            {
                switch (prefab.name)
                {
                    case "Flamer":
                        StartCoroutine(Flamer(prefab, _v, transform.rotation));
                        break;
                    case "LightningStrike":
                        StartCoroutine(LightningStrike(prefab, _v, transform.rotation));
                        break;
                    case "ExplosiveShuriken":
                        StartCoroutine(ExplosiveShuriken(prefab, _v, transform.rotation));
                        break;
                }
            }
        }
    }

    private IEnumerator Flamer(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        var level = _player.Flamer.CalculateFinalValue();
        if (level < 1) yield break;
        
        yield return new WaitForSeconds(1);
        
        var fireDirection = Mathf.Sign((flamer.transform.position - transform.position).normalized.x);
        var fixedPosition = new Vector2(transform.position.x + fireDirection * 2.35f, transform.position.y + 0.175f);

        Instantiate(prefab, fixedPosition, rotation);
        if ((level < 5)) yield break;
        
        fixedPosition = new Vector2(transform.position.x + fireDirection * -2.35f, transform.position.y + 0.175f);
        var additional = Instantiate(prefab, fixedPosition, rotation);
        additional.GetComponent<Flamer>().SetOpposite(true);
    }
    
    private IEnumerator LightningStrike(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        var level = (int)_player.LightningStrike.CalculateFinalValue();
        if (level < 1) yield break;
        
        var count = level * 3 + 1;
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
