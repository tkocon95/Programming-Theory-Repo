using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUIHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
