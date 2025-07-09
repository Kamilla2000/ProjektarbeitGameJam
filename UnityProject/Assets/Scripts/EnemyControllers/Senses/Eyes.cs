using UnityEngine;

public class Eyes : Sense
{
    public LayerMask DetectionLayer;
    public float Fov = 120f;

    protected override void Start()
    {
        base.Start();
        _player = GameObject.FindGameObjectWithTag("Princess"); // Tag geändert
    }

    protected override void Update()
    {
        base.Update();

        if (_player == null) return;

        _directionToPlayer = _player.transform.position - HeadReferenceTransform.position;

        if (IsInRange() && IsInFieldOfView() && IsNotOccluded())
        {
            Debug.Log("Princess gesehen");
            IsDetecting = true;
        }
        else
        {
            IsDetecting = false;
        }
    }

    public bool IsInFieldOfView()
    {
        Vector3 direction = _directionToPlayer;
        direction.y = 0;
        Vector3 forwards = HeadReferenceTransform.forward;
        forwards.y = 0;

        float angleBetween = Vector3.Angle(forwards, direction);
        return angleBetween < Fov * 0.5f;
    }

    public bool IsNotOccluded()
    {
        RaycastHit hit;
        Ray ray = new Ray(HeadReferenceTransform.position, _directionToPlayer);

        if (Physics.Raycast(ray, out hit, Range, DetectionLayer))
        {
            return hit.collider.gameObject.CompareTag("Princess"); // Tag geändert
        }

        return false;
    }
}