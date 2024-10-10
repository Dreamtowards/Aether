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
/// Camera system for SCC. 
/// </summary>
[AddComponentMenu("BoneCracker Games/Simple Car Controller/SCC Camera")]
public class SCC_Camera : MonoBehaviour {

    public Transform playerCar;     //  Target vehicle.

    public float distance = 6f;      //  Distance.
    public float height = 2f;     //  Height.
    public float heightDamping = 1.2f;      //  Height damping.

    public bool useCameraCollision = true;      //  Use camera collision?
    public float closerRadius = 0.2f;       //  Radius of the collision.
    public float closerSnapLag = 0.2f;      //  Closer point of the collision.
    public float rotationSnapTime = 0.3F;       //  Rotation snap time.

    private Vector3 wantedPosition = Vector3.zero;      //  Wanted position.
    private float wantedRotationAngle = 0f;     //  Wanted rotation angle as degree.
    private float wantedHeight = 0f;        //  Wanted height.
    private float currentRotationAngle = 0f;        //  Current rotation angle as degree.
    public Vector3 lookAtOffset = Vector3.zero;

    private float currentDistance = 0f;       //  Current distance.
    private float yVelocity = 0f;
    private float zVelocity = 0f;
    private float targetDistance = 0f;        //  Target distance.

    private void Start() {

        //  Setting current distance at start of the game same as distance.
        currentDistance = distance;

    }

    private void LateUpdate() {

        //  If there is no target vehicle, return.
        if (!playerCar)
            return;

        //  Calculating wanted height depends on the target vehicle position Y.
        wantedHeight = playerCar.position.y + height;

        //  Angles of the target vehicle and camera.
        wantedRotationAngle = playerCar.eulerAngles.y;
        currentRotationAngle = transform.eulerAngles.y;

        //  Smoothly damping angle of the camera to wanted rotation angle.
        currentRotationAngle = Mathf.SmoothDampAngle(currentRotationAngle, wantedRotationAngle, ref yVelocity, rotationSnapTime);

        //  If camera collision is enabled, use raycast to detect obstacles and calculate the hit distance.
        if (useCameraCollision) {

            RaycastHit hit;

            if (Physics.Raycast(playerCar.position, transform.TransformDirection(-Vector3.forward), out hit, distance) && !hit.transform.IsChildOf(playerCar))
                targetDistance = hit.distance - closerRadius;
            else
                targetDistance = distance;

        } else {

            targetDistance = distance;

        }

        //  Smoothly damping the current distance to the target distance.
        currentDistance = Mathf.SmoothDamp(currentDistance, targetDistance, ref zVelocity, closerSnapLag * 0.3f);

        //  Wanted position of the target vehicle.
        wantedPosition = playerCar.position;

        //  Y of the wanted position.
        wantedPosition.y = wantedHeight;

        //  Moving wanted position to backwards related to calculated distance.
        wantedPosition += Quaternion.Euler(0, currentRotationAngle, 0) * new Vector3(0, 0, -currentDistance);

        //  And finally setting position and rotation of the camera.
        transform.position = wantedPosition;
        transform.LookAt(playerCar.position + lookAtOffset);

    }

}
