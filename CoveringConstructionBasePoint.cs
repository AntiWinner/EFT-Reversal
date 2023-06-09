using System;
using UnityEngine;

public class CoveringConstructionBasePoint : MonoBehaviour
{
	[HideInInspector]
	public bool Selected;

	public CoveringConstructionBasePoint LNeighbor;

	public CoveringConstructionBasePoint RNeighbor;

	public int Index;

	private void OnDrawGizmosSelected()
	{
		float num = 1.5f;
		Vector3 vector = base.transform.position + Vector3.up * num;
		int num2 = Convert.ToInt32(LNeighbor != null) + Convert.ToInt32(RNeighbor != null);
		Gizmos.color = new Color(1f, 1f, 0f, 0.75f);
		switch (num2)
		{
		case 0:
			Gizmos.color = Color.red;
			break;
		case 1:
			Gizmos.color = Color.magenta;
			break;
		}
		Gizmos.DrawWireCube(vector - Vector3.up * num / 2f, new Vector3(0.05f, num, 0.05f));
		Gizmos.color = Color.red;
		if (LNeighbor != null)
		{
			Gizmos.DrawLine(vector, LNeighbor.transform.position);
		}
		if (RNeighbor != null)
		{
			Gizmos.DrawLine(vector, RNeighbor.transform.position);
		}
	}
}
