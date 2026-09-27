using UnityEngine;

public class PlayerFSM
{
    StateMachine<Controller> stateMachine = new StateMachine<Controller>();
	public PlayerFSM(Controller target)
	{
		InitState(target);
		ChangeState("Idle");
	}
	private void InitState(Controller target)
	{
		// Movement
		AddState("Idle", new PlayerIdle(target));
		AddState("Run", new PlayerRun(target));
		AddState("RunTurn", new PlayerRunTurn(target));
		AddState("Jump", new PlayerJump(target));
		AddState("Fall", new PlayerFall(target));
		AddState("TurnInAir", new PlayerTurnInAir(target));
		AddState("WallSlide", new PlayerWallSlide(target));
		AddState("WallJump", new PlayerWallJump(target));
		AddState("Dash", new PlayerDash(target));
		AddState("DoubleJump", new PlayerDoubleJump(target));
		AddState("Crouch", new PlayerCrouch(target));
		AddState("ChargeJump", new PlayerChargeJump(target));

		// Ability
		AddState("Grappling", new PlayerGrappling(target));
		AddState("GrapplingJump", new PlayerGrapplingJump(target));
		
	}
	public void AddState(string key, State<Controller> value)
	{
		stateMachine.AddState(key, value);
	}
    public void Update()
	{
		stateMachine.Update();
	}
	public void FixedUpdate()
	{
		stateMachine.FixedUpdate();
	}
	public void ChangeState(string key)
	{
		stateMachine.ChangeState(key);
	}
	public bool CurStateIs(string key)
	{
		if (stateMachine.curState == stateMachine.GetState(key)) return true;
		return false;
	}
}
