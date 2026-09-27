using UnityEngine;

public class State<T>
{
	protected T target;
	protected int anim;
	protected Animator animator;

	public State(T _target) => target = _target;

	virtual public void Enter() { }
	virtual public void Update() { }
	virtual public void FixedUpdate() { }
	virtual public void Exit() { }
	virtual public void SetAnimator(string key, Animator _animator)
	{
		anim = Animator.StringToHash(key);
		animator = _animator;
	}
	virtual public void Play() => animator.Play(anim);
}
