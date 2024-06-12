using System;
using System.Collections.Generic;
using Chunks.Chunk_Additions;
using UnityEngine;

namespace Chunks
{
    public class DataChunks
    {
        private static readonly Dictionary<Vector2Int, Chunk> SpawnedChunks = new Dictionary<Vector2Int, Chunk>(10);

        public DataChunks(Chunk startChunk)
        {
            startChunk.VisibleChunk = true;
            SpawnedChunks.Clear();
            SpawnedChunks.Add(startChunk.GetChunkNumber(), startChunk);
        }

        public void Add(Chunk newChunk)
        {
            newChunk.VisibleChunk = true;
            SpawnedChunks.Add(newChunk.GetChunkNumber(), newChunk);
        }

        public Chunk GetChunk(Vector2Int numberChunk)
        {
            if (SpawnedChunks.ContainsKey(numberChunk))
                return SpawnedChunks[numberChunk];
            throw new Exception("There is no chunk under this number");
        }

        public bool ContainsKey(Vector2Int key) => SpawnedChunks.ContainsKey(key);

        public void DisablingVisibility()
        {
            foreach (var spawnedChunk in SpawnedChunks)
            {
                spawnedChunk.Value.VisibleChunk = false;
            }
        }
        
        public List<Chunk> GetInvisibles(bool beingDeleted)
        {
            var invisibles = new List<Chunk>();
            var keys = new List<Vector2Int>();
            foreach (var (key, chunk) in SpawnedChunks)
            {
                if (chunk.VisibleChunk) continue;
                invisibles.Add(chunk);
                keys.Add(key);
            }

            if (beingDeleted) DeleteFromList(keys);
        
            return invisibles;
        }

        private void DeleteFromList(List<Vector2Int> keys)
        {
            foreach (var key in keys)
            {
                SpawnedChunks.Remove(key);
            }
        }
    }
}