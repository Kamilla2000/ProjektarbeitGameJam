using UnityEngine;

public class Ears : Sense
{
    private AnimationAndMovementController playerController;

    protected override void Start()
    {
        base.Start();
        playerController = _player.GetComponent<AnimationAndMovementController>();

        if (playerController == null)
        {
            Debug.LogError("Ears.cs: Player controller not found or missing AnimationAndMovementController component.");
        }
    }

    protected override void Update()
    {
        base.Update();

        if (IsInRange() && playerController != null && playerController.IsAudible)
        {
            Debug.Log(" Player heard");
            IsDetecting = true;
        }
        else
        {
            IsDetecting = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _directionToPlayer = _player.transform.position - HeadReferenceTransform.position;

        SenseGizmos.DrawRangeDisc(HeadReferenceTransform.position, transform.up, Range);
    }
}