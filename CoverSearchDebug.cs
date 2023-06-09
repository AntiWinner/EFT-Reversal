using System.Collections.Generic;
using UnityEngine;

public class CoverSearchDebug : MonoBehaviour
{
	public static CoverSearchDebug Instance;

	[SerializeField]
	private List<CoverSearchDebugIteration> _coverSearchIterations = new List<CoverSearchDebugIteration>();

	private _E087 _E000;

	public List<CoverSearchDebugIteration> CoverSearchIterations => _coverSearchIterations;

	private void Awake()
	{
		Instance = this;
	}

	public void SetCoverPointsDataCollector(_E087 collector, Vector3 centerPoint, float radius, float deltaRadius)
	{
		if (_E000 != collector)
		{
			_E000 = collector;
		}
		if (_coverSearchIterations.Count == 0 || _coverSearchIterations[_coverSearchIterations.Count - 1].CenterPoint.SqrDistance(centerPoint) > 0.25f)
		{
			CoverSearchDebugIteration coverSearchDebugIteration = new CoverSearchDebugIteration(_ED3E._E000(30808));
			coverSearchDebugIteration.CoverPointGroups.Radius = 0.5f;
			_coverSearchIterations.Add(coverSearchDebugIteration);
			coverSearchDebugIteration.CenterPoint = centerPoint;
		}
	}

	public void ProcessAddNearGroups(HashSet<CustomNavigationPoint> nearGroups, int level, Vector3 centerPos, float sqrRadius, string searchLabel)
	{
		CoverSearchDebugIteration coverSearchDebugIteration;
		if (level == 0)
		{
			coverSearchDebugIteration = new CoverSearchDebugIteration(_ED3E._E000(30808));
			_coverSearchIterations.Add(coverSearchDebugIteration);
			coverSearchDebugIteration.CenterPoint = centerPos;
			coverSearchDebugIteration.CoverPointGroups.Radius = 0.5f;
			coverSearchDebugIteration.searchLabel = searchLabel;
		}
		else
		{
			coverSearchDebugIteration = _coverSearchIterations[_coverSearchIterations.Count - 1];
		}
		foreach (CustomNavigationPoint nearGroup in nearGroups)
		{
			coverSearchDebugIteration.AddPointToIteration(nearGroup.Position, level, sqrRadius);
		}
	}

	public void SetResultCover(CustomNavigationPoint point)
	{
		if (point != null)
		{
			_coverSearchIterations[_coverSearchIterations.Count - 1].ResultCover = point.Position;
		}
	}

	private void OnDrawGizmos()
	{
		foreach (CoverSearchDebugIteration coverSearchIteration in _coverSearchIterations)
		{
			coverSearchIteration.Draw();
		}
	}
}
