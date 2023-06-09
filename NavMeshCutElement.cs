using UnityEngine;
using UnityEngine.AI;

public class NavMeshCutElement : MonoBehaviour
{
	public NavMeshCutGroup Group;

	public NavMeshObstacle Obstacle;

	public BoxCollider Collider;

	private float _E000;

	public float CutPeriodSec = 300f;

	public bool IsCut => Obstacle.carving;

	public Vector3 Position => base.transform.position;

	public bool TryCut()
	{
		if (HaveFreeToCut())
		{
			if (Obstacle.carving)
			{
				return true;
			}
			Group.Cut(this);
			return true;
		}
		if (Group.UnCutOldest())
		{
			Group.Cut(this);
			return true;
		}
		return false;
	}

	private void Update()
	{
		if (Obstacle.carving && _E000 < Time.time)
		{
			Group.UnCut(this);
		}
	}

	public bool HaveFreeToCut()
	{
		return Group.HaveFreeToCut(this);
	}

	private void OnDrawGizmosSelected()
	{
		if (Group != null)
		{
			Group.DrawGizmo();
		}
	}

	public bool IsInside(Vector3 pos)
	{
		return _E079.PointInOABB(pos, Collider);
	}

	public void SetUncutTime()
	{
		_E000 = Time.time + CutPeriodSec;
	}
}
