using System.Collections.Generic;
using UnityEngine;

public class NavMeshCutGroup : MonoBehaviour
{
	public List<NavMeshCutElement> _elements = new List<NavMeshCutElement>();

	private List<NavMeshCutElement> _E000 = new List<NavMeshCutElement>();

	public bool HaveFreeToCut(NavMeshCutElement element)
	{
		if (element.IsCut)
		{
			return true;
		}
		int num = 0;
		for (int i = 0; i < _elements.Count; i++)
		{
			if (_elements[i].IsCut)
			{
				num++;
			}
		}
		return num < _elements.Count - 1;
	}

	private void OnDrawGizmosSelected()
	{
		DrawGizmo();
	}

	public void DrawGizmo()
	{
		Gizmos.color = Color.yellow;
		for (int i = 0; i < _elements.Count; i++)
		{
			NavMeshCutElement navMeshCutElement = _elements[i];
			Gizmos.DrawWireSphere(navMeshCutElement.Position, 0.2f);
			Gizmos.DrawWireSphere(navMeshCutElement.Position, 0.3f);
		}
		for (int j = 0; j < _elements.Count; j++)
		{
			for (int k = j; k < _elements.Count; k++)
			{
				NavMeshCutElement navMeshCutElement2 = _elements[j];
				Gizmos.DrawLine(to: _elements[k].Position, from: navMeshCutElement2.Position);
			}
		}
	}

	public bool UnCutOldest()
	{
		if (_E000.Count > 0)
		{
			NavMeshCutElement element = _E000[0];
			UnCut(element);
			return true;
		}
		return false;
	}

	public void Cut(NavMeshCutElement element)
	{
		_E000.Add(element);
		element.Obstacle.carving = true;
		element.SetUncutTime();
	}

	public void UnCut(NavMeshCutElement element)
	{
		_E000.Remove(element);
		element.Obstacle.carving = false;
	}
}
