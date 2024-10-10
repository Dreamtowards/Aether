//----------------------------------------------
//            Simple Car Controller
//
// Copyright © 2014 - 2023 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Input processor of the vehicle.
/// </summary>
[AddComponentMenu("BoneCracker Games/Simple Car Controller/SCC Input Processor")]
public class SCC_InputProcessor : MonoBehaviour {

    public SCC_Inputs inputs = new SCC_Inputs();        //  Target inputs.

    public bool receiveInputsFromInputManager = true;       //  Receive inputs directly from the SCC_InputManager.
    public bool smoothInputs = true;        //  Smoothly lerp the inputs?
    public float smoothingFactor = 5f;      //  Smoothing factor.

    private void Update() {

        //  Creating new inputs if doesn't exists yet.
        if (inputs == null)
            inputs = new SCC_Inputs();

        //Receiving all inputs from the SCC_InputManager.
        if (receiveInputsFromInputManager) {

            if (smoothInputs) {

                inputs.throttleInput = Mathf.MoveTowards(inputs.throttleInput, SCC_InputManager.Instance.inputs.throttleInput, Time.deltaTime * smoothingFactor);
                inputs.steerInput = Mathf.MoveTowards(inputs.steerInput, SCC_InputManager.Instance.inputs.steerInput, Time.deltaTime * smoothingFactor);
                inputs.brakeInput = Mathf.MoveTowards(inputs.brakeInput, SCC_InputManager.Instance.inputs.brakeInput, Time.deltaTime * smoothingFactor);
                inputs.handbrakeInput = Mathf.MoveTowards(inputs.handbrakeInput, SCC_InputManager.Instance.inputs.handbrakeInput, Time.deltaTime * smoothingFactor);

            } else {

                inputs = SCC_InputManager.Instance.inputs;

            }

        }

    }

    /// <summary>
    /// Overrides inputs with given inputs. Be sure to disable the receiveInputsFromInputManager while overriding inputs. 
    /// </summary>
    /// <param name="newInputs"></param>
    public void OverrideInputs(SCC_Inputs newInputs) {

        if (!smoothInputs) {

            inputs = newInputs;

        } else {

            inputs.throttleInput = Mathf.MoveTowards(inputs.throttleInput, newInputs.throttleInput, Time.deltaTime * smoothingFactor);
            inputs.steerInput = Mathf.MoveTowards(inputs.steerInput, newInputs.steerInput, Time.deltaTime * smoothingFactor);
            inputs.brakeInput = Mathf.MoveTowards(inputs.brakeInput, newInputs.brakeInput, Time.deltaTime * smoothingFactor);
            inputs.handbrakeInput = Mathf.MoveTowards(inputs.handbrakeInput, newInputs.handbrakeInput, Time.deltaTime * smoothingFactor);

        }

    }

}
