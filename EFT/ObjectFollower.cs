using System;
using System.Collections.Generic;
using UnityEngine;

namespace EFT;

public class ObjectFollower : MonoBehaviour
{
	[Serializable]
	public struct Line
	{
		public Vector3 Point1;

		public Vector3 Point2;

		public Vector3 GetNearestPointOnLine(Vector3 position)
		{
			Vector3 vector = Point2 - Point1;
			float magnitude = vector.magnitude;
			vector.Normalize();
			float value = Vector3.Dot(position - Point1, vector);
			value = Mathf.Clamp(value, 0f, magnitude);
			return Point1 + vector * value;
		}
	}

	[SerializeField]
	private Transform _object;

	[SerializeField]
	private List<Line> _lines;

	public void Follow(Vector3 position)
	{
		position = base.transform.InverseTransformPoint(position);
		_object.transform.position = base.transform.TransformPoint(_E000(position));
	}

	private Vector3 _E000(Vector3 worldPosition)
	{
		Vector3 result = Vector3.zero;
		float num = float.MaxValue;
		foreach (Line line in _lines)
		{
			Vector3 nearestPointOnLine = line.GetNearestPointOnLine(worldPosition);
			float magnitude = (nearestPointOnLine - worldPosition).magnitude;
			if (magnitude < num)
			{
				num = magnitude;
				result = nearestPointOnLine;
			}
		}
		return result;
	}
}
