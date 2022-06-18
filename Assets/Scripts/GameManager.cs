using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] GameObject monsterPrefab;

    [SerializeField] float createDelay = 2f;
    PlayerStatus playerStatus;

    bool isGameOver;

    void Start()
    {
        InitializeGameManger();
        Time.timeScale = 1;
        if (spawnPoints.Length > 0)
        {
            StartCoroutine(this.CreateMonster());
        }
    }

    private void Update()
    {
        CheckGameConditon();
        if (isGameOver)
        {
            Time.timeScale = 0;
        }
    }

    IEnumerator CreateMonster()
    {
        while (!isGameOver)
        {
            yield return new WaitForSeconds(createDelay);

            int idx = Random.Range(1, spawnPoints.Length);
            Instantiate(monsterPrefab, spawnPoints[idx].position, spawnPoints[idx].rotation);
        }
    }

    void CheckGameConditon()
    {
        float playerHp = playerStatus.GetCurrentHp();
        if (playerHp <= 0)
        {
            isGameOver = true;
        }
    }

    void InitializeGameManger()
    {
        playerStatus.InitializeStatus();
    }
}
