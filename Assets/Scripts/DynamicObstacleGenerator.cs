using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class DynamicObstacleGenerator : MonoBehaviour
{
    [SerializeField] private int _gridSize = 10;
    [SerializeField] private GameObject _blockPrefab;
    [SerializeField] private GameObject _obstaclePrefab;
    [SerializeField] private float _spawnInterval = 10f;
    [SerializeField] private int _spawnDistance = 20;
    [SerializeField] private Transform _cameraTransform;

    private Dictionary<string, List<Vector2>> _playerFigure;
    private List<GameObject> _obstacles = new();
    private float _nextSpawnDistance;
    private int _lastGeneratedSideIndex;

    private void Start()
    {
        StartCoroutine(ObstacleSpawner());
    }

    private void OnEnable()
    {
        DynamicFigureGenerator.OnFigureGenerated += SetupPlayerFigure;
    }

    private void OnDisable()
    {
        DynamicFigureGenerator.OnFigureGenerated -= SetupPlayerFigure;
    }

    private void SetupPlayerFigure(Dictionary<string, List<Vector2>> playerFigure)
    {
        _playerFigure = playerFigure;
    }

    private IEnumerator ObstacleSpawner()
    {
        yield return new WaitUntil(() => _playerFigure!= null);
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
        
        List<Vector2> points = new();
        _lastGeneratedSideIndex = GenerateNewIndex();
        points.AddRange(_playerFigure["Main"]);
        points.AddRange(_playerFigure.ElementAt(_lastGeneratedSideIndex).Value);
        _nextSpawnDistance += _spawnDistance;
        GameObject obstacle = Instantiate(_obstaclePrefab, new Vector3(0f, 0f, _nextSpawnDistance), Quaternion.identity, transform);

        for (int x = -5; x < 5; x++)
        {
            for (int y = 0; y < _gridSize; y++)
            {
                if (!points.Any(vector2 => vector2.x == x && vector2.y == y))
                {
                    Instantiate(_blockPrefab, new Vector3(x, y, _nextSpawnDistance), Quaternion.identity, obstacle.transform);
                }
            }
        }
        _obstacles.Add(obstacle);
    }

    private int GenerateNewIndex()
    {
        int index = Random.Range(1, _playerFigure.Count);
        while (_lastGeneratedSideIndex == index)
        {
            index = Random.Range(1, _playerFigure.Count);
        }

        return index;
    }
}