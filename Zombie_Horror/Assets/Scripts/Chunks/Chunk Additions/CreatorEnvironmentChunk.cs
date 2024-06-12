using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Chunks.Chunk_Additions
{
    public class CreatorEnvironmentChunk
    {
        private Chunk _chunk;
        private int _startingPoolCapacity;
        private int _numberObjects;
        private ObjectConfig[] _objectConfigs;

        private List<EnvironmentStruct> _objects;
        private PoolObjects[] _pools;
        private int[,] _fieldChunk;

        public CreatorEnvironmentChunk(ObjectsOnChunkConfig config, Chunk chunk)
        {
            _startingPoolCapacity = config.StartingPoolCapacity;
            _numberObjects = config.NumberObjects;
            _objectConfigs = config.ObjectConfigs;
            _chunk = chunk;
            
            Start();
        }
        
        private void Start()
        {
            _objects = new List<EnvironmentStruct>(_numberObjects);
            _fieldChunk = new int[_chunk.NumberEdgeVoxels, _chunk.NumberEdgeVoxels];
            
            _pools = new PoolObjects[_objectConfigs.Length];
            for (int i = 0; i < _pools.Length; i++)
            {
                _objectConfigs[i].Object.gameObject.SetActive(false);
                _pools[i] = new PoolObjects(_objectConfigs[i].Object, _startingPoolCapacity, _chunk.transform);
            }
            
            for (int i = 0; i < _numberObjects; i++)
            {
                var index = GetRandomObject();
                CreateObjects(index);
            }
        }

        private int GetRandomObject()
        {
            var chances = new List<int>();
            for (int i = 0; i < _objectConfigs.Length; i++)
            {
                chances.Add(_objectConfigs[i].ChanceSpawn);
            }

            var value = Random.Range(0, chances.Sum());
            var sum = 0;

            for (int i = 0; i < chances.Count; i++)
            {
                sum += chances[i];
                if (value < sum)
                    return i;
            }

            return _objectConfigs.Length - 1;
        }

        private void CreateObjects(int indexObject)
        {
            var count = 0;
            int randomX, randomY;
            while (true)
            {
                randomX = Random.Range(_objectConfigs[indexObject].ObjectSize, _fieldChunk.GetUpperBound(0) - _objectConfigs[indexObject].ObjectSize);
                randomY = Random.Range(_objectConfigs[indexObject].ObjectSize, _fieldChunk.GetUpperBound(1) - _objectConfigs[indexObject].ObjectSize);
                if(!IsPlaceTaken(randomX, randomY, indexObject)) break;
                count++;
                if(count >= _numberObjects + _objectConfigs.Length) break;
            }
            
            if(!IsPlaceTaken(randomX, randomY, indexObject)) 
                SpawnObject(randomX, randomY, indexObject);
        }
        
        private bool IsPlaceTaken(int x, int y, int indexPrefab)
        {
            for (var unitX = -_objectConfigs[indexPrefab].ObjectSize; unitX < _objectConfigs[indexPrefab].ObjectSize; unitX++)
            {
                for (int unitY = -_objectConfigs[indexPrefab].ObjectSize; unitY < _objectConfigs[indexPrefab].ObjectSize; unitY++)
                {
                    if (_fieldChunk[x + unitX, y + unitY] != 0) return true;
                }
            }

            return false;
        }

        private void SpawnObject(int x, int y, int indexPrefab)
        {
            for (var unitX = -_objectConfigs[indexPrefab].ObjectSize; unitX < _objectConfigs[indexPrefab].ObjectSize; unitX++)
            {
                for (int unitY = -_objectConfigs[indexPrefab].ObjectSize; unitY < _objectConfigs[indexPrefab].ObjectSize; unitY++)
                {
                    _fieldChunk[x + unitX, y + unitY] = 1;
                }
            }

            var newObj = (Transform)_pools[indexPrefab].SpawnObject();
            newObj.gameObject.SetActive(true);
            var localPosition = _chunk.transform.localPosition;
            newObj.position = new Vector3
            (
                GetAxis(x, localPosition.x),
                _objectConfigs[indexPrefab].Height,
                GetAxis(y, localPosition.z)
            );

            _objects.Add(new EnvironmentStruct(indexPrefab, newObj.gameObject));
                
            float GetAxis(int index, float axis)
            {
                return (float) Math.Round((_objectConfigs[indexPrefab].VoxelSize * index - _chunk.ChunkSize / 2 + axis) * 2, MidpointRounding.AwayFromZero) / 2;
            }
        }

        public void Reset()
        {
            for (int i = 0; i < _objects.Count; i++)
            {
                _objects[i].GameObject.SetActive(false);
                _pools[_objects[i].Id].ReturnToPool(_objects[i].GameObject);
            }
        }
    }
}