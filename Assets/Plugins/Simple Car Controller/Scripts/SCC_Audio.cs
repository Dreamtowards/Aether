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
/// Handles engine audio based on vehicle engine rpm and speed.
/// </summary>
[AddComponentMenu("BoneCracker Games/Simple Car Controller/SCC Audio")]
public class SCC_Audio : MonoBehaviour {

    //  Drivetrain.
    private SCC_Drivetrain drivetrain;
    public SCC_Drivetrain Drivetrain {

        get {

            if (drivetrain == null)
                drivetrain = GetComponent<SCC_Drivetrain>();

            return drivetrain;

        }

    }

    //  Input processor.
    private SCC_InputProcessor inputProcessor;
    public SCC_InputProcessor InputProcessor {

        get {

            if (inputProcessor == null)
                inputProcessor = GetComponent<SCC_InputProcessor>();

            return inputProcessor;

        }

    }

    //  Audioclips.
    public AudioClip engineOn;
    public AudioClip engineOff;

    //  Audiosources.
    private AudioSource engineOnSource;
    private AudioSource engineOffSource;

    //  Volume values for min and max volume.
    public float minimumVolume = .1f;
    public float maximumVolume = 1f;

    //  Pitch values for min and max pitch.
    public float minimumPitch = .75f;
    public float maximumPitch = 1.25f;

    private void Start() {

        //  Creating two audiosources for engine on and off clips.
        GameObject engineOnGO = new GameObject("Engine On AudioSource");
        engineOnGO.transform.SetParent(transform, false);
        engineOnSource = engineOnGO.AddComponent<AudioSource>();
        engineOnSource.clip = engineOn;
        engineOnSource.loop = true;
        engineOnSource.spatialBlend = 1f;
        engineOnSource.Play();

        GameObject engineOffGO = new GameObject("Engine Off AudioSource");
        engineOffGO.transform.SetParent(transform, false);
        engineOffSource = engineOffGO.AddComponent<AudioSource>();
        engineOffSource.clip = engineOff;
        engineOffSource.loop = true;
        engineOffSource.spatialBlend = 1f;
        engineOffSource.Play();

    }

    private void Update() {

        //  If there is no drivetrain attached to the vehicle or input processor, return and disable this component.
        if (!Drivetrain || !InputProcessor) {

            enabled = false;
            return;

        }

        //  Calculating the target volume depends on the throttle / brake.
        float volume = Drivetrain.direction == 1 ? InputProcessor.inputs.throttleInput : InputProcessor.inputs.brakeInput;

        //  Setting volumes.
        engineOnSource.volume = Mathf.Lerp(minimumVolume, maximumVolume, volume);
        engineOffSource.volume = Mathf.Lerp(maximumVolume, 0f, volume);

        //  Setting pitches.
        engineOnSource.pitch = Mathf.Lerp(minimumPitch, maximumPitch, Drivetrain.currentEngineRPM / Drivetrain.maximumEngineRPM);
        engineOffSource.pitch = engineOnSource.pitch;

    }

}
