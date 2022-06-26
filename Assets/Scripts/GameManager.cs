using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Monsters;
using Pickups;
using Status;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Vector3 = UnityEngine.Vector3;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool isSpawning;
    
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject monsterPrefab;
    [SerializeField] private Player player;
    [SerializeField] private LevelUpRewards levelUpRewards;
    [SerializeField] public PostProcessVolume post;
    [SerializeField] public ExpSoul1 expSoul1;
    
    public static GameManager Instance;
    public Dictionary<string, float> ActivatedSkills = new();
    private readonly List<GameObject> _enemies = new();
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
        _defaultMaterial = player.GetComponent<SpriteRenderer>().material;
    }

    private void FixedUpdate()
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

    public List<Vector3> GetTargets(int count)
    {
        try
        {
            return _enemies.GetRange(0, count).Select(e => e.transform.position).ToList();
        }
        catch
        {
            return _enemies.Select(e => e.transform.position).ToList();
        }
    }
    
    public Vector3 GetTarget()
    {
            return _enemies[0].transform.position;
    }

    public void AddTarget(GameObject enemy)
    {
        _enemies.Add(enemy);
    }

    public void RemoveTarget(GameObject enemy)
    {
        _enemies.Remove(enemy);
    }

    public Vector3 GetClosestTarget(float distance)
    {
        var closestDistance = distance;
        Vector3 pos = default;

        Debug.Log("Count: " + _enemies.Count);
        foreach (var enemy in _enemies)
        {
            var currentDistance = Vector3.Distance(transform.position, enemy.transform.position);
            if (currentDistance < closestDistance)
            {
                closestDistance = currentDistance;
                pos = enemy.transform.position;
            }
        }

        return pos;
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
