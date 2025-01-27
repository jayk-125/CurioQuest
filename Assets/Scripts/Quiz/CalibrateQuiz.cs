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

    // List containing spawned quiz board
    private List<GameObject> spawnedPoint = new List<GameObject>();
    // List containing spawned quiz objects
    private List<GameObject> quizObjects = new List<GameObject>();

    // Boolean to handle whether player can set start transform
    private bool setStartPoint;
    // Boolean to start quiz timer
    private bool canTimerStart;
    // Boolean to allow question to be set
    private bool setTheQuestion;

    // Curent time float
    private float currentTime;
    // Timer float
    private float quizTimer = 15f;
    // Question number count
    private int questionNum;

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
    // Question options list
    List<GameObject> optionObjects = new List<GameObject>();
    // Question answer object
    GameObject randomQAnsObj;

    // Reference to question text
    [SerializeField]
    private TextMeshProUGUI questionText;
    // Reference to question number text
    [SerializeField]
    private TextMeshProUGUI questionNumText;
    // Reference to button text
    [SerializeField]
    private TextMeshProUGUI buttonText;
    // Reference to quiz timer text
    [SerializeField]
    private TextMeshProUGUI quizTimerText;
    // Reference to quiz score text
    [SerializeField]
    private TextMeshProUGUI quizScoreText;

    // Start is called before the first frame update
    void Awake()
    {
        // Disallow players from setting start pos
        setStartPoint = false;
        // Disallow question setting
        setTheQuestion = false;

        // Show start page
        startPage.SetActive(true);
        // Hide other pages
        calibratePage.SetActive(false);
        quizPage.SetActive(false);

        // Set the current time as the same as the quiz timer
        currentTime = quizTimer;
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
                canTimerStart = false;
                Debug.Log("Quiz has been ended");
                //EndQuiz();
            }
        }

        // Setting the quiz questions
        // If can set the question
        if (setTheQuestion)
        {
            // Question number increment
            questionNum++;
            // Show question number
            questionNumText.text = "Question " + questionNum.ToString() + ": ";
            Debug.Log("Question " + questionNum);
                

            Debug.Log("Setting a question");
            // Set the question
            SetQuestion();

            // Disallow question setting
            setTheQuestion = false;
        }
    }

    public void SwitchToQuiz()
    {
        // Show quiz page
        quizPage.SetActive(true);
        // Hide other pages
        startPage.SetActive(false);
        calibratePage.SetActive(false);

        Debug.Log("Starting quiz!");
        // Allow quiz to start
        StartQuiz();
    }

    // Quiz starts
    public void StartQuiz()
    {
        // Allow timer to start
        canTimerStart = true;

        // Prepare the question
        // Number starts at 0
        questionNum = 0;

        Debug.Log("Questions starting to be set");
        // While the timer is active
        setTheQuestion = true;
    }

    // Prepare the quiz question
    public void SetQuestion()
    {
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
        Debug.Log(qList[randomQNum].Split('~')[0]);
        // Split the question
        var randomQList = qList[randomQNum].Split('~');
        Debug.Log(randomQList);
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
            // Compare game object tag to current question ID
            var checkObj = opsList[i];
            if (checkObj.tag == randomQID)
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

        // Check thru the Answer Object List
        var ansList = quizList.ansList;
        for (int i = 0; i < ansList.Length; i++)
        {
            // Compare game object tag to current question ID
            var checkObj = ansList[i];
            if (checkObj.tag == randomQID)
            {
                // Retrieve the ANSWER OBJECT
                randomQAnsObj = checkObj;
                Debug.Log("Answer object obtained!");
                // After finding, end loop
                break;
            }
        }
    }


    /*
    public void EndQuiz()
    {
        
    }
    */
}
