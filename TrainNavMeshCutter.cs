using Comfort.Common;
using EFT.MovingPlatforms;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshObstacle))]
public class TrainNavMeshCutter : MonoBehaviour
{
	public Locomotive.ETravelState CarvingOn = Locomotive.ETravelState.OnRouteToDestination;

	public Locomotive.ETravelState CarvingOff = Locomotive.ETravelState.OnRouteToDestination;

	private _E307 m__E000;

	private NavMeshObstacle _E001;

	private void Awake()
	{
		_E001 = GetComponent<NavMeshObstacle>();
		_E001.carving = false;
	}

	private void Start()
	{
		if (!Singleton<_E307>.Instantiated)
		{
			Singleton<_E307>.Instance = new _E307();
		}
		this.m__E000 = Singleton<_E307>.Instance;
		this.m__E000.OnTrainCome += _E000;
	}

	private void OnDestroy()
	{
		this.m__E000.OnTrainCome -= _E000;
	}

	private void _E000(Locomotive.ETravelState obj)
	{
		if (obj == CarvingOn)
		{
			_E001.carving = true;
		}
		else if (obj == CarvingOff)
		{
			_E001.carving = false;
		}
	}
}
