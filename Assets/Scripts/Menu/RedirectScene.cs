/* Author: Loh Shau Ern Shaun
 * Date: 08/02/2025
 * Desc:
 * - Change between scenes with this code
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RedirectScene : MonoBehaviour
{
    // Load the learning scene
    public void GoLearnScene()
    {
        Debug.Log("Going to learn scene!");
        //SceneManager.LoadScene("LearnScene");
    }

    // Load the learning scene
    public void GoQuizScene()
    {
        Debug.Log("Going to quiz scene!");
        SceneManager.LoadScene("QuizScene");
    }

    // Load the profile scene
    public void GoProfileScene()
    {
        Debug.Log("Going to profile scene!");
        SceneManager.LoadScene("ProfileScene");
    }

    // Load the learning scene
    public void GoLeaderboardScene()
    {
        Debug.Log("Going to leaderboard scene!");
        SceneManager.LoadScene("LeaderboardScene");
    }

    // Return to the login scene
    public void LogOut()
    {
        Debug.Log("Logging Out!");
        SceneManager.LoadScene("LoginScene");
    }

    // Return to main menu scene
    public void GoMainMenuScene()
    {
        Debug.Log("Going to main menu scene!");
        SceneManager.LoadScene("MenuScene");
    }
}
