using UnityEngine;

using Unity.Cinemachine;

public class MovingPlatform : MonoBehaviour
{
    // Get nice curve in inspector to control animation
    public AnimationCurve AnimationCurve;

    // How long does it take to sample the curve
    public float Duration = 3f;

    // Play form the beginning
    public bool PlayOnStart = true;

    // Elapse time
    private float _progress;

    // Moves the platform via cinemachin
    private CinemachineSplineCart _cart;

    // Update the position of the platform?
    private bool _isMoving;

    // Start is called before the first frame update
    void Start()
    {
        _cart = GetComponent<CinemachineSplineCart>();
        _isMoving = PlayOnStart;
    }

    private void FixedUpdate()
    {
        if (_isMoving)
        {
            _progress += Time.deltaTime / Duration;

            float position = AnimationCurve.Evaluate(_progress);
            _cart.SplinePosition = position;

            if (_progress >= 1f)
            {
                _progress = 0f; // Wiederhole Bewegung optional
            }
        }
    }

    // Wenn  kein Play on Awake, dann die n�chste Funktionen

    // Enter trigger > make player child of this transform
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            // geht mit Play On Start ; entscheidet, ob es bewegt werden soll
            _isMoving = true;
            other.transform.SetParent(transform);
        }        
    }

    // Exit trigger > unparent the player
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(null);
        }
    }
}

