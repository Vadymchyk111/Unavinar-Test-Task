using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _roadPrefab;
    [SerializeField] private float _spawnDistance = 20f;
    [SerializeField] private GameObject _firstSegment;

    private Queue<GameObject> _roadSegments = new Queue<GameObject>();
    private float _nextSpawnDistance;

    private void Start()
    {
        _roadSegments.Enqueue(_firstSegment);
        _nextSpawnDistance = transform.position.z + _spawnDistance;
        SpawnRoadSegment();
    }

    private void Update()
    {
        if (transform.position.z >= _nextSpawnDistance)
        {
            _nextSpawnDistance += _spawnDistance;
            SpawnRoadSegment();
            RemoveOldestRoadSegment();
        }
    }

    private void SpawnRoadSegment()
    {
        GameObject roadSegment = Instantiate(_roadPrefab, new Vector3(0f, -.6f, _nextSpawnDistance), Quaternion.identity);
        _roadSegments.Enqueue(roadSegment);
    }

    private void RemoveOldestRoadSegment()
    {
        GameObject oldestRoadSegment = _roadSegments.Dequeue();
        Destroy(oldestRoadSegment);
    }
}