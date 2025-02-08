/* Author: Loh Shau Ern Shaun & Jonathan Low Jerome Enting
 * Date: 04/02/2025
 * Desc:
 * - Store player database details
 * - Check and change highscore if needed
 * - Persists to other scenes
 * - Will not make another copy of this object if it already exists
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;

public class AccountManager : MonoBehaviour
{
    // Stored data variables
    public string currentUID;
    public string currentName;
    public int storedHighscore;

    // Make instance
    public static AccountManager instance;

    // Bring this object & script across to different scenes
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }

    }

    // Set the player data when function is called
    public void SetPlayerData(string uid, string displayName, int highScore)
    {
        currentUID = uid;
        currentName = displayName;
        storedHighscore = highScore;
    }

    // Check the current highest score
    public void CheckPlayerHighscore(int currentHighscore)
    {
        // If stored highscore is less than current highscore
        if (storedHighscore < currentHighscore)
        {
            // Change accManager stored highscore
            storedHighscore =  currentHighscore;

            // Find the account database with uid
            FirebaseDatabase.DefaultInstance
                .GetReference("players")
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
                        Debug.Log("Unable to create user!");
                        return;
                    }

                    // start retrieving values and printout
                    DataSnapshot snapshot = task.Result;
                    if (snapshot.Exists)
                    {
                        // Look thru each existing Json for the acc
                        foreach (DataSnapshot ds in snapshot.Children)
                        {
                            // Set account path reference
                            PlayerData accPath = JsonUtility.FromJson<PlayerData>(ds.GetRawJsonValue());
                            // If current UID is the same as the UID being checked
                            if (currentUID == accPath.uid)
                            {
                                Debug.Log("Data found!");
                                // Insert post account login here:
                                // Get path referencing found existing name
                                var playerReference = FirebaseDatabase
                                    .DefaultInstance
                                    .RootReference
                                    .Child("players")
                                    .Child(currentUID);
                                // Alter stored highscore
                                var updateValues = new Dictionary<string, object>();
                                updateValues.Add("highscore", currentHighscore);
                                playerReference.UpdateChildrenAsync(updateValues);
                                Debug.Log("Uploaded highscore!");
                            }
                        }
                    }
                });
        }
        else
        {
            Debug.Log("The current score is less than the stored highscore!");
        }

    }

    // Change the player username
    public void ChangePlayerUsername(string newUsername)
    {
        // If current username is different from new username
        if (currentName != newUsername)
        {
            // Change accManager stored highscore
            currentName = newUsername;

            // Find the account database with uid
            FirebaseDatabase.DefaultInstance
                .GetReference("players")
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
                        Debug.Log("Unable to create user!");
                        return;
                    }

                    // start retrieving values and printout
                    DataSnapshot snapshot = task.Result;
                    if (snapshot.Exists)
                    {
                        // Look thru each existing Json for the acc
                        foreach (DataSnapshot ds in snapshot.Children)
                        {
                            // Set account path reference
                            PlayerData accPath = JsonUtility.FromJson<PlayerData>(ds.GetRawJsonValue());
                            // If current UID is the same as the UID being checked
                            if (currentUID == accPath.uid)
                            {
                                Debug.Log("Data found!");
                                // Insert post account login here:
                                // Get path referencing found existing name
                                var playerReference = FirebaseDatabase
                                    .DefaultInstance
                                    .RootReference
                                    .Child("players")
                                    .Child(currentUID);
                                // Alter stored highscore
                                var updateValues = new Dictionary<string, object>();
                                updateValues.Add("username", newUsername);
                                playerReference.UpdateChildrenAsync(updateValues);
                                Debug.Log("Uploaded username!");
                            }
                        }
                    }
                });
        }
        else
        {
            Debug.Log("The current score is less than the stored highscore!");
        }

    }
}
