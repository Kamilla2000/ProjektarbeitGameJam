using UnityEngine;

[System.Serializable]
public class Task
{
    public enum TaskType
    {
        Move,   // Player moves using WASD or similar
        Run,    // Player holds Shift to run
        Jump    // Player jumps
    }

    public string taskTitle;         // Title shown in UI
    public string taskDescription;   // Description shown in UI
    public TaskType taskType;        // Type of task
    public int requiredAmount = 1;   // How many times the action is needed
    [HideInInspector] public bool isCompleted = false;
}
