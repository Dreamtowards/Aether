//----------------------------------------------
//            Simple Car Controller
//
// Copyright © 2014 - 2023 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Class was used for creating new WheelColliders on Editor.
/// </summary>
public class SCC_CreateWheelCollider {

    // Method was used for creating new WheelColliders on Editor.
    public static WheelCollider CreateWheelCollider(GameObject car, Transform wheel) {

        // If we don't have any wheelmodels, throw an error.
        if (!wheel) {
            Debug.LogError("You haven't selected your Wheel Model. Please your Wheel Model before creating Wheel Colliders. Script needs to know their sizes and positions, aye?");
            return null;
        }

        // Holding default rotation.
        Quaternion currentRotation = car.transform.rotation;

        // Resetting rotation.
        car.transform.rotation = Quaternion.identity;

        // Creating a new gameobject called Wheel Colliders for all Wheel Colliders, and parenting it to this gameobject.
        GameObject wheelColliders;

        if (!car.transform.Find("Wheel Colliders")) {

            wheelColliders = new GameObject("Wheel Colliders");
            wheelColliders.transform.SetParent(car.transform, false);
            wheelColliders.transform.localRotation = Quaternion.identity;
            wheelColliders.transform.localPosition = Vector3.zero;
            wheelColliders.transform.localScale = Vector3.one;

        } else {

            wheelColliders = car.transform.Find("Wheel Colliders").gameObject;

        }

        GameObject wheelcollider = new GameObject(wheel.transform.name);

        wheelcollider.transform.position = wheel.transform.position;
        wheelcollider.transform.rotation = car.transform.rotation;
        wheelcollider.transform.name = wheel.transform.name;
        wheelcollider.transform.SetParent(wheelColliders.transform);
        wheelcollider.transform.localScale = Vector3.one;
        wheelcollider.AddComponent<WheelCollider>();

        Bounds biggestBound = new Bounds();
        Renderer[] renderers = wheel.GetComponentsInChildren<Renderer>();

        foreach (Renderer render in renderers) {
            if (render.bounds.size.z > biggestBound.size.z)
                biggestBound = render.bounds;
        }

        wheelcollider.GetComponent<WheelCollider>().radius = (biggestBound.extents.y) / car.transform.localScale.y;
        wheelcollider.AddComponent<SCC_Wheel>();

        JointSpring spring = wheelcollider.GetComponent<WheelCollider>().suspensionSpring;

        spring.spring = 35000f;
        spring.damper = 1500f;
        spring.targetPosition = .5f;

        wheelcollider.GetComponent<WheelCollider>().suspensionSpring = spring;
        wheelcollider.GetComponent<WheelCollider>().suspensionDistance = .2f;
        wheelcollider.GetComponent<WheelCollider>().forceAppPointDistance = .1f;
        wheelcollider.GetComponent<WheelCollider>().mass = 40f;
        wheelcollider.GetComponent<WheelCollider>().wheelDampingRate = 1f;

        WheelFrictionCurve sidewaysFriction;
        WheelFrictionCurve forwardFriction;

        sidewaysFriction = wheelcollider.GetComponent<WheelCollider>().sidewaysFriction;
        forwardFriction = wheelcollider.GetComponent<WheelCollider>().forwardFriction;

        forwardFriction.extremumSlip = .3f;
        forwardFriction.extremumValue = 1;
        forwardFriction.asymptoteSlip = 1f;
        forwardFriction.asymptoteValue = 1f;
        forwardFriction.stiffness = 1.5f;

        sidewaysFriction.extremumSlip = .3f;
        sidewaysFriction.extremumValue = 1;
        sidewaysFriction.asymptoteSlip = 1f;
        sidewaysFriction.asymptoteValue = 1f;
        sidewaysFriction.stiffness = 1.5f;

        wheelcollider.GetComponent<WheelCollider>().sidewaysFriction = sidewaysFriction;
        wheelcollider.GetComponent<WheelCollider>().forwardFriction = forwardFriction;

        car.transform.rotation = currentRotation;

        return wheelcollider.GetComponent<WheelCollider>();

    }

}
