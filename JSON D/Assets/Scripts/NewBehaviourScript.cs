using System;
using UnityEngine;

[Serializable]
public class WorkoutDetail
{
    public int ballId;
    public float speed;
    public float ballDirection;
}

[Serializable]
public class WorkoutInfo
{
    public int workoutID;
    public string workoutName;
    public string description;
    public string ballType;
    public WorkoutDetail[] workoutDetails;
}

[Serializable]
public class WorkoutData
{
    public string ProjectName;
    public int numberOfWorkoutBalls;
    public WorkoutInfo[] workoutInfo;
}
