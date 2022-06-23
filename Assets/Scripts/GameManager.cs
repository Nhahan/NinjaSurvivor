using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Monsters;
using Status;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject monsterPrefab;

    private const float CreateDelay = 1f;
    [SerializeField] private Player player;
    [SerializeField] private LevelUpRewards levelUpRewards;
    [SerializeField] public PostProcessVolume post;

    private bool _isGameOver;
    public static GameManager Instance;
    public Dictionary<string, float> ActivatedSkills = new();
    public List<Vector3> nearestTargets = new();

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

        StartCoroutine(CreateMonster());
        StartCoroutine(FindNearestTargets(1.5f));
    }

    public Player GetPlayer()
    {
        return player;
    }

    private IEnumerator CreateMonster()
    {
        while (!_isGameOver)
        {
            yield return new WaitForSeconds(CreateDelay);

            var idx = Random.Range(1, spawnPoints.Length);
            Instantiate(monsterPrefab, spawnPoints[idx].position, spawnPoints[idx].rotation);
        }
    }

    public void SetIsGameOver(bool value)
    {
        _isGameOver = value;
        AllStop();
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
        ActivatedSkills = player.GetActivatedSkills(true);
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator FindNearestTargets(float second)
    {
        while (!_isGameOver) {
            yield return new WaitForSeconds(second);
            var enemies = GameObject.FindGameObjectsWithTag("Enemy").Select(enemy => enemy.transform.position).ToList();
            nearestTargets = enemies.OrderBy(position => Vector3.Distance(transform.position, position)).ToList();
        }
    }

    public List<Vector3> GetNearestTargets(int count)
    {
        try
        {
            return nearestTargets.GetRange(0, count);
        }
        catch
        {
            return nearestTargets;
        }
    }
    
    public Vector3 GetNearestTarget()
    {
        return nearestTargets[0];
    }
}
