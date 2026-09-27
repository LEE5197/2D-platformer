using UnityEngine;
using System.Collections;
public class PlayerMoveFunction
{
	private Controller target;
	public PlayerMoveFunction(Controller player)
	{
		target = player;
	}
	public void Move()
	{
		float targetSpeed = target.inputVec.x * target.data.maxSpeed;
		float speedDif = targetSpeed - target.rigid.linearVelocityX;
		float force;

		if (target.inputVec.x * target.rigid.linearVelocityX > 0)
		{
			force = Mathf.Pow(Mathf.Abs(speedDif) * target.data.acceleration, target.data.velPower) * Mathf.Sign(speedDif);
		}
		else
		{
			force = Mathf.Pow(Mathf.Abs(speedDif) * target.data.decceleration, target.data.velPower) * Mathf.Sign(speedDif);
		}

		target.rigid.AddForceX(force);
	}

	public void MoveInAir()
	{
		float targetSpeed = target.inputVec.x * target.data.maxSpeed;
		float speedDif = targetSpeed - target.rigid.linearVelocityX;
		float force;

		if (target.inputVec.x * target.rigid.linearVelocityX > 0 && Mathf.Abs(target.rigid.linearVelocityX) > target.data.maxSpeed) return;

		if (target.inputVec.x * target.rigid.linearVelocityX > 0)
		{
			force = Mathf.Pow(Mathf.Abs(speedDif) * target.data.acceleration * target.data.inAirWeight, target.data.velPower) * Mathf.Sign(speedDif);
		}
		else
		{
			force = Mathf.Pow(Mathf.Abs(speedDif) * target.data.decceleration * target.data.inAirWeight, target.data.velPower) * Mathf.Sign(speedDif);
		}

		target.rigid.AddForceX(force);
	}

	public void Jump()
	{
		target.coyoteTimer = target.data.coyoteTime;
		target.bufferTimer = target.data.bufferTime;
		target.rigid.linearVelocityY = 0f;

		Vector2 jumpForce = Vector2.zero;

		/*
		if (target.accel_platform != null && Time.fixedDeltaTime != 0)
		{
			jumpForce = target.accel_platform.force / Time.fixedDeltaTime;
		}
		*/

		jumpForce.y += target.data.jumpForce;
		target.rigid.AddForce(jumpForce, ForceMode2D.Impulse);
	}

	public void WallJump()
	{
		target.coyoteTimer = target.data.coyoteTime;
		target.bufferTimer = target.data.bufferTime;
		target.rigid.linearVelocity = Vector2.zero;

		Vector2 jumpDir = target.data.wallJumpRate.normalized;
		jumpDir.x *= target.render.flipX ? 1 : -1;

		Vector2 jumpForce = Vector2.zero;

		/*
		if (target.accel_platform != null && Time.fixedDeltaTime != 0)
		{
			jumpForce = target.accel_platform.force / Time.fixedDeltaTime;
		}
		*/

		jumpForce += jumpDir * target.data.wallJumpForce;
		target.rigid.AddForce(jumpForce, ForceMode2D.Impulse);
	}
}