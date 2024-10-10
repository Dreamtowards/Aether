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
public class SCC_EditorWindows : Editor {

    [MenuItem("Tools/BoneCracker Games/Simple Car Controller/Add Main Controller To Vehicle", false)]
    public static void CreateBehavior() {

        if (!Selection.activeGameObject.GetComponentInParent<SCC_Drivetrain>()) {

            GameObject pivot = new GameObject(Selection.activeGameObject.name);
            pivot.transform.position = SCC_GetBounds.GetBoundsCenter(Selection.activeGameObject.transform);
            pivot.transform.rotation = Selection.activeGameObject.transform.rotation;
            pivot.transform.localScale = Vector3.one;

            pivot.AddComponent<SCC_Drivetrain>();

            Selection.activeGameObject.transform.SetParent(pivot.transform);
            Selection.activeGameObject = pivot;

            EditorUtility.DisplayDialog("SCC Initialized", "SCC Initialized. Select all of your wheels and create their wheelcolliders.", "Ok");

        } else {

            EditorUtility.DisplayDialog("Your Gameobject Already Has Simple Car Controller", "Your gameobject already has simple car controller.", "Ok");

        }

    }

    [MenuItem("Tools/BoneCracker Games/Simple Car Controller/Add Main Controller To Vehicle", true)]
    public static bool CheckCreateBehavior() {

        if (Selection.gameObjects.Length > 1 || !Selection.activeTransform)
            return false;
        else
            return true;

    }

    [MenuItem("Tools/BoneCracker Games/Simple Car Controller/Add SCC Camera To Scene", false)]
    public static void CreateCamera() {

        if (!FindObjectOfType<SCC_Camera>()) {

            Selection.activeGameObject = (GameObject)Instantiate(Resources.Load("SCC_Camera", typeof(GameObject)));

        } else {

            EditorUtility.DisplayDialog("Your Scene Already Has SCC_Camera", "Your scene already has SCC_Camera.", "Ok");
            Selection.activeGameObject = FindObjectOfType<SCC_Camera>().gameObject;

        }

    }

    [MenuItem("Tools/BoneCracker Games/Simple Car Controller/Add SCC Canvas To Scene", false)]
    public static void CreateCanvas() {

        if (!FindObjectOfType<SCC_Dashboard>()) {

            Selection.activeGameObject = (GameObject)Instantiate(Resources.Load("SCC_Canvas", typeof(GameObject)));

        } else {

            EditorUtility.DisplayDialog("Your Scene Already Has SCC_Canvas", "Your scene already has SCC_Canvas.", "Ok");
            Selection.activeGameObject = FindObjectOfType<SCC_Dashboard>().gameObject;

        }

    }

}
