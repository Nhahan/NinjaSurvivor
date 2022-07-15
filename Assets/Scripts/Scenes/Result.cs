using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Result : MonoBehaviour
{
    [SerializeField] private List<GameObject> maps;

    private readonly List<int> _pickedMaps;

    private void Start()
    {
        StartCoroutine(GenerateMap());
    }

    private IEnumerator GenerateMap()
    {
        while (true)
        {
            yield return new WaitForSeconds(17f); // 17초
            
            var count = maps.Count; // 맵의 갯수
            int idx;
            while (true) {
                idx = Random.Range(0, count);
                if (!_pickedMaps.Contains(idx)) break; // 같은 숫자 아닐 때까지 뽑기
            }

            _pickedMaps.Add(idx); // 뽑은 숫자 뽑은 리스트에 넣기
            Instantiate(maps[idx], transform.position, Quaternion.identity); // 해당 인덱스 맵 생성

            if (_pickedMaps.Count > count / 2) _pickedMaps.Clear(); // 일정 숫자 이상 뽑혔으면 뽑은 리스트 초기화 1/3이나 1/2 정도가 좋을듯
        }
    }
}
