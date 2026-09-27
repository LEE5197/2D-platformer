using System.Collections.Generic;
public class StateMachine<T>
{
	private Dictionary<string, State<T>> dic = new();
	public State<T> curState = null;


	public void AddState(string key, State<T> value)
	{
		dic[key] = value;
	}
	public void ChangeState(string key)
	{
		State<T> nextState = dic[key];
		if (nextState == null || nextState == curState) return;
		curState?.Exit();
		curState = nextState;
		curState?.Enter();
	}
	public void Update()
	{
		curState?.Update();
	}
	public void FixedUpdate()
	{
		curState?.FixedUpdate();
	}
	public State<T> GetState(string key)
	{
		return dic[key];
	}
}
