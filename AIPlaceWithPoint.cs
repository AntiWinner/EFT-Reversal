using UnityEngine;

public class AIPlaceWithPoint : AIPlaceInfo
{
	public Transform Point;

	private void OnDrawGizmos()
	{
		Gizmos.DrawSphere(base.transform.position, 0.3f);
		Gizmos.DrawSphere(base.transform.position, 0.5f);
	}
}
