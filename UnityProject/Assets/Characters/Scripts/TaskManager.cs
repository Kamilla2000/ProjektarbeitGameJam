using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TaskManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject taskPanel;                 // Task panel on screen
    public TMP_Text taskTitleText;              // Text for task title
    public TMP_Text taskDescriptionText;        // Text for task description

    [Header("Tasks")]
    public List<Task> tasks = new List<Task>(); // List of all tasks
    private int currentTaskIndex = 0;           // Which task we're on

    // Counters for player actions
    private int moveCount = 0;
    private int runCount = 0;
    private int jumpCount = 0;

    void Start()
    {
        taskPanel.SetActive(false); // Hide panel at start
    }

    public void StartTasks()
    {
        // Reset everything and show the first task
        currentTaskIndex = 0;
        moveCount = 0;
        runCount = 0;
        jumpCount = 0;
        ShowTask();
    }

    void ShowTask()
    {
        if (currentTaskIndex < tasks.Count)
        {
            // Show current task in UI
            Task currentTask = tasks[currentTaskIndex];
            taskPanel.SetActive(true);
            taskTitleText.text = currentTask.taskTitle;
            taskDescriptionText.text = currentTask.taskDescription;
        }
        else
        {
            // No more tasks left
            taskPanel.SetActive(false);
            Debug.Log("All tasks completed!");
        }
    }

    void CompleteCurrentTask()
    {
        // Move to next task
        currentTaskIndex++;
        moveCount = 0;
        runCount = 0;
        jumpCount = 0;
        StartCoroutine(ShowNextTaskDelayed());
    }

    IEnumerator ShowNextTaskDelayed()
    {
        // Hide panel and wait 4 seconds before showing the next
        taskPanel.SetActive(false);
        yield return new WaitForSeconds(4f);
        ShowTask();
    }

    // These methods are called from player controller
    public void RegisterMove()
    {
        moveCount++;
        CheckProgress();
    }

    public void RegisterRun()
    {
        runCount++;
        CheckProgress();
    }

    public void RegisterJump()
    {
        jumpCount++;
        CheckProgress();
    }

    void CheckProgress()
    {
        if (currentTaskIndex >= tasks.Count) return;

        Task currentTask = tasks[currentTaskIndex];

        // Check if player completed the current task
        switch (currentTask.taskType)
        {
            case Task.TaskType.Move:
                if (moveCount >= currentTask.requiredAmount)
                    CompleteCurrentTask();
                break;
            case Task.TaskType.Run:
                if (runCount >= currentTask.requiredAmount)
                    CompleteCurrentTask();
                break;
            case Task.TaskType.Jump:
                if (jumpCount >= currentTask.requiredAmount)
                    CompleteCurrentTask();
                break;
        }
    }
}