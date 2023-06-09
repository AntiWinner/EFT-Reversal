using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshObstacle))]
public class NavMeshObstacleWrap : MonoBehaviour
{
	public ENavMeshObstacleType Type;

	public NavMeshObstacle NavMeshObstacle;

	public bool Carving
	{
		get
		{
			return NavMeshObstacle;
		}
		set
		{
			NavMeshObstacle.carving = value;
		}
	}
}
