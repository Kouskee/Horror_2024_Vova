using System.Collections;
using UnityEngine;

public class KeySystem : MonoBehaviour
{
    [SerializeField] private GameObject _text;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Inventory inventory))
        {
            _text.SetActive(true);
            StartCoroutine(AwaitButton(inventory));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _text.SetActive(false);
    }

    private IEnumerator AwaitButton(Inventory inventory)
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                PickUpKey(inventory);
                break;
            }

            yield return null;
        }
    }

    private void PickUpKey(Inventory inventory)
    {
        _text.SetActive(false);
        inventory.PickUpKey();
        Destroy(this);
        transform.position = new Vector3(0,-100, 0);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
