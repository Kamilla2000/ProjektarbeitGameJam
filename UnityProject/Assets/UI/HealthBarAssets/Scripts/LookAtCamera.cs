using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Transform camera;

    private void Start()
    {
        // Suche automatisch nach der Kamera mit dem Tag "MainCamera"
        if (Camera.main != null)
        {
            camera = Camera.main.transform;
        }
        else
        {
            Debug.LogWarning("Keine Kamera mit dem Tag 'MainCamera' gefunden!");
        }
    }

    private void LateUpdate()
    {
        if (camera == null) return;

        // Zur Kamera schauen
        transform.LookAt(camera);

        // 180 Grad um Y drehen, damit z. B. UI richtig herum ist
        transform.Rotate(0, 180f, 0);
    }
}