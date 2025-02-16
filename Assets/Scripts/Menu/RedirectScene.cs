/* Author: Loh Shau Ern Shaun & Arwen Josephine Loh
 * Date: 08/02/2025
 * Desc:
 * - Change between scenes and panels with this code 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RedirectScene : MonoBehaviour
{
    public GameObject leaderboardPanel;
    public GameObject profilePanel;
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;

    // Go to the quiz scene
    public void GoQuizScene()
    {
        Debug.Log("Going to quiz scene!");
        SceneManager.LoadScene("QuizScene");
    }

    // Go to the learn scene
    public void GoLearnScene()
    {
        Debug.Log("Going to learn scene!");
        SceneManager.LoadScene("LearnScene");
    }

    // Return to the main scene
    public void GoMainScene()
    {
        Debug.Log("Going to main scene!");
        SceneManager.LoadScene("MenuScene");
    }


    // Show the profile panel
    public void SetProfilePanelActive()
    {
        Debug.Log("Showing profile panel!");
        ActivatePanel(profilePanel);
    }

    // Show the leaderboard panel
    public void SetLeaderboardPanelActive()
    {
        Debug.Log("Showing leaderboard panel!");
        ActivatePanel(leaderboardPanel);
    }

    // Show the settings panel
    public void SetSettingsPanelActive()
    {
        Debug.Log("Showing settings panel!");
        ActivatePanel(settingsPanel);
    }

    // Return to the main menu panel
    public void ReturnToMainMenuPanel()
    {
        Debug.Log("Returning to main menu!");
        ActivatePanel(mainMenuPanel);
    }

    // Logout and go back to login scene
    public void LogOut()
    {
        Debug.Log("Logging Out!");
        SceneManager.LoadScene("LoginScene");
    }

    // function to switch panels
    private void ActivatePanel(GameObject panelToActivate)
    {
        // Deactivate all panels
        leaderboardPanel.SetActive(false);
        profilePanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);

        // Activate the selected panel
        panelToActivate.SetActive(true);
    }
}