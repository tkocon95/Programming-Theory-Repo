using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set;}

    public PlayerInfo highScorePlayer;

    public PlayerInfo CurrentPlayer {get; private set;}

    [Header("Save Settings")]
    [SerializeField] private string fileName = "resources.json";

    private string path;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        path = Path.Combine(Application.persistentDataPath, fileName);

        highScorePlayer = GetHighScore();
    }

    [Serializable]
    public class PlayerInfo
    {
        public string name;
        public int score;
    }

    [Serializable]
    class SaveData
    {
        public List<PlayerInfo> players = new();
    }

    private List<PlayerInfo> LoadPlayers()
    {
        if (!File.Exists(path))
            return null;

        string json = File.ReadAllText(path);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        if (data == null || data.players == null || data.players.Count == 0)
            return null;

        return data.players;
    }

    public void SaveGame(string pname = "", int pscore = 0)
    {
        if (string.IsNullOrWhiteSpace(pname))
            return;
        
        List<PlayerInfo> players = LoadPlayers();

        if (players == null)
            players = new List<PlayerInfo>();

        PlayerInfo player = players.Find(p => p.name == pname);

        if (player != null)
        {
            // Chỉ cập nhật nếu điểm mới cao hơn
            if (pscore > player.score)
            {
                player.score = pscore;
            }
            CurrentPlayer = player;
        }
        else
        {
            CurrentPlayer = new PlayerInfo {
                name = pname,
                score = pscore
            };

            players.Add(CurrentPlayer);
        }

        // Lưu danh sách xuống JSON
        SaveData data = new SaveData{players = players};
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(path, json);
    }

    public PlayerInfo GetPlayer(string playerName = "")
    {
        if (string.IsNullOrEmpty(playerName))
            return null;

        List<PlayerInfo> players = LoadPlayers();

        if (players == null)
            return null;

        return players.Find(p => p.name == playerName);
    }

    public PlayerInfo GetHighScore()
    {
        List<PlayerInfo> players = LoadPlayers();

        if (players == null || players.Count == 0)
            return null;

        PlayerInfo bestPlayer = players[0];

        foreach (PlayerInfo player in players)
        {
            if (player.score > bestPlayer.score)
            {
                bestPlayer = player;
            }
        }

        return bestPlayer;
    }

    public string GetLeaderBoard(int topCount = 5)
    {
        List<PlayerInfo> players = LoadPlayers();

        if (players == null || players.Count == 0)
            return "";

        var topPlayers = players.OrderByDescending(p => p.score).Take(topCount).ToList();
        string result = "";
        for (int i = 0; i < topPlayers.Count; i++)
        {
            result += $"{i + 1}. {topPlayers[i].name} - {topPlayers[i].score}\n";
        }

        return result.TrimEnd();
    }
}
