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
    public GameObject learnPanel;
    public GameObject trexPanel;
    public GameObject ovirPanel;
    public GameObject brachioPanel;
    public void GoQuizScene()
    {
        Debug.Log("Going to quiz scene!");
        SceneManager.LoadScene("QuizScene");
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

    // Show the learn panel
    public void SetLearnPanelActive()
    {
        Debug.Log("Showing learn panel!");
        ActivatePanel(learnPanel);
    }

    // Show T-rex panel
    public void SetTrexPanelActive()
    {
        ActivatePanel(settingsPanel);
    }
    public void SetOvirPanelActive()
    {
        ActivatePanel(ovirPanel);
    }
    public void SetBrachioPanelActive()
    {
        ActivatePanel(brachioPanel);
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

        // Activate the selected panel
        panelToActivate.SetActive(true);
    }
}