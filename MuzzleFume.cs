using UnityEngine;

public class MuzzleFume : MuzzleEffect
{
	public float StartPos;

	public float EmitterRadius;

	public float ConusSize = 1f;

	public AnimationCurve Sizes = AnimationCurve.Linear(0f, 1f, 1f, 1f);

	public AnimationCurve Speeds = AnimationCurve.Linear(0f, 1f, 1f, 1f);

	public AnimationCurve LifeTimes = AnimationCurve.Linear(0f, 1f, 1f, 1f);

	public float SizesRnd = 0.5f;

	public float Size = 1f;

	public float Speed = 1f;

	public float LifeTime = 1f;

	public Gradient Color;

	public int CountMin = 1;

	public int CountRange = 3;

	private Vector3 _E000;

	private Transform _E001;

	private void Awake()
	{
		_E001 = base.transform;
		_E000 = _E001.position - _E001.up * StartPos;
	}

	public void UpdateValues()
	{
		_E000 = _E001.position;
	}

	public void Emit(_E412 emitter)
	{
		Vector3 position = _E001.position;
		Vector3 vector = -_E001.up;
		Vector3 vector2 = position + vector * StartPos;
		Vector3 vector3 = position - _E000;
		int num = ((CountRange < 1) ? CountMin : _E8EE.Int(CountMin, CountRange));
		for (int i = 0; i < num; i++)
		{
			Vector3 position2 = vector2;
			if (EmitterRadius >= float.Epsilon)
			{
				position2 += _E8EE.VectorNormalized() * EmitterRadius;
			}
			float time = _E8EE.Float();
			Vector3 velocity = vector3 / 2f + (vector + _E8EE.VectorNormalized() * ConusSize).normalized * Speeds.Evaluate(time) * Speed;
			float size = (Sizes.Evaluate(time) + Random.Range(0f - SizesRnd, SizesRnd)) * Size;
			float lifetime = LifeTimes.Evaluate(time) * LifeTime;
			emitter.Emit(position2, velocity, size, lifetime, Color.Evaluate(time));
		}
	}
}
