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
		// Is Gameplay Control/Input Enabled e.g. WSAD/CursorLook etc
		public static bool IsPlayingInput;// { get; private set; }

		public static void UpdateIsPlayingInput()
		{
			IsPlayingInput = UIManager.CurrentScreen == null && !Input.GetKey(KeyCode.LeftAlt);
			Utility.LockCursor(InputManager.IsPlayingInput);
		}
		
		
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

		public InputAction actionCameraDistanceModifier;
		// public InputActionReference actionCameraDistanceModifierRef;

		private void Start() {
			Assert.IsNull(instance);
			instance = this;
			
			// LockMouse
			UpdateIsPlayingInput();

			actionCameraDistanceModifier.Enable();
			// actionCameraDistanceModifierRef.action.Enable();
		}

		void Update()
		{
			if (Input.GetKeyDown(KeyCode.LeftAlt) || Input.GetKeyUp(KeyCode.LeftAlt))
				UpdateIsPlayingInput();
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

		public bool isFlying;

		public void OnJump(InputValue value)
		{
			var time = Time.time;
			if (time - m_TimeLastJump < 0.3f) {
				// Double Click Jump
				isFlying = !isFlying;
			}
			m_TimeLastJump = time;

			if (isFlying)
				return;  // No Jump when DoubleJump Fly
			
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

		void OnEscape(InputValue val)
		{
			if (UIManager.CurrentScreen)
				UIManager.PopScreen();
			else 
				UIManager.PushScreen(UIManager.instance.ScreenPause);
		}

		void OnCommand(InputValue val)
		{
			if (IsPlayingInput)
				UIManager.PushScreen(UIManager.instance.ScreenChat);
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

		// private void OnApplicationFocus(bool hasFocus)
		// {
		// 	Utility.LockCursor(IsPlayingInput && cursorLocked && hasFocus);
		// }
	}
	
}