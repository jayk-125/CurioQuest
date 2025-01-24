/* Author: Loh Shau Ern Shaun
 * Date: 24/01/2025
 * Desc:
 * - Reset quiz when needed (EG: Start)
 * - Counts player current score 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreHandler : MonoBehaviour
{
    // Reference text object
    public TextMeshProUGUI scoreText;
    // Reference score integer
    private int score;

    // Start is called before the first frame update
    void Start()
    {
        // Reset the score
        ResetScore();
        // Copy score to text
        CopyScoreToText();
    }

    // Function for reseting score
    public void ResetScore()
    {
        // Set score to 0
        score = 0;
    }

    // When player gets a question correct
    public void OnCorrect()
    {
        // Increase score by 1
        score += 1;
    }

    // When called, copy the current score to the score text
    void CopyScoreToText()
    {
        scoreText.text = "Current score: " + score.ToString();
    }

}
