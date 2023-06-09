using System;
using System.Threading;
using UnityEngine;

namespace MirzaBeig.Scripting.Effects;

[Serializable]
public class ParticleAffectorMT : MonoBehaviour
{
	public float force = 1f;

	public float speed = 1f;

	private ParticleSystem particleSystem;

	private ParticleSystem.Particle[] particles;

	private float randomX;

	private float randomY;

	private float randomZ;

	private float offsetX;

	private float offsetY;

	private float offsetZ;

	private float deltaTime;

	private Thread t;

	private readonly object locker = new object();

	private bool processing;

	private bool isDoneAssigning;

	private void Awake()
	{
	}

	private void Start()
	{
		particleSystem = GetComponent<ParticleSystem>();
		randomX = UnityEngine.Random.Range(-32f, 32f);
		randomY = UnityEngine.Random.Range(-32f, 32f);
		randomZ = UnityEngine.Random.Range(-32f, 32f);
		t = new Thread(_E000);
		t.Start();
		isDoneAssigning = true;
	}

	private void LateUpdate()
	{
		lock (locker)
		{
			if (!processing && isDoneAssigning)
			{
				particles = new ParticleSystem.Particle[particleSystem.particleCount];
				particleSystem.GetParticles(particles);
				float time = Time.time;
				deltaTime = Time.deltaTime;
				offsetX = time * speed * randomX;
				offsetY = time * speed * randomY;
				offsetZ = time * speed * randomZ;
				processing = true;
				isDoneAssigning = false;
			}
		}
		if (t.ThreadState == ThreadState.Stopped)
		{
			t = new Thread(_E000);
			t.Start();
		}
		lock (locker)
		{
			if (!processing && !isDoneAssigning)
			{
				particleSystem.SetParticles(particles, particles.Length);
				isDoneAssigning = true;
			}
		}
	}

	private void _E000()
	{
		lock (locker)
		{
			if (processing)
			{
				for (int i = 0; i < particles.Length; i++)
				{
					ParticleSystem.Particle particle = particles[i];
					Vector3 position = particle.position;
					Vector3 vector = new Vector3(Noise.perlin(offsetX + position.x, offsetX + position.y, offsetX + position.z), Noise.perlin(offsetY + position.x, offsetY + position.y, offsetY + position.z), Noise.perlin(offsetZ + position.x, offsetZ + position.y, offsetZ + position.z)) * force;
					vector *= deltaTime;
					particle.velocity += vector;
					particles[i] = particle;
				}
				processing = false;
			}
		}
	}

	private void OnDisable()
	{
	}

	private void OnApplicationQuit()
	{
	}
}
