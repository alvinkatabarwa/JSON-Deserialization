using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Collections;


[System.Serializable]
public class WorkoutDetails
{
    public int numberOfWorkoutBalls;
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
            if (selectedWorkoutDetails != null)
            {
                spawnCoroutine = StartCoroutine(SpawnBalls(selectedWorkoutDetails));
                playPauseButton.GetComponentInChildren<TextMeshProUGUI>().text = "Pause";
            }
            else
            {
                Debug.LogError("No workout selected!"); // Prevents null errors
            }
        }


        isPlaying = !isPlaying;
    }

    IEnumerator SpawnBalls(WorkoutDetails details)
    {
        Debug.Log("Starting ball spawn , Ball count: " + details.numberOfWorkoutBalls);
        for (int i = 0; i < details.numberOfWorkoutBalls; i++)
        {
            // Spawn a single ball with a 2-second delay between each one
            SpawnBall(details.ballDirection); //dealing with the balls Direction
            yield return new WaitForSeconds(2f); //timing in between ball spawns
        }
        Debug.Log("Ball spawning complete");
        //resetting the Play button when completed
        playPauseButton.GetComponentInChildren<TextMeshProUGUI>().text = "Play";
        isPlaying = false;
    }

    void SpawnBall(string direction)
    {
        Vector3 spawnDirection = Vector3.zero;
        if (direction == "right") spawnDirection = new Vector3(1f, 0f, 0f);
        else if (direction == "left") spawnDirection = new Vector3(-1f, 0f, 0f);
        else if (direction == "center") spawnDirection = new Vector3(0f, 0f, 0f);

        Vector3 spawnPosition = new Vector3(0, 1, 0); // Spawn at (0, 1, 0)
        GameObject ball = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);

        Rigidbody rb = ball.GetComponent<Rigidbody>();
        rb.AddForce(spawnDirection *5f, ForceMode.Impulse);

        //need to see where this damn ball is
        Debug.Log("Ball spawned at position: " + spawnPosition);
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
        Debug.Log("Ball Count: " + workout.workoutDetails.numberOfWorkoutBalls);


        selectedWorkoutDetails = workout.workoutDetails; // Store selected workout details
        workoutDescriptionText.text = workout.description;
    }
}
