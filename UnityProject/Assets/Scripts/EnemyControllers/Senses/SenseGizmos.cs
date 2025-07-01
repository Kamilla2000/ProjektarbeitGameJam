using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
/// <summary>
/// Helper class to draw custom gizmos for the senes (eyes, ears)
/// </summary>
public class SenseGizmos
{
    private static float _lineWidth = 5f;
    private static Color _colorRadius = new Color(1, 0.9215686f, 0.01568628f, 1);
    private static Color _colorDisc = new Color(0.3357821f, 0, 1, 0.1294118f);
    private static Color _colorArc = new Color(0, 0, 1, 0.5f);
    private static Color _colorInverseArc = new Color(0, 0, 0, 0.2f);
    private static Color _colorRayMissing = Color.magenta;
    private static Color _colorRayHitting = Color.green;


    public static void DrawRangeCircle(Vector3 startPosition, Vector3 normal, float radius) 
    {
        Handles.color = _colorRadius;
        Handles.DrawWireDisc(startPosition, normal, radius, _lineWidth);
    }

    public static void DrawRangeDisc(Vector3 startPosition, Vector3 normal, float radius) 
    {
        Handles.color = _colorDisc;
        Handles.DrawSolidDisc(startPosition, normal, radius);
    }

    public static void DrawFOV(Vector3 position, Vector3 forward, Vector3 normal, float range, float fov)
    {
        forward.y = 0;

        Handles.color = _colorArc;
        Vector3 startFovArc = Quaternion.AngleAxis(-fov * 0.5f, Vector3.up) * forward;

        Handles.DrawSolidArc(position, normal, startFovArc, fov, range);

        Vector3 startInverseFovArc = Quaternion.AngleAxis(fov * 0.5f, Vector3.up) * forward;

        Handles.color = _colorInverseArc;
        Handles.DrawSolidArc(position, normal, startInverseFovArc, 360f - fov, range);
    }

    public static void DrawRay(Vector3 from, Vector3 to, bool isHitting) 
    {
        Handles.color = isHitting ? _colorRayHitting : _colorRayMissing;
        Handles.DrawLine(from, to);
    }
}
#endif