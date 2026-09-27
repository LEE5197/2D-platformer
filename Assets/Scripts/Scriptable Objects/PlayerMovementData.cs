using UnityEngine;

[CreateAssetMenu(fileName = "Player Data", menuName = "Scriptable Object/Player Movement Data")]
public class PlayerMovementData : ScriptableObject
{
    [Header("Movement")]
    [SerializeField] private float MaxSpeed = 9f;
    [SerializeField] private float Acceleration = 13f;
    [SerializeField] private float Decceleration = 8f;
    [SerializeField] private float VelPower = 0.96f;
    [SerializeField] private float Friction = 0.2f;

    public float maxSpeed => MaxSpeed;
    public float acceleration => Acceleration;
    public float decceleration => Decceleration;
    public float velPower => VelPower;
    public float friction => Friction;

    [Space]
    [Header("Jump")]
    [SerializeField] private float JumpForce = 13f;
    [SerializeField] private float JumpCutMultiflier = 0.4f;
    [SerializeField] private float CoyoteTime = 0.15f;
    [SerializeField] private float BufferTime = 0.1f;
    [SerializeField] private float FallMultiflier = 2.5f;
    [SerializeField] private float GravityScale = 2f;
    [SerializeField] private float MaxFallSpeed = 30f;

    public float jumpForce => JumpForce;
    public float jumpCutMultiflier => JumpCutMultiflier;
    public float coyoteTime => CoyoteTime;
    public float bufferTime => BufferTime;
    public float fallMultiflier => FallMultiflier;
    public float gravityScale => GravityScale;
    public float maxFallSpeed => MaxFallSpeed;

    public bool jumpCut { get; set; } = false;

    [Space]
    [Range(0f, 1f)]
    [SerializeField] private float InAirWeight = 0.5f;
    public float inAirWeight => InAirWeight;

    [Space]
    [SerializeField] private float WallSlideSpeed = 3f;
    [SerializeField] private float WallJumpForce = 13f;
    [SerializeField] private Vector2 WallJumpRate = new Vector2(.5f, .5f);
    public float wallSlideSpeed => WallSlideSpeed;
    public float wallJumpForce => WallJumpForce;
    public Vector2 wallJumpRate => WallJumpRate;

    [Space]
    [Header("Check")]
    [SerializeField] private Vector2 GroundSize;
    [SerializeField] private Vector2 GroundPos;
    [SerializeField] private LayerMask GroundLayer;

    public Vector2 groundSize => GroundSize;
    public Vector2 groundPos => GroundPos;
    public LayerMask groundLayer => GroundLayer;

    [Space]

    [SerializeField] private Vector2 LeftWallSize;
    [SerializeField] private Vector2 LeftWallPos;
    public Vector2 leftWallSize => LeftWallSize;
    public Vector2 leftWallPos => LeftWallPos;

    [Space]
    [SerializeField] private Vector2 RightWallSize;
    [SerializeField] private Vector2 RightWallPos;
    public Vector2 rightWallSize => RightWallSize;
    public Vector2 rightWallPos => RightWallPos;

    [Space]
    [Header("ability")]
    
    [Space]
    [Header("Dash")]
    [SerializeField] private float DashForce;
    [SerializeField] private float DashCooldown;
    [SerializeField] private float DashTime;
    public float dashForce => DashForce;
    public float dashCooldown => DashCooldown;
    public float dashTime => DashTime;

    [Space]
    [Header("Charge Jump")]
    [SerializeField] private float ChargeJumpForce;
    [SerializeField] private float ChargeTime;
    public float chargeJumpForce => ChargeJumpForce;
    public float chargeTime => ChargeTime;

    [Space]
    [Header("Squash and Stretch")]
    [SerializeField] private float Duration;
    [SerializeField] private Vector2 SquashRate;
    [SerializeField] private Vector2 StretchRate;
    public float duration => Duration;
    public Vector2 squashRate => SquashRate;
    public Vector2 stretchRate => StretchRate;

}
