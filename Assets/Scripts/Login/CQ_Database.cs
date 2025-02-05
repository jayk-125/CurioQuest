/* Author: Loh Shau Ern Shaun
 * Date: 04/02/2025
 * Desc:
 * - Handle player registering account
 * - Create a new database for new acc
 * - After registering for new acc, go back to login page
 * 
 * - Handle player logging in
 * - Find corresponding acc database
 * - Store all data in AccountManager
 * - After logging in, go to next scene
 * 
 * - Handle player forgot password
 * - Player input acc email
 * - After clicking button, send reset password to given email
 * 
 * - Data is stored in AccountManager script
 * - All input fields are cleared whenever moving to different page
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Database;
using TMPro;

public class CQ_Database : MonoBehaviour
{
    // Reference all input fields
    // Sign In UI input fields 
    public TMP_InputField emailS;
    public TMP_InputField passwordS;
    public TMP_InputField usernameS;
    // Login UI input fields
    public TMP_InputField emailL;
    public TMP_InputField passwordL;
    // Forgot Password UI input fields
    public TMP_InputField emailF;

    // Reference the canvases
    // Sign up canvas
    public GameObject signupCanvas;
    // Login canvas
    public GameObject loginCanvas;
    // Forgot Password canvas
    public GameObject fPasswordCanvas;

    // Firebase Database + Authentication references
    // Database ref
    DatabaseReference dbDataRef;
    // Auth ref
    Firebase.Auth.FirebaseAuth dbAuthRef;
    // Player path ref
    DatabaseReference playerRef;

    // When the scene is started
    void Awake()
    {
        // Initialize firebase stuff
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            Firebase.DependencyStatus dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Firebase is ready to use
                dbAuthRef = FirebaseAuth.DefaultInstance;
                dbDataRef = FirebaseDatabase.DefaultInstance.RootReference;
                playerRef = FirebaseDatabase.DefaultInstance.GetReference("players");

                if (dbAuthRef == null)
                {
                    Debug.LogError("Firebase Authentication reference (dbAuthRef) is null!");
                    return;
                }

            }
            else
            {
                Debug.LogError($"Firebase dependencies are not met: {dependencyStatus}");
            }
        });

        // Start in login page
        loginCanvas.SetActive(true);
        signupCanvas.SetActive(false);
        fPasswordCanvas.SetActive(false);
    }

    // When user signs up a new acc
    public void SignUpUser()
    {
        // Get email and password from input fields
        string email = emailS.text.Trim();
        string password = passwordS.text.Trim();
        Debug.LogFormat("input fields obtained {0}, {1}",email, password);
        // pass user info to the firebase project
        // attempts to create a new user / check if alr exists
        dbAuthRef
            .CreateUserWithEmailAndPasswordAsync(email, password)
            .ContinueWithOnMainThread(task =>
            {
                // perform task handling
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

                // Insert post-sign up actions here
                // Get reference of authentication acc
                Firebase.Auth.AuthResult result = task.Result;
                // Make variable that calls acc user id
                var playerUID = result.User.UserId;
                Debug.LogFormat("Welcome to curio quest, {0}!", playerUID);

                // Get display name variable
                string displayName = usernameS.text.Trim();
                // Create database based on email provided
                CreatePlayerDatabase(playerUID, displayName,0);

                // Switch to login canvas after creating account
                signupCanvas.SetActive(false);
                loginCanvas.SetActive(true);
            });
    }

    // When user is created, create corresponding account database 
    void CreatePlayerDatabase(string uidNew, string displayName,int highscore)
    {
        Debug.Log("Creating database...");
        // Get constructor from PlayerData
        PlayerData pData = new PlayerData(uidNew, displayName, highscore);
        // Convert pData to a json
        string json = JsonUtility.ToJson(pData);

        // Set database with unique acc uid
        playerRef
            .Child(uidNew)
            .SetRawJsonValueAsync(json)
            .ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("Failed to set JSON data: " + task.Exception);
                }
                else if (task.IsCompletedSuccessfully)
                {
                    Debug.Log("Data successfully written to database.");
                }

                Debug.LogFormat("UID: {0}, DisplayName: {1}, Highscore: {2}", uidNew, displayName, highscore);
                Debug.Log("Database created!");
            });
    }

    // When logging in user
    public void LoginUser()
    {
        // Get email and password from input fields
        string email = emailL.text.Trim();
        string password = passwordL.text.Trim();

        // pass user info to the firebase project
        // attempts to create a new user / check if alr exists
        dbAuthRef
            .SignInWithEmailAndPasswordAsync(email, password)
            .ContinueWith(task =>
            {
                // perform task handling
                if (task.IsFaulted)
                {
                    Debug.LogError("Sorry, there was an error! ERROR: " + task.Exception);
                    return; // exit from attempt
                }
                else if (!task.IsCompletedSuccessfully)
                {
                    Debug.Log("Unable to login user!");
                    return;
                }

                // Insert post-sign up actions here
                // Get reference of authentication acc
                Firebase.Auth.AuthResult result = task.Result;
                // Make variable that calls acc user id
                var playerUID = result.User.UserId;
                Debug.LogFormat("Welcome back, {0}!", playerUID);

                // Find entered player database based on email
                FindPlayerDatabase(playerUID);

                // Go to the main scene page
                //EnterMainScene();
            });
    }

    // When user account is verifed, find corresponding account database 
    void FindPlayerDatabase(string uidCompare)
    {
        Debug.Log("Finding database...");

        // Find the account database with uid
        playerRef
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

                // Initialize data variables
                string uidSave = "";
                string displayName = "";
                int highScore = 0;

                // start retrieving values and printout
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    // Look thru each existing Json for the acc
                    foreach (DataSnapshot ds in snapshot.Children)
                    {
                        // Set account path reference
                        PlayerData accPath = JsonUtility.FromJson<PlayerData>(ds.GetRawJsonValue());

                        if (uidCompare == accPath.uid)
                        {
                            Debug.Log("Data found!");
                            // Insert post account login here:
                            // Retrieve data from servers
                            uidSave = accPath.uid;
                            displayName = accPath.username;
                            highScore = accPath.highscore;

                            Debug.LogFormat("UID: {0}, DisplayName: {1}, Highscore: {2}", uidSave, displayName, highScore);

                            // Save details in AccountManager
                            CopyToAccManager(uidSave, displayName, highScore);

                            // After finding player database, end loop
                            break;
                        }
                        else
                        {
                            Debug.Log("No data was found in account!");
                        }
                    }
                }
            });
    }

    // Send the forgot password email
    public void ForgotPasswordSend()
    {
        // Get email from input fields
        string email = emailF.text.Trim();

        // Send password reset email
        dbAuthRef
            .SendPasswordResetEmailAsync(email)
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

                Debug.Log("Password reset email has been sent successfully!");
            });
    }

    // When called, save to the account manager object
    void CopyToAccManager(string uid, string displayName, int highScore)
    {
        // Get reference to Account Manager obj
        AccountManager accManager = GameObject.Find("/AccountManager").GetComponent<AccountManager>();
        // Save the data to the Account Manager obj
        accManager.SetPlayerData(uid, displayName, highScore);

        Debug.Log("Details copied");
    }

    // When called, change scene to the main scene
    void EnterMainScene()
    {
        // Set scene variable 
        Scene scene = SceneManager.GetActiveScene();
        // Open the menu scene
        SceneManager.LoadScene(1);
    }

    // Clear all the input fields
    public void ClearAllInputFields()
    {
        // Clear login input fields
        emailL.text = "";
        passwordL.text = "";
        // Clear registration input fields
        emailS.text = "";
        passwordS.text = "";
        usernameS.text = "";
        // Clear forgot password input fields
        emailF.text = "";
    }
}
