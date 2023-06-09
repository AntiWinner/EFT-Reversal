using System;
using System.Collections.Generic;
using UnityEngine;

namespace MirzaBeig.Scripting.Effects;

[Serializable]
[RequireComponent(typeof(ParticleSystem))]
public class ParticleLights : MonoBehaviour
{
	private ParticleSystem ps;

	private List<Light> lights = new List<Light>();

	public float scale = 2f;

	[Range(0f, 8f)]
	public float intensity = 8f;

	public Color colour = Color.white;

	[Range(0f, 1f)]
	public float colourFromParticle = 1f;

	public LightShadows shadows;

	private GameObject template;

	private void Awake()
	{
	}

	private void Start()
	{
		ps = GetComponent<ParticleSystem>();
		template = new GameObject();
		template.transform.SetParent(base.transform);
		template.name = _ED3E._E000(128409);
	}

	private void Update()
	{
	}

	private void LateUpdate()
	{
		ParticleSystem.Particle[] array = new ParticleSystem.Particle[ps.particleCount];
		int particles = ps.GetParticles(array);
		if (lights.Count != particles)
		{
			for (int i = 0; i < lights.Count; i++)
			{
				UnityEngine.Object.Destroy(lights[i].gameObject);
			}
			lights.Clear();
			for (int j = 0; j < particles; j++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(template, base.transform);
				gameObject.name = _ED3E._E000(128402) + (j + 1);
				lights.Add(gameObject.AddComponent<Light>());
			}
		}
		bool flag = ps.main.simulationSpace == ParticleSystemSimulationSpace.World;
		for (int k = 0; k < particles; k++)
		{
			ParticleSystem.Particle particle = array[k];
			Light light = lights[k];
			light.range = particle.GetCurrentSize(ps) * scale;
			light.color = Color.Lerp(colour, particle.GetCurrentColor(ps), colourFromParticle);
			light.intensity = intensity;
			light.shadows = shadows;
			light.transform.position = (flag ? particle.position : ps.transform.TransformPoint(particle.position));
		}
	}
}
