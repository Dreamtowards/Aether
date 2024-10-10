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
/// Handling particle systems on vehicle. Exhausts and wheel particles.
/// </summary>
[AddComponentMenu("BoneCracker Games/Simple Car Controller/SCC Particles")]
public class SCC_Particles : MonoBehaviour {

    //  Input processor.
    private SCC_InputProcessor inputProcessor;
    public SCC_InputProcessor InputProcessor {

        get {

            if (inputProcessor == null)
                inputProcessor = GetComponent<SCC_InputProcessor>();

            return inputProcessor;

        }

    }

    //  All wheels of the vehicle.
    private SCC_Wheel[] wheels;

    //  Exhaust.
    public ParticleSystem[] exhaustParticles;
    private ParticleSystem.EmissionModule[] exhaustEmissions;

    //  Wheel particle prefab will be instantiated for each wheel.
    public ParticleSystem wheelParticlePrefab;

    //  Created wheel particles for all wheels.
    private List<ParticleSystem> createdParticles = new List<ParticleSystem>();
    private ParticleSystem.EmissionModule[] wheelEmissions;

    public float slip = .25f;       //  If wheel sideways or forward skid exceeds this limit slip, emission will be enabled.

    private void Awake() {

        //  Getting all wheels of the vehicle.
        wheels = GetComponentsInChildren<SCC_Wheel>();

        //  If wheel particle is selected, instantiate it per each wheel. Disable their emissions at start, and store them in the list.
        if (wheelParticlePrefab) {

            for (int i = 0; i < wheels.Length; i++) {

                ParticleSystem particle = Instantiate(wheelParticlePrefab, wheels[i].transform.position, wheels[i].transform.rotation, wheels[i].transform);
                createdParticles.Add(particle.GetComponent<ParticleSystem>());

            }

            wheelEmissions = new ParticleSystem.EmissionModule[createdParticles.Count];

            for (int i = 0; i < createdParticles.Count; i++)
                wheelEmissions[i] = createdParticles[i].emission;

        }

        //  If exhaust particles selected, disable their emissions at start.
        if (exhaustParticles != null && exhaustParticles.Length >= 1) {

            exhaustEmissions = new ParticleSystem.EmissionModule[exhaustParticles.Length];

            for (int i = 0; i < exhaustParticles.Length; i++)
                exhaustEmissions[i] = exhaustParticles[i].emission;

        }

    }

    private void Update() {

        WheelParticles();
        ExhaustParticles();

    }

    /// <summary>
    /// Enabling / disabling emission of the wheel particles.
    /// </summary>
    private void WheelParticles() {

        //  If wheel particle is not selected, return.
        if (!wheelParticlePrefab)
            return;

        //  If wheel particle is not selected, return.
        if (createdParticles.Count < 1)
            return;

        //  Getting sideways and forward slips of the each wheel and deciding to enable emission or not.
        for (int i = 0; i < wheels.Length; i++) {

            WheelHit hit;
            wheels[i].WheelCollider.GetGroundHit(out hit);

            if (Mathf.Abs(hit.sidewaysSlip) >= slip || Mathf.Abs(hit.forwardSlip) >= slip)
                wheelEmissions[i].enabled = true;
            else
                wheelEmissions[i].enabled = false;

        }

    }

    /// <summary>
    /// Enabling / disabling emission of the exhaust particles.
    /// </summary>
    private void ExhaustParticles() {

        if (exhaustParticles.Length < 1)
            return;

        for (int i = 0; i < exhaustEmissions.Length; i++)
            exhaustEmissions[i].rate = Mathf.Lerp(1f, 20f, InputProcessor.inputs.throttleInput);

    }

}
