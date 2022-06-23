using System;
using System.Collections;
using System.Collections.Generic;
using Status;
using UnityEngine;

public class PlayerApAttack : MonoBehaviour
{
    [SerializeField] private List<GameObject> apSkillPrefabs = new();
    [SerializeField] private GameObject flamer;

    private Player _player;
    private float _createDelay;


    private IEnumerator Start()
    {
        _player = GameManager.Instance.GetPlayer();
        _createDelay = _player.Cooltime.CalculateFinalValue();

        while (_player.Hp.CalculateFinalValue() > 0)
        {
            yield return new WaitForSeconds(_createDelay);
            foreach (var prefab in apSkillPrefabs)
            {
                Debug.Log(prefab.name);
                var fixedTransform = transform;
                switch (prefab.name)
                {
                    case "Flamer": StartCoroutine(Flamer(prefab, flamer.transform.position, fixedTransform.rotation)); break;
                }
            }
        }
    }

    private IEnumerator Flamer(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (_player.Flamer.CalculateFinalValue() < 1) yield break;
        Instantiate(prefab, flamer.transform.position, rotation);
        yield return new WaitForSeconds(_createDelay / 2f);
        Instantiate(prefab, flamer.transform.position, rotation);
    }
}
