/* Author: Loh Shau Ern Shaun & Jaykin Lee
 * Date: 04/02/2025
 * Desc:
 * - Store player database details
 * - Check and change highscore if needed
 * - Persists to other scenes
 * - Will not make another copy of this object if it already exists
 */
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;

public class LeaderboardDetails : MonoBehaviour
{
    // Reference to each text field
    public TextMeshProUGUI[] enterPlayerNames;
    public TextMeshProUGUI[] enterHighscore;

    // When scene is started
    void Awake()
    {
        // Find the account database with uid
        FirebaseDatabase.DefaultInstance
            .GetReference("players")
            .OrderByChild("highScore") // Order by highscore 
            .GetValueAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("Sorry, there was an error! ERROR: " + task.Exception);
                    return; // exit from attempt
                }

                if (!task.IsCompletedSuccessfully)
                {
                    Debug.Log("Unable to retrieve data!");
                    return;
                }

                // start retrieving values and printout
                DataSnapshot snapshot = task.Result;
                // Initialize player data list
                List<PlayerData> playerDataList = new List<PlayerData>();

                if (snapshot.Exists)
                {
                    // Look thru each existing Json for the acc
                    foreach (DataSnapshot ds in snapshot.Children)
                    {
                        // Set account path reference
                        var json = ds.GetRawJsonValue();

                        // Store json in list obj
                        PlayerData player = JsonUtility.FromJson<PlayerData>(json);
                        playerDataList.Add(player);
                    }
                }

                // Sort by highscore in descending order
                playerDataList = playerDataList.OrderByDescending(player => player.highscore).ToList();

                // List to fill in top 5 places
                for (int i = 0; i < 5; i++)
                {
                    // Player obj data list reference
                    var currentPlayerObj = playerDataList[i];
                    Debug.LogFormat("UID: {0}, DisplayName: {1}, Highscore: {2}", currentPlayerObj.uid, currentPlayerObj.username, currentPlayerObj.highscore);

                    // Fill in details of leaderboard
                    enterPlayerNames[i].text = currentPlayerObj.username;
                    enterHighscore[i].text = currentPlayerObj.highscore.ToString();
                }
            });
    }
}
