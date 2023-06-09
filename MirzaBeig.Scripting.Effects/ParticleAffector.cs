using System;
using System.Collections.Generic;
using UnityEngine;

namespace MirzaBeig.Scripting.Effects;

public abstract class ParticleAffector : MonoBehaviour
{
	protected struct GetForceParameters
	{
		public float distanceToAffectorCenterSqr;

		public Vector3 scaledDirectionToAffectorCenter;

		public Vector3 particlePosition;
	}

	[Header("Common Controls")]
	public float radius = float.PositiveInfinity;

	public float force = 5f;

	public Vector3 offset = Vector3.zero;

	private float _E002;

	private float _E003;

	private float _E004;

	private Vector3 _E005;

	private float[] _E006;

	public AnimationCurve scaleForceByDistance = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 1f));

	private ParticleSystem _E007;

	public List<ParticleSystem> _particleSystems;

	private int _E008;

	private List<ParticleSystem> _E009 = new List<ParticleSystem>();

	private ParticleSystem.Particle[][] _E00A;

	private ParticleSystem.MainModule[] _E00B;

	private Renderer[] _E00C;

	protected ParticleSystem currentParticleSystem;

	protected GetForceParameters parameters;

	public bool alwaysUpdate;

	public float scaledRadius => radius * base.transform.lossyScale.x;

	protected virtual void Awake()
	{
	}

	protected virtual void Start()
	{
		_E007 = GetComponent<ParticleSystem>();
	}

	protected virtual void PerParticleSystemSetup()
	{
	}

	protected virtual Vector3 GetForce()
	{
		return Vector3.zero;
	}

	protected virtual void Update()
	{
	}

	public void AddParticleSystem(ParticleSystem particleSystem)
	{
		_particleSystems.Add(particleSystem);
	}

	public void RemoveParticleSystem(ParticleSystem particleSystem)
	{
		_particleSystems.Remove(particleSystem);
	}

	protected virtual void LateUpdate()
	{
		_E002 = scaledRadius;
		_E003 = _E002 * _E002;
		_E004 = force * Time.deltaTime;
		_E005 = base.transform.position + offset;
		if (_particleSystems.Count != 0)
		{
			if (_E009.Count != _particleSystems.Count)
			{
				_E009.Clear();
				_E009.AddRange(_particleSystems);
			}
			else
			{
				for (int i = 0; i < _particleSystems.Count; i++)
				{
					_E009[i] = _particleSystems[i];
				}
			}
		}
		else if ((bool)_E007)
		{
			if (_E009.Count == 1)
			{
				_E009[0] = _E007;
			}
			else
			{
				_E009.Clear();
				_E009.Add(_E007);
			}
		}
		else
		{
			_E009.Clear();
			_E009.AddRange(_E3AA.FindUnityObjectsOfType<ParticleSystem>());
		}
		parameters = default(GetForceParameters);
		_E008 = _E009.Count;
		if (_E00A == null || _E00A.Length < _E008)
		{
			_E00A = new ParticleSystem.Particle[_E008][];
			_E00B = new ParticleSystem.MainModule[_E008];
			_E00C = new Renderer[_E008];
			_E006 = new float[_E008];
			for (int j = 0; j < _E008; j++)
			{
				_E00B[j] = _E009[j].main;
				_E00C[j] = _E009[j].GetComponent<Renderer>();
				_E006[j] = _E009[j].externalForces.multiplier;
			}
		}
		for (int k = 0; k < _E008; k++)
		{
			if (!_E00C[k].isVisible && !alwaysUpdate)
			{
				continue;
			}
			int maxParticles = _E00B[k].maxParticles;
			if (_E00A[k] == null || _E00A[k].Length < maxParticles)
			{
				_E00A[k] = new ParticleSystem.Particle[maxParticles];
			}
			currentParticleSystem = _E009[k];
			PerParticleSystemSetup();
			int particles = currentParticleSystem.GetParticles(_E00A[k]);
			ParticleSystemSimulationSpace simulationSpace = _E00B[k].simulationSpace;
			ParticleSystemScalingMode scalingMode = _E00B[k].scalingMode;
			Transform transform = currentParticleSystem.transform;
			Transform customSimulationSpace = _E00B[k].customSimulationSpace;
			if (simulationSpace == ParticleSystemSimulationSpace.World)
			{
				for (int l = 0; l < particles; l++)
				{
					parameters.particlePosition = _E00A[k][l].position;
					parameters.scaledDirectionToAffectorCenter.x = _E005.x - parameters.particlePosition.x;
					parameters.scaledDirectionToAffectorCenter.y = _E005.y - parameters.particlePosition.y;
					parameters.scaledDirectionToAffectorCenter.z = _E005.z - parameters.particlePosition.z;
					parameters.distanceToAffectorCenterSqr = parameters.scaledDirectionToAffectorCenter.sqrMagnitude;
					if (parameters.distanceToAffectorCenterSqr < _E003)
					{
						float time = parameters.distanceToAffectorCenterSqr / _E003;
						float num = scaleForceByDistance.Evaluate(time);
						Vector3 vector = GetForce();
						float num2 = _E004 * num * _E006[k];
						vector.x *= num2;
						vector.y *= num2;
						vector.z *= num2;
						Vector3 velocity = _E00A[k][l].velocity;
						velocity.x += vector.x;
						velocity.y += vector.y;
						velocity.z += vector.z;
						_E00A[k][l].velocity = velocity;
					}
				}
			}
			else
			{
				Vector3 zero = Vector3.zero;
				Quaternion identity = Quaternion.identity;
				Vector3 one = Vector3.one;
				Transform transform2 = transform;
				switch (simulationSpace)
				{
				case ParticleSystemSimulationSpace.Local:
					zero = transform2.position;
					identity = transform2.rotation;
					one = transform2.localScale;
					break;
				case ParticleSystemSimulationSpace.Custom:
					transform2 = customSimulationSpace;
					zero = transform2.position;
					identity = transform2.rotation;
					one = transform2.localScale;
					break;
				default:
					throw new NotSupportedException(string.Format(_ED3E._E000(128377), simulationSpace));
				}
				for (int m = 0; m < particles; m++)
				{
					parameters.particlePosition = _E00A[k][m].position;
					if (simulationSpace == ParticleSystemSimulationSpace.Local || simulationSpace == ParticleSystemSimulationSpace.Custom)
					{
						switch (scalingMode)
						{
						case ParticleSystemScalingMode.Hierarchy:
							parameters.particlePosition = transform2.TransformPoint(_E00A[k][m].position);
							break;
						case ParticleSystemScalingMode.Local:
							parameters.particlePosition = Vector3.Scale(parameters.particlePosition, one);
							parameters.particlePosition = identity * parameters.particlePosition;
							parameters.particlePosition += zero;
							break;
						case ParticleSystemScalingMode.Shape:
							parameters.particlePosition = identity * parameters.particlePosition;
							parameters.particlePosition += zero;
							break;
						default:
							throw new NotSupportedException(string.Format(_ED3E._E000(128377), scalingMode));
						}
					}
					parameters.scaledDirectionToAffectorCenter.x = _E005.x - parameters.particlePosition.x;
					parameters.scaledDirectionToAffectorCenter.y = _E005.y - parameters.particlePosition.y;
					parameters.scaledDirectionToAffectorCenter.z = _E005.z - parameters.particlePosition.z;
					parameters.distanceToAffectorCenterSqr = parameters.scaledDirectionToAffectorCenter.sqrMagnitude;
					if (!(parameters.distanceToAffectorCenterSqr < _E003))
					{
						continue;
					}
					float time2 = parameters.distanceToAffectorCenterSqr / _E003;
					float num3 = scaleForceByDistance.Evaluate(time2);
					Vector3 vector2 = GetForce();
					float num4 = _E004 * num3 * _E006[k];
					vector2.x *= num4;
					vector2.y *= num4;
					vector2.z *= num4;
					if (simulationSpace == ParticleSystemSimulationSpace.Local || simulationSpace == ParticleSystemSimulationSpace.Custom)
					{
						switch (scalingMode)
						{
						case ParticleSystemScalingMode.Hierarchy:
							vector2 = transform2.InverseTransformVector(vector2);
							break;
						case ParticleSystemScalingMode.Local:
							vector2 = Quaternion.Inverse(identity) * vector2;
							vector2 = Vector3.Scale(vector2, new Vector3(1f / one.x, 1f / one.y, 1f / one.z));
							break;
						case ParticleSystemScalingMode.Shape:
							vector2 = Quaternion.Inverse(identity) * vector2;
							break;
						default:
							throw new NotSupportedException(string.Format(_ED3E._E000(128377), scalingMode));
						}
					}
					Vector3 velocity2 = _E00A[k][m].velocity;
					velocity2.x += vector2.x;
					velocity2.y += vector2.y;
					velocity2.z += vector2.z;
					_E00A[k][m].velocity = velocity2;
				}
			}
			currentParticleSystem.SetParticles(_E00A[k], particles);
		}
	}

	private void OnApplicationQuit()
	{
	}

	protected virtual void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(base.transform.position + offset, scaledRadius);
	}
}
