using UnityEngine;

public class LoadWorkoutJson : MonoBehaviour
{
    private void Start()
    {
        LoadJson();
    }

    void LoadJson()
    {
        // Load the JSON file from Resources (without .json extension)
        TextAsset jsonFile = Resources.Load<TextAsset>("workoutData");

        if (jsonFile != null)
        {
            // Parse JSON data into C# objects
            WorkoutData workoutData = JsonUtility.FromJson<WorkoutData>(jsonFile.text);

            // Display parsed data in the console
            Debug.Log($"Project: {workoutData.ProjectName}, Number of Workout Balls: {workoutData.numberOfWorkoutBalls}");

            foreach (var workout in workoutData.workoutInfo)
            {
                Debug.Log($"Workout ID: {workout.workoutID}, Name: {workout.workoutName}, Type: {workout.ballType}");

                foreach (var detail in workout.workoutDetails)
                {
                    Debug.Log($" - Ball ID: {detail.ballId}, Speed: {detail.speed}, Direction: {detail.ballDirection}");
                }
            }
        }
        else
        {
            Debug.LogError("JSON file not found in Resources!");
        }
    }
}
