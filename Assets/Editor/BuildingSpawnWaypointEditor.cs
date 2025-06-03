using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BuildingSpawnWaypoints))]

public class BuildingWaypointEditor : Editor
{
    private void OnSceneGUI()
    {
        BuildingSpawnWaypoints component = target as BuildingSpawnWaypoints;
        List<Vector3> waypoints = component.GetWaypoints();

        for (int i = 0; i < waypoints.Count; i++)
        {
            Vector3 position = waypoints[i];

            EditorGUI.BeginChangeCheck();
            position = Handles.PositionHandle(position, Quaternion.identity);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(component, "Moved building Waypoint");
                waypoints[i] = position;
                EditorUtility.SetDirty(component);
            }
        }
    }
}
