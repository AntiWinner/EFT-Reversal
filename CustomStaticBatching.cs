using System.Collections.Generic;
using UnityEngine;

public class CustomStaticBatching : MonoBehaviour
{
	private void Start()
	{
		Renderer[] componentsInChildren = base.gameObject.GetComponentsInChildren<Renderer>();
		LinkedList<GameObject> linkedList = new LinkedList<GameObject>();
		foreach (Renderer renderer in componentsInChildren)
		{
			if (renderer.enabled && renderer.sharedMaterial != null)
			{
				GameObject gameObject = renderer.gameObject;
				if (gameObject.isStatic)
				{
					linkedList.AddLast(gameObject);
				}
			}
		}
		GameObject[] array = new GameObject[linkedList.Count];
		linkedList.CopyTo(array, 0);
		StaticBatchingUtility.Combine(array, base.gameObject);
	}
}
