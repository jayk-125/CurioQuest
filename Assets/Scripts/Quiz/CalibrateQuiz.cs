/* Author: Loh Shau Ern Shaun
 * Date: 24/01/2025
 * Desc:
 * - Handles quiz UI pages
 * - When starting quiz, player can select location for interactive quiz objects to spawn
 * - After selecting spawn location, go to Quiz
 * 
 * - When quiz starts, timer begins
 * - End of timer stops quiz. Pressing "End Quiz" also stops quiz
 * - When quiz is started, a question is called
 * - Question will have a ID assigned to it, and takes all necessary text and objects with the same ID
 * - When answer object is dragged to an option, it returns the answer to be checked. If answer is correct, add to score
 * - After checking, show an explanation to question and clear the quiz board, then set next question
 * 
 * - At the end of the quiz, go to "Game Over page"
 * - Displays current score and highscore
 * - Allows restart of quiz
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CalibrateQuiz : MonoBehaviour
{
    // Reference to ScoreHandler script
    [SerializeField]
    private ScoreHandler scoreHandler;

    // Reference different pages
    public GameObject startPage;
    public GameObject calibratePage;
    public GameObject quizPage;
    public GameObject gameOverPage;

    // Spawning of Quiz Board
    // Reference the start pos object here
    [SerializeField]
    private GameObject startPointObj;
    // Boolean to handle whether player can set start transform
    private bool setStartPoint;
    // List containing spawned quiz board
    private List<GameObject> spawnedPoint = new List<GameObject>();


    // Overall quiz timer
    // Boolean to start quiz timer
    private bool canTimerStart;
    // Current time float
    private float currentTime;
    // Timer float
    public float quizTimer = 15f;
    
    // Question number count
    private int questionNum;
    // Current selected option
    private string chosenOption;
    // Current button settings
    private string buttonSettings;

    // Reference to QuizList script
    public QuizList quizList;
    // Reference to camera
    [SerializeField]
    private Camera mainCamera;
    // Vector3 to handle saved position point
    private Vector3 savedStartPoint;

    // Quiz question variables
    // Question text
    string randomQtext;
    // Question correct ans
    string randomQcorAns;
    // Question explanation
    string randomQExplanation;
    // List containing all CURRENTLY spawn objects
    List<GameObject> optionObjects = new List<GameObject>();
    // Question answer object
    GameObject randomQAnsObj;
    // List containing all TO BE SPAWNED quiz objects
    private List<GameObject> quizObjects = new List<GameObject>();

    // UI objects
    // Reference to question text
    [SerializeField]
    private TextMeshProUGUI questionText;
    // Reference to question number text
    [SerializeField]
    private TextMeshProUGUI questionNumText;
    // Reference to button text
    [SerializeField]
    private TextMeshProUGUI buttonText;
    // Reference to button object
    [SerializeField]
    private Button buttonObj;
    // Reference to quiz timer text
    [SerializeField]
    private TextMeshProUGUI quizTimerText;
    // Reference to quiz score text
    [SerializeField]
    private TextMeshProUGUI quizScoreText;

    // Game Over page
    // Reference to current score text
    [SerializeField]
    private TextMeshProUGUI currentScoreText;
    // Reference to highscore text
    [SerializeField]
    private TextMeshProUGUI highscoreText; // Currently just a placeholder

    // Start is called before the first frame update
    void Awake()
    {
        // Reset the quiz
        Restart();
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
        gameOverPage.SetActive(false);

        // Allow player to set start point
        setStartPoint = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Spawning quiz board code
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

        // Quiz timer
        if (canTimerStart)
        {
            // Flow of time per frame
            currentTime -= Time.deltaTime;
            quizTimerText.text = "Time: " + Mathf.Ceil(currentTime).ToString();
            // If time is up
            if (currentTime <= 0f)
            {
                // Stop timer
                canTimerStart = false;
                Debug.Log("Quiz has been ended");
                // End the quiz
                EndQuiz();
            }
        }
    }

    // Switch to the quiz page
    public void SwitchToQuiz()
    {
        // Show quiz page
        quizPage.SetActive(true);
        // Hide other pages
        startPage.SetActive(false);
        calibratePage.SetActive(false);
        gameOverPage.SetActive(false);

        Debug.Log("Starting quiz!");
        // Allow quiz to start
        StartQuiz();
    }

    // Quiz starts
    public void StartQuiz()
    {
        // Allow timer to start
        canTimerStart = true;

        // Reset player score
        scoreHandler.ResetScore();

        // Prepare the question
        // Number starts at 0
        questionNum = 0;

        Debug.Log("Questions starting to be set");
        
        // Set a question
        SetQuestion();
    }

    // Prepare the quiz question
    public void SetQuestion()
    {
        // Question number increment
        questionNum++;
        // Show question number
        questionNumText.text = "Question " + questionNum.ToString() + ": ";
        Debug.Log("Question " + questionNum);

        // Button setting is "submit"
        buttonSettings = "submit";

        // Retrieve a question
        GetQuestion();

        // Set question text
        questionText.text = randomQtext;
        // Set button text
        buttonText.text = "Submit";

        // Find object spawn point
        Vector3 spawnPoint = spawnedPoint[0].transform.Find("ObjectSpawn").transform.position;
        // Spawn answer object at point
        GameObject ansObj = Instantiate(randomQAnsObj,spawnPoint,Quaternion.identity);
        // Add ans object to quiz objects list
        quizObjects.Add(ansObj);

        int corAns = int.Parse(randomQcorAns);

        // Handles all the objects being spawned
        for (int i = 0; i < optionObjects.Count; i++)
        {
            // Initialize spawn point
            Vector3 optionSpawnPoint;
            // Set first spawn
            if (i == 0)
            {
                Debug.Log("Option 1 set");
                // Find object spawn point
                optionSpawnPoint = spawnedPoint[0].transform.Find("Option1Spawn").transform.position;
            }
            // Set second spawn
            else if (i == 1)
            {
                Debug.Log("Option 2 set");
                // Find object spawn point
                optionSpawnPoint = spawnedPoint[0].transform.Find("Option2Spawn").transform.position;
            }
            // Set last spawn
            else
            {
                Debug.Log("Option 3 set");
                // Find object spawn point
                optionSpawnPoint = spawnedPoint[0].transform.Find("Option3Spawn").transform.position;
            }
            // Spawn option object at point
            GameObject optObj = Instantiate(optionObjects[i], optionSpawnPoint, Quaternion.identity);
            // Add ans object to quiz objects list
            quizObjects.Add(optObj);
        }
    }

    // Retrieve a question
    public void GetQuestion()
    {
        Debug.Log("Getting a random question");

        var qList = quizList.questionList;
        // Get a random question number from quiz list
        int randomQNum = Random.Range(0, qList.Length);
        // Split the question
        var randomQList = qList[randomQNum].Split('~');
        // Retrieve the ID from Question
        string randomQID = randomQList[1].ToString();
        Debug.Log("ID obtained!");

        // Retrieve the QUESTION
        randomQtext = randomQList[0].ToString();
        Debug.Log("Question text obtained!");

        // Check thru the Correct Ans List
        var caList = quizList.corAnsList;
        for (int i = 0; i < caList.Length; i++)
        {
            // Split each corAns obj from quiz list
            var splitCorAns = caList[i].Split('~');
            var splitCorAnsID = splitCorAns[1].ToString();
            // Retrieve ID of this obj and compare to current question ID
            if (splitCorAnsID == randomQID)
            {
                // Retrieve the CORRECT ANSWER
                randomQcorAns = splitCorAns[0];

                Debug.Log("Correct ans obtained!");
                // After finding, end loop
                break;
            }
        }
        // Check thru the Explanation List
        var expList = quizList.explanationList;
        for (int i = 0; i < expList.Length; i++)
        {
            // Split each explanation obj from quiz list
            var splitExplanation = expList[i].Split('~');
            // Retrieve ID of this obj and compare to current question ID
            if (splitExplanation[1] == randomQID)
            {
                // Retrieve the EXPLANATION
                randomQExplanation = splitExplanation[0];

                Debug.Log("Explanation obtained!");
                // After finding, end loop
                break;
            }
        }
        // Check thru the Option Object List
        var opsList = quizList.optionsList;
        var countOps = 0;
        for (int i = 0; i < opsList.Length; i++)
        {
            // Compare game object ID to current question ID
            var checkObj = opsList[i];
            // Check each ID in game object ID list
            foreach (string id in checkObj.GetComponent<TagHolder>().ID_list)
            {
                // If an ID matches
                if (id == randomQID)
                {
                    // Retrieve the OPTIONS OBJECTS
                    optionObjects.Add(checkObj);

                    // Increment count
                    countOps++;
                    // If all 3 option objects are received
                    if (countOps == 3)
                    {
                        Debug.Log("All options obtained!");
                        // After finding, end loop
                        break;
                    }
                }
            }
        }

        // Check thru the Answer Object List
        var ansList = quizList.ansList;
        for (int i = 0; i < ansList.Length; i++)
        {
            // Compare game object tag to current question ID
            var checkObj = ansList[i];
            foreach (string id in checkObj.GetComponent<TagHolder>().ID_list)
            {
                if (id == randomQID)
                {
                    // Retrieve the ANSWER OBJECT
                    randomQAnsObj = checkObj;
                    Debug.Log("Answer object obtained!");
                    // After finding, end loop
                    break;
                }
            }
        }
    }

    // SocketSending functions:
    // Allow button to submit
    public void AllowSubmit()
    {
        buttonObj.interactable = true;
    }
    // Disallow button to submit
    public void DisallowSubmit()
    {
        buttonObj.interactable = false;
    }
    // Current receieved option
    public void PublishCurrentOp(string option)
    {
        chosenOption = option;
    }
    // When button pressed
    public void ButtonPressed()
    {
        // If button is in "Submit" mode
        if (buttonSettings == "submit")
        {
            SubmittedAnswer();
        }
        // Else if button is in "Next question" mode
        else
        {
            NextQuestion();
        }

    }

    // When submitted
    void SubmittedAnswer()
    {
        // Initialize string variable
        string response;
        // If chosen answer is correct
        if (chosenOption == randomQcorAns)
        {
            // Actions for "Correct" here
            Debug.Log("Yippee, correct ans!");
            response = "CORRECT!\n";
            // Increase the score
            scoreHandler.OnCorrect();
        }
        // Else if answer is wrong
        else
        {
            // Actions for "Incorrect" here
            Debug.Log("U suck bruh");
            response = "INCORRECT...\n";
        }
        // Show Response + Explanation
        questionText.text = response + randomQExplanation;

        ClearQuizObj();

        // Change button text
        buttonText.text = "Next";
        // Change button settings
        buttonSettings = "go next";
        // Enable button
        buttonObj.interactable = true;
    }

    // Going to the next question
    void NextQuestion()
    {
        // Set the next question
        SetQuestion();
        // Disable the button
        buttonObj.interactable = false;
    }

    // End the quiz
    public void EndQuiz()
    {
        // Stop timer
        canTimerStart = false;
        
        // Show game over page
        gameOverPage.SetActive(true);
        // Hide other pages
        startPage.SetActive(false);
        calibratePage.SetActive(false);
        quizPage.SetActive(false);

        Debug.Log("clearing quiz board");
        // Remove all quiz interactable objects
        ClearQuizObj();
        // Get the saved quiz board object
        foreach (GameObject quizBrd in spawnedPoint)
        {
            // Destroy quiz board object
            Destroy(quizBrd);
        }
        // Clear the list
        spawnedPoint.Clear();
        Debug.Log("Cleared!");

        // Set the current score text as current score
        currentScoreText.text = "Current score:\n" + scoreHandler.score.ToString();
    }

    // Restart button
    public void Restart()
    {
        // Disallow players from setting start pos
        setStartPoint = false;

        // Show start page
        startPage.SetActive(true);
        // Hide other pages
        calibratePage.SetActive(false);
        quizPage.SetActive(false);
        gameOverPage.SetActive(false);

        // Set the current time as the same as the quiz timer
        currentTime = quizTimer;
    }

    // Remove all quiz interactable objects
    void ClearQuizObj()
    {
        // Loop each stored object type
        foreach (GameObject quizObj in quizObjects)
        {
            // Destroy this object
            Destroy(quizObj);
        }
        // Clear the quiz list now that all spawned objects are removed
        quizObjects.Clear();
        // Clear the current spawned quiz interactable objects list
        optionObjects.Clear();

        // Destroy the randomQAnsObj
        Destroy(randomQAnsObj);

        Debug.Log("All quiz objects cleared!");
    }
}
