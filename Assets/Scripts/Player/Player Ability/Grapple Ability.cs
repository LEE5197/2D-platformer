using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class GrappleAbility : AbilityBase
{
	[SerializeField] private Controller target;
	[SerializeField] private LineRenderer prevLine;
	[SerializeField] private LineRenderer activeLine;

	private Vector2 targetPoint;
	private float time;
	private float amplitude;
	private bool isTargetFound;

	private void Awake()
	{
		prevLine.startWidth = prevLine.endWidth = data.lineWidth;
		prevLine.positionCount = 2;
		activeLine.startWidth = activeLine.endWidth = data.lineWidth;
		activeLine.positionCount = data.pointNum;
	}

	private void OnEnable()
	{
		prevLine.gameObject.SetActive(true);
		activeLine.gameObject.SetActive(false);
	}
	private void OnDisable()
	{
		target.StopCoroutine(ActiveGrapplingRoutine());

		activeLine.gameObject.SetActive(false);
		prevLine.gameObject.SetActive(false);
	}
	private void Update()
	{
		time = Mathf.Clamp(time + Time.deltaTime, 0, data.grappleTime);
		amplitude = Mathf.Lerp(data.amplitude, 0, time / data.grappleTime);
	}

	private void LateUpdate()
	{
		if(activeLine.gameObject.activeSelf)
			DrawActiveLine();
		if(prevLine.gameObject.activeSelf)
			DrawPreviewLine();
	}
	public override void OnLeftClick()
	{
		if (!isTargetFound || Vector2.Distance(targetPoint, (Vector2)transform.position) < 2f) return;

		target.StartCoroutine(ActiveGrapplingRoutine());

		time = 0f;
		amplitude = data.amplitude;

		prevLine.gameObject.SetActive(false);
		activeLine.gameObject.SetActive(true);
	}

	private IEnumerator ActiveGrapplingRoutine()
	{
		target.ChangeState("Grappling");
		Vector2 dir = targetPoint - (Vector2)transform.position;

		yield return new WaitForSeconds(data.grappleTime);

		target.rigid.AddForce(dir.normalized * data.grappleForce, ForceMode2D.Impulse);

		activeLine.gameObject.SetActive(false);

		yield return new WaitForSeconds(data.grappleCooltime);

		prevLine.gameObject.SetActive(true);

	}
	private void DrawPreviewLine()
	{
		if (!prevLine.gameObject.activeSelf) return;

		Vector2 target = Mouse.current.position.value;
		if (target == null) return;
		target = Camera.main.ScreenToWorldPoint(target);

		Vector2 dir = target - (Vector2)transform.position;
		dir = dir.normalized;

		Vector2 offset = Vector2.one * dir;

		RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + offset, dir, data.grappleRange);

		if (hit.collider != null && (data.grappleTarget.value & (1 << hit.collider.gameObject.layer)) != 0)
		{
			isTargetFound = true;
			targetPoint = hit.point;
		}
		else
		{
			isTargetFound = false;
			targetPoint = (Vector2)transform.position + offset;
		}

		prevLine.SetPosition(0, (Vector2)transform.position + offset);
		prevLine.SetPosition(1, targetPoint);
	}
	private void DrawActiveLine()
	{
		Vector2 dir = targetPoint - (Vector2)transform.position;
		float distance = Mathf.Lerp(0, dir.magnitude, time / data.grappleTime);

		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		Quaternion rotation = Quaternion.Euler(0, 0, angle);

		for(int i = 0; i < data.pointNum; i++)
		{
			Vector2 localPos = DampedSin(distance, amplitude, i);
			Vector2 worldPos = transform.position + (rotation * localPos);

			activeLine.SetPosition(i, worldPos);
		}
	}
	private Vector2 DampedSin(float distance, float amplitude, int index)
	{
		float t = (float)index / (data.pointNum - 1);
		float wave = Mathf.Sin(t * 3f * Mathf.PI);

		float currentAmplitude = amplitude * (1f - t);

		float x = distance * t;
		float y = wave * currentAmplitude;

		return new Vector2(x, y);
	}

	private void OnDrawGizmos()
	{
		if (Application.isPlaying) return;

		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, data.grappleRange);
	}
}
