/* Author: Loh Shau Ern Shaun
Date: 15/2/2025
Desc:
- Set the dinosaur to spawn
- Spawn the dinosaur with the environment
- Allow for despawning the enviornment
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearnSpawner : MonoBehaviour
{
    // Reference different pages
    public GameObject startPage;
    public GameObject spawnPage;
    public GameObject calibratePage;
    public GameObject despawnPage;

    // Reference to player camera
    public Camera mainCamera;

    // Variable for SoundPrompt type
    SoundPrompt soundPrompt;

    // List containing spawned dinosaurs
    private List<GameObject> spawnedDinos = new List<GameObject>();

    // Array containing spawnable dinosaurs
    public GameObject[] dinoArray;
    // Reference the dinoland
    public GameObject dinoLand;

    // Bool to allow players to set dino spawn
    private bool setStartPoint = false;

    // Vector3 to store spawnpoint
    private Vector3 savedStartPoint;

    // Variable to store dino gameobject to spawn
    private GameObject dinoSpawnable;
    // Variable to store dino to spawn
    private string dinoName;

    // When object is spawned
    void Awake()
    {
        // Set reference to sound prompt
        soundPrompt = FindObjectOfType<SoundPrompt>();
    }

    // Set the dino to spawn as TRex
    public void ChooseTRex()
    {
        // Set name of dino
        dinoName = "Tyrannosaurus Rex";
        // Get the object to spawn
        dinoSpawnable = dinoArray[0];
        // Start calibrating spawn point
        CalibrateSpawnPoint();
    }

    // Set the dino to spawn as Brachiosaurus
    public void ChooseBrachi()
    {
        // Set name of dino
        dinoName = "Brachiosaurus";
        // Get the object to spawn
        dinoSpawnable = dinoArray[1];
        // Start calibrating spawn point
        CalibrateSpawnPoint();
    }

    // Set the dino to spawn as Oviraptor
    public void ChooseOvi()
    {
        // Set name of dino
        dinoName = "Oviraptor";
        // Get the object to spawn
        dinoSpawnable = dinoArray[2];
        // Start calibrating spawn point
        CalibrateSpawnPoint();
    }

    // Allow player to select dinosaur spawn
    public void CalibrateSpawnPoint()
    {
        // Show calibrate page
        calibratePage.SetActive(true);
        // Hide other pages
        startPage.SetActive(false);
        spawnPage.SetActive(false);
        despawnPage.SetActive(false);

        // Allow player to set start point
        setStartPoint = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Spawning dinosaurs code
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
                    GameObject dinoSpawn = Instantiate(dinoSpawnable, savedStartPoint, Quaternion.identity);
                    // Add to spawned array
                    spawnedDinos.Add(dinoSpawn);

                    // Disallow players from setting start pos
                    setStartPoint = false;
                    // Switch the page to the learn page
                    SwitchToLearn();
                }
            }
        }
    }

    // Open the learn page
    public void SwitchToLearn()
    {
        // Play calibrate audio
        soundPrompt.OnCalibrate();

        // Show despawn page
        despawnPage.SetActive(true);
        // Hide other pages
        startPage.SetActive(false);
        spawnPage.SetActive(false);
        calibratePage.SetActive(false);
    }

    // Players despawn the dinosaur and return to start 
    public void DespawnModels()
    {
        // Play despawn audio
        soundPrompt.OnDespawn();

        // Loop each stored dinosaur type
        foreach (GameObject dinoObj in spawnedDinos)
        {
            // Destroy this object
            Destroy(dinoObj);
        }
        // Clear the dino list now that all spawned dinos are removed
        spawnedDinos.Clear();

        // Show start page
        startPage.SetActive(true);
        // Hide other pages
        despawnPage.SetActive(false);
        spawnPage.SetActive(false);
        calibratePage.SetActive(false);
    }
}
