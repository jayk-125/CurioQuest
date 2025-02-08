/* Author: Loh Shau Ern Shaun
 * Date: 27/01/2025
 * Desc:
 * - Holds all the quiz related objects and texts
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuizList : MonoBehaviour
{
    // Reference any and all game objects to be used fore the quiz here:
    // Valid question ID list
    public string[] validIDList;
    
    // Option object list
    public GameObject[] optionsList;

    // Answer object list
    public GameObject[] ansList;

    // Question list
    public string[] questionList;

    // Option number list
    public string[] opNumList;

    // Correct ans list
    public string[] corAnsList;

    // Explanations list
    public string[] explanationList;
}
