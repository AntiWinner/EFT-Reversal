using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PatrolPoints : MonoBehaviour
{
	public bool ShowGizmo;

	private readonly List<Transform> m__E000 = new List<Transform>();

	private void OnDrawGizmos()
	{
		if (!ShowGizmo)
		{
			return;
		}
		if (base.transform.childCount != this.m__E000.Count)
		{
			this.m__E000.Clear();
			this.m__E000.AddRange(from x in base.transform.GetComponentsInChildren<Transform>()
				where x.transform != base.transform
				select x);
			return;
		}
		Gizmos.color = Color.cyan;
		for (int i = 0; i < this.m__E000.Count; i++)
		{
			Gizmos.DrawWireCube(this.m__E000[i].position, Vector3.one * 0.2f);
		}
	}

	[CompilerGenerated]
	private bool _E000(Transform x)
	{
		return x.transform != base.transform;
	}
}
