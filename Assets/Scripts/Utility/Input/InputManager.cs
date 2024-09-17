using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Assertions;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace Aether
{
	public class InputManager : MonoBehaviour
	{
		public bool enabledGameInputs;
		
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

		public PlayerInput m_PlayerInput;
		
		public static bool IsCurrentDeviceMouse => instance.m_PlayerInput.currentControlScheme == "KeyboardMouse";

		public static InputManager instance;

		private void Start() {
			Assert.IsNull(instance);
			instance = this;
			
			
		}

#if ENABLE_INPUT_SYSTEM
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		private float m_TimeLastJump;

		public void OnJump(InputValue value)
		{
			var time = Time.time;
			if (time - m_TimeLastJump < 0.3f) {
				// Double Click Jump
			}
			m_TimeLastJump = time;
			
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

		void OnEscape(InputValue val)
		{
			enabledGameInputs = !enabledGameInputs;
		}
#endif
		

		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

		private void OnApplicationFocus(bool hasFocus)
		{
			if (enabledGameInputs)
				LockCursor(cursorLocked && hasFocus);
		}

		public static void LockCursor(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}