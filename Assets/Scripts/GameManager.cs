using System;
using System.Collections.Generic;
using System.Linq;
using Facebook.Unity;
using Monsters;
using Newtonsoft.Json;
using Pickups;
using Status;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using Vector3 = UnityEngine.Vector3;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool isSpawning;
    
    [SerializeField] private Player player;
    [SerializeField] private LevelUpRewards levelUpRewards;
    [SerializeField] public PostProcessVolume post;
    [SerializeField] public GameObject indicator;
    [SerializeField] public PlayerStat playerStat;
    [SerializeField] public ExpSoul1 expSoul1;
    [SerializeField] public GameObject joystick;
    
    public static GameManager Instance;
    public Dictionary<string, float> ActivatedSkills = new();
    private List<GameObject> _enemies = new();

    private Material _defaultMaterial;
    
    private bool _isGameOver;
    private float _playtime;
    
    public int monsterCount;

    private Player _initialPlayerStatus;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    
        FB.Init(InitCallback, OnHideUnity);

            if(Instance == null)
        {
            Instance = this;
        } else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    
    private void InitCallback ()
    {
        if (FB.IsInitialized) {
            Debug.Log("Facebook SDK initialized");

            FB.ActivateApp();
        } else {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    private void OnHideUnity (bool isGameShown)
    {
    }
    
    private void Start()
    {
        Initialize();
        LevelUpEvent();
    }

    private void Initialize()
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

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        // SetIsGameOver(true);
        // Initialize();
        // playerStat.Initialize();
        // player.Initialize();
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
        _enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        foreach (var enemy in _enemies)
        {
            enemy.GetComponent<IMonster>().StopMonster();
        }

        player.GetComponent<PlayerController>().ResetJoystick();
        joystick.SetActive(false);
    }

    public void Resume()
    {
        Time.timeScale = 1;
        Debug.Log("Resume");
        _enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        foreach (var enemy in _enemies)
        {
            enemy.GetComponent<IMonster>().ResumeMonster();
        }
        ActivatedSkills = player.GetActivatedSkills(true);
        joystick.SetActive(true);
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

        // Debug.Log("Count: " + enemiesTree.Count);
        var pos = enemiesTree.FindClosest(player.transform.position).position;
        
        if (Vector3.Distance(pos, player.transform.position) <= distance)
        {
            return pos;
        }
        throw new InvalidOperationException();
    }
    
    public List<Vector3> GetClosestTargets(float distance, int count)
    {
        KdTree<Transform> enemiesTree = new();
        enemiesTree.AddAll(_enemies.Select(e => e.transform).ToList());

        List<Vector3> targets = new();

        var p = player.transform.position;
        foreach (var e in enemiesTree) 
        {
            var pos = enemiesTree.FindClosest(p).position;
            targets.Add(pos);
            
            enemiesTree.RemoveAt(0);
            if (targets.Count == count)
            {
                return targets; 
            }
        }

        return targets;
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
