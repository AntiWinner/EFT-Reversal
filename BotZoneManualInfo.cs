using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BotZone))]
public class BotZoneManualInfo : MonoBehaviour
{
	public const int MANUAL_ZONE_START_ID = 100000;

	public bool _alwaysDraw;

	public bool DrawSides;

	public float MinDefenceLevelToDraw;

	public BoxCollider[] ExcludableColliders;

	public List<CustomNavigationPoint> Points = new List<CustomNavigationPoint>(100);

	public void Init()
	{
		SetCllidersToTrigers();
	}

	public void Add(CustomNavigationPoint c)
	{
		c.Id = Points.Count;
		c.CovPointsPlaceSerializable.Id = c.Id;
		Points.Add(c);
	}

	public void SetCllidersToTrigers()
	{
		BoxCollider[] excludableColliders = ExcludableColliders;
		for (int i = 0; i < excludableColliders.Length; i++)
		{
			excludableColliders[i].enabled = false;
		}
	}

	public void SetAlwaysDraw(bool alwaysDraw)
	{
		_alwaysDraw = alwaysDraw;
	}

	public void OptimizeWithZone()
	{
		_E003();
		BotZone component = GetComponent<BotZone>();
		if (ExcludableColliders != null && ExcludableColliders.Length != 0)
		{
			component.CoverPoints = RemoveByExcutableColiders(component.CoverPoints, ExcludableColliders);
			component.AmbushPoints = RemoveByExcutableColiders(component.AmbushPoints, ExcludableColliders);
		}
		if (Points.Count > 0)
		{
			_E002(component.CoverPoints, Points);
			_E002(component.AmbushPoints, Points);
		}
	}

	public T[] RemoveByExcutableColiders<T>(T[] coreList, BoxCollider[] colliders) where T : IPositionPoint
	{
		List<T> list = new List<T>();
		int num = 0;
		for (int i = 0; i < coreList.Length; i++)
		{
			T item = coreList[i];
			bool flag = false;
			foreach (BoxCollider box in colliders)
			{
				if (_E079.PointInOABB(item.Position, box))
				{
					flag = true;
				}
			}
			if (!flag)
			{
				list.Add(item);
			}
		}
		Debug.Log(_ED3E._E000(13942) + num);
		return list.ToArray();
	}

	private void OnDrawGizmosSelected()
	{
		if (!_alwaysDraw)
		{
			_E000();
		}
	}

	private void OnDrawGizmos()
	{
		if (_alwaysDraw)
		{
			_E000();
		}
	}

	private void _E000()
	{
		foreach (CustomNavigationPoint point in Points)
		{
			if (MinDefenceLevelToDraw > 0f)
			{
				Gizmos.color = new Color(0.9f, 0.2f, 0.2f, 0.5f);
				if (point.CovPointsPlaceSerializable.DefenceLevel > MinDefenceLevelToDraw)
				{
					float num = point.CovPointsPlaceSerializable.DefenceLevel / 3f;
					_E395.DrawCube(point.Position + Vector3.up * num * 0.5f, base.transform.rotation, new Vector3(1f, num, 1f));
				}
			}
			if (point.StrategyType == PointWithNeighborType.ambush)
			{
				point.OnDrawGizmosAsAmbush(null, 0f, DrawSides);
			}
			else
			{
				point.OnDrawGizmosFullAsCover(null, 0f, DrawSides);
			}
		}
	}

	private void _E001()
	{
	}

	private void _E002(CustomNavigationPoint[] coreList, List<CustomNavigationPoint> mergedList)
	{
		List<CustomNavigationPoint> list = new List<CustomNavigationPoint>();
		CustomNavigationPoint[] array = coreList;
		foreach (CustomNavigationPoint customNavigationPoint in array)
		{
			foreach (CustomNavigationPoint merged in mergedList)
			{
				if ((customNavigationPoint.Position - merged.Position).sqrMagnitude < 0.3f)
				{
					list.Add(merged);
				}
			}
		}
		if (list.Count > 0)
		{
			Debug.LogError(_ED3E._E000(13972) + list.Count + _ED3E._E000(13957));
			array = list.ToArray();
			foreach (CustomNavigationPoint item in array)
			{
				mergedList.Remove(item);
			}
		}
		else
		{
			Debug.Log(_ED3E._E000(14014));
		}
	}

	private void _E003()
	{
		List<CustomNavigationPoint> list = new List<CustomNavigationPoint>();
		for (int i = 0; i < Points.Count; i++)
		{
			CustomNavigationPoint customNavigationPoint = Points[i];
			for (int j = i + 1; j < Points.Count; j++)
			{
				CustomNavigationPoint customNavigationPoint2 = Points[j];
				if ((customNavigationPoint2.Position - customNavigationPoint.Position).sqrMagnitude < 0.3f)
				{
					list.Add(customNavigationPoint2);
				}
			}
		}
		if (list.Count > 0)
		{
			Debug.LogError(_ED3E._E000(14044) + list.Count + _ED3E._E000(13957));
			CustomNavigationPoint[] array = list.ToArray();
			foreach (CustomNavigationPoint item in array)
			{
				Points.Remove(item);
			}
		}
		else
		{
			Debug.Log(_ED3E._E000(14014));
		}
	}
}
