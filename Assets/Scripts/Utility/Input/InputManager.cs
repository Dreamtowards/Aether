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
		public bool analogMovement;

		public PlayerInput m_PlayerInput;

		public static bool IsCurrentDeviceMouse => instance.m_PlayerInput.currentControlScheme == "KeyboardMouse";

		public static InputManager instance;

		public InputAction actionUse;
		public InputAction actionAttack;
		public InputAction actionDropItem;
		
		public InputAction actionSprint;
		public InputAction actionCameraZoom;
		
		public InputAction actionCameraDistanceModifier;
		// public InputActionReference actionCameraDistanceModifierRef;

		// the controlling player
		public EntityPlayer player;


		private void Start() {
			Assert.IsNull(instance);
			instance = this;
			
			// LockMouse
			UpdateIsPlayingInput();
			
			actionUse.Enable();
			actionAttack.Enable();
			actionDropItem.Enable();
			
			actionSprint.Enable();
			actionCameraZoom.Enable();
			actionCameraDistanceModifier.Enable();
			// actionCameraDistanceModifierRef.action.Enable();
		}

		void Update()
		{
			// Pause Game Control, Release Cursor
			if (Input.GetKeyDown(KeyCode.LeftAlt) || Input.GetKeyUp(KeyCode.LeftAlt))
				UpdateIsPlayingInput();


			if (actionDropItem.WasPressedThisFrame())
			{
				player.DropHoldingItem(Input.GetKey(KeyCode.LeftControl));
			}
			
			UpdateUseItem();
		}

		public bool isBreakingVoxel;

		public float itemUseTimeLeft;
		public float itemUseTimeMax;
		
		private void UpdateUseItem()
		{
			var holdItem = player.GetHoldingItem();

			if (player.isUsingItem)
			{
				if (!actionUse.IsPressed()) {
					// Sync Selection, UseStopped.
					// holdItem.item.OnUseStopped();
					player.isUsingItem = false;
				}
				
				itemUseTimeLeft -= Time.deltaTime;
				UIManager.instance.SetCrosshairProgress(1 - itemUseTimeLeft / itemUseTimeMax);

				if (itemUseTimeLeft < 0)
				{
					holdItem.item.OnUseCompleted(player, holdItem);
					player.isUsingItem = false;
				}
				else
				{
					holdItem.item.OnUsingTick();
				}
			}
			else
			{
				UIManager.instance.SetCrosshairProgress(-1);
				
				// Use
				if (actionUse.WasPressedThisFrame())
				{
					HandleHandUse();
				}

				// Attack
				if (actionAttack.WasPressedThisFrame())
				{
					DoAttack();
				}
				
				// Pick Item "MouseMiddleKey"
			}
			
		}

		private void HandleHandUse() {
			if (isBreakingVoxel)
				return;
			
			var hit = CursorRaycaster.instance.hitResult;

			if (hit.GetHitEntity(out var entity))
			{
				entity.Interact(player, hit.point);
			}
			else if (hit.isHitVoxel)
			{
				
			}
			
			var holdItem = player.GetHoldingItem();
			if (!holdItem.IsEmpty) {
				holdItem.item.OnUse(player, holdItem);
				itemUseTimeLeft = itemUseTimeMax = holdItem.item.GetMaxUseTime();
				if (itemUseTimeMax > 0) {
					player.isUsingItem = true;
				}
			}
		}

		public void DoAttack()
		{
			var hit = CursorRaycaster.instance.hitResult;
			if (hit.isHitVoxel)
			{
						
			}
			// player.Attack();
			// Entity or Voxel
			// PlayAnimSwingHand()
		}

		public void OnMove(InputValue value)
		{
			move = value.Get<Vector2>();
		}

		public void OnLook(InputValue value)
		{
			//cursorInputForLook
			look = value.Get<Vector2>();
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
			
			jump = value.isPressed;
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

	}
	
}