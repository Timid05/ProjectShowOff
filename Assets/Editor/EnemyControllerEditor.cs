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

        if (component.fogs.Count < Enum.GetValues(typeof(EnemyStateMachine.State)).Length)
        {
            component.fogs.Add(0);
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

        EditorGUILayout.Space(20);

        for (int i = 0; i < component.fogs.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Fog " + (EnemyStateMachine.State)i, EditorStyles.boldLabel);

            EditorGUI.BeginChangeCheck();
            Undo.RecordObject(component, "fog distance changed");
            component.fogs[i] = EditorGUILayout.Slider(component.fogs[i], 0, 3);

            if (EditorGUI.EndChangeCheck()  )
            {           
                EditorUtility.SetDirty(component);
                if (component.fsm != null)
                {
                    component.UpdateFogDistances();
                }      
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
