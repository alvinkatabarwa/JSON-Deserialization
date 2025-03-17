using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Collections;


[System.Serializable]
public class WorkoutDetails
{
    public int ballCount;
    public string ballDirection;
}

[System.Serializable]
public class WorkoutInfo
{
    public string workoutName;
    public string description;
    public WorkoutDetails workoutDetails;
}

[System.Serializable]
public class WorkoutData
{
    public string ProjectName;
    public List<WorkoutInfo> workoutInfo;
}

public class JSONLoader : MonoBehaviour
{
    public TextMeshProUGUI projectNameText;
    public GameObject buttonPrefab;
    public Transform buttonsParent;
    public Button playPauseButton; 
    private bool isPlaying = false; // tracking the play state
    private Coroutine spawnCoroutine; //  the ball spawning coroutine
    public GameObject ballPrefab; //  the ball prefab

    private WorkoutDetails selectedWorkoutDetails;  // Store selected workout details
    public TextMeshProUGUI workoutDescriptionText;

    void Start()
    {
        LoadJSON();

        //  Play/Pause button listener
        playPauseButton.onClick.AddListener(TogglePlayPause);
    }


    void TogglePlayPause()
    {
        if (isPlaying)
        {
            // Pause the spawning coroutine
            if (spawnCoroutine != null)
            {
                StopCoroutine(spawnCoroutine);
                spawnCoroutine = null;
            }
            playPauseButton.GetComponentInChildren<TextMeshProUGUI>().text = "Play";
        }
        else
        {
            // Start spawning balls based on the selected workout
            spawnCoroutine = StartCoroutine(SpawnBalls(selectedWorkoutDetails));
            playPauseButton.GetComponentInChildren<TextMeshProUGUI>().text = "Pause";
        }

        isPlaying = !isPlaying;
    }

    IEnumerator SpawnBalls(WorkoutDetails details)
    {
        for (int i = 0; i < details.ballCount; i++)
        {
            // Spawn a single ball with a 2-second delay between each one
            SpawnBall(details.ballDirection);
            yield return new WaitForSeconds(2f);
        }
    }

    void SpawnBall(string direction)
    {
        Vector3 spawnDirection = Vector3.zero;

        if (direction == "right") spawnDirection = new Vector3(0.5f, 0f, 0f);
        else if (direction == "left") spawnDirection = new Vector3(-0.5f, 0f, 0f);

        GameObject ball = Instantiate(ballPrefab, transform.position, Quaternion.identity);
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        rb.AddForce(spawnDirection, ForceMode.Impulse);
    }

    void LoadJSON()
    {
        // Load JSON file from Resources folder
        TextAsset jsonFile = Resources.Load<TextAsset>("JSON/workoutData");
       
        if (jsonFile == null)
        {
            Debug.LogError("JSON file not found! Ensure it's inside Resources/JSON/");
            return;
        }

        WorkoutData data = JsonUtility.FromJson<WorkoutData>(jsonFile.ToString());

        // Set project name text
        projectNameText.text = data.ProjectName;

        // Create buttons dynamically based on workoutInfo array
        foreach (var workout in data.workoutInfo)
        {
            GameObject button = Instantiate(buttonPrefab, buttonsParent); // call an instance of the  button prefab
            button.GetComponentInChildren<TextMeshProUGUI>().text = workout.workoutName; // Set button text to workout name
            button.GetComponent<Button>().onClick.AddListener(() => OnWorkoutSelected(workout)); // Add listener for any clicking
        }
    }

    void OnWorkoutSelected(WorkoutInfo workout)
    {
        Debug.Log("Selected Workout: " + workout.workoutName);
        // Update UI to show description and balls 

        selectedWorkoutDetails = workout.workoutDetails; // Store selected workout details
        workoutDescriptionText.text = workout.description;
    }
}
