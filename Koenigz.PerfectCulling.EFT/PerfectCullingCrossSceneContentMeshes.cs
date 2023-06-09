using System.Collections.Generic;
using UnityEngine;

namespace Koenigz.PerfectCulling.EFT;

[DisallowMultipleComponent]
public sealed class PerfectCullingCrossSceneContentMeshes : PerfectCullingCrossSceneContent
{
	[SerializeField]
	private Renderer[] _windows;

	public Renderer[] Windows => _windows;

	public void FillWindows()
	{
		Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>();
		List<Renderer> list = new List<Renderer>();
		Renderer[] array = componentsInChildren;
		foreach (Renderer renderer in array)
		{
			if (!_E4AB.NotBatchedByWindowsManager(renderer))
			{
				list.Add(renderer);
			}
		}
		_windows = list.ToArray();
	}
}
