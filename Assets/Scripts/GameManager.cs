using System;
using TMPro;
using Unity.VisualScripting;
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

    public TextMeshProUGUI welcomeText;
    public TextMeshProUGUI highScoreText;

    public GameObject leaderboardPanel;
    public TextMeshProUGUI congratsText;

    private ResourceManager rm;
    private ObjectManager objManager;

    private bool isGameActive = false;
    public bool GameActive => isGameActive;

    private int totalPoint = 0;

    public string CurrentPlayerName { get; private set; }

    void Awake()
    {
        Instance = this;
        rm = ResourceManager.Instance;
        if (rm == null)
        {
            SceneManager.LoadScene(0);
            return;
        }
    }

    void Start()
    {
        // Get current PlayerScore
        CurrentPlayerName = rm.CurrentPlayer.name;

        if (!String.IsNullOrEmpty(CurrentPlayerName)) {
            welcomeText.text = "Welcome, " + CurrentPlayerName;
            int playerHighScore = rm.CurrentPlayer.score;
            if (playerHighScore > 0) {
                highScoreText.gameObject.SetActive(true);
                highScoreText.text = $"High Score : {playerHighScore}";
            }
        }
        else
            welcomeText.text = "Welcome";
    }

    public void StartGame(int numberOfBall)
    {
        objManager = ObjectManager.Instance;

        if (objManager == null)
            return;

        if (rm == null)
            return;

        // Tạo số Ball theo lựa chọn của người chơi
        objManager.InitializePool(numberOfBall);

        totalPoint = 0;
        isGameActive = true;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (difficultyPanel != null)
            difficultyPanel.SetActive(false);

        UpdateLeaderBoardUI();
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

    private void UpdateLeaderBoardUI()
    {
        if (leaderboardPanel != null)
        {
            string leaderBoardText = rm.GetLeaderBoard();
            if (!string.IsNullOrEmpty(leaderBoardText))
            {
                leaderboardPanel.gameObject.SetActive(true);
                Transform listTransform = leaderboardPanel.transform.Find("LeaderBoard List");
                if (listTransform != null)
                {
                    TMP_Text listText = listTransform.GetComponent<TMP_Text>();

                    if (listText != null)
                    {
                        listText.text = leaderBoardText;
                    }
                }
            }
        }
    }

    private void UpdatePointUI()
    {
        if (totalPointText != null) {
            if (!totalPointText.gameObject.activeSelf)
                totalPointText.gameObject.SetActive(true);

            totalPointText.text = "Points: " + totalPoint;
        }
    }

    private void SetGameOver()
    {
        if (!isGameActive)
            return;
        int highScore = rm.CurrentPlayer.score;
        if (totalPoint > highScore) 
        {
            congratsText.text = "New record!!!!";
            gameOverText.text = "You scored " + totalPoint.ToString() + " points";
        }
        else
        {
            congratsText.text = "Try Harder T_T";
            gameOverText.text = "Your high score: " + highScore + " points";
        }
        rm.SaveGame(CurrentPlayerName, totalPoint);
        gameOverPanel.SetActive(true);
        isGameActive = false;
    }
}
