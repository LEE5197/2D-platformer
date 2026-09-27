using UnityEngine;
using System.Collections.Generic;

public class AbilityController : MonoBehaviour
{
	private PlayerInput input;
	[SerializeField] private List<AbilityBase> abilityList = new();
	private bool isLeftClick => input.LeftPressed;
	private bool isRightClick => input.RightPressed;
	private int curAbility = 0;

	private void Awake()
	{
		input = GetComponent<PlayerInput>();
	}
	private void Start()
	{
		input.OnLeftClickEvent += HandlerLeftClick;
		input.OnRightClickEvent += HandlerRightClick;

		foreach (var it in abilityList)
		{
			it.gameObject.SetActive(false);
		}
		abilityList[0]?.gameObject.SetActive(true);
	}

	private void HandlerLeftClick()
	{
		if (!isLeftClick) return;

		abilityList[curAbility].OnLeftClick();
	}
	private void HandlerRightClick()
	{
		if (!isRightClick) return;
		if (abilityList.Count <= 1) return;

		abilityList[curAbility].gameObject.SetActive(false);
		curAbility = (curAbility + 1) % abilityList.Count;
		abilityList[curAbility].gameObject.SetActive(true);
	}
}
