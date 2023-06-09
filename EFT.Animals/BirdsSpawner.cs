using System.Collections;
using BezierSplineTools;
using UnityEngine;

namespace EFT.Animals;

public class BirdsSpawner : MonoBehaviour
{
	[SerializeField]
	private BezierSpline[] _paths;

	[SerializeField]
	private GameObject[] _birdsPrefabs;

	[SerializeField]
	private int _count;

	[SerializeField]
	private float _spawnDelay = 5f;

	private void Awake()
	{
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
		if (_paths.Length == 0 || _birdsPrefabs.Length == 0)
		{
			Debug.LogError(_ED3E._E000(242397));
		}
		else
		{
			StartCoroutine(_E000(count));
		}
	}

	private IEnumerator _E000(int count)
	{
		int num = 0;
		while (num < count)
		{
			BezierSpline[] paths = _paths;
			foreach (BezierSpline spline in paths)
			{
				Bird component = Object.Instantiate(_birdsPrefabs[Random.Range(0, _birdsPrefabs.Length)]).GetComponent<Bird>();
				component.gameObject.transform.SetParent(base.transform, worldPositionStays: false);
				component.gameObject.AddComponent<BirdCurveBrain>().Init(spline);
				num++;
				if (num >= count)
				{
					yield break;
				}
			}
			yield return new WaitForSeconds(_spawnDelay);
		}
	}
}
