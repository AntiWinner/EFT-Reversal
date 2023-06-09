using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Comfort.Common;
using EFT;
using UnityEngine;

public class AISuspiciousPlace : MonoBehaviour, IPhysicsTrigger
{
	public List<int> lookPlaces = new List<int>();

	public List<Vector3> lookPlacesP = new List<Vector3>();

	[CompilerGenerated]
	private readonly string _E000 = _ED3E._E000(15069);

	public string Description
	{
		[CompilerGenerated]
		get
		{
			return _E000;
		}
	}

	public void DropPlaces()
	{
		lookPlaces = new List<int>();
		lookPlacesP = new List<Vector3>();
	}

	public void AddPlaceToLook(CustomNavigationPoint customNavigationPoint)
	{
		lookPlaces.Add(customNavigationPoint.CovPointsPlace.Id);
		lookPlacesP.Add(customNavigationPoint.CovPointsPlace.Origin);
	}

	public bool EnoughtPlaces()
	{
		return lookPlaces.Count >= 2;
	}

	void IPhysicsTrigger.OnTriggerExit(Collider col)
	{
		Player playerByCollider = Singleton<GameWorld>.Instance.GetPlayerByCollider(col);
		if (!(playerByCollider == null) && playerByCollider.AIData != null)
		{
			playerByCollider.AIData.SuspiciousPlace = this;
		}
	}

	void IPhysicsTrigger.OnTriggerEnter(Collider col)
	{
	}

	private void OnDrawGizmosSelected()
	{
		foreach (Vector3 item in lookPlacesP)
		{
			Gizmos.color = new Color(0.5f, 1f, 0.2f, 0.7f);
			Vector3 vector = item + Vector3.up;
			Gizmos.DrawSphere(vector, 0.3f);
			Gizmos.DrawLine(base.transform.position, vector);
		}
	}

	[SpecialName]
	bool IPhysicsTrigger.get_enabled()
	{
		return base.enabled;
	}

	[SpecialName]
	void IPhysicsTrigger.set_enabled(bool value)
	{
		base.enabled = value;
	}
}
