/* Author: Loh Shau Ern Shaun
 * Date: 28/01/2025
 * Desc:
 * - When socket is connected, allow answer checking
 * - If no socket connection, disallow answer checking
 * - Send current selected socket option when called
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocketSending : MonoBehaviour
{
    // Reference calibrate quiz script
    public CalibrateQuiz calibrateQuiz;

    // When this object is spawned
    void Start()
    {
        // Find the script object and link it to variable
        calibrateQuiz = FindObjectOfType<CalibrateQuiz>();
    }

    // Button for submitting is activated
    public void AllowSubmit()
    {
        calibrateQuiz.AllowSubmit();
    }

    // Button for submitting is deactivated
    public void DisallowSubmit()
    { 
        calibrateQuiz.DisallowSubmit();
    }

    // When option 1 is selected, broadcast to CalibrateQuiz
    public void PublishCurrentOption1()
    {
        calibrateQuiz.PublishCurrentOp("1");
    }

    // When option 2 is selected, broadcast to CalibrateQuiz
    public void PublishCurrentOption2()
    {
        calibrateQuiz.PublishCurrentOp("2");
    }

    // When option 3 is selected, broadcast to CalibrateQuiz
    public void PublishCurrentOption3()
    {
        calibrateQuiz.PublishCurrentOp("3");
    }
}
