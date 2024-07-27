using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class LeaderboardFetch : MonoBehaviour
{
    public void GetLeaderboard()
    {
        StartCoroutine(DownloadLeaderboard());
    }

    private IEnumerator DownloadLeaderboard()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://<your-api-id>.execute-api.<region>.amazonaws.com/<stage>/leaderboard");
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            // Parse and display the leaderboard data
            Debug.Log("Leaderboard data: " + www.downloadHandler.text);
            DisplayLeaderboard(www.downloadHandler.text);
        }
        else
        {
            Debug.Log("Error retrieving leaderboard: " + www.error);
        }
    }

    private void DisplayLeaderboard(string json)
    {
        // Parse the JSON and display it in the UI
        // Example using JSONUtility
        LeaderboardData leaderboard = JsonUtility.FromJson<LeaderboardData>(json);
        foreach (var entry in leaderboard.entries)
        {
            Debug.Log(entry.username + ": " + entry.score);
            // Update your UI elements here
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