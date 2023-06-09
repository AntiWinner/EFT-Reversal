using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EFT.Interactive;

public class BetterPropagationGroups : MonoBehaviour
{
	[Serializable]
	public class Volumes
	{
		public Vector4 Area;

		public BetterPropagationVolume[] PropagationVolumes;

		public bool Contains2dPoint(Vector3 position)
		{
			if (position.x > Area.x && position.x < Area.z && position.z > Area.y)
			{
				return position.z < Area.w;
			}
			return false;
		}
	}

	[Header("Keep Area size: X < Z and Y < W")]
	public Volumes[] Groups;

	private List<BetterPropagationVolume> m__E000 = new List<BetterPropagationVolume>(5);

	public void GetVolumes(Vector3 position, in List<BetterPropagationVolume> buffer)
	{
		this.m__E000.Clear();
		buffer.Clear();
		Volumes[] groups = Groups;
		foreach (Volumes volumes in groups)
		{
			if (!volumes.Contains2dPoint(position))
			{
				continue;
			}
			BetterPropagationVolume[] propagationVolumes = volumes.PropagationVolumes;
			foreach (BetterPropagationVolume betterPropagationVolume in propagationVolumes)
			{
				if (betterPropagationVolume.Collider.ContainsPoint(position))
				{
					buffer.Add(betterPropagationVolume);
				}
			}
		}
	}

	public List<BetterPropagationVolume> GetIsolatedVolumes(Vector3 position, List<BetterPropagationVolume> volumesBuffer)
	{
		Volumes[] groups = Groups;
		foreach (Volumes volumes in groups)
		{
			if (!volumes.Contains2dPoint(position))
			{
				continue;
			}
			BetterPropagationVolume[] propagationVolumes = volumes.PropagationVolumes;
			foreach (BetterPropagationVolume betterPropagationVolume in propagationVolumes)
			{
				if (betterPropagationVolume.IsIsolated && !volumesBuffer.Contains(betterPropagationVolume) && betterPropagationVolume.Collider.ContainsPoint(position))
				{
					volumesBuffer.Add(betterPropagationVolume);
				}
			}
		}
		return volumesBuffer;
	}

	[ContextMenu("Collect volumes")]
	private void _E000()
	{
		BetterPropagationVolume[] componentsInChildren = base.gameObject.GetComponentsInChildren<BetterPropagationVolume>();
		Volumes[] groups = Groups;
		foreach (Volumes volumes in groups)
		{
			volumes.PropagationVolumes = new BetterPropagationVolume[0];
			BetterPropagationVolume[] array = componentsInChildren;
			foreach (BetterPropagationVolume betterPropagationVolume in array)
			{
				Vector3 position = betterPropagationVolume.transform.position;
				Quaternion rotation = betterPropagationVolume.transform.rotation;
				Vector3 size = betterPropagationVolume.Collider.size;
				size.x *= betterPropagationVolume.transform.localScale.x;
				size.z *= betterPropagationVolume.transform.localScale.z;
				Vector3[] array2 = new Vector3[4]
				{
					position + rotation * new Vector3(size.x, 0f, size.z) / 2f,
					position + rotation * new Vector3(size.x, 0f, 0f - size.z) / 2f,
					position + rotation * new Vector3(0f - size.x, 0f, size.z) / 2f,
					position + rotation * new Vector3(0f - size.x, 0f, 0f - size.z) / 2f
				};
				foreach (Vector3 position2 in array2)
				{
					if (volumes.Contains2dPoint(position2))
					{
						volumes.PropagationVolumes = volumes.PropagationVolumes.Concat(new BetterPropagationVolume[1] { betterPropagationVolume }).ToArray();
						break;
					}
				}
			}
			volumes.PropagationVolumes = volumes.PropagationVolumes.OrderByDescending((BetterPropagationVolume v) => v.transform.localScale.magnitude).ToArray();
		}
	}
}
