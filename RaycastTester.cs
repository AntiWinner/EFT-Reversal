using UnityEngine;

public class RaycastTester : MonoBehaviour
{
	public float dist = 11f;

	public int X_size = 20;

	public float step = 0.1f;

	public Vector3 dir = Vector3.up;

	private void OnDrawGizmosSelected()
	{
		int layerMask = (1 << LayerMask.NameToLayer(_ED3E._E000(25436))) | (1 << LayerMask.NameToLayer(_ED3E._E000(25428)));
		for (int i = 0; i < X_size; i++)
		{
			for (int j = 0; j < X_size; j++)
			{
				Vector3 vector = base.transform.position + new Vector3((float)i * step, 0f, (float)j * step);
				Ray ray = new Ray(vector, dir);
				if (Physics.Raycast(ray, out var hitInfo, dist, layerMask))
				{
					Debug.DrawLine(ray.origin, hitInfo.point, Color.yellow);
				}
				else
				{
					Debug.DrawLine(ray.origin, vector + dir * dist, Color.green);
				}
			}
		}
	}
}
