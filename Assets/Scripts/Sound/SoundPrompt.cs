/* Author: Jaykin Lee
Date: 15/2/2025
Desc:
- Controls the triggers of audios
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPrompt : MonoBehaviour
{
    // Play click when called
    public void OnNormalClick()
    {
        FindObjectOfType<SoundManagerScript>().Play("click");
    }

    // Play calibrate when called
    public void OnCalibrate ()
    {
        FindObjectOfType<SoundManagerScript>().Play("calibrate");
    }

    // Play despawn when called
    public void OnDespawn()
    {
        FindObjectOfType<SoundManagerScript>().Play("despawn");
    }

    // Play correct when called
    public void OnCorrect()
    {
        FindObjectOfType<SoundManagerScript>().Play("correct");
    }

    // Play incorrect when called
    public void OnIncorrect()
    {
        FindObjectOfType<SoundManagerScript>().Play("incorrect");
    }

    // Play socket snap when called
    public void OnSocketSnap()
    {
        FindObjectOfType<SoundManagerScript>().Play("snap");
    }

    // Play quiz end when called
    public void OnQuizEnd()
    {
        FindObjectOfType<SoundManagerScript>().Play("endquiz");
    }
}
