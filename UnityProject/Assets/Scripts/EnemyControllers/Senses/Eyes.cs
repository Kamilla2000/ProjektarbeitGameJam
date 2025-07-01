using UnityEngine;

public class Eyes : Sense
{
    public LayerMask DetectionLayer;

    //Winkel 120 Grad
    public float Fov = 120f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        // Richtungsvektor von NPC
        _directionToPlayer = _player.transform.position - HeadReferenceTransform.position;

        if (IsInRange() && IsInFieldOfView() && IsNotOccluded())
        {
            IsDetecting = true;
        }
        else
        {
            IsDetecting = false;
        }
    }

    // Visibility of the Raycast: Gismos
    private void OnDrawGizmosSelected()
    {
        //Sicherstellen, dass die Befehle während der Playermodus ausgeführt werden
        _player = GameObject.FindGameObjectWithTag("Player");
        _directionToPlayer = _player.transform.position - HeadReferenceTransform.position;

        // Range Kreis
        SenseGizmos.DrawRangeCircle(HeadReferenceTransform.position, transform.up, Range); // transform.up damit es parallel zu Boden ist

        if (IsInRange())
        {
            // Field of View
            SenseGizmos.DrawFOV(HeadReferenceTransform.position, HeadReferenceTransform.forward, Vector3.up, Range, Fov);

            if (IsInFieldOfView())
            {
                // Raycast Strahl
                SenseGizmos.DrawRay(HeadReferenceTransform.position, _player.transform.position, IsNotOccluded());
            }
        }
    }

    public bool IsInFieldOfView()
    {
        Vector3 direction = _directionToPlayer;
        direction.y = 0;
        // Von Kopf abhängig 
        Vector3 forwards = HeadReferenceTransform.forward;
        forwards.y = 0;

        // Winkel 
        float angleBetween = Vector3.Angle(forwards, direction);

        // den Winkel auf 2 Teilen
        // return ist dasgleiche wie if, das returns true
        return angleBetween < Fov * 0.5f;
    }

    // RayCast Methode
    public bool IsNotOccluded()
    {
        RaycastHit hit;
        Ray ray = new Ray(HeadReferenceTransform.position, _directionToPlayer); //_directionToPlayer winkel für die Kopfrichtung

        if (Physics.Raycast(ray, out hit, Range, DetectionLayer)) // Range für die Schießdistanz
        {
            return hit.collider.gameObject.CompareTag("Player");
        }
        else
        {
            return false;
        }
    }
}
