using System.Collections;
using System.Collections.Generic;
using Status;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MonsterSpawner
{
    public class MonsterSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject anteater;
        [SerializeField] private GameObject suicider;
        [SerializeField] private GameObject cannibalisia;
        [SerializeField] private GameObject acidSpitter;
        [SerializeField] private GameObject redAnteater;
        [SerializeField] private GameObject disist;
        [SerializeField] private GameObject redDisist;

        private Player _player;

        private readonly List<Transform> _spawnPoints = new();
        private float _pointsCount;

        private void Start()
        {
            _player = GameManager.Instance.GetPlayer();
            
            foreach(Transform child in transform)
            {
                _spawnPoints.Add(child);
            }

            _pointsCount = _spawnPoints.Count;

            StartCoroutine(SpawnMonster(9f));
            StartCoroutine(SpawnBasicMonster(2.5f));
        }

        private IEnumerator SpawnMonster(float second)
        {
            var r = transform.rotation;
            RandomSpawn(100f, anteater, r);
            RandomSpawn(100f, suicider, r);
            while (!GameManager.Instance.GetIsGameOver())
            {
                var level = _player.GetLevel();
                var difficulty = 4f + second / level;
                yield return new WaitForSeconds(difficulty);
                
                if (GameManager.Instance.monsterCount > 350) { continue; }
                
                switch (level)
                {
                    case < 4:
                    {
                        for (var i = 0; i < level; i++)
                        {
                            RandomSpawn(90f, disist, r);
                        }
                        RandomSpawn(level * 33f, suicider, r);
                        RandomSpawn(1.5f, redAnteater, r);
                        RandomSpawn(50f, anteater, r);
                        break;
                    }
                    case < 7:
                    {
                        for (var i = 0; i < level-2; i++)
                        {
                            RandomSpawn(100f, anteater, r);
                        }
                        RandomSpawn(level * 50f, suicider, r);
                        RandomSpawn(1.5f, redAnteater, r);
                        RandomSpawn(50f, anteater, r);
                        break;
                    }
                    case < 10:
                    {
                        for (var i = 0; i < level - 4; i++)
                        {
                            RandomSpawn(100f, anteater, r);
                            RandomSpawn(100f, anteater, r);
                        }
                        RandomSpawn(77f, suicider, r);
                        RandomSpawn(33, suicider, r);
                        RandomSpawn(16f, redAnteater, r);
                        RandomSpawn(level * 8f, anteater, r);
                        RandomSpawn(level * 2.3f, cannibalisia, r);
                        break;
                    }
                    case < 14:
                    {
                        for (var i = 0; i < 7; i++)
                        {
                            RandomSpawn(100f, anteater, r);
                            RandomSpawn(100f, anteater, r);
                            RandomSpawn(100f, suicider, r);
                            RandomSpawn(15f, suicider, r);
                        }
                        RandomSpawn(level * 6f, cannibalisia, r);
                        RandomSpawn(16f, redAnteater, r);
                        RandomSpawn(level * 0.3f, redAnteater, r);
                        break;
                    }
                    case < 18:
                    {
                        for (var i = 0; i < 8; i++)
                        {
                            RandomSpawn(100f, anteater, r);
                            RandomSpawn(30, redAnteater, r);
                            RandomSpawn(100f, suicider, r);
                        }
                        RandomSpawn(level * 0.5f, acidSpitter, r);
                        RandomSpawn(level * 4f, cannibalisia, r);
                        break;
                    }
                    case < 24:
                    {
                        for (var i = 0; i < 9; i++)
                        {
                            RandomSpawn(100f, anteater, r);
                            RandomSpawn(100f, anteater, r);
                            RandomSpawn(100f, suicider, r);
                        }
                        RandomSpawn(66f, cannibalisia, r);
                        RandomSpawn(level * 0.5f, acidSpitter, r);
                        RandomSpawn(level * 1f, redAnteater, r);
                        break;
                    }
                    case < 30:
                    {
                        for (var i = 0; i < 10; i++)
                        {
                            RandomSpawn(10f, acidSpitter, r);
                            RandomSpawn(5f, acidSpitter, r);
                            RandomSpawn(100f, anteater, r);
                            RandomSpawn(100f, anteater, r);
                            RandomSpawn(100f, suicider, r);
                        }
                        RandomSpawn(66f, cannibalisia, r);
                        RandomSpawn(level * 1f, redAnteater, r);
                        RandomSpawn(level * 0.5f, acidSpitter, r);
                        break;
                    }
                    // case 30:
                    // {
                    //     // Golem Boss
                    //     break;
                    // }
                    default:
                    {
                        for (var i = 0; i < 7; i++)
                        {
                            RandomSpawn(100f, anteater, r);
                            RandomSpawn(20f, acidSpitter, r);
                            RandomSpawn(level * 2f, acidSpitter, r);
                            RandomSpawn(100f, redAnteater, r);
                            RandomSpawn(100f, redAnteater, r);
                            RandomSpawn(100f, suicider, r);
                        }
                        RandomSpawn(66f, cannibalisia, r);
                        RandomSpawn(66f, cannibalisia, r);
                        break;
                    }
                }
            }
        }
        
        private IEnumerator SpawnBasicMonster(float second)
        {
            var r = transform.rotation;
            RandomSpawn(100f, disist, r);
            RandomSpawn(100f, disist, r);
            RandomSpawn(100f, disist, r);
            RandomSpawn(100f, disist, r);
            RandomSpawn(100f, disist, r);
            RandomSpawn(100f, disist, r);
            RandomSpawn(100f, disist, r);
            RandomSpawn(100f, disist, r);
            while (!GameManager.Instance.GetIsGameOver())
            {
                var level = _player.GetLevel();
                yield return new WaitForSeconds(second);
                switch (level) {
                    case < 10:
                    {
                        RandomSpawn(100f, disist, r);
                        RandomSpawn(100f, disist, r);
                        RandomSpawn(100f, disist, r);
                        RandomSpawn(100f, disist, r);
                        RandomSpawn(5f, redDisist, r);
                        break;
                    }
                    case < 20:
                    {
                        RandomSpawn(100f, disist, r);
                        RandomSpawn(100f, disist, r);
                        RandomSpawn(100f, disist, r);
                        RandomSpawn(100f, redDisist, r);
                        RandomSpawn(50f, redDisist, r);
                        break;
                    }
                    default:
                    {
                        RandomSpawn(100f, redDisist, r);
                        RandomSpawn(100f, redDisist, r);
                        RandomSpawn(100f, redDisist, r);
                        RandomSpawn(100f, disist, r);
                        RandomSpawn(10f, disist, r);
                        RandomSpawn(10f, disist, r);
                        break;
                    }
                }
            }
        }

        private Vector3 GetRandomSpawnPoint()
        {
            try
            {
                return _spawnPoints[(int)Random.Range(0, _pointsCount)].position + new Vector3(Random.Range(-2, 2), Random.Range(-2, 2), 0);
            }
            catch
            {
                return _spawnPoints[0].position;
            }
        }

        private void RandomSpawn(float percentage, GameObject prefab, Quaternion r)
        {
            if (percentage >= 100f || Random.Range(0, 100) < percentage)
            {
                GameManager.Instance.AddTarget(Instantiate(prefab, GetRandomSpawnPoint(), r));
                GameManager.Instance.monsterCount++;
            }
        }
    }
}
