using UnityEngine;

namespace ChartAndGraph;

[RequireComponent(typeof(LineRenderer))]
public class LineRendererPathGenerator : PathGenerator
{
	private LineRenderer _E00B;

	public void EnsureRenderer()
	{
		_E00B = GetComponent<LineRenderer>();
	}

	public override void Clear()
	{
		EnsureRenderer();
		if (_E00B != null)
		{
			_E00B.positionCount = 0;
		}
	}

	public override void Generator(Vector3[] path, float thickness, bool closed)
	{
		EnsureRenderer();
		if (_E00B != null)
		{
			_E00B.startWidth = thickness;
			_E00B.endWidth = thickness;
			_E00B.positionCount = path.Length;
			for (int i = 0; i < path.Length; i++)
			{
				_E00B.SetPosition(i, path[i]);
			}
		}
	}
}
