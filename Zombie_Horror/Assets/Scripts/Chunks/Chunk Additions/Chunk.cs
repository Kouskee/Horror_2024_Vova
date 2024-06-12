using NaughtyAttributes;
using UnityEngine;

namespace Chunks.Chunk_Additions
{
    [SelectionBase]
    public class Chunk : MonoBehaviour
    {
        [SerializeField] private Vector2Int _chunkNumber;
        [SerializeField] private float _voxelSize = 0.1f;
        [SerializeField] private int _numberEdgeVoxels = 40;
        [Space] 
        [SerializeField] private bool _needCreatingObjects;
        [SerializeField, ShowIf(nameof(_needCreatingObjects))] private ObjectsOnChunkConfig _config;

        private CreatorEnvironmentChunk _creatorObjects;

        public Vector2Int GetChunkNumber()
        {
            var sizeUnit = _voxelSize * _numberEdgeVoxels;
            _chunkNumber = new Vector2Int(Mathf.RoundToInt(transform.position.x / sizeUnit),
                Mathf.RoundToInt(transform.position.z / sizeUnit));
            return _chunkNumber;
        }

        private void OnEnable()
        {
            if(!_needCreatingObjects) return;
            _creatorObjects = new CreatorEnvironmentChunk(_config, this);
        }

        public void Reset()
        {
            if(_needCreatingObjects) _creatorObjects.Reset();
            gameObject.SetActive(false);
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
            _chunkNumber = Vector2Int.zero;
        }

        public int NumberEdgeVoxels => _numberEdgeVoxels;
        public float ChunkSize => _voxelSize * _numberEdgeVoxels;
        public bool VisibleChunk { get; set; }
        public int Id { get; set; }
    }
}