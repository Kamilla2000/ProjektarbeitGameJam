using UnityEngine;

public class LookAtCamera: MonoBehaviour
{
    public Transform camera;

    private void LateUpdate()
    {
        // Schau zur Kamera
        transform.LookAt(camera);

        // Um 180 Grad um die Y-Achse drehen
        transform.Rotate(0, 180f, 0);
    }
}