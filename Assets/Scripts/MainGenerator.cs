using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[Serializable]
public class ObjectsToSpawn
{
    public GameObject figurePrefab;
    public GameObject[] obstaclePrefabs;
}

public class MainGenerator : MonoBehaviour
{
    [SerializeField] private float _spawnInterval = 10f;
    [SerializeField] private int _spawnDistance = 20;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private List<ObjectsToSpawn> _objectsToSpawns;
    [SerializeField] private Transform _playerParent;

    private int _playerFigureIndex;
    private List<GameObject> _obstacles = new();
    private float _nextSpawnDistance;
    private int _lastGeneratedSideIndex;

    private void Start()
    {
        _playerFigureIndex = Random.Range(0, _objectsToSpawns.Count);
        Instantiate(_objectsToSpawns[_playerFigureIndex].figurePrefab, transform.position, quaternion.identity, _playerParent);
        StartCoroutine(ObstacleSpawner());
    }

    private IEnumerator ObstacleSpawner()
    {
        for (int i = 0; i < 3; i++)
        {
            GenerateObstacle();
        }

        while (true)
        {
            yield return new WaitForSeconds(_spawnInterval);
            GenerateObstacle();
            DeletePassedObstacle();
        }
    }

    private void DeletePassedObstacle()
    {
        List<GameObject> obstaclesToDelete =
            _obstacles.FindAll(x => x.transform.position.z < _cameraTransform.transform.position.z);

        foreach (GameObject obstacleToDelete in obstaclesToDelete)
        {
            _obstacles.Remove(obstacleToDelete);
            Destroy(obstacleToDelete);
            if (_obstacles.Count < 5)
            {
                GenerateObstacle();
            }
        }

    }

    private void GenerateObstacle()
    {
        if (_obstacles.Count > 10)
        {
            return;
        }
        
        _nextSpawnDistance += _spawnDistance;
        GameObject obstacle = Instantiate(_objectsToSpawns[_playerFigureIndex].obstaclePrefabs[Random.Range(0,4)], new Vector3(0f, 0f, _nextSpawnDistance), Quaternion.identity, transform);
        _obstacles.Add(obstacle);
    }
}