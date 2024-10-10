//----------------------------------------------
//            Simple Car Controller
//
// Copyright © 2014 - 2023 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using System.Collections;

/// <summary>
/// Script was used for stabilizing the car to avoid flip overs.
/// </summary>
[AddComponentMenu("BoneCracker Games/Simple Car Controller/SCC Rigid Stabilizer")]
[RequireComponent(typeof(Rigidbody))]
public class SCC_RigidStabilizer : MonoBehaviour {

    private Rigidbody rigid;
    private Rigidbody Rigid {

        get {

            if (rigid == null)
                rigid = GetComponent<Rigidbody>();

            return rigid;

        }

    }

    private SCC_Wheel[] wheels;

    public float reflection = 100f;
    public float stability = .5f;

    private void Start() {

        wheels = GetComponentsInChildren<SCC_Wheel>();

    }

    private void FixedUpdate() {

        if (!Rigid) {

            enabled = false;
            return;

        }

        Vector3 predictedUp = Quaternion.AngleAxis(Rigid.linearVelocity.magnitude * Mathf.Rad2Deg * stability / reflection, Rigid.angularVelocity) * transform.up;
        Vector3 torqueVector = Vector3.Cross(predictedUp, Vector3.up);

        bool grounded = false;

        for (int i = 0; i < wheels.Length; i++) {

            if (wheels[i].isGrounded)
                grounded = true;

        }

        if (!grounded)
            Rigid.AddTorque(torqueVector * reflection * reflection);

    }

}
