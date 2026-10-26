using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public InputAction pauseAction;

    private GameManager gameManager;
    private bool isPaused = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameManager.Instance;
        pauseAction.Enable();
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseAction.WasPressedThisFrame() && gameManager.GameActive)
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
    }

    void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }

    void OnDestroy()
    {
        pauseAction.Disable();
    }
}