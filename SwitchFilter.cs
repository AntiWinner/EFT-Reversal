using System;
using UnityEngine;

public class SwitchFilter : MonoBehaviour
{
	public float RotationSpeed;

	public AnimationCurve AnimationCurve;

	private Action<float> m__E000;

	private float _E001;

	private float _E002;

	public void Init(Action<float> action)
	{
		this.m__E000 = action;
	}

	public void Deinit()
	{
		this.m__E000 = null;
		base.enabled = false;
	}

	public void Set(bool isOn, bool initial)
	{
		if (initial)
		{
			_E001 = (isOn ? 1f : 0f);
			this.m__E000(AnimationCurve.Evaluate(_E001));
			base.enabled = false;
			return;
		}
		_E002 = (isOn ? 1f : 0f);
		if (!base.enabled && _E000())
		{
			base.enabled = true;
		}
	}

	private bool _E000()
	{
		return !Mathf.Approximately(_E001, _E002);
	}

	private void Update()
	{
		if (_E000())
		{
			_E001 = Mathf.MoveTowards(_E001, _E002, Time.deltaTime * RotationSpeed);
			this.m__E000(AnimationCurve.Evaluate(_E001));
		}
		else
		{
			base.enabled = false;
		}
	}
}
