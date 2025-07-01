using UnityEngine;

public class Ears : Sense
{
    private ThirdPersonController playerController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start(); // base: basisklasse; zuerst Reference von Sense Klasse start und danach dieses Script
        playerController = _player.GetComponent<ThirdPersonController>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if(IsInRange() && playerController.IsAudible)
        {
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
