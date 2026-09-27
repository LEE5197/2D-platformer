using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class OrbAbilityBase : AbilityBase
{
	private Vector2[] points = new Vector2[4];

	public override void OnLeftClick()
	{
		Vector2 mousePos = Mouse.current.position.value;
		mousePos = Camera.main.ScreenToWorldPoint(mousePos);
		SetInitPos(transform.position, mousePos);
	}


	protected void SetInitPos(Vector2 startPos, Vector2 endPos)
	{
		points[0] = startPos;
		points[3] = endPos;

		float distance = Vector2.Distance(startPos, endPos);
		float weight = distance / 2;

		Vector2 offset1 = new Vector2(Random.Range(-weight, weight), Random.Range(-weight, weight));
		Vector2 offset2 = new Vector2(Random.Range(-weight, weight), Random.Range(-weight, weight));

		points[1] = points[0] + offset1;
		points[2] = points[3] + offset2;
	}
	protected Vector2 CubicBezierCurve(Vector2 []points, float t)
	{
		Vector2 p1 = Vector2.Lerp(points[0], points[1], t);
		Vector2 p2 = Vector2.Lerp(points[1], points[2], t);
		Vector2 p3 = Vector2.Lerp(points[2], points[3], t);

		Vector2 p4 = Vector2.Lerp(p1, p2, t);
		Vector2 p5 = Vector2.Lerp(p2, p3, t);

		return Vector2.Lerp(p4, p5, t);
	}
}
