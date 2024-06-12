using System;
using UnityEngine;

namespace Chunks.Chunk_Additions
{
    [CreateAssetMenu(fileName = "Objects_Config", menuName = "Chunk Additions", order = 0)]
    public class ObjectsOnChunkConfig : ScriptableObject
    {
        [SerializeField, Range(1, 10)] private int _numberObjects = 1;
        [SerializeField, Range(1, 10)] private int _startingPoolCapacity = 1;
        [SerializeField] private ObjectConfig[] _objectConfigs;

        public int NumberObjects => _numberObjects;
        public int StartingPoolCapacity => _startingPoolCapacity;
        public ObjectConfig[] ObjectConfigs => _objectConfigs;
    }
    
    [Serializable]
    public class ObjectConfig
    {
        [SerializeField] private Transform _object;
        [SerializeField] private float _voxelSize = 0.1f;
        [SerializeField] private int _numberEdgeVoxels = 10;
        [SerializeField, Range(1, 100)] private int _chanceSpawn = 50;
        
        public Transform Object => _object;
        public float VoxelSize => _voxelSize;
        public int ObjectSize => _numberEdgeVoxels / 2;
        public float Height => _voxelSize;
        public int ChanceSpawn => _chanceSpawn;
    }
}