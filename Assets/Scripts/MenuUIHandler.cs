using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuUIHandler : MonoBehaviour
{
    [Header("UI")]
    public TMP_InputField playerNameInput;
    public TextMeshProUGUI errorText;

    private ResourceManager rm;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
         rm = ResourceManager.Instance;
    }

    public void StartNew()
    {
        string playerName = playerNameInput.text.Trim();
        if (string.IsNullOrEmpty(playerName))
        {
            errorText.gameObject.SetActive(true);
            errorText.text = "Error: Please enter your name!";
            playerNameInput.ActivateInputField();
            return;
        }
        rm.SaveGame(playerName);
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
#if UNITY_EDITOR
            //Don't forget since this use editor code, need to add "using UnityEditor" at the top and wrap it between #if
            EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
    }
}
