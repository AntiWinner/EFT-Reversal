using UnityEngine;

namespace ChartAndGraph;

internal class ChartItemGrowEffect : ChartItemEffect
{
	private const int _E003 = 0;

	private const int _E004 = 1;

	private const int _E005 = 2;

	private const int _E006 = 3;

	public float GrowMultiplier = 1.2f;

	public bool VerticalOnly;

	public float TimeScale = 1f;

	public AnimationCurve GrowEaseFunction = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

	public AnimationCurve ShrinkEaseFunction = AnimationCurve.EaseInOut(1f, 1f, 0f, 0f);

	private new float m__E007 = 1f;

	private new float m__E008;

	private new float m__E009;

	private int _E00A;

	private bool _E00B;

	internal override Vector3 _E007
	{
		get
		{
			if (VerticalOnly)
			{
				return new Vector3(1f, m__E007, 1f);
			}
			return new Vector3(m__E007, m__E007, m__E007);
		}
	}

	internal override Quaternion _E008 => Quaternion.identity;

	internal override Vector3 _E009 => Vector3.zero;

	public void GrowAndShrink()
	{
		m__E008 = Time.time;
		m__E009 = m__E007;
		_E00A = 3;
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

	private void Update()
	{
		float num = Time.time - m__E008;
		num *= TimeScale;
		switch (_E00A)
		{
		case 1:
		{
			_E000(GrowEaseFunction);
			float factor = GrowEaseFunction.Evaluate(num);
			m__E007 = _ED15._E000(m__E009, GrowMultiplier, factor);
			if (CheckAnimationEnded(num, GrowEaseFunction))
			{
				_E00A = 0;
				m__E007 = GrowMultiplier;
			}
			break;
		}
		case 2:
		{
			_E000(ShrinkEaseFunction);
			float factor = ShrinkEaseFunction.Evaluate(num);
			m__E007 = _ED15._E000(m__E009, 1f, factor);
			if (CheckAnimationEnded(num, ShrinkEaseFunction))
			{
				_E00A = 0;
				m__E007 = 1f;
			}
			break;
		}
		case 3:
		{
			_E000(GrowEaseFunction);
			float factor = GrowEaseFunction.Evaluate(num);
			m__E007 = _ED15._E000(m__E009, GrowMultiplier, factor);
			if (CheckAnimationEnded(num, GrowEaseFunction))
			{
				m__E007 = GrowMultiplier;
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
		m__E008 = Time.time;
		m__E009 = m__E007;
		_E00A = 1;
	}

	public void Shrink()
	{
		m__E008 = Time.time;
		m__E009 = m__E007;
		_E00A = 2;
	}
}
