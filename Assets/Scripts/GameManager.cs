using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [Header("UI")]
    public TextMeshProUGUI remainBallText;
    public TextMeshProUGUI totalPointText;

    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverText;
    public GameObject difficultyPanel;

    private ObjectManager objManager;
    private bool isGameActive = false;
    public bool GameActive => isGameActive;

    private int totalPoint = 0;

    void Awake()
    {
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    public void StartGame(int numberOfBall)
    {
        objManager = ObjectManager.Instance;

        if (objManager == null)
        {
            Debug.LogError("Không tìm thấy ObjectManager!");
            return;
        }

        // Tạo số Ball theo lựa chọn của người chơi
        objManager.InitializePool(numberOfBall);

        totalPoint = 0;
        isGameActive = true;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (difficultyPanel != null)
            difficultyPanel.SetActive(false);

        UpdateBall();
        UpdatePointUI();
    }

    void Update()
    {
        if (!isGameActive || objManager == null)
            return;

        if (objManager.GetRemainObj() <= 0 && objManager.AreAllObjectsInactive())
            SetGameOver();
    }

    public void UpdateBall()
    {
        if (objManager == null)
            return;
        //if (remainBall <= 0)
        //    isGameActive = false;
        remainBallText.text = "Remaining: " + objManager.GetRemainObj().ToString();
    }

    public void AddPoint(int points)
    {
        if (!isGameActive)
            return;

        totalPoint += points;
        UpdatePointUI();

        //Debug.Log("Total Point: " + totalPoint);
    }

    private void UpdatePointUI()
    {
        if (totalPointText != null)
            totalPointText.text = "Points: " + totalPoint;
    }

    private void SetGameOver()
    {
        if (!isGameActive)
            return;
        gameOverText.text = "You scored " + totalPoint.ToString() + " points";
        gameOverPanel.SetActive(true);
        isGameActive = false;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
