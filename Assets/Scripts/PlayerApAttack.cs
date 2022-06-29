using System.Collections;
using System.Collections.Generic;
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

        while (!GameManager.Instance.GetIsGameOver())
        {
            yield return new WaitForSeconds(_createDelay);
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

        for (var i = 0; i < 2; i ++) 
        {
            yield return new WaitForSeconds(_createDelay / 2);
            Instantiate(prefab, fixedPosition, rotation);
            if ((level < 5)) yield break;
            
            fixedPosition = new Vector2(transform.position.x + fireDirection * -2.35f, transform.position.y + 0.175f);
            Instantiate(prefab, fixedPosition, rotation).GetComponent<Flamer>().SetOpposite(true);
        }
    }
    
    private IEnumerator LightningStrike(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        var level = (int)_player.LightningStrike.CalculateFinalValue();
        if (level < 1) yield break;
        
        var count = level * 2 + 3;

        for (var i = 0; i < count; i++)
        {
            try
            {
                var target = GameManager.Instance.GetClosestTarget(8.25f) + new Vector3(Random.Range(-2,2) ,Random.Range(-2, 2), 0);
                Debug.Log(target);
                Instantiate(prefab, target + new Vector3(0, 8, 0), rotation).GetComponent<LightningStrike>().SetTarget(target);
                
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
