using UnityEngine;

public class ParticleSystemAdapter : Emitter
{
	public ParticleSystem ParticleSystemObject;

	public RundomRange Lifetime;

	public RundomRange Speed;

	public RundomRange Size;

	public bool RotationRandom;

	public float Rotation;

	public Color32 Color;

	private static readonly int m__E000 = 10;

	private int _E001;

	private float[] _E002 = new float[ParticleSystemAdapter.m__E000];

	private float[] _E003 = new float[ParticleSystemAdapter.m__E000];

	private float[] _E004 = new float[ParticleSystemAdapter.m__E000];

	private float[] _E005 = new float[ParticleSystemAdapter.m__E000];

	private uint[] _E006 = new uint[ParticleSystemAdapter.m__E000];

	private void Awake()
	{
		if (ParticleSystemObject == null)
		{
			ParticleSystemObject = GetComponent<ParticleSystem>();
		}
		Lifetime.Init();
		Speed.Init();
		Size.Init();
		_E000();
	}

	public override void Emit(Vector3 pos, Vector3 vel)
	{
		float startLifetime = _E002[_E001];
		float num = _E003[_E001];
		float startSize = _E004[_E001];
		float rotation = (RotationRandom ? _E005[_E001] : Rotation);
		ParticleSystemObject.Emit(new ParticleSystem.EmitParams
		{
			position = pos,
			startLifetime = startLifetime,
			velocity = vel * num,
			randomSeed = _E006[_E001],
			startColor = Color,
			startSize = startSize,
			rotation = rotation
		}, 1);
		_E001++;
		_E001 %= ParticleSystemAdapter.m__E000;
	}

	public void Emit(Vector3 pos, Vector3 vel, float sizeMult)
	{
		float startLifetime = _E002[_E001];
		float num = _E003[_E001];
		float num2 = _E004[_E001];
		float rotation = (RotationRandom ? _E005[_E001] : Rotation);
		num2 *= sizeMult;
		ParticleSystemObject.Emit(new ParticleSystem.EmitParams
		{
			position = pos,
			startLifetime = startLifetime,
			velocity = vel * num,
			randomSeed = _E006[_E001],
			startColor = Color,
			startSize = num2,
			rotation = rotation
		}, 1);
		_E001++;
		_E001 %= ParticleSystemAdapter.m__E000;
	}

	private void _E000()
	{
		_E002 = new float[ParticleSystemAdapter.m__E000];
		_E003 = new float[ParticleSystemAdapter.m__E000];
		_E004 = new float[ParticleSystemAdapter.m__E000];
		_E005 = new float[ParticleSystemAdapter.m__E000];
		_E006 = new uint[ParticleSystemAdapter.m__E000];
		for (int i = 0; i < ParticleSystemAdapter.m__E000; i++)
		{
			_E002[i] = Lifetime.Value;
			_E003[i] = Speed.Value;
			_E004[i] = Size.Value;
			_E005[i] = _E8EE.FloatRotation();
			_E006[i] = _E8EE.Uint();
		}
	}
}
