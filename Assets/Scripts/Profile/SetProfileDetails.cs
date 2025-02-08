/* Author: Loh Shau Ern Shaun & Arwen Josephine Loh
 * Date: 08/02/2025
 * Desc:
 * - Set the details the profile page
 * - Allow users to change username
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetProfileDetails : MonoBehaviour
{
    // Reference to username text object
    public TextMeshProUGUI usernameText;
    // Reference to highscore text object
    public TextMeshProUGUI highscoreText;

    // Reference to change username input field
    public TMP_InputField nameChangeField;

    // Reference to player profile page
    public GameObject profilePage;
    // Reference to change username page
    public GameObject changeNamePage;

    AccountManager accManager;

    // When scene is entered
    void Awake()
    {
        // Show profile page only
        profilePage.SetActive(true);
        changeNamePage.SetActive(false);

        // Get reference to Account Manager obj
        accManager = GameObject.Find("/AccountManager").GetComponent<AccountManager>();
        // When acc manager is found
        if (accManager != null)
        {
            // Set username text
            usernameText.text = "Username:\n " + accManager.currentName;
            // Set highscore text
            highscoreText.text = "Highscore:\n " + accManager.storedHighscore;
        }
    }

    // Change the username based on provided input field
    public void UsernameChange()
    {
        // Get the new name from input field
        string newName = nameChangeField.text.Trim();
        // If new username was added and is within length range
        if (newName != null && (newName.Length > 3) && (newName.Length < 25))
        {
            // Get acc manager to change player username
            accManager.ChangePlayerUsername(newName);
            // Set username text
            usernameText.text = "Username:\n " + accManager.currentName;
        }

        // Clear the field
        ClearNameChangeField();

        // Show profile page only
        profilePage.SetActive(true);
        changeNamePage.SetActive(false);
    }

    // Clear the input field
    public void ClearNameChangeField()
    {
        // Set as empty
        nameChangeField.text = "";
    }
}
