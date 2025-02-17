using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendDespawn : MonoBehaviour
{
    // Reference calibrate quiz script
    private LearnSpawner learnSpawner;

    // When this object is spawned
    void Awake()
    {
        // Find the script object and link it to variable
        learnSpawner = FindObjectOfType<LearnSpawner>();
    }

    // Send over the despawn message
    public void SendDespawnMessage()
    {
        // Do the DespawnModels function
        learnSpawner.DespawnModels();
    }
}
