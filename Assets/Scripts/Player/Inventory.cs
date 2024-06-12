using UnityEngine;

public class Inventory : MonoBehaviour
{
    private bool _hasKey;
    public bool HasKey => _hasKey;

    public void PickUpKey()
    {
        _hasKey = true;
    }
}
