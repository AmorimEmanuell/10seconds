using System;
using System.Collections.Generic;
using UnityEngine;

public class ChunkSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _chunkPrefab;
    [SerializeField] private GameObject _cubePrefab;
    [SerializeField] private float _secondsBetweenChunks;

    // TODO: Use a better data structure to represent chunks
    private List<int[,]> _chunks = new List<int[,]> {
        new int[,] {
            {1, 1, 1},
            {1, 1, 1},
            {1, 1, 1}
        },
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
            {1, 1, 1, 0, 0},
            {1, 1, 1, 0, 0},
            {1, 1, 1, 0, 0},
            {1, 1, 1, 0, 0},
            {1, 1, 1, 0, 0}
        },
        new int[,] {
            {1, 1, 1, 0, 0},
            {1, 1, 1, 0, 0},
            {1, 1, 1, 0, 0},
            {1, 1, 1, 0, 0},
            {1, 1, 1, 0, 0}
        },
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
            {1, 1},
            {1, 1}
        }
    };

    private void Start()
    {
        GameEvents.OnGameStart += StartGame;
        GameEvents.OnGameEnded += EndGame;
    }

    private void StartGame()
    {
        InvokeRepeating("Spawn", 0f, _secondsBetweenChunks);
    }

    private void EndGame()
    {
        CancelInvoke();
    }

    void Spawn()
    {
        int nextChunkIndex = UnityEngine.Random.Range(0, _chunks.Count - 1);
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
}
