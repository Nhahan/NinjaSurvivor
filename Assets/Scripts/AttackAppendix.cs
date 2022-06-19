using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAppendix : MonoBehaviour
{
    public void CreateLuckySeven((GameObject prefab, Vector3 position, Quaternion rotation) info, int luckySeven)
    {
        StartCoroutine(this.StartLuckySeven(info, luckySeven));
    }

    private IEnumerator StartLuckySeven((GameObject prefab, Vector3 position, Quaternion rotation) info, int luckySeven)
    {
        for (var i = 0; i < luckySeven; i++) {
            yield return new WaitForSeconds(0.3f);
            Instantiate(info.prefab, info.position, info.rotation);
        }
    }
}
