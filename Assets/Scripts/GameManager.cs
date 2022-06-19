using System.Collections;
using Monsters;
using Status;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject monsterPrefab;

    [SerializeField] private float createDelay = 2f;
    [SerializeField] private Player player;
    [SerializeField] private LevelUpRewards levelUpRewards;

    private bool isGameOver;
    public static GameManager Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        } else if(Instance != this)
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        levelUpRewards.HideRewards();
        isGameOver = false;
        Time.timeScale = 1;
        if (spawnPoints.Length > 0)
        {
            StartCoroutine(this.CreateMonster());
        }
    }

    private void Update()
    {
    }

    private IEnumerator CreateMonster()
    {
        while (!isGameOver)
        {
            yield return new WaitForSeconds(createDelay);

            var idx = Random.Range(1, spawnPoints.Length);
            Instantiate(monsterPrefab, spawnPoints[idx].position, spawnPoints[idx].rotation);
        }
    }

    public void SetIsGameOver(bool value)
    {
        isGameOver = value;
    }

    public bool GetIsGameOver()
    {
        return isGameOver;
    }

    public void LevelUpEvent()
    {
        levelUpRewards.ShowRewards();
    }

    public void AllStop()
    {
        Time.timeScale = 0;
        Debug.Log("AllStop");
        // var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        // foreach (var enemy in enemies)
        // {
        //     enemy.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        // }
    }

    public void Resume()
    {
        Time.timeScale = 1;
        Debug.Log("Resume");
        // var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        // foreach (var enemy in enemies)
        // {
        //     enemy.GetComponent<Rigidbody2D>().velocity = new Vector2(1, 1);
        // }
    }
}
