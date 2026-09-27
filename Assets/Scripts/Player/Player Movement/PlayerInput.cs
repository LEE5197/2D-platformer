using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerInput : MonoBehaviour
{
	public Vector2 inputVec { get; private set; }
	void OnMove(InputValue value)
	{
		inputVec = value.Get<Vector2>();
	}
	
	public bool jumpPressed { get; private set; }
	public event Action OnJumpEvent;
	void OnJump(InputValue value)
	{
		jumpPressed = value.isPressed;

		if (jumpPressed)
		{
			OnJumpEvent?.Invoke();
		}
	}

	public bool dashPressed { get; private set; }
	public event Action OnDashEvent;
	void OnDash(InputValue value)
	{
		dashPressed = value.isPressed;

		if (dashPressed)
		{
			OnDashEvent?.Invoke();
		}
	}

	public event Action OnLeftClickEvent;
	public bool LeftPressed { get; private set; }
	void OnLeftClick(InputValue value)
	{
		bool isPressed = value.isPressed;
		if (LeftPressed == isPressed) return;

		LeftPressed = value.isPressed;
		OnLeftClickEvent?.Invoke();
	}

	public event Action OnRightClickEvent;
	public bool RightPressed { get; private set; }
	void OnRightClick(InputValue value)
	{
		bool isPressed = value.isPressed;
		if (RightPressed == isPressed) return;

		RightPressed = isPressed;

		RightPressed = value.isPressed;
		OnRightClickEvent?.Invoke();
	}
}
