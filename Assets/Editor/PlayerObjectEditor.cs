using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerObject))]

public class PlayerObjectEditor : Editor
{
    private void OnSceneGUI()
    {
        PlayerObject component = target as PlayerObject;

        Vector3 position = component.carriedObjectPosition;

        EditorGUI.BeginChangeCheck();
        position = Handles.PositionHandle(position, Quaternion.identity);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(component, "Moved carry object Waypoint");
            component.carriedObjectPosition = position;
            EditorUtility.SetDirty(component);
        }
    }
}
