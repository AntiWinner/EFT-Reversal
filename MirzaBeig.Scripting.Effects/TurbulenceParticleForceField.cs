using UnityEngine;

namespace MirzaBeig.Scripting.Effects;

[AddComponentMenu("Effects/Particle Force Fields/Turbulence Particle Force Field")]
public class TurbulenceParticleForceField : ParticleForceField
{
	public enum NoiseType
	{
		PseudoPerlin,
		Perlin,
		Simplex,
		OctavePerlin,
		OctaveSimplex
	}

	[Tooltip("Noise texture mutation speed.")]
	[Header("ForceField Controls")]
	public float scrollSpeed = 1f;

	[Range(0f, 8f)]
	[Tooltip("Noise texture detail amplifier.")]
	public float frequency = 1f;

	public NoiseType noiseType = NoiseType.Perlin;

	[Tooltip("Overlapping noise iterations. 1 = no additional iterations.")]
	[Header("Octave Variant-Only Controls")]
	[Range(1f, 8f)]
	public int octaves = 1;

	[Tooltip("Frequency scale per-octave. Can be used to change the overlap every iteration.")]
	[Range(0f, 4f)]
	public float octaveMultiplier = 0.5f;

	[Range(0f, 1f)]
	[Tooltip("Amplitude scale per-octave. Can be used to change the overlap every iteration.")]
	public float octaveScale = 2f;

	private float _E00D;

	private float _E00E;

	private float _E00F;

	private float _E010;

	private float _E011;

	private float _E012;

	private float _E013;

	protected override void Awake()
	{
		base.Awake();
	}

	protected override void Start()
	{
		base.Start();
		_E00E = Random.Range(-32f, 32f);
		_E00F = Random.Range(-32f, 32f);
		_E010 = Random.Range(-32f, 32f);
	}

	protected override void Update()
	{
		_E00D = Time.time;
		base.Update();
	}

	protected override void LateUpdate()
	{
		_E011 = _E00D * scrollSpeed + _E00E;
		_E012 = _E00D * scrollSpeed + _E00F;
		_E013 = _E00D * scrollSpeed + _E010;
		base.LateUpdate();
	}

	protected override Vector3 GetForce()
	{
		float num = parameters.particlePosition.x + _E011;
		float num2 = parameters.particlePosition.y + _E011;
		float num3 = parameters.particlePosition.z + _E011;
		float num4 = parameters.particlePosition.x + _E012;
		float num5 = parameters.particlePosition.y + _E012;
		float num6 = parameters.particlePosition.z + _E012;
		float num7 = parameters.particlePosition.x + _E013;
		float num8 = parameters.particlePosition.y + _E013;
		float num9 = parameters.particlePosition.z + _E013;
		Vector3 result = default(Vector3);
		switch (noiseType)
		{
		case NoiseType.PseudoPerlin:
		{
			float t = Mathf.PerlinNoise(num * frequency, num5 * frequency);
			float t2 = Mathf.PerlinNoise(num * frequency, num6 * frequency);
			float t3 = Mathf.PerlinNoise(num * frequency, num4 * frequency);
			t = Mathf.Lerp(-1f, 1f, t);
			t2 = Mathf.Lerp(-1f, 1f, t2);
			t3 = Mathf.Lerp(-1f, 1f, t3);
			Vector3 vector = Vector3.right * t;
			Vector3 vector2 = Vector3.up * t2;
			Vector3 vector3 = Vector3.forward * t3;
			return vector + vector2 + vector3;
		}
		default:
			result.x = Noise2.perlin(num * frequency, num2 * frequency, num3 * frequency);
			result.y = Noise2.perlin(num4 * frequency, num5 * frequency, num6 * frequency);
			result.z = Noise2.perlin(num7 * frequency, num8 * frequency, num9 * frequency);
			return result;
		case NoiseType.Simplex:
			result.x = Noise2.simplex(num * frequency, num2 * frequency, num3 * frequency);
			result.y = Noise2.simplex(num4 * frequency, num5 * frequency, num6 * frequency);
			result.z = Noise2.simplex(num7 * frequency, num8 * frequency, num9 * frequency);
			break;
		case NoiseType.OctavePerlin:
			result.x = Noise2.octavePerlin(num, num2, num3, frequency, octaves, octaveMultiplier, octaveScale);
			result.y = Noise2.octavePerlin(num4, num5, num6, frequency, octaves, octaveMultiplier, octaveScale);
			result.z = Noise2.octavePerlin(num7, num8, num9, frequency, octaves, octaveMultiplier, octaveScale);
			break;
		case NoiseType.OctaveSimplex:
			result.x = Noise2.octaveSimplex(num, num2, num3, frequency, octaves, octaveMultiplier, octaveScale);
			result.y = Noise2.octaveSimplex(num4, num5, num6, frequency, octaves, octaveMultiplier, octaveScale);
			result.z = Noise2.octaveSimplex(num7, num8, num9, frequency, octaves, octaveMultiplier, octaveScale);
			break;
		}
		return result;
	}

	protected override void OnDrawGizmosSelected()
	{
		if (base.enabled)
		{
			base.OnDrawGizmosSelected();
		}
	}
}
