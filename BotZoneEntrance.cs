using System;
using UnityEngine;

[Serializable]
public class BotZoneEntrance
{
	public Vector3 PointOutSide;

	public Vector3 PointInside;

	public Vector3 CenterPoint;

	public int ConnectedAreaId;

	public BotZoneEntrance(Vector3 center, int connectedAreaId, Vector3 pointOutSide, Vector3 pointInside)
	{
		PointInside = pointInside;
		PointOutSide = pointOutSide;
		CenterPoint = center;
		ConnectedAreaId = connectedAreaId;
	}

	public void DrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(CenterPoint, 0.5f);
		Gizmos.DrawWireSphere(CenterPoint, 0.52f);
		Gizmos.color = new Color(1f, 0.5f, 0.1f);
		Gizmos.DrawLine(CenterPoint, PointInside);
		Gizmos.DrawSphere(PointInside, 0.2f);
		Gizmos.DrawWireSphere(PointInside, 0.22f);
		Gizmos.color = new Color(1f, 0.1f, 0.5f);
		Gizmos.DrawLine(CenterPoint, PointOutSide);
		Gizmos.DrawSphere(PointOutSide, 0.2f);
		Gizmos.DrawWireSphere(PointOutSide, 0.22f);
	}
}
