using UnityEngine;
using UnityEngine.AI;

public class NavMeshPathTester : MonoBehaviour
{
	[SerializeField]
	private Transform pointA;

	[SerializeField]
	private Transform pointB;

	public void BuildPath()
	{
		NavMeshPath navMeshPath = new NavMeshPath();
		NavMesh.CalculatePath(pointA.position, pointB.position, -1, navMeshPath);
		BotZoneDebug botZoneDebug = Object.FindObjectOfType<BotZoneDebug>();
		if (botZoneDebug != null)
		{
			botZoneDebug.Clear();
			for (int i = 0; i < navMeshPath.corners.Length - 1; i++)
			{
				botZoneDebug.AddLine(_ED3E._E000(36567), navMeshPath.corners[i], navMeshPath.corners[i + 1]);
				botZoneDebug.Add(_ED3E._E000(36567), navMeshPath.corners[i]);
			}
			botZoneDebug.Add(_ED3E._E000(36567), navMeshPath.corners[navMeshPath.corners.Length - 1]);
			botZoneDebug.Add(_ED3E._E000(36556), pointA.position);
			botZoneDebug.Add(_ED3E._E000(36556), pointB.position);
		}
		Debug.LogError(_ED3E._E000(36555) + navMeshPath.status);
	}
}
