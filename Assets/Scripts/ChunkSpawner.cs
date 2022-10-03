using System;
using System.Collections.Generic;
using UnityEngine;

public class ChunkSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _chunkPrefab;
    [SerializeField] private GameObject _cubePrefab;
    [SerializeField] private float _secondsBetweenChunks;

    [SerializeField] private int _showNewCunksEvery = 2;

    [SerializeField] private MeshRenderer _backgroundRenderer;
    private float _backgroundSpeed = 0.4f;

    private bool _isSpawning = false;
    private float _elapsedTime = 0f;

    private int _increaseCount = 0;

    private bool _startIncreaseChunkSpeed = false;

    private int _lastRandomChunkIndex = -1;
    private int _currentChunkRange = 0;

    // TODO: Use a better data structure to represent chunks
    private List<int[,]> _chunks = new List<int[,]> {
        // 0 - 7
        new int[,] {
            {1, 1},
        },
        new int[,] {
            {1, 1},
        },
        new int[,] {
            {1},
            {1}
        },
        new int[,] {
            {1},
            {1}
        },
        new int[,] {
            {1},
            {1},
            {1}
        },
        new int[,] {
            {1, 1, 1}
        },
        new int[,] {
            {1, 1},
            {1, 1}
        },
        new int[,] {
            {1, 1},
            {1, 1}
        },


        // 8 - 11
        new int[,] {
            {1},
            {1},
            {1},
            {1}
        },
        new int[,] {
            {1, 1, 1, 1}
        },
        new int[,] {
            {1, 1, 1},
            {1, 1, 0},
            {1, 0, 0}
        },
        new int[,] {
            {1, 1, 1},
            {0, 1, 1},
            {0, 0, 0}
        },


        // 12 -
        new int[,] {
            {1, 1, 1, 1, 1},
            {0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0},
            {1, 1, 1, 1, 1}
        },
        new int[,] {
            {1, 0, 0, 0, 1},
            {1, 0, 0, 0, 1},
            {1, 0, 0, 0, 1},
            {1, 0, 0, 0, 1},
            {1, 0, 0, 0, 1}
        },
        new int[,] {
            {1, 1},
            {1, 1},
            {1, 1},
            {1, 1},
            {1, 1}
        },
        new int[,] {
            {1, 1, 1, 1, 1},
            {1, 1, 1, 1, 1},
        },
        new int[,] {
            {0, 0, 1, 1, 1},
            {0, 0, 0, 1, 1},
            {1, 0, 0, 0, 1},
            {1, 1, 0, 0, 0},
            {1, 1, 1, 0, 0},
        },
    };

    private List<Range> _chunkRanges = new List<Range> {
        0..8,
        4..12,
        8..17
    };

    private void Start()
    {
        GameEvents.OnGameStart += StartGame;
        GameEvents.OnGameEnded += EndGame;

        TimeCounter.OnTimeReached += IncreaseChunksRange;
    }

    private void StartGame()
    {
        _isSpawning = true;
        _elapsedTime = 0f;
        _currentChunkRange = 0;
        _lastRandomChunkIndex = -1;
        _increaseCount = 0;
    }

    private void EndGame()
    {
        _isSpawning = false;
        foreach(Transform child in transform) {
            Destroy(child.gameObject, 1f);
        }
    }

    void Update()
    {
        if (_isSpawning) {
            _elapsedTime -= Time.deltaTime;

            if (_elapsedTime <= 0) {
                Spawn();
                _elapsedTime = _secondsBetweenChunks;
            }
        }
    }

    void Spawn()
    {
        int nextChunkIndex = _lastRandomChunkIndex;
        while (nextChunkIndex == _lastRandomChunkIndex) {
            nextChunkIndex = UnityEngine.Random.Range(
                _chunkRanges[_currentChunkRange].Start.Value,
                _chunkRanges[_currentChunkRange].End.Value);
        } 
        _lastRandomChunkIndex = nextChunkIndex;
        // nextChunkIndex = 4;
        //Debug.Log(nextChunkIndex);

        int chunkHeight = _chunks[nextChunkIndex].GetLength(0); 
        int chunkWidth = _chunks[nextChunkIndex].GetLength(1);
        // Debug.Log($"width {chunkWidth}");
        // Debug.Log($"height {chunkHeight}");

        int nextX = UnityEngine.Random.Range(0, Constants.GRID_SIZE - 1);
        int nextY = UnityEngine.Random.Range(0, Constants.GRID_SIZE - 1);
        // nextX = 2;
        // nextY = 1;
        // Debug.Log($"next X {nextX}");
        // Debug.Log($"next Y {nextY}");

        //2 + 5 > 5 ? 2 - ((2 + 5) -5 ) + 1
        //nextX = nextX + chunkWidth > Constants.GRID_SIZE ? nextX - (nextX - chunkWidth) : nextX;
        nextX = nextX + chunkWidth > Constants.GRID_SIZE ? nextX - ((nextX + chunkWidth) - chunkWidth): nextX;
        nextY = nextY + chunkHeight > Constants.GRID_SIZE ? nextY - ((nextY + chunkHeight) - chunkHeight) : nextY;
        // Debug.Log($"corrected next X {nextX}");
        // Debug.Log($"corrected next Y {nextY}");

        var chunkPrefab = Instantiate(_chunkPrefab, this.transform);
        chunkPrefab.name = $"Chunk Number {nextChunkIndex}";

        int min = Constants.GRID_SIZE % 2 == 0 ? -(Constants.GRID_SIZE / 2) : -((Constants.GRID_SIZE-1) / 2);
        int max = Constants.GRID_SIZE % 2 == 0 ? (Constants.GRID_SIZE / 2)-1 : (Constants.GRID_SIZE-1) / 2;
        // Debug.Log("---");

        for (int x = 0; x < Constants.GRID_SIZE; x++)
        {
            for (int y = 0; y < Constants.GRID_SIZE; y++)
            {
                if (x >= nextX && x < chunkWidth + nextX && 
                    y >= nextY && y < chunkHeight +nextY)
                {
                    if (_chunks[nextChunkIndex][(y - nextY), (x - nextX)] == 1)
                    {
                        var cube = Instantiate(
                            _cubePrefab, 
                            new Vector3(
                                (x + min) * 1.5f,
                                (y + min) * -1.5f, 
                                this.transform.position.z), 
                        Quaternion.identity);
                        cube.transform.parent = chunkPrefab.transform;
                    }
                }
            }
        }
    }

    private void IncreaseChunksRange()
    {
        const float minTimeDistanceBetweenChunks = 0.25f;

        if (++_increaseCount == _showNewCunksEvery) {
            _increaseCount = 0;
            _currentChunkRange++;
        }

        if (_currentChunkRange == _chunkRanges.Count) {
            _currentChunkRange = _chunkRanges.Count - 1;
            _startIncreaseChunkSpeed = true;
        }

        if (_startIncreaseChunkSpeed) {
            _secondsBetweenChunks -= 0.05f;
            _backgroundSpeed += 0.05f;
            _backgroundRenderer.material.SetFloat("_Speed", _backgroundSpeed);


            if (_secondsBetweenChunks < minTimeDistanceBetweenChunks) {
                _secondsBetweenChunks = minTimeDistanceBetweenChunks;
            }
        }
    }
}
