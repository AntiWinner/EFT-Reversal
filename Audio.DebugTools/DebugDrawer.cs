using UnityEngine;

namespace Audio.DebugTools;

public class DebugDrawer : MonoBehaviour
{
	private Vector3 _E000;

	private float _E001 = 0.1f;

	public void Awake()
	{
		_E000 = base.transform.position;
	}

	public void SetRadius(float radius)
	{
		_E001 = radius;
	}

	public void SetPosition(Vector3 position)
	{
		_E000 = position;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(_E000, _E001);
	}
}
