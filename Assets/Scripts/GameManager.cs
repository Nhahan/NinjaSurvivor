using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Monsters;
using Newtonsoft.Json;
using Pickups;
using Status;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Vector3 = UnityEngine.Vector3;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool isSpawning;
    
    [SerializeField] private Player player;
    [SerializeField] private LevelUpRewards levelUpRewards;
    [SerializeField] public PostProcessVolume post;
    [SerializeField] public GameObject indicator;
    [SerializeField] public ExpSoul1 expSoul1;
    
    public static GameManager Instance;
    public Dictionary<string, float> ActivatedSkills = new();
    private List<GameObject> _enemies = new();

    private Material _defaultMaterial;
    
    private bool _isGameOver;
    private float _playtime;

    private Player _initialPlayerStatus;

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

        _initialPlayerStatus = DeepClone(player);
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

    public void Restart()
    {
        _isGameOver = true;
        player = _initialPlayerStatus;
        Resume();
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
            return DeepClone(_enemies.GetRange(0, count).Select(e => e.transform.position).ToList());
        }
        catch
        {
            return DeepClone(_enemies.Select(e => e.transform.position).ToList());
        }
    }
    
    public Vector3 GetTarget()
    {
        return DeepClone(_enemies[0].transform.position);
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
        KdTree<Transform> enemiesTree = new();
        enemiesTree.AddAll(_enemies.Select(e => e.transform).ToList());

        Debug.Log("Count: " + enemiesTree.Count);
        var pos = enemiesTree.FindClosest(player.transform.position).position;
        
        if (Vector3.Distance(pos, player.transform.position) <= distance) 
        {
            return enemiesTree.FindClosest(player.transform.position).position;
        }
        throw new InvalidOperationException();
    }

    public Material GetFlashMaterial()
    {
        return GetComponent<SpriteRenderer>().material;
    }
    
    public Material GetDefaultMaterial()
    {
        return _defaultMaterial;
    }
    
    public T DeepClone<T>(T instance)
    {
        return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(instance));
    }
}
