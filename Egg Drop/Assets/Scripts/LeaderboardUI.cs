using UnityEngine;
using UnityEngine.UI; // Add this line

public class LeaderboardUI : MonoBehaviour
{
    public GameObject leaderboardEntryPrefab;
    public Transform leaderboardContent;

    public void UpdateLeaderboardUI(LeaderboardData leaderboard)
    {
        foreach (Transform child in leaderboardContent)
        {
            Destroy(child.gameObject);
        }

        foreach (var entry in leaderboard.entries)
        {
            GameObject entryObj = Instantiate(leaderboardEntryPrefab, leaderboardContent);
            Text[] texts = entryObj.GetComponentsInChildren<Text>();
            texts[0].text = entry.username;
            texts[1].text = entry.score.ToString();
        }
    }
}

[System.Serializable]
public class LeaderboardData
{
    public LeaderboardEntry[] entries;
}

[System.Serializable]
public class LeaderboardEntry
{
    public string username;
    public int score;
}