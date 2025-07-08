using UnityEngine;
using UnityEngine.InputSystem;

public class MagicAttack : MonoBehaviour
{
    [SerializeField] private GameObject magicPrefab;
    [SerializeField] private Transform castPoint;  
    [SerializeField] private float magicSpeed = 10f;
    [SerializeField] private float cooldown = 1f;

    public int damageAmount = 20;

    private float lastCastTime;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Keyboard.current.qKey.wasPressedThisFrame && Time.time >= lastCastTime + cooldown)
        {
            CastMagic();
            lastCastTime = Time.time;
        }
    }

    void CastMagic()
    {
        animator.SetTrigger("Cast");

        GameObject magic = Instantiate(magicPrefab, castPoint.position, castPoint.rotation);

         
        Collider playerCollider = GetComponent<Collider>();
        Collider magicCollider = magic.GetComponent<Collider>();
        if (playerCollider != null && magicCollider != null)
        {
            Physics.IgnoreCollision(magicCollider, playerCollider);
        }

        Rigidbody rb = magic.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = castPoint.forward * magicSpeed;
        }

        Destroy(magic, 5f); // Удаляем магию через 5 сек
        Debug.Log($"Magic spawned at position: {magic.transform.position}, velocity: {rb.linearVelocity}");
    }
}