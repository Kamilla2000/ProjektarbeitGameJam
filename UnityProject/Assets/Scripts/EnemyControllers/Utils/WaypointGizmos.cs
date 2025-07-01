using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
/// <summary>
/// Helper class to draw custom gizmos for the senes (eyes, ears)
/// </summary>
public class WaypointGizmos
{
    private static float lineWidth = 2f;
    private static float discRadius = 0.5f;
    private static Color colorLine = new Color(1, 0.28f, 0, 1);
    private static Color colorDisc = new Color(1, 0.28f, 0, 1f);
    

    public static void DrawWayPoints(Transform[] waypoints) 
    {
        if (waypoints == null || waypoints.Length <= 0) return;

        var startWaypoint = waypoints[waypoints.Length - 1];
        var endWaypoint = waypoints[0];

        if(startWaypoint == null || endWaypoint == null) return;

        DrawLine(waypoints[waypoints.Length - 1].position, waypoints[0].position);
        DrawDisc(waypoints[0].position, Vector3.up);
        for (int i = 1; i < waypoints.Length; i++)
        {
            var previousWaypoint = waypoints[i - 1];
            var currentWaypoint = waypoints[i];

            if (previousWaypoint == null || currentWaypoint == null) break;

            DrawLine(waypoints[i - 1].position, waypoints[i].position);
            DrawDisc(waypoints[i].position, Vector3.up);
        }
    }

    public static void DrawDisc(Vector3 startPosition, Vector3 normal) 
    {
        Handles.color = colorDisc;
        Handles.DrawSolidDisc(startPosition, normal, discRadius);
    }    

    public static void DrawLine(Vector3 from, Vector3 to) 
    {
        Handles.color = colorLine;
        Handles.DrawLine(from, to, lineWidth);
    }
}
#endif
