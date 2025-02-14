/* Author: Loh Shau Ern Shaun
 * Date: 08/02/2025
 * Desc:
 * - Get stored username from acc manager
 * - Set the menu scene with player name
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnterText : MonoBehaviour
{
    // Reference to welcome text object
    public TextMeshProUGUI welcomeText;

    // When scene is entered
    void Awake()
    {
        // Get reference to Account Manager obj
        AccountManager accManager = GameObject.Find("/AccountManager").GetComponent<AccountManager>();
        // When acc manager is found
        if (accManager != null)
        {
            // Set welcome text
            welcomeText.text = "Hello " + accManager.currentName + "!";
        }
    }
}
