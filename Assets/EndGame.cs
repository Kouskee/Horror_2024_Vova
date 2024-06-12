using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    [SerializeField] private GameObject _text;
    private void OnTriggerEnter(Collider other)
    {
        _text.SetActive(true);
        StartCoroutine(EndGameAsync());
    }

    IEnumerator EndGameAsync()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Menu");
    }
}
