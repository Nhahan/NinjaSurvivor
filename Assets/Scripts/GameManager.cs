using System.Collections;
using Status;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject monsterPrefab;

    [SerializeField] private float createDelay = 2f;
    [SerializeField] private Player player;

    [FormerlySerializedAs("_isGameOver")] public bool isGameOver;
    public static GameManager Instance;

    private void Awake()
    {
        if(Instance == null) // If there is no instance already
        {
            DontDestroyOnLoad(gameObject); // Keep the GameObject, this component is attached to, across different scenes
            Instance = this;
        } else if(Instance != this) // If there is already an instance and it's not `this` instance
        {
            Destroy(gameObject); // Destroy the GameObject, this component is attached to
        }
    }
    private void Start()
    {
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

    private void GameOver()
    {
        isGameOver = true;
    }

    public void LevelUpEvent()
    {
        Debug.Log("Need to define LevelUpEvent()");
    }
}
