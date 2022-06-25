using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Status;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool isSpawning;
    
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject monsterPrefab;
    [SerializeField] private Player player;
    [SerializeField] private LevelUpRewards levelUpRewards;
    [SerializeField] public PostProcessVolume post;
    [SerializeField] public GameObject spawnerParent;
    
    public static GameManager Instance;
    public Dictionary<string, float> ActivatedSkills = new();
    private List<Vector3> _nearestTargets = new();
    private MonsterSpawner.MonsterSpawner _monsterSpawner;
    private Material _defaultMaterial;
    
    private bool _isGameOver;
    private float _playtime;

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
        StartCoroutine(FindNearestTargets(1.5f));
        
        _monsterSpawner = spawnerParent.GetComponent<MonsterSpawner.MonsterSpawner>();
        _defaultMaterial = player.GetComponent<SpriteRenderer>().material;
    }

    private void Update()
    {
        _playtime += Time.deltaTime;
    }

    public Player GetPlayer()
    {
        return player;
    }
    
    public float GetPlayTime()
    {
        return _playtime;
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
            
            _nearestTargets = enemies.OrderBy(position => Vector3.Distance(transform.position, position)).ToList();
        }
    }

    public List<Vector3> GetNearestTargets(int count)
    {
        try
        {
            return _nearestTargets.GetRange(0, count);
        }
        catch
        {
            return _nearestTargets;
        }
    }
    
    public Vector3 GetNearestTarget()
    {
        return _nearestTargets[0];
    }

    public Material GetFlashMaterial()
    {
        return GetComponent<SpriteRenderer>().material;
    }
    
    public Material GetDefaultMaterial()
    {
        return _defaultMaterial;
    }
}
