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
/// Handling UI dashboard. RPM and KMH gauges.
/// </summary>
[AddComponentMenu("BoneCracker Games/Simple Car Controller/SCC Dashboard")]
public class SCC_Dashboard : MonoBehaviour {

    public SCC_Drivetrain car;      //  Target vehicle.

    //  RPM and KMH.
    private float rpm = 0f;
    private float kmh = 0f;

    //  Needles.
    public RectTransform RPMNeedle;
    public RectTransform KMHNeedle;

    //  Multipliers of the needles for adjusting min and max rotations.
    public float RPMNeedleMultiplier = 1.2f;
    public float KMHNeedleMultiplier = 1.2f;

    //  Original angles of the needles.
    private float orgRPMNeedleAngle = 0f;
    private float orgKMHNeedleAngle = 0f;

    private void Awake() {

        //  Getting original angle values of the needles.
        orgRPMNeedleAngle = RPMNeedle.transform.localEulerAngles.z;
        orgKMHNeedleAngle = KMHNeedle.transform.localEulerAngles.z;

    }

    private void Update() {

        //  If there is no target vehicle, return.
        if (!car) {

            rpm = 0f;
            kmh = 0f;

        } else {

            rpm = car.currentEngineRPM * RPMNeedleMultiplier;
            kmh = car.speed * KMHNeedleMultiplier;

        }

        //  Rotating the needles smoothly.
        Quaternion target = Quaternion.Euler(0f, 0f, orgKMHNeedleAngle - kmh);
        KMHNeedle.rotation = Quaternion.Slerp(KMHNeedle.rotation, target, Time.deltaTime * 2f);

        Quaternion target2 = Quaternion.Euler(0f, 0f, orgRPMNeedleAngle - (rpm / 40f));
        RPMNeedle.rotation = Quaternion.Slerp(RPMNeedle.rotation, target2, Time.deltaTime * 2f);

    }

}
