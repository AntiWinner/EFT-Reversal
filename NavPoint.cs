using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class NavPoint
{
	[FormerlySerializedAs("causeNotValid")]
	public string reasonNotValid;

	public int Index;

	public bool IsValid = true;

	public Vector3 Pos;

	public NavPoint(int index, Vector3 pos)
	{
		Index = index;
		Pos = pos;
	}

	public NavPoint(NavPoint other)
	{
		Index = other.Index;
		Pos = other.Pos;
		IsValid = other.IsValid;
		reasonNotValid = other.reasonNotValid;
	}

	public void SetNotValidReason(string c)
	{
		IsValid = false;
		reasonNotValid = c;
	}
}
