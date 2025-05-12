using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class VoteCountUpdater : MonoBehaviour
{
    public string nodeAppUrl = "https://commit-3e48a13ebc10.herokuapp.com/latest-data";
    public float pollInterval = 0.5f;

    private void Start()
    {
        // Start polling the Node.js app for vote count.
        StartCoroutine(PollNodeApp());
    }

    private IEnumerator PollNodeApp()
    {
        while (true)
        {
            // Send a GET request to your Node.js app.
            using (WWW www = new WWW(nodeAppUrl))
            {
                yield return www;

                if (www.error != null)
                {
                    Debug.LogError("Error fetching data: " + www.error);
                }
                else
                {
                    // Parse the response data and log the vote counts.
                    LogVoteCounts(www.text);
                }
            }

            // Wait for the next poll interval.
            yield return new WaitForSeconds(pollInterval);
        }
    }

    private void LogVoteCounts(string jsonData)
    {
        try
        {
            // Parse JSON data to get the vote counts.
            VoteData voteData = JsonUtility.FromJson<VoteData>(jsonData);

            // Log the vote counts to the Unity console.
            Debug.Log("Vote Count 1: " + voteData.button1.ToString());
            Debug.Log("Vote Count 2: " + voteData.button2.ToString());
        }
        catch (Exception e)
        {
            Debug.LogError("Error parsing JSON: " + e.Message);
        }
    }

    [Serializable]
    private class VoteData
    {
        public float button1;
        public float button2;
    }
}
