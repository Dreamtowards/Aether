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
/// Used on SCC demo scenes to instantiate new vehicles, and set target vehicle of the camera and canvas.
/// </summary>
[AddComponentMenu("BoneCracker Games/Simple Car Controller/SCC Demo")]
public class SCC_Demo : MonoBehaviour {

    public GameObject[] spawnableCars;      //  Spawnable vehicles.
    public Transform defaultSpawnPoint;     //  Spawn point.

    public bool destroyAllCars = true;         //   Destroy all other existing vehicles on new spawns?

    /// <summary>
    /// Spawns new vehicle.
    /// </summary>
    /// <param name="selectedCar"></param>
    public void SpawnCar(int selectedCar) {

        Vector3 spawnPoint;
        Quaternion spawnRot;

        //  Getting all other existing vehicles and destroying them.
        if (destroyAllCars) {

            SCC_Drivetrain[] cars = FindObjectsOfType<SCC_Drivetrain>();

            foreach (SCC_Drivetrain car in cars)
                Destroy(car.gameObject);

        }

        //  Getting camera and canvas components.
        SCC_Camera camera = FindObjectOfType<SCC_Camera>();
        SCC_Dashboard dashboard = FindObjectOfType<SCC_Dashboard>();

        //  If camera found, get it's position and rotation to spawn new vehicles at that vector3.
        if (camera) {

            spawnPoint = camera.transform.position;
            spawnRot = camera.transform.rotation;
            spawnPoint += camera.transform.forward * camera.distance;

            //  If camera is not found, take the default spawn point.
        } else {

            if (defaultSpawnPoint) {

                spawnPoint = defaultSpawnPoint.position;
                spawnRot = defaultSpawnPoint.rotation;

            } else {

                spawnPoint = Vector3.zero;
                spawnRot = Quaternion.identity;

            }

        }

        //  Instantiating new vehicle.
        GameObject playerCar = Instantiate(spawnableCars[selectedCar], spawnPoint, spawnRot);

        //  If camera found, set target of the camera.
        if (camera)
            camera.playerCar = playerCar.GetComponent<SCC_Drivetrain>().transform;

        //  If canvas found, set target of the canvas.
        if (dashboard)
            dashboard.car = playerCar.GetComponent<SCC_Drivetrain>();

    }

}
