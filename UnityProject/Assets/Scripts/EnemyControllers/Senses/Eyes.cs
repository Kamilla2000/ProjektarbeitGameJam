using UnityEngine;

public class Eyes : Sense
{
    public LayerMask DetectionLayer;
    public float Fov = 120f;

    protected override void Start()
    {
        base.Start();
        _player = GameObject.FindGameObjectWithTag("Princess"); // Tag ge�ndert
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

    private void OnDrawGizmosSelected()
    {
        _player = GameObject.FindGameObjectWithTag("Princess"); // Tag ge�ndert

        if (_player == null) return;

        _directionToPlayer = _player.transform.position - HeadReferenceTransform.position;

        SenseGizmos.DrawRangeCircle(HeadReferenceTransform.position, transform.up, Range);

        if (IsInRange())
        {
            SenseGizmos.DrawFOV(HeadReferenceTransform.position, HeadReferenceTransform.forward, Vector3.up, Range, Fov);

            if (IsInFieldOfView())
            {
                SenseGizmos.DrawRay(HeadReferenceTransform.position, _player.transform.position, IsNotOccluded());
            }
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
            return hit.collider.gameObject.CompareTag("Princess"); // Tag ge�ndert
        }

        return false;
    }
}