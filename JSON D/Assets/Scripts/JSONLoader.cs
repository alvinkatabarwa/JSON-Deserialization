using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;


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

    void Start()
    {
        LoadJSON();
    }

    void LoadJSON()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("JSON/workoutData");
        WorkoutData data = JsonUtility.FromJson<WorkoutData>(jsonFile.ToString());
        projectNameText.text = data.ProjectName;

        foreach (var workout in data.workoutInfo)
        {
            GameObject button = Instantiate(buttonPrefab, buttonsParent);
            button.GetComponentInChildren<Text>().text = workout.workoutName;
            button.GetComponent<Button>().onClick.AddListener(() => OnWorkoutSelected(workout));
        }
    }

    void OnWorkoutSelected(WorkoutInfo workout)
    {
        Debug.Log("Selected Workout: " + workout.workoutName);
        // Update UI to show description and balls 
    }
}
