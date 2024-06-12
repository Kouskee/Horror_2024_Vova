using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class HorrorTrigger : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _clips;
    [SerializeField] private Collider _collider;
    [Space]
    [SerializeField] private Vector2 _screamerDelayRange = new (0,0);
    [SerializeField] private float _screamerDuration = 2;
    [Space]
    public UnityEvent SreamerEvent;
    public UnityEvent SreamerEndEvent;

    private void Start()
    {
        var rnd = Random.Range(0f, 1f);
        if (rnd < .35f)
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        _collider.enabled = false;
        StartCoroutine(StartScreamerWithDelay());
    }

    private IEnumerator StartScreamerWithDelay()
    {
        yield return new WaitForSeconds(Random.Range(_screamerDelayRange.x, _screamerDelayRange.y));

        if (_clips.Length > 1)
            _audioSource.clip = _clips[Random.Range(0, _clips.Length)];
        _audioSource.Play();
        SreamerEvent.Invoke();

        StartCoroutine(StopScreamer());
    }

    private IEnumerator StopScreamer()
    {
        yield return new WaitForSeconds(_screamerDuration);

        _audioSource.Stop();
        SreamerEndEvent.Invoke();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}