using UnityEngine;

namespace Audio.SpatialSystem;

[RequireComponent(typeof(AudioReverbFilter))]
public class SpatialReverbFilter : AudioFilter
{
	private const float _E00B = -10000f;

	private const float _E00C = -2800f;

	private const float _E00D = 340.29f;

	private AudioReverbFilter _E00E;

	private AnimationCurve _E00F;

	private float _E000 => _E8A8.Instance.Distance(base.transform.position);

	private void Awake()
	{
		Initialize();
		ResetFilter();
	}

	public void SetCurve(AnimationCurve curve)
	{
		_E00F = curve;
	}

	public override void SetFilterParams(float value, bool applyImmediately = false, ESoundOcclusionType occlusionType = ESoundOcclusionType.FullOcclusion)
	{
		if ((object)_E00E != null && _E00F != null)
		{
			_E00E.reverbLevel = _E00F.Evaluate(value);
			_E000();
		}
	}

	private void _E000()
	{
		float t = this._E000 / 340.29f;
		_E00E.reverbDelay = Mathf.Lerp(0f, 0.1f, t);
	}

	public override void ResetFilter()
	{
		if ((object)_E00E == null)
		{
			Initialize();
		}
		base.FilterLevel = 0f;
		_E00E.roomHF = -2800f;
		_E00E.reverbLevel = -10000f;
	}

	protected override void Initialize()
	{
		_E00E = GetComponent<AudioReverbFilter>();
	}
}
