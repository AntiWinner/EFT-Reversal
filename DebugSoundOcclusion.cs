using System.Collections.Generic;
using UnityEngine;

public class DebugSoundOcclusion : MonoBehaviour
{
	private List<_E3C0> _E000;

	private void Awake()
	{
		_E000 = new List<_E3C0>();
	}

	public void UpdateDebugInfo(_E3C0 debugOcclusionRayInfo)
	{
		_E000.Add(debugOcclusionRayInfo);
	}

	public void RenderLines()
	{
		if (_E000.Count == 0)
		{
			return;
		}
		foreach (_E3C0 item in _E000)
		{
			Debug.DrawLine(item.StartPoint, item.EndPoint, item.Color);
		}
		_E000.Clear();
	}
}
