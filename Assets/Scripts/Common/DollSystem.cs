using UnityEngine;

public class DollSystem : MonoBehaviour
{
    [SerializeField] private HorrorTrigger trigger;
    [SerializeField] private Animator _animator;
    [Space]
    [SerializeField] private Transform _doll;
    [SerializeField] private Transform[] _spawnPoints;

    private void Start()
    {
        var point = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
        _doll.SetPositionAndRotation(point.position, point.rotation);

        trigger.SreamerEvent.AddListener(() => _animator.SetTrigger(Random.Range(1, 4).ToString()));
    }
}