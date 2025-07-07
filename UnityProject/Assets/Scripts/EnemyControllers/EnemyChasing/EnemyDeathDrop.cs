using UnityEngine;

public class EnemyDeathDrop : MonoBehaviour
{
    public GameObject heartPrefab;       // Prefab für das Herz
    public int heartAmount = 3;          // Wie viele Herzen sollen spawnen
    public float spawnRadius = 1.5f;     // Random Radius
    public float heartLifetime = 10f;    // Wie lange die Herzen bleiben

    private Animator animator;
    private bool isDead = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        // Starte Animation
        animator.SetTrigger("die");

        // Warte, bis die Animation wirklich abgespielt wurde
        StartCoroutine(WaitForDieAnimationAndSpawn());
    }

    private System.Collections.IEnumerator WaitForDieAnimationAndSpawn()
    {
        // Warte, bis Animator im "Die"-State ist
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        while (!stateInfo.IsName("die"))
        {
            yield return null;
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        }

        // Jetzt läuft die "Die"-Animation – warte bis sie zu Ende ist
        while (stateInfo.normalizedTime < 1f)
        {
            yield return null;
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        }

        // Jetzt spawne Herzen random um NPC herum
        for (int i = 0; i < heartAmount; i++)
        {
            Vector3 offset = Random.insideUnitSphere * spawnRadius;
            offset.y = 0;
            Vector3 spawnPos = transform.position + offset;

            GameObject heart = Instantiate(heartPrefab, spawnPos, Quaternion.identity);
            Destroy(heart, heartLifetime);
        }

        // NPC löschen
        Destroy(gameObject);
    }
}