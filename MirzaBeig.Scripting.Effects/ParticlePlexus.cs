using System;
using System.Collections.Generic;
using UnityEngine;

namespace MirzaBeig.Scripting.Effects;

[AddComponentMenu("Effects/Particle Plexus")]
[RequireComponent(typeof(ParticleSystem))]
public class ParticlePlexus : MonoBehaviour
{
	public float maxDistance = 1f;

	public int maxConnections = 5;

	public int maxLineRenderers = 100;

	[Range(0f, 1f)]
	public float widthFromParticle = 0.125f;

	[Range(0f, 1f)]
	public float colourFromParticle = 1f;

	[Range(0f, 1f)]
	public float alphaFromParticle = 1f;

	private ParticleSystem _E000;

	private ParticleSystem.Particle[] _E001;

	private Vector3[] _E002;

	private Color[] _E003;

	private float[] _E004;

	private ParticleSystem.MainModule _E005;

	public LineRenderer lineRendererTemplate;

	private List<LineRenderer> _E006 = new List<LineRenderer>();

	private Transform _E007;

	[Header("General Performance Settings")]
	[Range(0f, 1f)]
	public float delay;

	private float _E008;

	public bool alwaysUpdate;

	private bool _E009;

	private void Start()
	{
		_E000 = GetComponent<ParticleSystem>();
		_E005 = _E000.main;
		_E007 = base.transform;
	}

	private void OnDisable()
	{
		for (int i = 0; i < _E006.Count; i++)
		{
			_E006[i].enabled = false;
		}
	}

	private void OnBecameVisible()
	{
		_E009 = true;
	}

	private void OnBecameInvisible()
	{
		_E009 = false;
	}

	private void LateUpdate()
	{
		int num = _E006.Count;
		if (num > maxLineRenderers)
		{
			for (int i = maxLineRenderers; i < num; i++)
			{
				UnityEngine.Object.Destroy(_E006[i].gameObject);
			}
			_E006.RemoveRange(maxLineRenderers, num - maxLineRenderers);
			num -= num - maxLineRenderers;
		}
		if (!alwaysUpdate && !_E009)
		{
			return;
		}
		int maxParticles = _E005.maxParticles;
		if (_E001 == null || _E001.Length < maxParticles)
		{
			_E001 = new ParticleSystem.Particle[maxParticles];
			_E002 = new Vector3[maxParticles];
			_E003 = new Color[maxParticles];
			_E004 = new float[maxParticles];
		}
		_E008 += Time.deltaTime;
		if (!(_E008 >= delay))
		{
			return;
		}
		_E008 = 0f;
		int num2 = 0;
		if (maxConnections > 0 && maxLineRenderers > 0)
		{
			_E000.GetParticles(_E001);
			int particleCount = _E000.particleCount;
			float num3 = maxDistance * maxDistance;
			ParticleSystemSimulationSpace simulationSpace = _E005.simulationSpace;
			ParticleSystemScalingMode scalingMode = _E005.scalingMode;
			Transform customSimulationSpace = _E005.customSimulationSpace;
			Color startColor = lineRendererTemplate.startColor;
			Color endColor = lineRendererTemplate.endColor;
			float a = lineRendererTemplate.startWidth * lineRendererTemplate.widthMultiplier;
			float a2 = lineRendererTemplate.endWidth * lineRendererTemplate.widthMultiplier;
			for (int j = 0; j < particleCount; j++)
			{
				_E002[j] = _E001[j].position;
				_E003[j] = _E001[j].GetCurrentColor(_E000);
				_E004[j] = _E001[j].GetCurrentSize(_E000);
			}
			Vector3 vector = default(Vector3);
			if (simulationSpace == ParticleSystemSimulationSpace.World)
			{
				for (int k = 0; k < particleCount; k++)
				{
					if (num2 == maxLineRenderers)
					{
						break;
					}
					Color b = _E003[k];
					Color startColor2 = Color.LerpUnclamped(startColor, b, colourFromParticle);
					startColor2.a = Mathf.LerpUnclamped(startColor.a, b.a, alphaFromParticle);
					float startWidth = Mathf.LerpUnclamped(a, _E004[k], widthFromParticle);
					int num4 = 0;
					for (int l = k + 1; l < particleCount; l++)
					{
						vector.x = _E002[k].x - _E002[l].x;
						vector.y = _E002[k].y - _E002[l].y;
						vector.z = _E002[k].z - _E002[l].z;
						if (vector.x * vector.x + vector.y * vector.y + vector.z * vector.z <= num3)
						{
							LineRenderer item;
							if (num2 == num)
							{
								item = UnityEngine.Object.Instantiate(lineRendererTemplate, _E007, worldPositionStays: false);
								_E006.Add(item);
								num++;
							}
							item = _E006[num2];
							item.enabled = true;
							item.SetPosition(0, _E002[k]);
							item.SetPosition(1, _E002[l]);
							item.startColor = startColor2;
							b = _E003[l];
							Color endColor2 = Color.LerpUnclamped(endColor, b, colourFromParticle);
							endColor2.a = Mathf.LerpUnclamped(endColor.a, b.a, alphaFromParticle);
							item.endColor = endColor2;
							item.startWidth = startWidth;
							item.endWidth = Mathf.LerpUnclamped(a2, _E004[l], widthFromParticle);
							num2++;
							num4++;
							if (num4 == maxConnections || num2 == maxLineRenderers)
							{
								break;
							}
						}
					}
				}
			}
			else
			{
				Vector3 zero = Vector3.zero;
				Quaternion identity = Quaternion.identity;
				Vector3 one = Vector3.one;
				Transform transform = _E007;
				switch (simulationSpace)
				{
				case ParticleSystemSimulationSpace.Local:
					zero = transform.position;
					identity = transform.rotation;
					one = transform.localScale;
					break;
				case ParticleSystemSimulationSpace.Custom:
					transform = customSimulationSpace;
					zero = transform.position;
					identity = transform.rotation;
					one = transform.localScale;
					break;
				default:
					throw new NotSupportedException(string.Format(_ED3E._E000(128377), simulationSpace));
				}
				Vector3 vector2 = Vector3.zero;
				Vector3 vector3 = Vector3.zero;
				for (int m = 0; m < particleCount; m++)
				{
					if (num2 == maxLineRenderers)
					{
						break;
					}
					if (simulationSpace == ParticleSystemSimulationSpace.Local || simulationSpace == ParticleSystemSimulationSpace.Custom)
					{
						switch (scalingMode)
						{
						case ParticleSystemScalingMode.Hierarchy:
							vector2 = transform.TransformPoint(_E002[m]);
							break;
						case ParticleSystemScalingMode.Local:
							vector2.x = _E002[m].x * one.x;
							vector2.y = _E002[m].y * one.y;
							vector2.z = _E002[m].z * one.z;
							vector2 = identity * vector2;
							vector2.x += zero.x;
							vector2.y += zero.y;
							vector2.z += zero.z;
							break;
						case ParticleSystemScalingMode.Shape:
							vector2 = identity * _E002[m];
							vector2.x += zero.x;
							vector2.y += zero.y;
							vector2.z += zero.z;
							break;
						default:
							throw new NotSupportedException(string.Format(_ED3E._E000(128377), scalingMode));
						}
					}
					Color b2 = _E003[m];
					Color startColor3 = Color.LerpUnclamped(startColor, b2, colourFromParticle);
					startColor3.a = Mathf.LerpUnclamped(startColor.a, b2.a, alphaFromParticle);
					float startWidth2 = Mathf.LerpUnclamped(a, _E004[m], widthFromParticle);
					int num5 = 0;
					for (int n = m + 1; n < particleCount; n++)
					{
						if (simulationSpace == ParticleSystemSimulationSpace.Local || simulationSpace == ParticleSystemSimulationSpace.Custom)
						{
							switch (scalingMode)
							{
							case ParticleSystemScalingMode.Hierarchy:
								vector3 = transform.TransformPoint(_E002[n]);
								break;
							case ParticleSystemScalingMode.Local:
								vector3.x = _E002[n].x * one.x;
								vector3.y = _E002[n].y * one.y;
								vector3.z = _E002[n].z * one.z;
								vector3 = identity * vector3;
								vector3.x += zero.x;
								vector3.y += zero.y;
								vector3.z += zero.z;
								break;
							case ParticleSystemScalingMode.Shape:
								vector3 = identity * _E002[n];
								vector3.x += zero.x;
								vector3.y += zero.y;
								vector3.z += zero.z;
								break;
							default:
								throw new NotSupportedException(string.Format(_ED3E._E000(128377), scalingMode));
							}
						}
						vector.x = _E002[m].x - _E002[n].x;
						vector.y = _E002[m].y - _E002[n].y;
						vector.z = _E002[m].z - _E002[n].z;
						if (vector.x * vector.x + vector.y * vector.y + vector.z * vector.z <= num3)
						{
							LineRenderer item2;
							if (num2 == num)
							{
								item2 = UnityEngine.Object.Instantiate(lineRendererTemplate, _E007, worldPositionStays: false);
								_E006.Add(item2);
								num++;
							}
							item2 = _E006[num2];
							item2.enabled = true;
							item2.SetPosition(0, vector2);
							item2.SetPosition(1, vector3);
							item2.startColor = startColor3;
							b2 = _E003[n];
							Color endColor3 = Color.LerpUnclamped(endColor, b2, colourFromParticle);
							endColor3.a = Mathf.LerpUnclamped(endColor.a, b2.a, alphaFromParticle);
							item2.endColor = endColor3;
							item2.startWidth = startWidth2;
							item2.endWidth = Mathf.LerpUnclamped(a2, _E004[n], widthFromParticle);
							num2++;
							num5++;
							if (num5 == maxConnections || num2 == maxLineRenderers)
							{
								break;
							}
						}
					}
				}
			}
		}
		for (int num6 = num2; num6 < num; num6++)
		{
			if (_E006[num6].enabled)
			{
				_E006[num6].enabled = false;
			}
		}
	}
}
