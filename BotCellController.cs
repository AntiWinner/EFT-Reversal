using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using EFT.Interactive;
using UnityEngine;

public class BotCellController : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public Vector3 pos;

		internal Door _E000(Door x1, Door x2)
		{
			if ((x1.transform.position - pos).magnitude > (x2.transform.position - pos).magnitude)
			{
				return x2;
			}
			return x1;
		}
	}

	public float CellDrawLevel = 10f;

	public float Dimention = 10f;

	[HideInInspector]
	public AICellData Data;

	private NavMeshDoorLink[] m__E000;

	public void InitInGame()
	{
		BoxCollider componentInChildren = GetComponentInChildren<BoxCollider>();
		if (componentInChildren != null)
		{
			componentInChildren.enabled = false;
		}
	}

	public AICell FindCell(Vector3 pos)
	{
		int i = _E000(pos.x, Data.StartX);
		int j = _E000(pos.z, Data.StartZ);
		return Data.GetCell(i, j);
	}

	public void FindDoorLinks(_E620 bc)
	{
		Door[] source = _E3AA.FindUnityObjectsOfType<Door>();
		List<NavMeshDoorLink> list = new List<NavMeshDoorLink>();
		for (int i = 0; i < Data.List.Length; i++)
		{
			AICell aICell = Data.List[i];
			if (aICell.Links == null || aICell.Links.Length == 0)
			{
				continue;
			}
			NavMeshDoorLink[] links = aICell.Links;
			foreach (NavMeshDoorLink navMeshDoorLink in links)
			{
				Vector3 pos = navMeshDoorLink.transform.position;
				Door door = source.Aggregate((Door x1, Door x2) => ((x1.transform.position - pos).magnitude > (x2.transform.position - pos).magnitude) ? x2 : x1);
				_ = (navMeshDoorLink.transform.position - door.transform.position).sqrMagnitude;
				_ = 1f;
				navMeshDoorLink.Init(bc);
				navMeshDoorLink.SetDoor(door, withSubscribe: true);
				list.Add(navMeshDoorLink);
			}
		}
		this.m__E000 = list.ToArray();
	}

	private void Update()
	{
		if (this.m__E000 != null)
		{
			for (int i = 0; i < this.m__E000.Length; i++)
			{
				this.m__E000[i].ManualUpdate();
			}
		}
	}

	private void OnDestroy()
	{
		this.m__E000 = null;
	}

	private int _E000(float f, float startCoef)
	{
		return (int)((f - startCoef) / Data.CellSize);
	}

	private void OnDrawGizmosSelected()
	{
		for (int i = 0; i < Data.MaxIx + 1; i++)
		{
			for (int j = 0; j < Data.MaxIz + 1; j++)
			{
				Vector3 vector = new Vector3(Data.StartX + Data.CellSize * (float)i, CellDrawLevel, Data.StartZ + Data.CellSize * (float)j);
				Vector3 to = vector + new Vector3(Data.CellSize, 0f, 0f);
				Vector3 to2 = vector + new Vector3(0f, 0f, Data.CellSize);
				_ = vector + new Vector3(Data.CellSize / 2f, 0f, Data.CellSize / 2f);
				Gizmos.color = Color.green;
				if (j != Data.MaxIz)
				{
					Gizmos.DrawLine(vector, to2);
				}
				if (i != Data.MaxIx)
				{
					Gizmos.DrawLine(vector, to);
				}
				if (i < Data.MaxIx && j < Data.MaxIz && Data.List != null)
				{
					_ = Data.GetCell(i, j)?.Links;
				}
			}
		}
	}
}
