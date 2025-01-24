/* Author: Loh Shau Ern Shaun
 * Date: 24/01/2025
 * Desc:
 * - Handles quiz UI pages
 * - When starting quiz, player can select location for interactive quiz objects to spawn
 * - After selecting spawn location, go to Quiz
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CalibrateQuiz : MonoBehaviour
{
    // Reference the start pos object here
    [SerializeField]
    private GameObject startPointObj;

    // Reference different pages
    public GameObject startPage;
    public GameObject calibratePage;
    public GameObject quizPage;

    // List of GameObjects spawned
    private List<GameObject> spawnedPoint = new List<GameObject>();

    // Boolean to handle whether player can set start transform
    private bool setStartPoint;

    // Reference to camera
    [SerializeField]
    private Camera mainCamera;
    // Vector3 to handle saved position point
    private Vector3 savedStartPoint;

    // Start is called before the first frame update
    void Start()
    {
        // Disallow players from setting start pos
        setStartPoint = false;

        // Show start page
        startPage.SetActive(true);
        // Hide other pages
        calibratePage.SetActive(false);
        quizPage.SetActive(false);
    }

    // When the game is started
    public void GameStart()
    {
        // Call player to set starting point
        CalibrateQuizBoard();
    }

    // Allow player to select quiz board spawn
    public void CalibrateQuizBoard()
    {
        // Show calibrate page
        calibratePage.SetActive(true);
        // Hide other pages
        startPage.SetActive(false);
        quizPage.SetActive(false);

        // Allow player to set start point
        setStartPoint = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Get raycast from camera to world
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        // If raycast exists
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            // Set the start point vector3
            savedStartPoint = raycastHit.point;
            // If the player is able to set the start point
            if (setStartPoint)
            {
                // When player taps screen
                if (Input.GetMouseButtonDown(0))
                {
                    // Create position at point on map
                    GameObject position = Instantiate(startPointObj, savedStartPoint, Quaternion.identity);
                    // Add to spawned array
                    spawnedPoint.Add(position);

                    // Disallow players from setting start pos
                    setStartPoint = false;
                    // Switch the page to the quiz page
                    SwitchToQuiz();
                }
            }
        }
    }

    public void SwitchToQuiz()
    {
        // Show quiz page
        quizPage.SetActive(true);
        // Hide other pages
        startPage.SetActive(false);
        calibratePage.SetActive(false);
    }
}
