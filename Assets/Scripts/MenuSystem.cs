using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuSystem : MonoBehaviour
{
    [SerializeField] private GameObject _loadingScreen;
    [SerializeField] private Slider _slider;
    [SerializeField] private TMP_Text _progressTxt;

    private void Awake()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void LoadScene()
    {
        StartCoroutine(LoadSceneAsync());
    }

    public void Quit()
    {
        Application.Quit();
    }

    private IEnumerator LoadSceneAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("Game");
        _loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            var progress = Mathf.Clamp01(operation.progress / .9f);
            _slider.value = progress;
            _progressTxt.text = progress * 100f + "%";

            yield return null;
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}