using UnityEngine;

public class Ears : Sense
{
    private AnimationAndMovementController playerController;

    protected override void Start()
    {
        base.Start();

        _player = GameObject.FindGameObjectWithTag("Princess"); // Tag angepasst
        if (_player != null)
        {
            playerController = _player.GetComponent<AnimationAndMovementController>();

            if (playerController == null)
            {
                Debug.LogError("Ears.cs: 'Princess' gefunden, aber kein AnimationAndMovementController-Component.");
            }
        }
        else
        {
            Debug.LogError("Ears.cs: Kein GameObject mit Tag 'Princess' gefunden.");
        }
    }

    protected override void Update()
    {
        base.Update();

        if (IsInRange() && playerController != null && playerController.IsAudible)
        {
            Debug.Log("Princess wurde gehört.");
            IsDetecting = true;
        }
        else
        {
            IsDetecting = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        _player = GameObject.FindGameObjectWithTag("Princess"); // Auch hier angepasst
        if (_player != null)
        {
            _directionToPlayer = _player.transform.position - HeadReferenceTransform.position;
            SenseGizmos.DrawRangeDisc(HeadReferenceTransform.position, transform.up, Range);
        }
    }
}