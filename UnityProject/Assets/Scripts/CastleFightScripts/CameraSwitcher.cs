using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera gameplayCamera;
    public Camera bossCamera;

    public void ShowGameplayCamera()
    {
        SetCamera(gameplayCamera);
    }

    public void ShowBossCamera()
    {
        SetCamera(bossCamera);
    }

    private void SetCamera(Camera target)
    {
        if (gameplayCamera != null) gameplayCamera.enabled = false;
        if (bossCamera != null) bossCamera.enabled = false;

        if (target != null) target.enabled = true;
    }
}
