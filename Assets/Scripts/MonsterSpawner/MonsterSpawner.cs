using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Monsters;
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

            StartCoroutine(SpawnMonster(7f));
            StartCoroutine(SpawnBasicMonster(4.5f));
        }

        private IEnumerator SpawnMonster(float second)
        {
            var r = transform.rotation;
            RandomSpawn(100f, anteater, r);
            RandomSpawn(77f, suicider, r);
            while (!GameManager.Instance.GetIsGameOver())
            {
                var level = _player.GetLevel();
                var difficulty = 2f + second / level;
                yield return new WaitForSeconds(difficulty);
                switch (level)
                {
                    case < 3:
                    {
                        for (var i = 0; i < level; i++)
                        {
                            RandomSpawn(100f, anteater, r);
                        }
                        RandomSpawn(level * 50f, suicider, r);
                        RandomSpawn(1.5f, redAnteater, r);
                        RandomSpawn(50f, anteater, r);
                        break;
                    }
                    case < 6:
                    {
                        for (var i = 0; i < level - 3; i++)
                        {
                            RandomSpawn(100f, anteater, r);
                            RandomSpawn(100f, anteater, r);
                        }
                        RandomSpawn(77f, suicider, r);
                        RandomSpawn(16f, redAnteater, r);
                        RandomSpawn(level * 8f, anteater, r);
                        RandomSpawn(level * 2.3f, cannibalisia, r);
                        RandomSpawn(level * 0.2f, acidSpitter, r);
                        break;
                    }
                    case < 10:
                    {
                        for (var i = 0; i < level - 6; i++)
                        {
                            RandomSpawn(100f, anteater, r);
                            RandomSpawn(100f, anteater, r);
                            RandomSpawn(100f, suicider, r);
                        }
                        RandomSpawn(level * 6f, cannibalisia, r);
                        RandomSpawn(16f, redAnteater, r);
                        RandomSpawn(level * 0.3f, acidSpitter, r);
                        break;
                    }
                    case < 13:
                    {
                        for (var i = 0; i < level - 9; i++)
                        {
                            RandomSpawn(100f, cannibalisia, r);
                            RandomSpawn(100f, redAnteater, r);
                            RandomSpawn(100f, suicider, r);
                        }
                        RandomSpawn(level * 0.3f, acidSpitter, r);
                        break;
                    }
                    case < 17:
                    {
                        for (var i = 0; i < level - 10; i++)
                        {
                            RandomSpawn(100f, anteater, r);
                            RandomSpawn(100f, anteater, r);
                            RandomSpawn(100f, suicider, r);
                        }
                        RandomSpawn(66f, cannibalisia, r);
                        RandomSpawn(level * 0.4f, acidSpitter, r);
                        RandomSpawn(level * 2f, redAnteater, r);
                        break;
                    }
                    case < 23:
                    {
                        for (var i = 0; i < level - 13; i++)
                        {
                            RandomSpawn(100f, acidSpitter, r);
                            RandomSpawn(100f, anteater, r);
                            RandomSpawn(100f, anteater, r);
                            RandomSpawn(100f, suicider, r);
                        }
                        RandomSpawn(66f, cannibalisia, r);
                        RandomSpawn(level * 1.5f, redAnteater, r);
                        RandomSpawn(level * 0.5f, acidSpitter, r);
                        break;
                    }
                    default:
                    {
                        for (var i = 0; i < 11; i++)
                        {
                            RandomSpawn(100f, acidSpitter, r);
                            RandomSpawn(100f, acidSpitter, r);
                            RandomSpawn(100f, acidSpitter, r);
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
            while (!GameManager.Instance.GetIsGameOver())
            {
                var level = _player.GetLevel();
                yield return new WaitForSeconds(second);
                switch (level) {
                    case < 10:
                    {                        
                        RandomSpawn(100f, disist, r);
                        RandomSpawn(100f, disist, r);
                        break;
                    }
                    case < 20:
                    {
                        RandomSpawn(100f, disist, r);
                        RandomSpawn(100f, redDisist, r);
                        break;
                    }
                    default:
                    {
                        RandomSpawn(100f, redDisist, r);
                        RandomSpawn(100f, redDisist, r);
                        break;
                    }
                }
            }
        }

        private Vector3 GetRandomSpawnPoint()
        {
            try
            {
                return _spawnPoints[(int)Random.Range(0, _pointsCount)].position;
            }
            catch
            {
                return _spawnPoints[0].position;
            }
        }

        private void RandomSpawn(float percentage, GameObject prefab, Quaternion r)
        {
            if (percentage == 100f || Random.Range(0, 100) < percentage)
            {
                GameManager.Instance.AddTarget(Instantiate(prefab, GetRandomSpawnPoint(), r));
            }
        }
    }
}
