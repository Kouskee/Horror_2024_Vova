using System;
using System.Collections;
using System.Collections.Generic;
using Chunks.Chunk_Additions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Chunks
{
    public class GenerateChunks : MonoBehaviour
    {
        [Header("Transforms")]
        [SerializeField] private Transform _player;
        [SerializeField] private Transform _parentChunk;
        [Header("Settings")]
        [SerializeField] private float _frequency;
        [SerializeField] private int _distanceVisibility;
        [Header("Chunks")]
        [SerializeField] private Chunk _currentChunk;
        [SerializeField] private List<Chunk> _chunkPrefabs;
        [Space]
        [SerializeField] private Vector2Int _chunkNumber;
        
        private DataChunks _dataChunks;
        private PoolObjects[] _pools;

        private void Start()
        {
            StartCoroutine(CoroutineStart());
        }

        private IEnumerator CoroutineStart()
        {
            yield return StartCoroutine(InitializePool());
            
            _dataChunks = new DataChunks(_currentChunk);

            StartCoroutine(CheckCurrentChunk());
            StartCoroutine(CalculateAroundChunks(_chunkNumber));
        }
        
        private IEnumerator InitializePool()
        {
            _pools = new PoolObjects[_chunkPrefabs.Count];
            for (int i = 0; i < _pools.Length; i++)
            {
                if(_chunkPrefabs[i].ChunkSize != _currentChunk.ChunkSize) 
                    throw new Exception($"{_chunkPrefabs[i].name} has another chunkSize than FirstChunk. Fix it");
                _pools[i] = new PoolObjects(_chunkPrefabs[i], 10, _parentChunk);
                yield return null;
            }
        }
        
        private IEnumerator CheckCurrentChunk()
        {
            while (true)
            {
                var currentChunk = new Vector2Int(Mathf.RoundToInt(_player.position.x / _currentChunk.ChunkSize),
                    Mathf.RoundToInt(_player.position.z / _currentChunk.ChunkSize));
    
                if (currentChunk != _chunkNumber)
                {
                    _currentChunk = _dataChunks.GetChunk(currentChunk);
                    yield return StartCoroutine(CalculateAroundChunks(currentChunk));
                    _chunkNumber = currentChunk;
                    CheckChunksForDelete();
                }

                yield return new WaitForSeconds(_frequency);
            }
        }
        
        private IEnumerator CalculateAroundChunks(Vector2Int currentChunk)
        {
            _dataChunks.DisablingVisibility();
            for (int x = -_distanceVisibility + currentChunk.x; x <= _distanceVisibility + currentChunk.x; x++)
            {
                for (int y = -_distanceVisibility + currentChunk.y; y <= _distanceVisibility + currentChunk.y; y++)
                {
                    var key = new Vector2Int(x, y);
                    if (_dataChunks.ContainsKey(key))
                    {
                        _dataChunks.GetChunk(key).VisibleChunk = true;
                        continue;
                    }

                    var position = new Vector3(x * _currentChunk.ChunkSize, 0, y * _currentChunk.ChunkSize);
   
                    _dataChunks.Add(SpawnNewChunks(position));
                    
                    yield return null;
                }
                yield return null;
            }
        }
        
        private Chunk SpawnNewChunks(Vector3 chunkPosition)
        {
            var rndIndexOfChunk = Random.Range(0, _pools.Length);
            var newChunk = (Chunk)_pools[rndIndexOfChunk].SpawnObject();
            newChunk.gameObject.SetActive(true);
            newChunk.transform.position = chunkPosition;
            newChunk.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360) / 90 * 90, 0);
            newChunk.VisibleChunk = true;
            newChunk.Id = rndIndexOfChunk;
            return newChunk;
        }
        
        private void CheckChunksForDelete()
        {
            var invisibles = _dataChunks.GetInvisibles(true);

            for (int i = 0; i < invisibles.Count; i++)
            {
                var id = invisibles[i].Id;
                invisibles[i].Reset();
                _pools[id].ReturnToPool(invisibles[i]);
            }
        }
    }
}