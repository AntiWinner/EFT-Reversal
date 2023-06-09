using System.Collections.Generic;
using UnityEngine;

namespace Koenigz.PerfectCulling.EFT;

public sealed class SharedOccluder : MonoBehaviour
{
	private void Awake()
	{
		base.gameObject.SetActive(value: false);
	}

	public List<Renderer> GetRenderers()
	{
		return new List<Renderer>(GetComponentsInChildren<MeshRenderer>());
	}
}
