using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] GameObject monsterPrefab;

    [SerializeField] float createDelay = 1f;
    [SerializeField] bool isGameOver = false;

    void Start()
    {
        if (spawnPoints.Length > 0)
        {
            //몬스터 생성 코루틴 함수 호출
            StartCoroutine(this.CreateMonster());
        }
    }

    IEnumerator CreateMonster()
    {
        //게임 종료 시까지 무한 루프
        while (!isGameOver)
        {
            //몬스터의 생성 주기 시간만큼 대기
            yield return new WaitForSeconds(createDelay);

            //불규칙적인 위치 산출
            int idx = Random.Range(1, spawnPoints.Length);
            //몬스터의 동적 생성
            Instantiate(monsterPrefab, spawnPoints[idx].position, spawnPoints[idx].rotation);
        }
    }
}
