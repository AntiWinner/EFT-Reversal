using UnityEngine;

namespace EFT.Hideout;

public sealed class StashItemSpawner : MonoBehaviour
{
	public GameObject ObjectToSpawn;

	public float RateOfSpawn = 1f;

	private float _E000;

	private void Update()
	{
		if (!(Time.time <= _E000))
		{
			_E000 = Time.time + RateOfSpawn;
			Vector3 vector = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
			vector = base.transform.TransformPoint(vector * 0.5f);
			Object.Instantiate(ObjectToSpawn, vector, base.transform.rotation);
		}
	}
}
