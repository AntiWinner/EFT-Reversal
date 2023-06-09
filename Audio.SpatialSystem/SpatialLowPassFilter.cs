using System.Runtime.CompilerServices;
using UnityEngine;

namespace Audio.SpatialSystem;

[RequireComponent(typeof(AudioLowPassFilter))]
public sealed class SpatialLowPassFilter : AudioFilter
{
	private const float m__E001 = 10f;

	private const float _E002 = 10f;

	[CompilerGenerated]
	private float _E003;

	private float _E004 = 22000f;

	private AudioLowPassFilter _E005;

	private float _E006 = 5000f;

	private AnimationCurve _E007 = new AnimationCurve();

	private AnimationCurve _E008 = new AnimationCurve();

	private AnimationCurve _E009 = new AnimationCurve();

	private float _E00A;

	public float CutoffFrequency
	{
		[CompilerGenerated]
		get
		{
			return _E003;
		}
		[CompilerGenerated]
		private set
		{
			_E003 = value;
		}
	}

	public void Awake()
	{
		Initialize();
		ResetFilter();
	}

	public void SetOcclusionCurves(AnimationCurve obstructionCurve, AnimationCurve propagationCurve)
	{
		_E007 = obstructionCurve;
		_E008 = propagationCurve;
		_E009 = propagationCurve;
	}

	public void Update()
	{
		_E001(Time.deltaTime);
	}

	public override void ResetFilter()
	{
		if ((object)_E005 == null)
		{
			Initialize();
		}
		_E005.cutoffFrequency = _E004;
		base.FilterLevel = 0f;
	}

	public override void SetFilterParams(float value, bool applyImmediately = false, ESoundOcclusionType occlusionType = ESoundOcclusionType.FullOcclusion)
	{
		base.FilterLevel = value;
		_E009 = ((occlusionType == ESoundOcclusionType.Obstruction) ? _E007 : _E008);
		if (applyImmediately)
		{
			_E001(1f);
		}
	}

	private bool _E000()
	{
		return Mathf.Abs(_E005.cutoffFrequency - _E00A) > 10f;
	}

	public void SetHighestFrequency(float frequency)
	{
		if (!(frequency <= _E006))
		{
			_E004 = frequency;
		}
	}

	public void SetLowerFrequency(float frequency)
	{
		if (!(frequency >= _E004))
		{
			_E006 = frequency;
		}
	}

	protected override void Initialize()
	{
		_E005 = GetComponent<AudioLowPassFilter>();
	}

	private void _E001(float time)
	{
		if (_E009 != null)
		{
			float cutoffFrequency = _E005.cutoffFrequency;
			float b = Mathf.Clamp(_E009.Evaluate(base.FilterLevel), _E006, _E004);
			_E00A = Mathf.Lerp(cutoffFrequency, b, time * 10f);
			if (_E000())
			{
				_E005.cutoffFrequency = _E00A;
				CutoffFrequency = _E00A;
			}
		}
	}

	private void OnDestroy()
	{
		ResetFilter();
	}
}
