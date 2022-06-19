using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] GameObject monsterPrefab;

    [SerializeField] float createDelay = 2f;
    [SerializeField] Player player;

    bool isGameOver;

    void Start()
    {
        isGameOver = false;
        Time.timeScale = 1;
        if (spawnPoints.Length > 0)
        {
            StartCoroutine(this.CreateMonster());
        }
    }

    void Update()
    {
        if (isGameOver == true)
        {
            Time.timeScale = 0;
        }

        Debug.Log(player.Hp.CalculateFinalValue());
        //CheckGameConditon();
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
        //float playerHp = playerStatus.GetCurrentHp();
        //if (playerHp <= 0)
        //{
        //    isGameOver = true;
        //}
    }
}
