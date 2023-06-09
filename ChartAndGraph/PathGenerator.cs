using UnityEngine;

namespace ChartAndGraph;

public abstract class PathGenerator : MonoBehaviour
{
	public abstract void Generator(Vector3[] path, float thickness, bool closed);

	public abstract void Clear();
}
