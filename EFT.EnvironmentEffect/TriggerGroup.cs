using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;

namespace EFT.EnvironmentEffect;

public class TriggerGroup : MonoBehaviour, EnvironmentManagerBase._E000
{
	[SerializeField]
	public Bounds Bounds;

	[SerializeField]
	public List<IndoorTrigger> _childrenTriggers = new List<IndoorTrigger>();

	[SerializeField]
	public List<TriggerGroup> _childrenGroups = new List<TriggerGroup>();

	public void OnValidate()
	{
		if (!Application.isPlaying)
		{
			Reinit();
		}
	}

	private void Awake()
	{
		if (Bounds.size.magnitude < Mathf.Epsilon)
		{
			Reinit();
		}
	}

	public void Reinit()
	{
		Bounds = default(Bounds);
		_childrenTriggers.Clear();
		_childrenGroups.Clear();
		IEnumerable<IndoorTrigger> enumerable = base.gameObject.GetComponentsInChildren<IndoorTrigger>().Reverse();
		_childrenGroups = (from t in base.gameObject.GetComponentsInChildren<TriggerGroup>()
			where t.transform.parent == base.transform
			select t).Reverse().ToList();
		foreach (IndoorTrigger item in enumerable)
		{
			_E000(item.Bounds);
			if (!(item.transform.parent != base.transform))
			{
				_childrenTriggers.Add(item);
			}
		}
	}

	[CanBeNull]
	public IndoorTrigger Check(Vector3 pos)
	{
		if (!Bounds.Contains(pos))
		{
			return null;
		}
		for (int i = 0; i < _childrenGroups.Count; i++)
		{
			IndoorTrigger indoorTrigger = _childrenGroups[i].Check(pos);
			if (indoorTrigger != null)
			{
				return indoorTrigger;
			}
		}
		for (int j = 0; j < _childrenTriggers.Count; j++)
		{
			IndoorTrigger indoorTrigger2 = _childrenTriggers[j].Check(pos);
			if (indoorTrigger2 != null)
			{
				return indoorTrigger2;
			}
		}
		return null;
	}

	private void _E000(Bounds bound)
	{
		if (Bounds.size.magnitude < Mathf.Epsilon)
		{
			Bounds = bound;
			return;
		}
		Vector3 min = bound.min;
		Vector3 max = bound.max;
		Vector3 vector = new Vector3(Mathf.Min(min.x, Bounds.min.x), Mathf.Min(min.y, Bounds.min.y), Mathf.Min(min.z, Bounds.min.z));
		Vector3 vector2 = new Vector3(Mathf.Max(max.x, Bounds.max.x), Mathf.Max(max.y, Bounds.max.y), Mathf.Max(max.z, Bounds.max.z));
		Vector3 center = 0.5f * (vector2 + vector);
		Vector3 size = vector2 - vector;
		Bounds = new Bounds(center, size);
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = new Color(0.4f, 0.2f, 0f, 0.2f);
		Gizmos.DrawCube(Bounds.center, Bounds.size);
	}

	[CompilerGenerated]
	private bool _E001(TriggerGroup t)
	{
		return t.transform.parent == base.transform;
	}
}
