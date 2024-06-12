using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField, Range(0.1f, 10)] private float move;
    [SerializeField] private float frequency;

    void Start()
    {
        //StartCoroutine(Move());
        StartCoroutine(Rotate());
    }

    IEnumerator Move()
    {
        while (true)
        {
            yield return new WaitForSeconds(frequency);
            transform.position += new Vector3(0, 0, move);
        }
    }

    IEnumerator Rotate()
    {
        var count = 0f;
        while (true)
        {
            transform.rotation = Quaternion.Euler(0, count, 0);
            count += move;
            yield return new WaitForEndOfFrame(); 
        }
    }
}