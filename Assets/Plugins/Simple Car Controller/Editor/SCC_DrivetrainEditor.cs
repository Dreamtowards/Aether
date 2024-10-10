//----------------------------------------------
//            Simple Car Controller
//
// Copyright © 2014 - 2018 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(SCC_Drivetrain))]
public class SCC_DrivetrainEditor : Editor {

    SCC_Drivetrain driveTrainScript;
    SCC_Drivetrain.SCC_Wheels[] wheels;
    Transform[] wheelModels;
    Color defaultGUIColor;

    public override void OnInspectorGUI() {

        driveTrainScript = (SCC_Drivetrain)target;
        serializedObject.Update();
        defaultGUIColor = GUI.color;

        EditorGUILayout.Space();

        wheels = driveTrainScript.wheels;

        float defaultLabelWidth = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = 100f;

        if (wheels != null) {

            for (int i = 0; i < wheels.Length; i++) {

                EditorGUILayout.BeginVertical(GUI.skin.box);

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Wheels " + i.ToString(), EditorStyles.boldLabel);
                GUILayout.FlexibleSpace();

                GUI.color = Color.red;

                if (GUILayout.Button("X"))
                    RemoveWheel(i);

                GUI.color = defaultGUIColor;

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();

                wheels[i].wheelTransform = (Transform)EditorGUILayout.ObjectField("Wheel Model", wheels[i].wheelTransform, typeof(Transform), true);
                wheels[i].wheelCollider = (SCC_Wheel)EditorGUILayout.ObjectField("Wheel Collider", wheels[i].wheelCollider, typeof(SCC_Wheel), true);

                if (wheels[i].wheelCollider == null) {

                    GUI.color = Color.cyan;

                    if (GUILayout.Button("Create WheelCollider")) {

                        WheelCollider newWheelCollider = SCC_CreateWheelCollider.CreateWheelCollider(driveTrainScript.gameObject, wheels[i].wheelTransform);
                        wheels[i].wheelCollider = newWheelCollider.gameObject.GetComponent<SCC_Wheel>();
                        wheels[i].wheelCollider.wheelModel = wheels[i].wheelTransform;
                        Debug.Log("Created wheelcollider for " + wheels[i].wheelTransform.name);

                    }

                    GUI.color = defaultGUIColor;

                }

                EditorGUILayout.EndHorizontal();

                EditorGUIUtility.labelWidth = 20f;

                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal(GUI.skin.button);
                wheels[i].isSteering = EditorGUILayout.ToggleLeft("Steering", wheels[i].isSteering);
                wheels[i].isTraction = EditorGUILayout.ToggleLeft("Traction", wheels[i].isTraction);
                wheels[i].isBrake = EditorGUILayout.ToggleLeft("Brake", wheels[i].isBrake);
                wheels[i].isHandbrake = EditorGUILayout.ToggleLeft("Handbrake", wheels[i].isHandbrake);
                EditorGUILayout.Space();
                EditorGUILayout.EndHorizontal();

                EditorGUIUtility.labelWidth = 100f;

                if (wheels[i].isSteering)
                    wheels[i].steeringAngle = EditorGUILayout.Slider("Steer Angle", wheels[i].steeringAngle, -45f, 45f);

                EditorGUILayout.Space();

                EditorGUILayout.EndVertical();

            }

        }

        GUI.color = Color.green;
        EditorGUILayout.Space();

        if (GUILayout.Button("Create New Wheel Slot"))
            AddNewWheel();

        GUI.color = defaultGUIColor;
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Configurations", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        EditorGUIUtility.labelWidth = defaultLabelWidth;

        EditorGUILayout.PropertyField(serializedObject.FindProperty("COM"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("engineTorque"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("brakeTorque"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("maximumSpeed"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("finalDriveRatio"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("highSpeedSteerAngle"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("direction"));

        if (GUI.changed)
            EditorUtility.SetDirty(driveTrainScript);

        serializedObject.ApplyModifiedProperties();

    }

    private void AddNewWheel() {

        List<SCC_Drivetrain.SCC_Wheels> currentWheels = new List<SCC_Drivetrain.SCC_Wheels>();

        if (wheels != null)
            currentWheels.AddRange(wheels);

        currentWheels.Add(null);

        driveTrainScript.wheels = currentWheels.ToArray();

    }

    private void RemoveWheel(int index) {

        List<SCC_Drivetrain.SCC_Wheels> currentWheels = new List<SCC_Drivetrain.SCC_Wheels>();

        if (wheels != null)
            currentWheels.AddRange(wheels);

        if (currentWheels[index].wheelCollider != null)
            DestroyImmediate(currentWheels[index].wheelCollider.gameObject);

        currentWheels.RemoveAt(index);

        driveTrainScript.wheels = currentWheels.ToArray();

    }

}
