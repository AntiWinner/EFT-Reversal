using UnityEngine;

namespace ChartAndGraph;

public abstract class ChartItemLerpEffect : ChartItemEffect
{
	private const int _E003 = 0;

	private const int _E004 = 1;

	private const int _E005 = 2;

	private const int _E006 = 3;

	public float TimeScale = 1f;

	public AnimationCurve GrowEaseFunction = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

	public AnimationCurve ShrinkEaseFunction = AnimationCurve.EaseInOut(1f, 1f, 0f, 0f);

	private new float _E008;

	private new float _E009;

	private int _E00A;

	private bool _E00B;

	protected override void Start()
	{
		base.Start();
		base.enabled = false;
	}

	public void GrowAndShrink()
	{
		_E008 = Time.time;
		_E009 = Mathf.Clamp(GetStartValue(), 0f, 1f);
		_E00A = 3;
		base.enabled = true;
	}

	public bool CheckAnimationEnded(float time, AnimationCurve curve)
	{
		if (curve.length == 0)
		{
			return true;
		}
		bool num = time > curve.keys[curve.length - 1].time;
		if (num && _E00B)
		{
			RaiseDeactivated();
			base.gameObject.SetActive(value: false);
			_E00B = false;
		}
		return num;
	}

	private new void _E000(AnimationCurve curve)
	{
		curve.postWrapMode = WrapMode.Once;
		curve.preWrapMode = WrapMode.Once;
	}

	protected abstract void ApplyLerp(float value);

	protected abstract float GetStartValue();

	private void Update()
	{
		float num = Time.time - _E008;
		num *= TimeScale;
		switch (_E00A)
		{
		case 1:
		{
			_E000(GrowEaseFunction);
			float t = GrowEaseFunction.Evaluate(num);
			t = Mathf.Lerp(_E009, 1f, t);
			ApplyLerp(t);
			if (CheckAnimationEnded(num, GrowEaseFunction))
			{
				_E00A = 0;
				ApplyLerp(1f);
				base.enabled = false;
			}
			break;
		}
		case 2:
		{
			_E000(ShrinkEaseFunction);
			float t = ShrinkEaseFunction.Evaluate(num);
			t = Mathf.Lerp(_E009, 0f, t);
			ApplyLerp(t);
			if (CheckAnimationEnded(num, ShrinkEaseFunction))
			{
				_E00A = 0;
				ApplyLerp(0f);
				base.enabled = false;
			}
			break;
		}
		case 3:
		{
			_E000(GrowEaseFunction);
			float t = GrowEaseFunction.Evaluate(num);
			t = Mathf.Lerp(_E009, 1f, t);
			ApplyLerp(t);
			if (CheckAnimationEnded(num, GrowEaseFunction))
			{
				ApplyLerp(1f);
				Shrink();
			}
			break;
		}
		}
	}

	public override void TriggerOut(bool deactivateOnEnd)
	{
		_E00B = deactivateOnEnd;
		Shrink();
	}

	public override void TriggerIn(bool deactivateOnEnd)
	{
		_E00B = deactivateOnEnd;
		Grow();
	}

	public void Grow()
	{
		_E008 = Time.time;
		_E009 = Mathf.Clamp(GetStartValue(), 0f, 1f);
		_E00A = 1;
		base.enabled = true;
	}

	public void Shrink()
	{
		_E008 = Time.time;
		_E009 = Mathf.Clamp(GetStartValue(), 0f, 1f);
		_E00A = 2;
		base.enabled = true;
	}
}
