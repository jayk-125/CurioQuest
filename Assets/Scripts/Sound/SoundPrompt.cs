/* Author: Jaykin Lee
Date: 15/2/2025
Desc:
- controls the triggers of audios
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPrompt : MonoBehaviour
{

    public void OnNormalClick()
    {
        FindObjectOfType<SoundManagerScript>().Play("click");
    }
}
