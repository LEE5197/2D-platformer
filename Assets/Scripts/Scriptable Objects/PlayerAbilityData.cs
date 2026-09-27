using UnityEngine;

[CreateAssetMenu(fileName = "Player Ability Data", menuName = "Scriptable Object/Player Ability Data")]
public class PlayerAbilityData : ScriptableObject
{
    [Header("Grapple")]
    [SerializeField] private float GrappleRange;
    [SerializeField] private float GrappleTime;
    [SerializeField] private float AngleModifier;
    [SerializeField] private float LineWidth;
    [SerializeField] private float Amplitude;
    [SerializeField] private int PointNum;
    [SerializeField] private float GrappleForce;
    [SerializeField] private float GrappleCooltime;
    [SerializeField] private LayerMask GrappleTarget;
    public float grappleRange => GrappleRange;
    public float grappleTime => GrappleTime;
    public float angleModifier => AngleModifier;
    public float lineWidth => LineWidth;
    public int pointNum => PointNum;
    public float amplitude => Amplitude;
    public float grappleForce => GrappleForce;
    public float grappleCooltime => GrappleCooltime;
    public LayerMask grappleTarget => GrappleTarget;

    [Space]
    [Header("Orb")]
    [SerializeField] private float OrbSetupTime = 1f;

    public float orbSetupTime => OrbSetupTime;
}
