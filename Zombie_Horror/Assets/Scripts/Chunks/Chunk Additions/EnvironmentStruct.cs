using UnityEngine;

namespace Chunks.Chunk_Additions
{
    public readonly struct EnvironmentStruct
    {
        public int Id { get;}
        public GameObject GameObject { get;}

        public EnvironmentStruct(int id, GameObject gameObject)
        {
            Id = id;
            GameObject = gameObject;
        }
    }
}