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

    [SerializeField] private float createDelay = 2f;
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
        _isGameOver = false;
        Resume();

        StartCoroutine(CreateMonster());
        StartCoroutine(FindNearestTargets(2f));
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
        var enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        nearestTargets = enemies.Select(enemy => enemy.transform.position)
            .OrderBy(position => Vector2.Distance(transform.position, position))
            .ToList();

        yield return new WaitForSeconds(second);
    }

    public List<Vector3> GetNearestTargets(int count)
    {
        try
        {
            return nearestTargets.Take(count) as List<Vector3>;
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
