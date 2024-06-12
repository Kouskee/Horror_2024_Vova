using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasSystem : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private GameObject _menu;
    [Space]
    [SerializeField] private Button BackInGameBtn;
    [SerializeField] private Button BackInMenuBtn;

    private void Start()
    {
        BackInGameBtn.onClick.AddListener(BackInGame);
        BackInMenuBtn.onClick.AddListener(BackInMenu);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!_menu.active)
            {
                _menu.SetActive(true);
                _playerController.CanMove = false;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

            }
            else
            {
                BackInGame();
            }
        }
    }

    private void BackInGame()
    {
        _menu.SetActive(false);
        _playerController.CanMove = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void BackInMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}