using System.Collections.Generic;
using Comfort.Common;
using UnityEngine;

public class BotEventDebug : MonoBehaviour
{
	public float Power = 10f;

	public bool CanBeUsed = true;

	public void PlaySound()
	{
		Singleton<_E307>.Instance?.PlaySound(null, base.transform.position, Power, AISoundType.gun);
	}

	public void ArtilleryStart()
	{
		Singleton<_E307>.Instance?.ArtilleryStart(base.transform.position, Power, 230f);
	}

	public void OnDrawGizmosSelected()
	{
		Gizmos.DrawWireSphere(base.transform.position, Power);
	}

	public void DoTest()
	{
		List<Transform> list = new List<Transform>();
		foreach (Transform item in base.transform)
		{
			list.Add(item);
		}
		if (list.Count >= 2)
		{
			Transform transform = list[0];
			Transform transform2 = list[1];
			float magnitude = (transform.position - transform2.position).magnitude;
			Debug.Log(_ED3E._E000(54021) + magnitude);
		}
	}
}
