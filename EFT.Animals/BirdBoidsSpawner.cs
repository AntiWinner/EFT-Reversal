using System.Collections;
using UnityEngine;

namespace EFT.Animals;

public class BirdBoidsSpawner : MonoBehaviour
{
	[SerializeField]
	[Header("Spawn")]
	private GameObject[] _birdsPrefabs;

	[SerializeField]
	private int _count;

	[SerializeField]
	private float _spawnDelay = 5f;

	[SerializeField]
	private float _spawnRadius = 10f;

	[Header("Behaviour")]
	[Header("Boids:")]
	public float BaseSpeed = 6f;

	public float CohesionRadius = 20f;

	public float SeparationMaxSqrDistance = 25f;

	public int MinBoidsForCohesion = 2;

	public int MaxBoidsForCohesion = 5;

	[Header("Update")]
	public float CalculationVelocityInterval = 1f;

	public float UpdateDirectionInterval = 0.1f;

	[Header("Mults")]
	public float VelocityMult = 1f;

	public float SeparationMult = 1f;

	public float CohesionMult = 1f;

	public float AlignmentMult = 1f;

	[Header("Swarm Center")]
	public float SwarmSqrRadius = 400f;

	[HideInInspector]
	public Transform TransformCache;

	private void Awake()
	{
		TransformCache = base.transform;
		Spawn();
	}

	public void Spawn()
	{
		if (_count > 0)
		{
			Spawn(_count);
		}
	}

	public void Spawn(int count)
	{
		StartCoroutine(_E000(count));
	}

	private IEnumerator _E000(int count)
	{
		int num = 0;
		while (num < count)
		{
			Bird component = Object.Instantiate(_birdsPrefabs[Random.Range(0, _birdsPrefabs.Length)]).GetComponent<Bird>();
			component.gameObject.transform.SetParent(base.transform, worldPositionStays: false);
			component.gameObject.transform.position = base.transform.position + Random.insideUnitSphere * _spawnRadius;
			component.gameObject.AddComponent<BirdBoidBrain>().Init(this);
			num++;
			if (num >= count)
			{
				break;
			}
			if (_spawnDelay > 0f)
			{
				yield return new WaitForSeconds(_spawnDelay);
			}
		}
	}
}
