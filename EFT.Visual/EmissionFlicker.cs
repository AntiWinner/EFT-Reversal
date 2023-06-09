using UnityEngine;

namespace EFT.Visual;

public class EmissionFlicker : Flicker
{
	private static readonly int m__E000 = Shader.PropertyToID(_ED3E._E000(168740));

	[SerializeField]
	private MeshRenderer _meshRenderer;

	private float _E001 = float.MaxValue;

	private MaterialPropertyBlock _E002;

	private float _E003
	{
		get
		{
			return _E001;
		}
		set
		{
			if (!Mathf.Approximately(_E001, value))
			{
				_E001 = value;
				if (_E002 == null)
				{
					_E000();
				}
				_E002.SetFloat(EmissionFlicker.m__E000, _E001);
				_meshRenderer.SetPropertyBlock(_E002);
			}
		}
	}

	protected override void Awake()
	{
		base.Awake();
		_E000();
	}

	private void _E000()
	{
		_E002 = new MaterialPropertyBlock();
	}

	protected override void OnDestroy()
	{
		_meshRenderer = null;
		base.OnDestroy();
	}

	public override void ManualUpdate()
	{
		float num = TimeShift + Time.time * Frequency;
		_E003 = (FullRandomCurve ? ((IntensityShift + Mathf.PerlinNoise(num, RandomSeed) * Intensity) * CullingCoef) : ((IntensityShift + Curve.Evaluate(num) * Intensity) * CullingCoef));
	}
}
