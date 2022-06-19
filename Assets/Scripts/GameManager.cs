using System.Collections;
using Status;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject monsterPrefab;

    [SerializeField] private float createDelay = 2f;
    [SerializeField] private Player player;

    private bool _isGameOver;

    private void Start()
    {
        _isGameOver = false;
        Time.timeScale = 1;
        if (spawnPoints.Length > 0)
        {
            StartCoroutine(this.CreateMonster());
        }
    }

    private void Update()
    {
        if (_isGameOver == true)
        {
            Time.timeScale = 0;
        }

        Debug.Log(player.Hp.CalculateFinalValue());
        //CheckGameConditon();
    }

    private IEnumerator CreateMonster()
    {
        while (!_isGameOver)
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
