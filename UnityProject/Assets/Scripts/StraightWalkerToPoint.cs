using UnityEngine;

public class StraightWalkerToPoint : MonoBehaviour
{
    [System.Serializable]
    public class Walker
    {
        public Transform character;
        public Animator animator;
        public Transform targetPoint;
        public float stopDistance = 0.1f;
    }

    public Walker[] characters;
    public float speed = 2f;
    public bool startWalking = false;

    void Update()
    {
        if (!startWalking) return;

        bool anyoneStillWalking = false;

        foreach (Walker walker in characters)
        {
            if (walker.character == null || walker.targetPoint == null)
                continue;

            float distance = Vector3.Distance(walker.character.position, walker.targetPoint.position);

            if (distance > walker.stopDistance)
            {
                // Move towards target
                Vector3 direction = (walker.targetPoint.position - walker.character.position).normalized;
                walker.character.position += direction * speed * Time.deltaTime;

                // Enable walk animation
                if (walker.animator != null)
                    walker.animator.SetBool("isWalking", true);

                anyoneStillWalking = true;
            }
            else
            {
                // Stop animation when close enough
                if (walker.animator != null)
                    walker.animator.SetBool("isWalking", false);
            }
        }

        if (!anyoneStillWalking)
        {
            startWalking = false;
            Debug.Log("? ??? ????? ?? ?????!");
        }
    }

    public void StartWalk()
    {
        startWalking = true;
    }
}