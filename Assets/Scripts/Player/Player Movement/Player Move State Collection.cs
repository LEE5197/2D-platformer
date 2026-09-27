using UnityEngine;
using System.Collections;

public class PlayerIdle : State<Controller>
{
	public PlayerIdle(Controller _target) : base(_target) {
		SetAnimator("Idle", _target.anim);
	}

	public override void Enter()
	{
		Play();
	}

	public override void Update()
	{
		if (target.CanJump())
		{
			target.ChangeState("Jump");
			return;
		}

		if (!target.isGround)
		{
			target.ChangeState("Fall");
			return;
		}

		if ((target.isLeftWall && target.inputVec.x == -1) || (target.isRightWall && target.inputVec.x == 1))
		{
			target.ChangeState("WallSlide");
			return;
		}

		if (target.isGround && target.inputVec.y == -1)
		{
			target.ChangeState("Crouch");
			return;
		}

		if (target.isGround && target.inputVec.x != 0)
		{
			target.ChangeState("Run");
			return;
		}
	}
	public override void FixedUpdate()
	{
		target.func.Move();
	}
}

public class PlayerRun : State<Controller>
{
	public PlayerRun(Controller target) : base(target)
	{
		SetAnimator("Run",target.anim);
	}

	public override void Enter()
	{
		Play();
	}
	public override void Update()
	{
		if (target.CanJump())
		{
			target.ChangeState("Jump");
			return;
		}

		if (!target.isGround && target.rigid.linearVelocityY < 0)
		{
			target.ChangeState("Fall");
			return;
		}

		if (target.IsTurn())
		{
			target.ChangeState("RunTurn");
			return;
		}

		if (target.isGround && target.inputVec.x == 0)
		{
			target.ChangeState("Idle");
			return;
		}
	}
	public override void FixedUpdate()
	{
		target.func.Move();
	}
}

public class PlayerRunTurn : State<Controller>
{
	public PlayerRunTurn(Controller target) : base(target)
	{
		SetAnimator("RunTurn", target.anim);
	}
	public override void Enter()
	{
		target.render.flipX = !target.render.flipX;
		Play();
	}
	public override void Update()
	{
		if (target.CanJump())
		{
			target.ChangeState("Jump");
			return;
		}

		if (!target.isGround && target.rigid.linearVelocityY < 0)
		{
			target.ChangeState("Fall");
			return;
		}

		AnimatorStateInfo info = target.anim.GetCurrentAnimatorStateInfo(0);

		if (!(info.IsName("RunTurn") && info.normalizedTime >= 1.0f)) return;

		if (target.isGround)
		{
			target.ChangeState("Idle");
			return;
		}

	}
	public override void FixedUpdate()
	{
		target.func.Move();
	}
}

public class PlayerJump : State<Controller>
{
	public PlayerJump(Controller target) : base(target)
	{
		SetAnimator("Jump", target.anim);
	}
	float jumpTime = 0f;
	public override void Enter()
	{
		Play();
		target.func.Jump();

		target.data.jumpCut = false;
		jumpTime = 0.1f;

		target.rigid.gravityScale = target.data.gravityScale;
	}

	public override void Update()
	{
		jumpTime = Mathf.Clamp(jumpTime -= Time.deltaTime, 0, 0.1f);
		if (!target.data.jumpCut && !target.jumpPress)
		{
			target.data.jumpCut = true;
			target.rigid.AddForceY(-target.rigid.linearVelocityY * (1 - target.data.jumpCutMultiflier), ForceMode2D.Impulse);
		}

		if (target.CanDoubleJump())
		{
			target.ChangeState("DoubleJump");
			return;
		}

		if (jumpTime != 0) return;

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

public class PlayerFall : State<Controller>
{
	public PlayerFall(Controller target) : base(target)
	{
		SetAnimator("Fall", target.anim);
	}
	public override void Enter()
	{
		target.rigid.gravityScale = target.data.gravityScale * target.data.fallMultiflier;
		Play();
	}
	public override void Update()
	{
		if (target.CanJump())
		{
			target.ChangeState("Jump");
			return;
		}

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

		if ((target.isLeftWall  && target.inputVec.x == -1) || (target.isRightWall  && target.inputVec.x == 1))
		{
			target.ChangeState("WallSlide");
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
	public override void Exit()
	{
		target.rigid.gravityScale = target.data.gravityScale;
	}
}

public class PlayerTurnInAir : State<Controller>
{
	public PlayerTurnInAir(Controller target) : base(target)
	{
		SetAnimator("TurnInAir", target.anim);
	}
	public override void Enter()
	{
		target.render.flipX = !target.render.flipX;
		Play();
	}
	public override void Update()
	{
		if (!target.data.jumpCut && !target.jumpPress)
		{
			target.data.jumpCut = true;
			target.rigid.AddForceY(-target.rigid.linearVelocityY * (1 - target.data.jumpCutMultiflier), ForceMode2D.Impulse);
		}

		AnimatorStateInfo info = target.anim.GetCurrentAnimatorStateInfo(0);
		if (!(info.IsName("TurnInAir") && info.normalizedTime >= 1.0f)) return;

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

public class PlayerWallSlide : State<Controller>
{
	public PlayerWallSlide(Controller target) : base(target)
	{
		SetAnimator("WallSlide", target.anim);
	}
	float inputTime;
	float time;
	public override void Enter()
	{
		Play();
		target.rigid.linearVelocityY = 0;
		inputTime = 0f;
		time = 0;
	}

	public override void Update()
	{
		target.rigid.linearVelocityY = Mathf.Lerp(0, -target.data.wallSlideSpeed, time / 3f);
		time += Time.deltaTime;

		if (target.inputVec.x != 0)
		{
			inputTime += Time.deltaTime;
		}
		else inputTime = 0f;

		if (!target.isLeftWall && !target.isRightWall)
		{
			target.ChangeState("Idle");
			return;
		}

		if (target.bufferTimer < target.data.bufferTime && target.inputVec.x != 0)
		{
			target.ChangeState("WallJump");
			return;
		}

		if (inputTime < 0.5f) return;

		if ((target.isRightWall && target.inputVec.x == -1) || (target.isLeftWall && target.inputVec.x == 1))
		{
			target.ChangeState("Idle");
			return;
		}

	}
}

public class PlayerWallJump : State<Controller>
{
	public PlayerWallJump(Controller target) : base(target)
	{
		SetAnimator("Jump", target.anim);
	}

	float jumpTime;
	public override void Enter()
	{
		jumpTime = .3f;
		target.data.jumpCut = false;
		target.rigid.gravityScale = target.data.gravityScale * target.data.fallMultiflier;
		Play();
		target.func.WallJump();
		target.render.flipX = target.inputVec.x == 1 ? false : true;
	}

	public override void Update()
	{
		jumpTime = Mathf.Clamp(jumpTime - Time.deltaTime, 0, 10f);
		if (!target.data.jumpCut && !target.jumpPress)
		{
			target.data.jumpCut = true;
			target.rigid.AddForceY(-target.rigid.linearVelocityY * (1 - target.data.jumpCutMultiflier), ForceMode2D.Impulse);
		}

		if (!target.isGround && target.rigid.linearVelocityY <= 0)
		{
			target.ChangeState("Fall");
			return;
		}

		if (jumpTime != 0) return;

		if ((target.isLeftWall && target.inputVec.x == -1) || (target.isRightWall && target.inputVec.x == 1))
		{
			target.ChangeState("WallSlide");
			return;
		}

		if (target.IsTurn())
		{
			target.ChangeState("TurnInAir");
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
		if (jumpTime != 0)
		{
			if (target.render.flipX && target.inputVec.x == -1) target.func.MoveInAir();
			else if (!target.render.flipX && target.inputVec.x == 1) target.func.MoveInAir();
		}
		else
			target.func.MoveInAir();
	}
	public override void Exit()
	{
		target.rigid.gravityScale = target.data.gravityScale;
	}
}

public class PlayerDash : State<Controller>
{
	public PlayerDash(Controller target) : base(target)
	{
		SetAnimator("Dash", target.anim);
	}
	float time;

	public override void Enter()
	{
		Play();

		time = target.data.dashTime;
		target.rigid.gravityScale = 0f;
		target.rigid.linearVelocity = Vector2.zero;

		float dir = target.inputVec.x;
		if (dir == 0)
		{
			if (target.render.flipX == true) dir = -1;
			else if (target.render.flipX == false) dir = 1;
		}

		target.rigid.AddForceX(dir * target.data.dashForce, ForceMode2D.Impulse);

		if (target.inputVec.x == 1) target.render.flipX = false;
		else if (target.inputVec.x == -1) target.render.flipX = true;
	}

	public override void Update()
	{
		time = Mathf.Clamp(time - Time.deltaTime, 0f, target.data.dashTime);
		if (time != 0f) return;

		target.ChangeState("Idle");
	}


	public override void Exit()
	{
		target.rigid.gravityScale = target.data.gravityScale;
		target.rigid.linearVelocityX = Mathf.Abs(target.rigid.linearVelocityX) > target.data.maxSpeed
			? target.data.maxSpeed * Mathf.Sign(target.rigid.linearVelocityX) : target.rigid.linearVelocityX;
	}
}

public class PlayerDoubleJump : State<Controller>
{
	public PlayerDoubleJump(Controller target) : base(target)
	{
		SetAnimator("DoubleJump", target.anim);
	}

	float jumpTime;
	public override void Enter()
	{
		Play();
		target.rigid.linearVelocityY = 0;
		target.func.Jump();

		target.data.jumpCut = false;
		jumpTime = 0.1f;

		target.rigid.gravityScale = target.data.gravityScale;
	}
	public override void Update()
	{
		jumpTime = Mathf.Clamp(jumpTime -= Time.deltaTime, 0, 0.1f);
		if (!target.data.jumpCut && !target.jumpPress)
		{
			target.data.jumpCut = true;
			target.rigid.AddForceY(-target.rigid.linearVelocityY * (1 - target.data.jumpCutMultiflier), ForceMode2D.Impulse);
		}

		if (jumpTime != 0) return;

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

public class PlayerCrouch : State<Controller>
{
	public PlayerCrouch(Controller target) : base(target)
	{
		SetAnimator("Crouch", target.anim);
	}
	float time;
	public override void Enter()
	{
		time = 0f;
		Play();
	}
	public override void Update()
	{
		time += Time.deltaTime;
		if (target.isGround && target.inputVec.y != -1)
		{
			target.ChangeState("Idle");
			return;
		}
		
		if (!target.isGround && target.rigid.linearVelocityY < 0f)
		{
			target.ChangeState("Fall");
			return;
		}

		if (target.CanJump() && time >= target.data.chargeTime)
		{
			target.ChangeState("ChargeJump");
			return;
		}
	}
	public override void FixedUpdate()
	{
		float targetSpeed = 0;
		float speedDif = targetSpeed - target.rigid.linearVelocityX;
		float force;

		force = Mathf.Pow(Mathf.Abs(speedDif) * target.data.decceleration, target.data.velPower) * Mathf.Sign(speedDif);
		

		target.rigid.AddForceX(force);
	}
}

public class PlayerChargeJump : State<Controller>
{
	public PlayerChargeJump(Controller target) : base(target)
	{
		SetAnimator("Jump", target.anim);
	}
	float jumpTime;
	public override void Enter()
	{
		Play();
		jumpTime = 0.1f;
		target.rigid.gravityScale = target.data.gravityScale * target.data.fallMultiflier;
		target.rigid.AddForceY(target.data.chargeJumpForce, ForceMode2D.Impulse);
	}
	public override void Update()
	{
		jumpTime = Mathf.Clamp(jumpTime - Time.deltaTime, 0, 0.1f);

		if (jumpTime != 0) return;

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
	public override void Exit()
	{
		target.rigid.gravityScale = target.data.gravityScale;
	}
}