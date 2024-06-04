using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class DynamicFigureGenerator : MonoBehaviour
{
    public static event Action<Dictionary<string, List<Vector2>>> OnFigureGenerated;
    
    [SerializeField] private GameObject _blockPrefab;
    [SerializeField] private int _baseLength = 5;
    [SerializeField] private int _maxAdditionalBlocks = 5;
    [SerializeField] private int _minAdditionalBlocks = 1;

    void Start()
    {
        GenerateRandomFigure();
    }

    private void GenerateRandomFigure()
    {
        var figureSides = new Dictionary<string, List<Vector2>>
        {
            { "Main", GenerateMainLine() },
            { "xGrid", GenerateRandomSideBlock() },
            { "zGrid", GenerateRandomSideBlock() },
        };

        InstantiateFigure(figureSides);

        figureSides.Add("-xGrid",figureSides["xGrid"].Select(vector2 => new Vector2(-vector2.x, vector2.y)).ToList());
        figureSides.Add("-zGrid",figureSides["zGrid"].Select(vector2 => new Vector2(-vector2.x, vector2.y)).ToList());

        OnFigureGenerated?.Invoke(figureSides);
    }

    private void InstantiateFigure(Dictionary<string, List<Vector2>> figureSides)
    {
        foreach (var keyValuePair in figureSides)
        {
            switch (keyValuePair.Key)
            {
                case "xGrid":
                {
                    foreach (var vector2 in keyValuePair.Value)
                    {
                        Instantiate(_blockPrefab, transform.position + new Vector3(vector2.x, vector2.y, 0f), Quaternion.identity, transform);
                    }

                    break;
                }
                case "zGrid":
                {
                    foreach (var vector2 in keyValuePair.Value)
                    {
                        Instantiate(_blockPrefab, transform.position + new Vector3(0f, vector2.y, vector2.x), Quaternion.identity, transform);
                    }

                    break;
                }
                case "Main":
                {
                    foreach (var vector2 in keyValuePair.Value)
                    {
                        Instantiate(_blockPrefab, transform.position + new Vector3(0f, vector2.y, 0f), Quaternion.identity, transform);
                    }

                    break;
                }
            }
        }
    }

    private List<Vector2> GenerateMainLine()
    {
        var mainLine = new List<Vector2>();
        for (int i = 0; i < _baseLength; i++)
        {
            mainLine.Add(new Vector2(0, i));
        }
        return mainLine;
    }

    private List<Vector2> GenerateRandomSideBlock()
    {
        var sideBlock = new List<Vector2>();
        int y = Random.Range(0, _baseLength);
        for (int i = 0; i < Random.Range(_minAdditionalBlocks, _maxAdditionalBlocks); i++)
        {
            sideBlock.Add(new Vector2(i + 1, y));
        }

        int newY = Random.Range(0, _baseLength);
        while (newY == y)
        {
            newY = Random.Range(0, _baseLength);
        }
        y = newY;

        for (int i = 0; i < Random.Range(_minAdditionalBlocks, _maxAdditionalBlocks); i++)
        {
            sideBlock.Add(new Vector2( -1 - i, y));
        }
        return sideBlock;
    }
}