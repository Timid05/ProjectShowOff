
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FollowPath))]

public class FollowPathEditor : Editor
{
    public override void OnInspectorGUI()
    {


        FollowPath component = target as FollowPath;

        GUILayout.Space(20);
        if (GUILayout.Button("Toggle Path Editor"))
        {
            if (component.drawPath)
            {
                component.drawPath = false;
                return;
            }
            else
            {
                component.drawPath = true;
            }
        }
        GUILayout.Space(20);

        base.OnInspectorGUI();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Speed: ");
        if (GUILayout.Button("-")) component.navmeshAgent.speed--;
        component.navmeshAgent.speed = EditorGUILayout.FloatField(component.navmeshAgent.speed);
        if (GUILayout.Button("+")) component.navmeshAgent.speed++;
        GUILayout.EndHorizontal();



        if (component.followType == FollowPath.FollowType.Target && component.target == null)
        {
            EditorGUILayout.HelpBox("Please drag in the desired target", MessageType.Warning);
        }
        else if (component.GetPath().Count < 2)
        {
            EditorGUILayout.HelpBox("This script needs at least 2 waypoints in its path to function", MessageType.Warning);
        }
    }

    private void OnSceneGUI()
    {
        FollowPath component = target as FollowPath;

        if (component.drawPath)
        {
            for (int i = 0; i < component.GetPath().Count; i++)
            {
                Vector3 position = component.GetPath()[i];

                EditorGUI.BeginChangeCheck();
                position = Handles.PositionHandle(position, Quaternion.identity);
                GUIStyle style = new GUIStyle();
                style.alignment = TextAnchor.MiddleCenter;
                style.fontSize = 20;
                style.normal.textColor = Color.cyan;
                Handles.Label(position, i.ToString(), style);

                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(component, "Moved Waypoint");
                    component.GetPath()[i] = position;
                    EditorUtility.SetDirty(component);
                }
            }
        }
    }
}
