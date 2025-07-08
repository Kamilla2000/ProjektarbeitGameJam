using UnityEngine;

public class Ears : Sense
{
    private AnimationAndMovementController playerController;

    protected override void Start()
    {
        base.Start();

        if (_player != null)
        {
            // Suche AnimationAndMovementController rekursiv in Children
            playerController = _player.GetComponentInChildren<AnimationAndMovementController>();

            if (playerController == null)
            {
                Debug.LogError("Ears.cs: AnimationAndMovementController wurde im Princess-Objekt oder seinen Kindern nicht gefunden.");
            }
        }
        else
        {
            Debug.LogError("Ears.cs: Kein Player gefunden.");
        }
    }

    protected override void Update()
    {
        base.Update();

        if (IsInRange() && playerController != null && playerController.IsAudible)
        {
            Debug.Log("👂 Player heard");
            IsDetecting = true;
        }
        else
        {
            IsDetecting = false;
        }
    }
}