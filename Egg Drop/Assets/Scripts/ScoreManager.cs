using UnityEngine.Networking;
using System.Collections;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public void RecordScore(int score)
    {
        StartCoroutine(UploadScore(Social.localUser.userName, score));
    }

    private IEnumerator UploadScore(string username, int score)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("score", score);

        UnityWebRequest www = UnityWebRequest.Post("dLpkl3mUS1anquJ9wQfRJa6LZDYIIhge1mSbumR3", form);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Score submitted successfully");
        }
        else
        {
            Debug.Log("Error submitting score: " + www.error);
        }
    }
}