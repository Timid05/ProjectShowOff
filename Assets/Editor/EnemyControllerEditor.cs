using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyController))]
public class EnemyControllerEditor : Editor
{
    bool editSpeedByState;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EnemyController component = target as EnemyController;
        GUILayout.Space(10);
        
        GUILayout.Space(10);

        foreach (EnemyStateMachine.State state in Enum.GetValues(typeof(EnemyStateMachine.State)))
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.EnumPopup("State Speed", state);
            EditorGUILayout.FloatField(0);
            EditorGUILayout.EndHorizontal();
        }     
    }
}
