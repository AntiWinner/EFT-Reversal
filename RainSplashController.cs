using UnityEngine;

public class RainSplashController : MonoBehaviour
{
	[SerializeField]
	private DepthPhotograper _depthPhoto;

	[SerializeField]
	private ParticleSystem _splashes;

	[SerializeField]
	private float _splashLifetime = 0.3f;

	[SerializeField]
	private Vector2 _particleSizeRange = new Vector2(0.01f, 0.08f);

	[SerializeField]
	private float _splashesOffset;

	[SerializeField]
	private float _minDistance = 1.25f;

	[SerializeField]
	private float _maxDistance = 20f;

	[SerializeField]
	private float _maxGeneratedParticlesInFrame = 50f;

	[SerializeField]
	private AnimationCurve _falloffCurve;

	[SerializeField]
	private AnimationCurve _dispersionCurve;

	private Transform _E000;

	private float _E001;

	private _EC19 _E002;

	public float Intensity
	{
		get
		{
			return _E001;
		}
		set
		{
			_E001 = Mathf.Clamp01(value);
		}
	}

	public void Init(Camera targetCamera)
	{
		_E000 = targetCamera.transform;
	}

	private void Awake()
	{
		_E002 = new _EC19(300, 42);
	}

	private void Update()
	{
		if (!(_E000 == null))
		{
			int num = Mathf.FloorToInt(_maxGeneratedParticlesInFrame * _E001);
			Color32 startColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			ParticleSystem.EmitParams emitParams = default(ParticleSystem.EmitParams);
			emitParams.velocity = Vector3.zero;
			ParticleSystem.EmitParams emitParams2 = emitParams;
			for (int i = 0; i < num; i++)
			{
				float num2 = (1f - _dispersionCurve.Evaluate(_E002.Next())) * (_maxDistance - _minDistance) + _minDistance;
				float num3 = 1f - _falloffCurve.Evaluate(num2 / _maxDistance);
				startColor.a = (byte)Mathf.RoundToInt((num3 * 0.7f + _E002.Next() * num3 * 0.3f) * 255f);
				emitParams2.position = Vector3.up * _splashesOffset;
				emitParams2.startSize = _particleSizeRange.x + _E002.Next() * (_particleSizeRange.y - _particleSizeRange.x);
				emitParams2.startLifetime = _splashLifetime * 0.7f + _E002.Next() * _splashLifetime * 0.3f;
				emitParams2.startColor = startColor;
				_splashes.Emit(emitParams2, 1);
			}
		}
	}
}
