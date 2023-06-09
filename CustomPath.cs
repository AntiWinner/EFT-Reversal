using System;
using System.Collections;
using System.Collections.Generic;
using EFT;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class CustomPath
{
	[NonSerialized]
	public Vector3 _up = Vector3.up * 1.1f;

	public List<Corner> Corners;

	[NonSerialized]
	public EPathStatus Status;

	private List<Corner> _temporarilyCorners = new List<Corner>(100);

	public CustomPath()
	{
		Corners = new List<Corner>(50);
	}

	public void AddCorners(params Corner[] corners)
	{
		Corners.AddRange(corners);
	}

	public void Update(Vector3 enemyWeaponRoot, List<_E0FC> leanPoints)
	{
		_temporarilyCorners.Clear();
		for (int i = 0; i < Corners.Count - 1; i++)
		{
			NavMeshPath navMeshPath = new NavMeshPath();
			if (NavMesh.CalculatePath(Corners[i].Pos, Corners[i + 1].Pos, -1, navMeshPath))
			{
				if (navMeshPath.status != 0)
				{
					Status = EPathStatus.Invalid;
				}
				Vector3[] corners = navMeshPath.corners;
				Corner[] array = new Corner[corners.Length];
				for (int j = 0; j < corners.Length; j++)
				{
					array[j] = new Corner
					{
						Pos = corners[j]
					};
				}
				array[0].PointsGroup = Corners[i].PointsGroup;
				array[array.Length - 1].PointsGroup = Corners[i + 1].PointsGroup;
				_temporarilyCorners.AddRange(array);
			}
		}
		int num = 1;
		while (num < _temporarilyCorners.Count - 1)
		{
			if ((_temporarilyCorners[num].Pos - _temporarilyCorners[num + 1].Pos).magnitude < 0.01f)
			{
				_temporarilyCorners.RemoveAt(num);
			}
			else
			{
				num++;
			}
		}
		for (int k = 0; k < _temporarilyCorners.Count - 1; k++)
		{
			Vector3 vector = (_temporarilyCorners[k + 1].Pos - _temporarilyCorners[k].Pos).normalized * 0.4f;
			vector.y = 0f;
			Vector3 vector2 = Quaternion.Euler(0f, 90f, 0f) * vector;
			Vector3 vector3 = Quaternion.Euler(0f, -90f, 0f) * vector;
			vector.y = 0f;
			NavMesh.SamplePosition(_temporarilyCorners[k].Pos + vector2, out var hit, 0.2f, -1);
			if (hit.hit)
			{
				_temporarilyCorners[k].Pos = hit.position;
				continue;
			}
			NavMesh.SamplePosition(_temporarilyCorners[k].Pos + vector3, out hit, 0.2f, -1);
			if (hit.hit)
			{
				_temporarilyCorners[k].Pos = hit.position;
			}
		}
		Corners = _temporarilyCorners;
		StaticManager.BeginCoroutine(AddInterractiveCheckPoints(enemyWeaponRoot, leanPoints, Corners));
	}

	public IEnumerator AddInterractiveCheckPoints(Vector3 enemyPos, List<_E0FC> leanPoints, List<Corner> corners)
	{
		yield return _E003(corners, enemyPos);
	}

	private bool _E000(Vector3 currPoint, CustomNavigationPoint currGroup, Vector3 dir, ICollection<_E0FC> leanPoints)
	{
		if (Physics.Raycast(currPoint, currGroup.ToWallVector, out var hitInfo, (int)_E37B.HighPolyWithTerrainMask))
		{
			if (hitInfo.distance < 10f)
			{
				return false;
			}
			leanPoints.Add(_E002(currPoint, currGroup.ToWallVector, _E001(currGroup.ToWallVector, dir)));
			return true;
		}
		leanPoints.Add(_E002(currPoint, currGroup.ToWallVector, _E001(currGroup.ToWallVector, dir)));
		return true;
	}

	private _E0FC.ELeanSide _E001(Vector3 a, Vector3 b)
	{
		if (!(a.z * b.x - a.x * b.z > 0f))
		{
			return _E0FC.ELeanSide.Right;
		}
		return _E0FC.ELeanSide.Left;
	}

	private _E0FC _E002(Vector3 pos, Vector3 dir, _E0FC.ELeanSide side)
	{
		return new _E0FC
		{
			Point = pos,
			Direction = dir,
			Side = side
		};
	}

	private IEnumerator _E003(List<Corner> corners, Vector3 enemyWeaponRoot)
	{
		int num = 0;
		float currStep = 0.5f;
		float num2 = 20f;
		float num3 = 20f;
		for (int i = 0; i < corners.Count - 1; i++)
		{
			Vector3 currPos = corners[i].Pos + _up;
			Vector3 vector = corners[i + 1].Pos + _up;
			Vector3 normalized = (vector - currPos).normalized;
			bool flag = false;
			while ((vector - currPos).magnitude > 0.01f)
			{
				if (!_E004(ref currPos, vector, ref currStep))
				{
					currPos = vector;
					continue;
				}
				if (!Physics.Linecast(currPos, enemyWeaponRoot, out var hitInfo, _E37B.HighPolyWithTerrainMask))
				{
					flag = true;
					break;
				}
				Vector3 vector2 = Quaternion.Euler(0f, 90f, 0f) * normalized;
				Vector3 vector3 = Quaternion.Euler(0f, -90f, 0f) * normalized;
				float num4 = (Physics.Raycast(currPos, vector2, out hitInfo, 20f, _E37B.HighPolyWithTerrainMask) ? hitInfo.distance : 20f);
				float num5 = (Physics.Raycast(currPos, vector3, out hitInfo, 20f, _E37B.HighPolyWithTerrainMask) ? hitInfo.distance : 20f);
				bool flag2 = num4 - num2 > 3f;
				bool flag3 = num5 - num3 > 3f;
				if (flag2 || flag3)
				{
					_E273 obj = new _E273
					{
						Point = currPos,
						Right = null,
						Left = null
					};
					if (flag2)
					{
						obj.Left = vector2;
					}
					if (flag3)
					{
						obj.Right = vector3;
					}
				}
				num2 = num4;
				num3 = num5;
				num++;
				if (num > 2)
				{
					num = 0;
					yield return null;
				}
			}
			if (flag)
			{
				break;
			}
		}
	}

	private bool _E004(ref Vector3 currPos, Vector3 end, ref float currStep)
	{
		Vector3 vector = end - currPos;
		if (currStep > vector.magnitude)
		{
			currStep -= vector.magnitude;
			return false;
		}
		currPos += vector.normalized * currStep;
		currStep = 0.8f;
		return true;
	}
}
