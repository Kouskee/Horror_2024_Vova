using System.Collections;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    [SerializeField] private GameObject _light;
    [SerializeField] private Vector2 _rangeDelay = new(0.01f, 1f);
    [SerializeField] private Vector2 _rangeLighting = new(0.05f, 0.5f);

    private void Start()
    {
        StartCoroutine(Flicker());
    }

    private IEnumerator Flicker()
    {
        while (true)
        {
            _light.SetActive(false);
            yield return new WaitForSeconds(Random.Range(_rangeLighting.x, _rangeLighting.y));
            _light.SetActive(true);
            yield return new WaitForSeconds(Random.Range(_rangeDelay.x, _rangeDelay.y));
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}