using System.Collections;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] private GameObject _text;
    [SerializeField] private Animation _animation;
    [Space]
    [SerializeField] private bool _needKey = false;

    private void OnTriggerEnter(Collider other)
    {
        if (_needKey)
        {
            if (other.TryGetComponent(out Inventory inventory))
            {
                if (!inventory.HasKey)
                    return;
            }
        }

        _text.SetActive(true);
        StartCoroutine(AwaitButton());
    }

    private void OnTriggerExit(Collider other)
    {
        _text.SetActive(false);
    }

    private IEnumerator AwaitButton()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                OpenTheDoor();
                break;
            }

            yield return null;
        }
    }

    private void OpenTheDoor()
    {
        _text.SetActive(false);
        _animation.Play();
        Destroy(this);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}