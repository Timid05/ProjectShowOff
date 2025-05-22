using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Subsystems;

[CustomEditor(typeof(EnemyController))]
public class EnemyControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.Space(20);

        EnemyController component = target as EnemyController;

        if (component.states.Count < Enum.GetValues(typeof(EnemyStateMachine.State)).Length)
        {
            Debug.Log("starting up states");
            foreach (EnemyStateMachine.State state in Enum.GetValues(typeof(EnemyStateMachine.State)))
            {
                component.states.Add(state);
                component.speeds.Add(0);
            }
        }

        foreach (EnemyStateMachine.State state in component.states)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(state.ToString() + " Speed", EditorStyles.boldLabel);

            EditorGUI.BeginChangeCheck();
            Undo.RecordObject(component, "State speed changed");
            component.speeds[(int)state] = EditorGUILayout.FloatField(component.speeds[(int)state]);

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(component);
                if (component.fsm != null)
                {
                    component.UpdateSpeeds();
                }
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
