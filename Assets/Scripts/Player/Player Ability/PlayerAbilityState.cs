using UnityEngine;

public class PlayerGrappling : State<Controller>
{
	public PlayerGrappling(Controller target) : base(target)
	{
		SetAnimator("Grappling", target.anim);
	}
	float time = 0f;
	public override void Enter()
	{
		Play();
		target.rigid.linearVelocity = Vector2.zero;
		target.rigid.gravityScale = 0f;

		target.SetCanDash(true);
		target.SetCanDoubleJump(true);
	}
	public override void Update()
	{
		time += Time.deltaTime;

		if (time > 1f)
		{
			target.ChangeState("Idle");
			return;
		}
	}
	public override void Exit()
	{
		target.rigid.gravityScale = target.data.gravityScale;
		target.transform.up = Vector2.up;
	}
}

public class PlayerGrapplingJump : State<Controller>
{
	public PlayerGrapplingJump(Controller target) : base(target)
	{
		SetAnimator("Jump", target.anim);
	}
	public override void Enter()
	{
		Play();
		target.func.Jump();

		target.SetCanDash(true);
		target.SetCanDoubleJump(true);
	}
	public override void Update()
	{
		if (target.CanDoubleJump())
		{
			target.ChangeState("DoubleJump");
			return;
		}

		if (target.IsTurn())
		{
			target.ChangeState("TurnInAir");
			return;
		}

		if ((target.isLeftWall && !target.jumpPress && target.inputVec.x == -1) || (target.isRightWall && !target.jumpPress && target.inputVec.x == 1))
		{
			target.ChangeState("WallSlide");
			return;
		}

		if (!target.isGround && target.rigid.linearVelocityY <= 0)
		{
			target.ChangeState("Fall");
			return;
		}

		if (target.isGround)
		{
			target.ChangeState("Idle");
			return;
		}
	}
	public override void FixedUpdate()
	{
		target.func.MoveInAir();
	}

}