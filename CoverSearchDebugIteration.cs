using System;
using UnityEngine;

[Serializable]
public class CoverSearchDebugIteration
{
	[SerializeField]
	private CoverSearchDebugPointsGroup _coverPointGroups;

	[SerializeField]
	public Vector3 CenterPoint;

	[SerializeField]
	public Vector3? ResultCover;

	public string timeMark;

	public string searchLabel;

	public CoverSearchDebugPointsGroup CoverPointGroups => _coverPointGroups;

	public CoverSearchDebugIteration(string label = "Default")
	{
		_coverPointGroups = new CoverSearchDebugPointsGroup(label);
		timeMark = Time.time.ToString();
	}

	public void AddPointToIteration(Vector3 point, int level, float sqrRadius)
	{
		string label = _ED3E._E000(36568) + level;
		_coverPointGroups.Add(label, point);
		_coverPointGroups.SetRadius(label, Mathf.Sqrt(sqrRadius));
	}

	public void Draw()
	{
		_coverPointGroups.Draw();
		if (_coverPointGroups.IsDraw)
		{
			Gizmos.color = Color.magenta;
			Gizmos.DrawSphere(CenterPoint, _coverPointGroups.SpecialRadius);
			Gizmos.color = Color.cyan;
			if (ResultCover.HasValue)
			{
				Gizmos.DrawSphere(ResultCover.Value, _coverPointGroups.SpecialRadius);
			}
			Gizmos.DrawWireSphere(CenterPoint, _coverPointGroups.SearchRadius);
		}
	}
}
