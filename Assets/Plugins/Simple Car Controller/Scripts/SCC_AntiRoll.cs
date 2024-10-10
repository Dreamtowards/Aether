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
/// Anti roll bars.
/// </summary>
[AddComponentMenu("BoneCracker Games/Simple Car Controller/SCC Antiroll")]
public class SCC_AntiRoll : MonoBehaviour {

    //  Rigidbody.
    private Rigidbody rigid;
    private Rigidbody Rigid {

        get {

            if (rigid == null)
                rigid = GetComponent<Rigidbody>();

            return rigid;

        }

    }

    //  Custom class for wheels.
    [System.Serializable]
    public class Wheels {

        public SCC_Wheel leftWheel;
        public SCC_Wheel rightWheel;
        public float force = 1000f;

    }

    //  All wheels.
    public Wheels[] wheels;

    private void FixedUpdate() {

        //  Getting all wheels for loop.
        for (int i = 0; i < wheels.Length; i++) {

            //  If left and right wheels are selected...
            if (wheels[i].leftWheel && wheels[i].rightWheel) {

                WheelHit FrontWheelHit;     //  Wheelhit data.

                //  Travel values for left and right wheels.
                float travelFL = 1.0f;
                float travelFR = 1.0f;

                //  Is left wheel grounded?
                bool groundedFL = wheels[i].leftWheel.WheelCollider.GetGroundHit(out FrontWheelHit);

                //  If so, calculate the travel distance. Otherwise distance will be 0.
                if (groundedFL)
                    travelFL = (-wheels[i].leftWheel.transform.InverseTransformPoint(FrontWheelHit.point).y - wheels[i].leftWheel.WheelCollider.radius) / wheels[i].leftWheel.WheelCollider.suspensionDistance;

                //  Is right wheel grounded?
                bool groundedFR = wheels[i].rightWheel.WheelCollider.GetGroundHit(out FrontWheelHit);

                //  If so, calculate the travel distance. Otherwise distance will be 0.
                if (groundedFR)
                    travelFR = (-wheels[i].rightWheel.transform.InverseTransformPoint(FrontWheelHit.point).y - wheels[i].rightWheel.WheelCollider.radius) / wheels[i].rightWheel.WheelCollider.suspensionDistance;

                //  Calculating the antiroll force.
                float antiRollForceFrontHorizontal = (travelFL - travelFR) * wheels[i].force;

                //  If wheel is grounded, apply the additional force.
                if (groundedFL)
                    Rigid.AddForceAtPosition(wheels[i].leftWheel.transform.up * -antiRollForceFrontHorizontal, wheels[i].leftWheel.transform.position);

                //  If wheel is grounded, apply the additional force.
                if (groundedFR)
                    Rigid.AddForceAtPosition(wheels[i].rightWheel.transform.up * antiRollForceFrontHorizontal, wheels[i].rightWheel.transform.position);

            }

        }

    }

}
