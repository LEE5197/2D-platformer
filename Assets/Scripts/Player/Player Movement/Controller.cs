using UnityEngine;

public class Controller : MonoBehaviour
{
	[SerializeField] private PlayerMovementData playerMovementData;
	public PlayerMovementData data => playerMovementData;
	private PlayerInput input;
	public Rigidbody2D rigid { get; private set; }
	
	public SpriteRenderer render { get; private set; }
	public Animator anim;

	public PlayerFSM fsm;
	public PlayerMoveFunction func { get; private set; }
	public Vector2 inputVec => input.inputVec;

	public float coyoteTimer { get; set; }
	public float bufferTimer { get; set; } = .25f;
	public bool isGround { get; private set; }
	public bool isLeftWall { get; private set; }
	public bool isRightWall { get; private set; }

	private void Awake()
	{
		input = GetComponent<PlayerInput>();
		rigid = GetComponent<Rigidbody2D>();
		render = anim.gameObject.GetComponent<SpriteRenderer>();
		func = new(this);
		fsm = new(this);
	}

	private void OnEnable()
	{
		input.OnJumpEvent += HandleJump;
		input.OnDashEvent += HandleDash;
	}
	private void OnDisable()
	{
		input.OnJumpEvent -= HandleJump;
		input.OnDashEvent -= HandleDash;
	}

	private void Update()
	{
		CollCheck();

		fsm.Update();
	}
	private void FixedUpdate()
	{

		fsm.FixedUpdate();
	}

	public void ChangeState(string key)
	{
		fsm.ChangeState(key);
	}

	public bool jumpPress => input.jumpPressed;

	private const float collisionTime = 0.025f;

	private float jumpTimer;
	private bool canJump = false;
	private void HandleJump()
	{
		bufferTimer = jumpPress ? 0f : bufferTimer;
	}

	public bool dashPressed => input.dashPressed;
	private bool canDash = false;
	private float dashCoolDown = 0f;
	private void HandleDash()
	{
		if (canDash && dashCoolDown == 0)
		{
			dashCoolDown = data.dashCooldown;
			canDash = false;
			fsm.ChangeState("Dash");
		}
	}

	public void SetCanDash(bool flag) => canDash = flag;
	public void SetCanDoubleJump(bool flag) => canDoubleJump = flag;

	public bool CanJump()
	{
		if (coyoteTimer < data.coyoteTime && bufferTimer < data.bufferTime && canJump)
		{
			canJump = false;
			return true;
		}
		return false;
	}

	float moveAbilityTimer = 0f;
	bool canDoubleJump = false;
	public bool CanDoubleJump()
	{
		if (!isGround && bufferTimer < data.bufferTime && canDoubleJump)
		{
			canDoubleJump = false;
			return true;
		}
		return false;
	}
	public bool IsTurn()
	{
		if ((inputVec.x == 1 && render.flipX == true) || (inputVec.x == -1 && render.flipX == false))
			return true;
		return false;
	}

	private void CollCheck()
	{
		Vector2 groundWorldPos = transform.TransformPoint(data.groundPos);
		Vector2 leftWallWorldPos = transform.TransformPoint(data.leftWallPos);
		Vector2 rightWallWorldPos = transform.TransformPoint(data.rightWallPos);

		isGround = Physics2D.OverlapBox(groundWorldPos, data.groundSize, 0f, data.groundLayer);
		isLeftWall = Physics2D.OverlapBox(leftWallWorldPos, data.leftWallSize, 0f, data.groundLayer);
		isRightWall = Physics2D.OverlapBox(rightWallWorldPos, data.rightWallSize, 0f, data.groundLayer);

		if (!isGround)
		{
			coyoteTimer = Mathf.Clamp(coyoteTimer + Time.deltaTime, 0, data.coyoteTime);
			if (coyoteTimer == data.coyoteTime) canJump = false;
			jumpTimer = collisionTime;
		}
		else
		{
			coyoteTimer = 0f;
			jumpTimer = Mathf.Clamp(jumpTimer - Time.deltaTime, 0, collisionTime);
		}

		if (jumpTimer == 0)
		{
			canJump = true;
			canDash = true;
			canDoubleJump = true;
		}

		if ((isLeftWall || isRightWall)&&fsm.CurStateIs("WallSlide"))
		{
			moveAbilityTimer = Mathf.Clamp(moveAbilityTimer - Time.deltaTime, 0, collisionTime);
		}
		else
		{
			moveAbilityTimer = collisionTime;
		}

		if (moveAbilityTimer == 0)
		{
			canDash = true;
			canDoubleJump = true;
		}

		bufferTimer = Mathf.Clamp(bufferTimer + Time.deltaTime, 0, data.bufferTime);
		dashCoolDown = Mathf.Clamp(dashCoolDown - Time.deltaTime, 0, data.dashCooldown);
	}
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;

		Gizmos.DrawWireCube(transform.TransformPoint(data.groundPos), data.groundSize);
		Gizmos.DrawWireCube(transform.TransformPoint(data.leftWallPos), data.leftWallSize);
		Gizmos.DrawWireCube(transform.TransformPoint(data.rightWallPos), data.rightWallSize);
	}
}
