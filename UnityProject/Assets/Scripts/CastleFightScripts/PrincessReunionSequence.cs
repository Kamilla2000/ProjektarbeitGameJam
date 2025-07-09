using UnityEngine;
using UnityEngine.AI;
using TMPro;
using System.Collections;

public class PrincessReunionFinale : MonoBehaviour
{
    [Header("Path Settings")]
    public Transform[] pathPoints;
    public float stopDistance = 0.3f;

    [Header("Dialog UI")]
    public GameObject princePanel;
    public GameObject princessPanel;
    public TextMeshProUGUI princeText;
    public TextMeshProUGUI princessText;

    [Header("Dialog Lines")]
    [TextArea] public string princeLine = "Please... come back to me!";
    [TextArea] public string princessLine = "I never stopped loving you. I’m just... tired of all this chaos.";

    private NavMeshAgent agent;
    private Animator animator;
    private PlayerHealthWithKissAndGameOver health;
    private int currentTargetIndex = 0;
    private bool hasStartedWalking = false;
    private float walkDelay = 10f;
    private float walkTimer = 0f;
    private bool dialogStarted = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        health = GetComponent<PlayerHealthWithKissAndGameOver>();

        animator?.SetBool("isWalking", false);
        agent.enabled = false;

        if (princePanel != null) princePanel.SetActive(false);
        if (princessPanel != null) princessPanel.SetActive(false);
    }

    void Update()
    {
        if (!hasStartedWalking)
        {
            walkTimer += Time.deltaTime;
            if (walkTimer >= walkDelay)
                BeginWalking();
            return;
        }

        if (health != null && health.isKissed)
        {
            agent.isStopped = true;
            animator.SetBool("isWalking", false);
            return;
        }

        if (agent.isStopped && !health.isKissed)
        {
            agent.isStopped = false;
            animator.SetBool("isWalking", true);
        }

        if (!agent.pathPending && agent.remainingDistance < stopDistance)
        {
            currentTargetIndex++;
            if (currentTargetIndex < pathPoints.Length)
            {
                agent.SetDestination(pathPoints[currentTargetIndex].position);
            }
            else if (!dialogStarted)
            {
                EndSequence();
            }
        }
    }

    void BeginWalking()
    {
        hasStartedWalking = true;
        agent.enabled = true;

        if (pathPoints.Length > 0)
            agent.SetDestination(pathPoints[currentTargetIndex].position);

        animator.SetBool("isWalking", true);
    }

    void EndSequence()
    {
        agent.ResetPath();
        animator.SetBool("isWalking", false);
        dialogStarted = true;

        GetComponent<PrincessDialogSystem>()?.DisableDialog();

        StartCoroutine(ShowFinalDialog());
    }

    IEnumerator ShowFinalDialog()
    {
        if (princePanel != null && princeText != null)
        {
            princePanel.SetActive(true);
            princeText.text = princeLine;
        }

        yield return new WaitForSeconds(2f);

        if (princessPanel != null && princessText != null)
        {
            princessPanel.SetActive(true);
            princessText.text = princessLine;
        }
    }

    public bool IsAudible => agent.enabled && agent.velocity.magnitude > 0.1f;
}
