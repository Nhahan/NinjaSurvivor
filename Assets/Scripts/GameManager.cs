using System.Collections;
using System.Collections.Generic;
using Monsters;
using Status;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject monsterPrefab;

    [SerializeField] private float createDelay = 2f;
    [SerializeField] private Player player;
    [SerializeField] private LevelUpRewards levelUpRewards;
    [SerializeField] public PostProcessVolume post;

    private bool _isGameOver;
    public static GameManager Instance;
    private List<PlayerStat> _activatedSkills = new();

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
        _isGameOver = false;
        Time.timeScale = 1;
        _activatedSkills = player.GetActivatedSkills();
        if (spawnPoints.Length > 0)
        {
            StartCoroutine(this.CreateMonster());
        }
    }

    public Player GetPlayer()
    {
        return player;
    }

    private IEnumerator CreateMonster()
    {
        while (!_isGameOver)
        {
            yield return new WaitForSeconds(createDelay);

            var idx = Random.Range(1, spawnPoints.Length);
            Instantiate(monsterPrefab, spawnPoints[idx].position, spawnPoints[idx].rotation);
        }
    }

    public void SetIsGameOver(bool value)
    {
        _isGameOver = value;
    }

    public bool GetIsGameOver()
    {
        return _isGameOver;
    }

    public void LevelUpEvent()
    {
        levelUpRewards.ShowRewards();
    }

    public void AllStop()
    {
        Time.timeScale = 0;
        Debug.Log("AllStop");
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            enemy.GetComponent<IMonster>().StopMonster();
        }
    }

    public void Resume()
    {
        Time.timeScale = 1;
        Debug.Log("Resume");
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            enemy.GetComponent<IMonster>().ResumeMonster();
        }
        _activatedSkills = player.GetActivatedSkills();
    }
}
