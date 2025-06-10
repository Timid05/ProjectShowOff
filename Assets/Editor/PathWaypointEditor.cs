using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathWaypoint))]

public class PathWaypointEditor : Editor
{
    private void OnSceneGUI()
    {
        PathWaypoint component = target as PathWaypoint;

        // Add text for all the waypoints
        for (int i = 0; i < component.waypoints.Count; i++)
        {
            if (component.waypoints.Keys[i] != null)
            {
                Vector3 position = component.waypoints.Keys[i].transform.position;
                AddText(position, i, "(W)");
            }
        }

        // Add text to all the path objects
        AddPathText(component.northPath, 0);
        AddPathText(component.southPath, 1);
        AddPathText(component.westPath, 2);
        AddPathText(component.eastPath, 3);
    }

    void AddText(Vector3 position, int index, string addition)
    {
        string text = "";

        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.MiddleCenter;
        style.fontSize = 20;
        style.normal.textColor = Color.cyan;

        switch (index)
        {
            case 0:
                text = "N";
                break;
            case 1:
                text = "S";
                break;
            case 2:
                text = "W";
                break;
            case 3:
                text = "E";
                break;
        }

        Handles.Label(position, text + addition, style);
    }

    void AddPathText(GameObject waypointPath, int forcedIndex)
    {
        if (waypointPath != null)
        {
            Vector3 position = waypointPath.transform.position;
            AddText(position, forcedIndex, "(P)");
        }
    }
}
