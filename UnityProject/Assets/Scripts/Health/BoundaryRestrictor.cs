using DialogueEditor;
using UnityEngine;




[RequireComponent(typeof(CharacterController))]
public class BoundaryRestrictor : MonoBehaviour
{
    public Collider boundaryZone; // перетащи сюда BoxCollider зоны
    public float pushBackForce = 3f; // сила отбрасывания

    private CharacterController characterController;
    private Vector3 lastSafePosition;
    private bool dialogueFinished = false;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        lastSafePosition = transform.position;

        ConversationManager.OnConversationEnded += OnConversationEnded;
    }

    void OnDestroy()
    {
        ConversationManager.OnConversationEnded -= OnConversationEnded;
    }

    void Update()
    {
        if (boundaryZone == null) return;

        if (IsInsideBoundary())
        {
            lastSafePosition = transform.position;
        }
        else if (!dialogueFinished)
        {
            // Вычисляем направление, в котором нужно оттолкнуть
            Vector3 pushDirection = (lastSafePosition - transform.position).normalized;

            // Применяем движение назад
            characterController.Move(pushDirection * pushBackForce * Time.deltaTime);
        }
    }

    bool IsInsideBoundary()
    {
        return boundaryZone.bounds.Contains(transform.position);
    }

    void OnConversationEnded()
    {
        dialogueFinished = true;
        Debug.Log("Диалог завершён — игрок теперь может выходить");
    }
}