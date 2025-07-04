using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{
    public float GroundCheckDistance = 0.2f;

    private float _ySpeed = 0;

    private float _gravityMultiplyer = 2;

    private Animator _animator;
    private CharacterController _characterController;

    public float ForceStrenght = 100;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(IsGrounded());
        /*if (IsGrounded())
        {
            // auf 0 setzen, damit es dann wenn man zu oft fällt
            _ySpeed = 0;
        }
        else
        {
            _ySpeed += Physics.gravity.y * _gravityMultiplyer * Time.deltaTime;
        }*/

       
        // Das Gleiche wie if-Anweisung
        _ySpeed = IsGrounded() ? _ySpeed = 0 : _ySpeed = _ySpeed + Physics.gravity.y * _gravityMultiplyer * Time.deltaTime;
    }


    // wenn während einer Bewerung man eine Collider trifft
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rigidbody = hit.collider.attachedRigidbody;

        if(rigidbody != null)
        {
            // wenn man einen Objekt trifft "hit", soll er prüfen ob es ein Collider hat,  mein eigener Collider soll für die Distanz abgezogen werden
            Vector3 direction = hit.gameObject.transform.position - transform.position;

            // damit das Objekt nicht nach oben verschoben sein kann
            direction.y = 0;
            direction.Normalize();

            // transform ist hier für die Characterposition, von welcher die Force ausgestrahlt wird
            rigidbody.AddForceAtPosition(direction * ForceStrenght, transform.position, ForceMode.Impulse);
        }
    }

    // Wenn Animation läuft und der Character fällt, damit man die Fallgeschwindigkeit steuern kann
    private void OnAnimatorMove()
    {
        _animator.ApplyBuiltinRootMotion();

        Vector3 velocity = _animator.deltaPosition;
        velocity.y = _ySpeed;

        _characterController.Move(velocity * Time.deltaTime);
    }

    void OnDrawGizmos()
    {
        // IsGrounded() ? Color.green : Color.red); wenn isGrounded true ist, dann green, ansongsten - rot
        Debug.DrawRay(transform.position, Vector3.down * GroundCheckDistance * 0.5f, IsGrounded() ? Color.green : Color.red);
    }

    private bool IsGrounded()
    {
        // y Wert ist die Offset für den Raycast
        return Physics.Raycast(transform.position + new Vector3(0, GroundCheckDistance * 0.5f, 0), Vector3.down, GroundCheckDistance);

    }
}
