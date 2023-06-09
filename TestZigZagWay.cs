using UnityEngine;
using UnityEngine.AI;

public class TestZigZagWay : MonoBehaviour
{
	public Transform P1;

	public Transform P2;

	public void BuildPath()
	{
		NavMeshPath navMeshPath = new NavMeshPath();
		if (NavMesh.CalculatePath(P1.position, P2.position, -1, navMeshPath) && navMeshPath.status == NavMeshPathStatus.PathComplete)
		{
			_E2B3.ModifyZigZag(navMeshPath.corners, out var modifedWay);
			_E07B.DrawPathDebug(navMeshPath.corners, Color.yellow, 5f, 0.5f);
			_E07B.DrawPathDebug(modifedWay.ToArray(), Color.red, 5f, 1f);
			_E2B3.ModifyZigZag2(navMeshPath.corners, out var modifedWay2);
			_E07B.DrawPathDebug(navMeshPath.corners, Color.yellow, 5f, 0.5f);
			_E07B.DrawPathDebug(modifedWay2.ToArray(), Color.magenta, 5f, 1.2f);
		}
	}
}
