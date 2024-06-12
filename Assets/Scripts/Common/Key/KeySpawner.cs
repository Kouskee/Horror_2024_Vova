using UnityEngine;

public class KeySpawner : MonoBehaviour
{
    [SerializeField] private Transform _key;
    [SerializeField] private Transform[] _spawnPoints;

    void Start()
    {
        var point = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
        _key.SetPositionAndRotation(point.position, point.rotation);
    }
}